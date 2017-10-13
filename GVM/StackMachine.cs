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
        private Hashtable Syscalls;

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

            SetDefaultSyscalls();
        }

        public void AddSyscall(IConvertible id, Action act)
        {
            Syscalls[id] = act;
        }

        public void SetDefaultSyscalls()
        {
            Syscalls = new Hashtable
            {
                [Calls.PrintInt32] = SysCall.GetAction(Calls.PrintInt32),
                [Calls.PrintFloat32] = SysCall.GetAction(Calls.PrintFloat32),
                [Calls.PrintStringAddr] = SysCall.GetAction(Calls.PrintStringAddr)
            };
        }

        public void ExecuteInstructions(List<Instruction> list)
        {
            while (CurrentState.ProgramCounter < list.Count() && list.ElementAt(CurrentState.ProgramCounter).Op != 0xFF)
            {
                try
                {
                    ExecuteInstruction(list.ElementAt(CurrentState.ProgramCounter++));
                } catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(list.ElementAt(--CurrentState.ProgramCounter).Op);
                    Console.WriteLine("An exception was thrown.\nCurrent stack state:");
                    foreach (IConvertible i in CurrentState.DataStack)
                    {
                        Console.WriteLine(i);
                    }
                    return;
                }
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

                case 0x0A:
                    PushState(inst.Value.ToInt32(null));
                    break;

                case 0x0B:
                    PopState();
                    break;

                case 0x0C:
                    BranchZero(inst.Value.ToInt32(null));
                    break;

                case 0x0D:
                    BranchNegative(inst.Value.ToInt32(null));
                    break;

                case 0x0E:
                    BranchPositive(inst.Value.ToInt32(null));
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

                case 0x18:
                    ModInt();
                    break;

                case 0x19:
                    Inc();
                    break;

                case 0x1A:
                    Dec();
                    break;

                case 0x20:
                    Not();
                    break;

                case 0x21:
                    And();
                    break;

                case 0x22:
                    Or();
                    break;

                case 0x23:
                    ShiftLeft(inst.Value.ToInt32(null));
                    break;

                case 0x24:
                    ShiftRight(inst.Value.ToInt32(null));
                    break;

                case 0x25:
                    RotateLeft(inst.Value.ToInt32(null));
                    break;

                case 0x26:
                    RotateRight(inst.Value.ToInt32(null));
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
            var a = CurrentState.DataStack.Pop();

            ((Action)Syscalls[a])();
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

        // Sets PC to given int if value on top is 0
        public void BranchZero(int pc)
        {
            if (CurrentState.DataStack.Pop().ToInt32(null) == 0)
            {
                CurrentState.ProgramCounter = pc;
            }
        }

        // Sets PC to given int if value on top is greater than 0
        public void BranchPositive(int pc)
        {
            if (CurrentState.DataStack.Pop().ToInt32(null) > 0)
            {
                CurrentState.ProgramCounter = pc;
            }
        }

        // Sets PC to given int if value on top is less than 0
        public void BranchNegative(int pc)
        {
            if (CurrentState.DataStack.Pop().ToInt32(null) < 0)
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
            var a = CurrentState.DataStack.Pop().ToInt32(null);
            CurrentState.DataStack.Push(CurrentState.DataStack.Pop().ToInt32(null) / a);
        }

        public void ModInt()
        {
            var a = CurrentState.DataStack.Pop().ToInt32(null);
            CurrentState.DataStack.Push(CurrentState.DataStack.Pop().ToInt32(null) % a);
        }

        public void Inc()
        {
            CurrentState.DataStack.Push(CurrentState.DataStack.Pop().ToInt32(null) + 1);
        }

        public void Dec()
        {
            CurrentState.DataStack.Push(CurrentState.DataStack.Pop().ToInt32(null) - 1);
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

        // bitwise operations
        public void Not()
        {
            CurrentState.DataStack.Push(~CurrentState.DataStack.Pop().ToUInt32(null));
        }

        public void And()
        {
            CurrentState.DataStack.Push(CurrentState.DataStack.Pop().ToUInt32(null) & CurrentState.DataStack.Pop().ToUInt32(null));
        }

        public void Or()
        {
            CurrentState.DataStack.Push(CurrentState.DataStack.Pop().ToUInt32(null) | CurrentState.DataStack.Pop().ToUInt32(null));
        }

        public void ShiftLeft(int val)
        {
            CurrentState.DataStack.Push(CurrentState.DataStack.Pop().ToUInt32(null) << val);
        }

        public void ShiftRight(int val)
        {
            CurrentState.DataStack.Push(CurrentState.DataStack.Pop().ToUInt32(null) >> val);
        }

        public void RotateLeft(int val)
        {
            var a = CurrentState.DataStack.Pop().ToUInt32(null);
            CurrentState.DataStack.Push((a << val) | (a >> -val));
        }

        public void RotateRight(int val)
        {
            var a = CurrentState.DataStack.Pop().ToUInt32(null);
            CurrentState.DataStack.Push((a >> val) | (a << -val));
        }
    }
}
