using System;
using System.Collections.Generic;

namespace GVM
{
    /* This class doesn't actually do anything and should never actually be instantiated.
     * This is only here to provide a place to implement whatever syscalls I may find necessary.
     */
    abstract class SysCall
    {
        public static void AddInt32(Stack<IConvertible> stack)
        {
            stack.Push(stack.Pop().ToInt32(null) + stack.Pop().ToInt32(null));
        }

        // temp <- top of stack
        // push NextInStack - temp
        public static void SubInt32(Stack<IConvertible> stack)
        {
            var temp = stack.Pop().ToInt32(null);
            stack.Push(stack.Pop().ToInt32(null) - temp);
        }

        public static void MulInt32(Stack<IConvertible> stack)
        {
            stack.Push(stack.Pop().ToInt32(null) * stack.Pop().ToInt32(null));
        }

        // temp <- top of stack
        // push NextInStack / temp
        public static void DivInt32(Stack<IConvertible> stack)
        {
            var temp = stack.Pop().ToInt32(null);
            stack.Push(stack.Pop().ToInt32(null) / temp);
        }

        public static void PrintInt32(Stack<IConvertible> stack)
        {
            Console.Write(stack.Peek().ToInt32(null));
        }

        public static void PrintString(Stack<IConvertible> stack)
        {
            Console.Write(stack.Peek().ToString());
        }
    }
}
