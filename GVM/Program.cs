using System;
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

            s.ExecuteInstructions(new List<Instruction> {
                // 1 + 3
                new Instruction(0x00,1), // 0: push 1
                new Instruction(0x00,3), // push 3
                new Instruction(0x10,null), // AddInt

                // print result
                new Instruction(0x04,0xFFFF), // 3: store_global 0xFFFF
                new Instruction(0x00,Calls.PrintInt32), // push 0
                new Instruction(0x02,null), // call

                // print new line
                new Instruction(0x00,"\n"), // 6: push "\n"
                new Instruction(0x04,0), // store_global 0
                new Instruction(0x01,null), // pop
                new Instruction(0x00,0), // push 0
                new Instruction(0x04,0xFFFF), // store_global 0xFFFF
                new Instruction(0x01,null), // pop
                new Instruction(0x00,Calls.PrintStringAddr), // push 2
                new Instruction(0x02,null), // call

                // 4 - 2
                new Instruction(0x00,2), // 14: push 2
                new Instruction(0x11,null), // SubInt

                // print result
                new Instruction(0x04,0xFFFF), // 16: store_global 0xFFFF
                new Instruction(0x00,Calls.PrintInt32), // push 0
                new Instruction(0x02,null), // call

                // print new line
                new Instruction(0x00,"\n"), // 19: push "\n"
                new Instruction(0x04,0), // store_global 0
                new Instruction(0x01,null), // pop
                new Instruction(0x00,0), // push 0
                new Instruction(0x04,0xFFFF), // store_global 0xFFFF
                new Instruction(0x01,null), // pop
                new Instruction(0x00,Calls.PrintStringAddr), // push 2
                new Instruction(0x02,null), // call

                // store and load local variable
                new Instruction(0x03, "SampleVariable"), // 27: store_local "SampleVariable"
                new Instruction(0x05, "SampleVariable"), // load_local "SampleVariable"

                // 2 ^ 4
                new Instruction(0x07, null), // 29: copy
                new Instruction(0x07, null), // copy
                new Instruction(0x07, null), // copy
                new Instruction(0x00, 2), // push 2
                new Instruction(0x12, null), // MulInt
                new Instruction(0x12, null), // MulInt
                new Instruction(0x12, null), // MulInt

                // print result
                new Instruction(0x04,0xFFFF), // 36: store_global 0xFFFF
                new Instruction(0x00,Calls.PrintInt32), // push 0
                new Instruction(0x02,null), // call

                // print new line
                new Instruction(0x00,"\n"), // 39: push "\n"
                new Instruction(0x04,0), // store_global 0
                new Instruction(0x01,null), // pop
                new Instruction(0x00,0), // push 0
                new Instruction(0x04,0xFFFF), // store_global 0xFFFF
                new Instruction(0x01,null), // pop
                new Instruction(0x00,Calls.PrintStringAddr), // push 2
                new Instruction(0x02,null), // call

                // swap and divide a couple times
                new Instruction(0x08,null), // 47: swap
                new Instruction(0x13,null), // DivInt
                new Instruction(0x08,null), // swap
                new Instruction(0x13,null), // DivInt

                // print result
                new Instruction(0x04,0xFFFF), // 51: store_global 0xFFFF
                new Instruction(0x00,Calls.PrintInt32), // push 0
                new Instruction(0x02,null), // call

                // skip print new line
                new Instruction(0x00,0), //54:  push 0
                new Instruction(0x0C,65), // bez 65

                // print new line
                new Instruction(0x00,"\n"), // 56: push "\n"
                new Instruction(0x04,0), // store_global 0
                new Instruction(0x01,null), // pop
                new Instruction(0x00,0), // push 0
                new Instruction(0x04,0xFFFF), // store_global 0xFFFF
                new Instruction(0x01,null), // pop
                new Instruction(0x00,Calls.PrintStringAddr), // push 2
                new Instruction(0x02,null), // call

                // end
                new Instruction(0xFF,null), // 64: end

                //branch destitination
                new Instruction(0x00,1), //65: push 1
                new Instruction(0x00,0), // push 0
                new Instruction(0x0C,56), // bez 56

                // shouldn't be called
                new Instruction(0x00,"This should not be reached\n"), // push "This should not be reached\n"
                new Instruction(0x04,0), // store_global 0
                new Instruction(0x01,null), // pop
                new Instruction(0x00,0), // push 0
                new Instruction(0x04,0xFFFF), // store_global 0xFFFF
                new Instruction(0x01,null), // pop
                new Instruction(0x00,Calls.PrintStringAddr), // push 2
                new Instruction(0x02,null), // call
            });
        }
    }
}
