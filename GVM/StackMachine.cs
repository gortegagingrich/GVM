using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVM
{
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

        public void StoreVal(Object addr)
        {
            CurrentState.LocalSymbolTable[addr] = CurrentState.DataStack.Peek();
        }

        public void LoadValue(Object addr)
        {
            CurrentState.DataStack.Push((IConvertible)CurrentState.LocalSymbolTable[addr]);
        }

        public void Copy()
        {
            CurrentState.DataStack.Push(CurrentState.DataStack.Peek());
        }

        public void Swap()
        {
            var a = CurrentState.DataStack.Pop();
            var b = CurrentState.DataStack.Pop();
            CurrentState.DataStack.Push(a);
            CurrentState.DataStack.Push(b);
        }
    }
}
