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

        internal static Hashtable GlobalSymbolTable = new Hashtable();

        public StackMachine()
        {
            CurrentState = new MachineState
            {
                DataStack = new Stack<IConvertible>(),
                LocalSymbolTable = new Hashtable()
            };
            
            PreviousStates = new Stack<MachineState>();
        }

        // removes and returns value on top of current data stack
        public IConvertible PopVal()
        {
            return CurrentState.DataStack.Pop();
        }

        // pushes given IConvertible value onto current data stack
        public void PushVal(IConvertible val)
        {
            CurrentState.DataStack.Push(val);
        }

        // performs given action on the current data stack
        // this is used for whatever you might want to implement through syscalls,
        // like printing out values from the stack or symbol tables.
        // Normally, Call() should be used instead; this is here to make testing syscalls easier.
        internal void Call(Action<Stack<IConvertible>> f)
        {
            f(CurrentState.DataStack);
        }

        public void Call()
        {
            SysCall.GetAction(CurrentState.DataStack.Pop())(CurrentState.DataStack);
        }

        // looks up the value at the top of the current data stack
        // and stores it in the current local symbol table with the given key
        public void StoreVal(Object addr)
        {
            CurrentState.LocalSymbolTable[addr] = CurrentState.DataStack.Peek();
        }

        // same as StoreVal, but stores in the global symbol table
        public void StoreValGlobal(Object addr)
        {
            GlobalSymbolTable[addr] = CurrentState.DataStack.Peek();
        }

        // pushes the corresponding value from the current local symbol table onto the local data stack
        public void LoadValue(Object addr)
        {
            CurrentState.DataStack.Push((IConvertible)CurrentState.LocalSymbolTable[addr]);
        }

        // same as load value, but uses the global symbol table
        public void LoadValueGlobal(Object addr)
        {
            CurrentState.DataStack.Push((IConvertible)GlobalSymbolTable[addr]);
        }

        // duplicates the value on top of the stack
        public void Copy()
        {
            CurrentState.DataStack.Push(CurrentState.DataStack.Peek());
        }

        // swaps the top two values on the stack
        public void Swap()
        {
            var a = CurrentState.DataStack.Pop();
            var b = CurrentState.DataStack.Pop();
            CurrentState.DataStack.Push(a);
            CurrentState.DataStack.Push(b);
        }

        // pushes a copy of the second element onto the data stack
        public void Over()
        {
            var a = CurrentState.DataStack.Pop();
            var b = CurrentState.DataStack.Pop();
            CurrentState.DataStack.Push(b);
            CurrentState.DataStack.Push(a);
            CurrentState.DataStack.Push(b);
        }

        // switches to a new state with a new data stack and local symbol table
        // will be used for stuff like subroutines
        public void PushState()
        {
            PreviousStates.Push(CurrentState);
            CurrentState = new MachineState
            {
                DataStack = new Stack<IConvertible>(),
                LocalSymbolTable = new Hashtable()
            };
        }

        // switches to the previous state
        // will be used for returning from a subroutine
        public void PopState()
        {
            CurrentState = PreviousStates.Pop();
        }

        // if value on top of stack is 0, jumps to instruction number
        // at CurrentState.LocalSymbolTable["If-Branch"]
        // Won't be implemented until I design an instruction set
        public void If()
        {
            throw new NotImplementedException();
        }
    }
}
