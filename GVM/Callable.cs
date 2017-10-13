using System;
using System.Collections.Generic;

namespace GVM
{
    // notes: 
    // syscalls should never have any effect on any data stacks
    // default syscalls should only take one argument from the global symbol table
    // GlobalSymbolTable[0] should be used for passing arguments to default syscalls
    abstract class SysCall
    {
        public static Action GetAction(IConvertible i)
        {
            Action fn;
            

            switch ((int)i)
            {
                case 0:
                    fn = PrintInt32;
                    break;

                case 1:
                    fn = PrintFloat32;
                    break;

                case 2:
                    fn = PrintString;
                    break;

                default:
                    fn = () =>
                    {
                        Console.WriteLine("Unknown syscall: " + i);
                    };
                    break;
            }

            return fn;
        }

        // prints value of argument as a 32 bit integer
        public static void PrintInt32()
        {
            Console.Write((int)((IConvertible)StackMachine.GlobalSymbolTable[0]).ToUInt32(null));
        }

        // prints value of argument as a 32 bit float
        public static void PrintFloat32()
        {
            Console.Write(((IConvertible)StackMachine.GlobalSymbolTable[0]).ToSingle(null));
        }

        // prints string at address listed in argument row
        public static void PrintString()
        {
            Console.Write(((IConvertible)StackMachine.GlobalSymbolTable[0]).ToString());
        }
    }
}
