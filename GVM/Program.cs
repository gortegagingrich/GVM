﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVM
{
    class Program
    {
        static void Main(string[] args)
        {
            StackMachine s = new StackMachine();
            var instructions = new List<Instruction> {
                // 1 + 3
                new Instruction(0x00,1), // 0: push 1
                new Instruction(0x00,3), // push 3
                new Instruction(0x10,null), // AddInt

                // print result
                new Instruction(0x04,0), // 3: store_global 0
                new Instruction(0x00,Calls.PrintInt32), // push 0
                new Instruction(0x02,null), // call

                // print new line
                new Instruction(0x00,"\n"), // 6: push "\n"
                new Instruction(0x04,0), // store_global 0
                new Instruction(0x01,null), // pop
                new Instruction(0x00,Calls.PrintString), // push 2
                new Instruction(0x02,null), // call

                // 4 - 2
                new Instruction(0x00,2), // 11: push 2
                new Instruction(0x11,null), // SubInt

                // print result
                new Instruction(0x04,0), // 13: store_global 0
                new Instruction(0x00,Calls.PrintInt32), // push 0
                new Instruction(0x02,null), // call

                // print new line
                new Instruction(0x00,"\n"), // 16: push "\n"
                new Instruction(0x04,0), // store_global 0
                new Instruction(0x01,null), // pop
                new Instruction(0x00,Calls.PrintString), // push 2
                new Instruction(0x02,null), // call

                // store and load local variable
                new Instruction(0x03, "SampleVariable"), // 21: store_local "SampleVariable"
                new Instruction(0x05, "SampleVariable"), // load_local "SampleVariable"

                // 2 ^ 4
                new Instruction(0x07, null), // 23: copy
                new Instruction(0x07, null), // copy
                new Instruction(0x07, null), // copy
                new Instruction(0x00, 2), // push 2
                new Instruction(0x12, null), // MulInt
                new Instruction(0x12, null), // MulInt
                new Instruction(0x12, null), // MulInt

                // print result
                new Instruction(0x04,0), // 30: store_global 0
                new Instruction(0x00,Calls.PrintInt32), // push 0
                new Instruction(0x02,null), // call

                // print new line
                new Instruction(0x00,"\n"), // 33: push "\n"
                new Instruction(0x04,0), // store_global 0
                new Instruction(0x01,null), // pop
                new Instruction(0x00,Calls.PrintString), // push 2
                new Instruction(0x02,null), // call

                // swap and divide a couple times
                new Instruction(0x08,null), // 38: swap
                new Instruction(0x13,null), // DivInt
                new Instruction(0x08,null), // swap
                new Instruction(0x13,null), // DivInt

                // print result
                new Instruction(0x04,0), // 42: store_global 0
                new Instruction(0x00,Calls.PrintInt32), // push 0
                new Instruction(0x02,null), // call

                // skip print new line
                new Instruction(0x00,0), //45:  push 0
                new Instruction(0x0C,53), // bez 53

                // print new line
                new Instruction(0x00,"\n"), // 47: push "\n"
                new Instruction(0x04,0), // store_global 0
                new Instruction(0x01,null), // pop
                new Instruction(0x00,Calls.PrintString), // push 2
                new Instruction(0x02,null), // call

                // end
                new Instruction(0xFF,null), // 52: end

                //branch destitination
                new Instruction(0x00,1), //53: push 1
                new Instruction(0x00,0), // push 0
                new Instruction(0x0C,47), // bez 47

                // shouldn't be called
                new Instruction(0x00,"This should not be reached\n"), // push "This should not be reached\n"
                new Instruction(0x04,0), // store_global 0
                new Instruction(0x01,null), // pop
                new Instruction(0x00,0), // push 0
                new Instruction(0x04,0xFFFF), // store_global 0xFFFF
                new Instruction(0x01,null), // pop
                new Instruction(0x00,Calls.PrintString), // push 2
                new Instruction(0x02,null), // call
            };

            var instructions2 = new List<Instruction>
            {
                new Instruction(0x00,7),
                new Instruction(0x20,null),

                // print result
                new Instruction(0x04,0), // 3: store_global 0
                new Instruction(0x00,Calls.PrintInt32), // push 0
                new Instruction(0x02,null), // call

                // print new line
                new Instruction(0,"\n"), // push "\n"
                new Instruction(0x04,0), // store_global 0
                new Instruction(0,Calls.PrintString), // push 2
                new Instruction(0x02,null), // call

                new Instruction(0xFF,null), // end
            };

            var instructions3 = new List<Instruction>
            {
                new Instruction(0, 2), // push 2
                new Instruction(0,3), // push 3
                new Instruction(0x10,null), // add
                new Instruction(0x01,null), // pop
                new Instruction(0x0A,7), // jump PrintVal
                new Instruction(0x0A,11), // jump PrintLine
                new Instruction(0xFF,null), // exit
                // PrintVal:
                new Instruction(0x04,0), // store_global 0
                new Instruction(0,Calls.PrintInt32), // push 0
                new Instruction(2,null), // call
                new Instruction(0x0B,null), // return
                // PrintLine:
                new Instruction(0,"\n"), // push "\n"
                new Instruction(0x04,0), // store_global 0
                new Instruction(0,Calls.PrintString), // push 2
                new Instruction(2,null), // call
                new Instruction(0x0B,null), // return
            };

            s.ExecuteInstructions(instructions);
            s.ExecuteInstructions(instructions2);
            s.ExecuteInstructions(instructions3);
        }
    }
}
