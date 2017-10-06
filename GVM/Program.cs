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

            // push a couple values
            s.PushVal(1);
            s.PushVal(3);

            // try adding
            s.PushVal(0);
            s.Call();
            s.PushVal(4);
            s.Call();
            Console.WriteLine();

            // try pushing and printing a string
            s.PushVal("test");
            s.StoreValGlobal(1);
            s.PushVal(1);
            s.PushVal(5); // print string in global symbol table with key on top of stack
            s.Call();
            Console.WriteLine();

            // try subtracting
            s.PopVal();
            s.PushVal(2);
            s.PushVal(1); // perform subtraction
            s.Call();
            s.PushVal(4); // print int32 on top of stack
            s.Call();
            Console.WriteLine();

            // try storing a local variable and copying the top of the stack a couple times
            s.StoreVal("sample variable");
            s.Copy();
            s.Copy();
            s.Copy();
            s.PushVal(2);

            // try multiplying
            s.Call(SysCall.MulInt32);
            s.Call(SysCall.PrintInt32);
            Console.WriteLine();

            // try loading a local variable
            s.LoadValue("sample variable");
            s.Call(SysCall.PrintInt32);
            Console.WriteLine();

            // try multiplying
            s.Call(SysCall.MulInt32);
            s.Call(SysCall.PrintInt32);
            Console.WriteLine();

            // do it again with the next value on the stack
            s.Call(SysCall.MulInt32);
            s.Call(SysCall.PrintInt32);
            Console.WriteLine();

            // try swapping and dividing a couple times
            s.Swap();
            s.Call(SysCall.DivInt32);
            s.Call(SysCall.PrintInt32);
            Console.WriteLine();

            s.Swap();
            s.Call(SysCall.DivInt32);
            s.Call(SysCall.PrintInt32);
            Console.WriteLine();
        }
    }
}
