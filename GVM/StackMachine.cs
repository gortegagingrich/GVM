using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVM
{

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
                LocalSymbolTable = new Hashtable(),
                ProgramCounter = 0
            };
            
            PreviousStates = new Stack<MachineState>();
        }

        public void ExecuteInstructions(List<Instruction> list)
        {
            while (CurrentState.ProgramCounter < list.Count() && list.ElementAt(CurrentState.ProgramCounter).Op != 0xFF)
            {
                ExecuteInstruction(list.ElementAt(CurrentState.ProgramCounter++));
            }
        }

        public void ExecuteInstruction(Instruction inst)
        {
            switch ((int)inst.Op)
            {
                case 0:
                    PushVal(inst.Value);
                    break;

                case 1:
                    PopVal();
                    break;

                case 2:
                    Call();
                    break;

                case 3:
                    StoreVal(inst.Value);
                    break;

                case 4:
                    StoreValGlobal(inst.Value);
                    break;

                case 5:
                    LoadValue(inst.Value);
                    break;

                case 6:
                    LoadValueGlobal(inst.Value);
                    break;

                case 7:
                    Copy();
                    break;

                case 8:
                    Swap();
                    break;

                case 9:
                    Over();
                    break;

                case 10:
                    PushState(inst.Value.ToInt32(null));
                    break;

                case 11:
                    PopState();
                    break;

                case 12:
                    If(inst.Value.ToInt32(null));
                    break;

                case 0x10:
                    AddInt();
                    break;

                case 0x11:
                    SubInt();
                    break;

                case 0x12:
                    MulInt();
                    break;

                case 0x13:
                    DivInt();
                    break;

                case 0x14:
                    AddFloat();
                    break;

                case 0x15:
                    SubFloat();
                    break;

                case 0x16:
                    MulFloat();
                    break;

                case 0x17:
                    DivFloat();
                    break;

                default:
                    Console.WriteLine("Unknown opcode: " + inst.Op);
                    break;
            }
        }

        // removes and returns value on top of current data stack
        public void PopVal()
        {
            CurrentState.DataStack.Pop();
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
        internal void Call(Action f)
        {
            f();
        }

        public void Call()
        {
            SysCall.GetAction(CurrentState.DataStack.Pop())();
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
        public void PushState(int pc)
        {
            PreviousStates.Push(CurrentState);
            CurrentState = new MachineState
            {
                DataStack = new Stack<IConvertible>(),
                LocalSymbolTable = new Hashtable(),
                ProgramCounter = pc
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
        public void If(int pc)
        {
            var top = CurrentState.DataStack.Pop().ToInt32(null);

            if (top == 0)
            {
                CurrentState.ProgramCounter = pc;
            }
        }

        // Int32 arithmetic
        public void AddInt()
        {
            CurrentState.DataStack.Push(CurrentState.DataStack.Pop().ToInt32(null) + CurrentState.DataStack.Pop().ToInt32(null));
        }

        public void SubInt()
        {
            var temp = CurrentState.DataStack.Pop().ToInt32(null);
            CurrentState.DataStack.Push(CurrentState.DataStack.Pop().ToInt32(null) - temp);
        }

        public void MulInt()
        {
            CurrentState.DataStack.Push(CurrentState.DataStack.Pop().ToInt32(null) * CurrentState.DataStack.Pop().ToInt32(null));
        }

        public void DivInt()
        {
            var temp = CurrentState.DataStack.Pop().ToInt32(null);
            CurrentState.DataStack.Push(CurrentState.DataStack.Pop().ToInt32(null) / temp);
        }

        // Float32 arithmetic
        public void AddFloat()
        {
            CurrentState.DataStack.Push(CurrentState.DataStack.Pop().ToSingle(null) + CurrentState.DataStack.Pop().ToSingle(null));
        }

        public void SubFloat()
        {
            var temp = CurrentState.DataStack.Pop().ToSingle(null);
            CurrentState.DataStack.Push(CurrentState.DataStack.Pop().ToSingle(null) - temp);
        }

        public void MulFloat()
        {
            CurrentState.DataStack.Push(CurrentState.DataStack.Pop().ToSingle(null) * CurrentState.DataStack.Pop().ToSingle(null));
        }

        public void DivFloat()
        {
            var temp = CurrentState.DataStack.Pop().ToSingle(null);
            CurrentState.DataStack.Push(CurrentState.DataStack.Pop().ToSingle(null) / temp);
        }
    }
}
