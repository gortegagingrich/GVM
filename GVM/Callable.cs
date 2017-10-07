using System;
using System.Collections.Generic;

namespace GVM
{
    abstract class SysCall
    {
        public static Action<Stack<IConvertible>> GetAction(IConvertible i)
        {
            Action<Stack<IConvertible>> fn;

            switch ((int)i)
            {
                case 0:
                    fn = AddInt32;
                    break;

                case 1:
                    fn = SubInt32;
                    break;

                case 2:
                    fn = MulInt32;
                    break;

                case 3:
                    fn = DivInt32;
                    break;

                case 4:
                    fn = AddFloat32;
                    break;

                case 5:
                    fn = SubFloat32;
                    break;

                case 6:
                    fn = MulFloat32;
                    break;

                case 7:
                    fn = DivFloat32;
                    break;

                case 8:
                    fn = PrintInt32;
                    break;

                case 10:
                    fn = PrintString;
                    break;

                default:
                    fn = (stack) =>
                    {
                        Console.WriteLine("Unknown syscall: " + i);
                    };
                    break;
            }

            return fn;
        }

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

        public static void AddFloat32(Stack<IConvertible> stack)
        {
            stack.Push(stack.Pop().ToSingle(null) + stack.Pop().ToSingle(null));
        }

        // temp <- top of stack
        // push NextInStack - temp
        public static void SubFloat32(Stack<IConvertible> stack)
        {
            var temp = stack.Pop().ToSingle(null);
            stack.Push(stack.Pop().ToSingle(null) - temp);
        }

        public static void MulFloat32(Stack<IConvertible> stack)
        {
            stack.Push(stack.Pop().ToSingle(null) * stack.Pop().ToSingle(null));
        }

        // temp <- top of stack
        // push NextInStack / temp
        public static void DivFloat32(Stack<IConvertible> stack)
        {
            var temp = stack.Pop().ToSingle(null);
            stack.Push(stack.Pop().ToSingle(null) / temp);
        }

        public static void PrintInt32(Stack<IConvertible> stack)
        {
            Console.Write(stack.Peek().ToInt32(null));
        }

        public static void PrintFloat32(Stack<IConvertible> stack)
        {
            Console.Write(stack.Peek().ToSingle(null));
        }

        public static void PrintString(Stack<IConvertible> stack)
        {
            Console.Write(StackMachine.GlobalSymbolTable[stack.Pop()]);
        }
    }
}
