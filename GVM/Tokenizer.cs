﻿using System;
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
        SingleToken = 0,
        DoubleToken = 1,
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
                case TokenType.SingleToken:
                    return "op_0arg";
                case TokenType.DoubleToken:
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
                    var str = Content.Substring(1,Content.Length - 2);
                    str = str.Replace("\\\"", "\"");
                    str = str.Replace("\\n", "\n");
                    return str;

                case TokenType.SingleToken:
                case TokenType.DoubleToken:
                default:
                    return "";
            }
        }
    }

    public class Tokenizer
    {
        private static Hashtable Patterns = new Hashtable
        {
            [TokenType.DoubleToken] = new Regex(@"[\w_]+ (\w|\"")+"),
            [TokenType.SingleToken] = new Regex(@"[\w_]+"),
            [TokenType.String] = new Regex("[\"]([\\\"]|[^\"])*[\"]"),
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
                if (TestMatch(line, TokenType.DoubleToken))
                {
                    // add the operator
                    tokens.Add(new Token
                    {
                        Type = TokenType.DoubleToken,
                        Content = ((Regex)Patterns[TokenType.SingleToken]).Match(line).ToString()
                    });

                    // add the argument
                    type = TokenType.String;
                    tempLine = Regex.Replace(line, @"\w+ ", "");
                    
                    if (TestMatch(tempLine, TokenType.String))
                    {
                        type = TokenType.String;
                    }
                    else if (TestMatch(tempLine, TokenType.DoubleToken))
                    {
                        type = TokenType.DoubleToken;
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
                else if (TestMatch(line, TokenType.SingleToken))
                {
                    // add the operator
                    tokens.Add(new Token {
                        Type = TokenType.SingleToken,
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
