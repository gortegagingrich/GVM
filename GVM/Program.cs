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

            s.PushVal(1);
            s.PushVal(3);
            s.Call(StackMachine.AddInt32);
            s.Call(StackMachine.PrintInt32);
            Console.WriteLine();
            s.PushVal("test");
            s.Call(StackMachine.PrintString);
            Console.WriteLine();
            s.PopVal();
            s.PushVal(2);
            s.Call(StackMachine.SubInt32);
            s.Call(StackMachine.PrintInt32);
            Console.WriteLine();
        }
    }
}
