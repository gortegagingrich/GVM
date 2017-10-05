using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVM
{
    interface Callable
    {
        void Call(Stack<IConvertible> stack);
    }

    // each state needs to keep track of the current data stack and a local symbol table
    struct MachineState
    {
        public Stack<IConvertible> DataStack;
        public Hashtable LocalSymbolTable;
        // not actually used yet
        // public int ProgramCounter;
    }

    class StackMachine
    {

        private MachineState CurrentState;
        private Stack<MachineState> PreviousStates;

        private Hashtable GlobalSymbolTable;

        public StackMachine()
        {
            CurrentState = new MachineState();
            CurrentState.DataStack = new Stack<IConvertible>();
            CurrentState.LocalSymbolTable = new Hashtable();

            GlobalSymbolTable = new Hashtable();
            PreviousStates = new Stack<MachineState>();
        }

        public IConvertible PopVal()
        {
            return CurrentState.DataStack.Pop();
        }

        public void PushVal(IConvertible val)
        {
            CurrentState.DataStack.Push(val);
        }

        public void Call(Action<Stack<IConvertible>> f)
        {
            f.Invoke(CurrentState.DataStack);
        }

        public static void AddInt32(Stack<IConvertible> stack) 
        {
            stack.Push(stack.Pop().ToInt32(null) + stack.Pop().ToInt32(null));
        }

        public static void SubInt32(Stack<IConvertible> stack)
        {
            Int32 temp;

            temp = stack.Pop().ToInt32(null);
            stack.Push(stack.Pop().ToInt32(null) - temp);
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
