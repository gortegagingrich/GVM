using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVM
{
    class Parser
    {
        public static List<Instruction> Parse(List<Token> tokens)
        {
            Token token;
            List<Instruction> instructions = new List<Instruction>();

            for (int i = 0; i < tokens.Count(); i++)
            {
                token = tokens[i];

                if (token.Type == TokenType.SingleToken)
                {
                    instructions.Add(new Instruction(Instruction.StringToOp(token.Content), null));
                }
                else if (token.Type == TokenType.DoubleToken)
                {
                    instructions.Add(new Instruction(Instruction.StringToOp(token.Content), tokens[++i].ConvertContent()));
                }
            }

            return instructions;
        }


    }
}
