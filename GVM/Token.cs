using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace GVM
{
    public enum TokenType
    {
        SingleArg = 0,
        DoubleArg = 1,
        String = 2,
        IntDec = 3,
        IntHex = 4,
        Float = 5
    }

    public struct Token
    {
        public TokenType Type;
        public string Content;

        public string GetString()
        {
            return String.Format("{{Type: {0}, Content: {1}}}",TypeTostring(), Content);
        }

        private string TypeTostring()
        {
            switch (Type)
            {
                case TokenType.SingleArg:
                    return "op_0arg";
                case TokenType.DoubleArg:
                    return "op_1arg";
                case TokenType.IntDec:
                    return "int_dec";
                case TokenType.IntHex:
                    return "int_hex";
                case TokenType.Float:
                    return "float";
                case TokenType.String:
                    return "string";
                default:
                    return "Arg";
            }
        }

        public IConvertible ConvertContent()
        {
            switch (Type)
            {
                case TokenType.IntDec:
                    return Int32.Parse(Content,null);

                case TokenType.IntHex:
                    return Int32.Parse(Content, null);

                case TokenType.Float:
                    return Single.Parse(Content, System.Globalization.NumberStyles.HexNumber);

                case TokenType.String:
                    var str = Content.Replace("\"","\"");
                    str = str.Replace("\\\"", "\"");
                    str = str.Replace("\\n", "\n");
                    return str;

                case TokenType.SingleArg:
                case TokenType.DoubleArg:
                default:
                    return "";
            }
        }
    }

    public class Tokenizer
    {
        private static Hashtable Patterns = new Hashtable
        {
            [TokenType.DoubleArg] = new Regex(@"\w+ \w+"),
            [TokenType.SingleArg] = new Regex(@"[a-z]+"),
            [TokenType.String] = new Regex("[\"][[\\\"]|[^\"]]*[\"]"),
            [TokenType.IntDec] = new Regex("[0-9]+"),
            [TokenType.IntHex] = new Regex("(0x|0X)[0-9a-fA-F]+"),
            [TokenType.Float] = new Regex(@"[0-9]+\.[0-9]+")
        };

        public static List<Token> Tokenize(string file)
        {
            List<Token> tokens = new List<Token>();
            var lines = File.ReadLines(file);
            string tempLine;
            TokenType type;

            foreach (string line in lines)
            {
                if (TestMatch(line, TokenType.DoubleArg))
                {
                    // add the operator
                    tokens.Add(new Token
                    {
                        Type = TokenType.DoubleArg,
                        Content = ((Regex)Patterns[TokenType.SingleArg]).Match(line).ToString()
                    });

                    // add the argument
                    type = TokenType.String;
                    tempLine = Regex.Replace(line, @"\w+ ", "");
                    
                    if (TestMatch(tempLine, TokenType.String))
                    {
                        type = TokenType.String;
                    }
                    else if (TestMatch(tempLine, TokenType.DoubleArg))
                    {
                        type = TokenType.DoubleArg;
                    }
                    else if (TestMatch(tempLine, TokenType.IntHex))
                    {
                        type = TokenType.IntHex;
                    }
                    else if (TestMatch(tempLine, TokenType.Float))
                    {
                        type = TokenType.Float;
                    }
                    else if (TestMatch(tempLine, TokenType.IntDec))
                    {
                        type = TokenType.IntDec;
                    }

                    tokens.Add(new Token
                    {
                        Type = type,
                        Content = tempLine
                    });
                }
                else if (TestMatch(line, TokenType.SingleArg))
                {
                    // add the operator
                    tokens.Add(new Token {
                        Type = TokenType.SingleArg,
                        Content = line
                    });
                }
            }

            return tokens;
        }

        internal static bool TestMatch(string s, TokenType t)
        {
            return ((Regex)Patterns[t]).IsMatch(s);
        }
    }
}
