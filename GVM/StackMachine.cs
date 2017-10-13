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
        private List<IConvertible> StaticData;

        internal static Hashtable GlobalSymbolTable = new Hashtable();

        public StackMachine()
        {
            CurrentState = new MachineState
            {
                Stack0 = new Stack<IConvertible>(),
                Stack1 = new Stack<IConvertible>(),
                LocalSymbolTable = new Hashtable(),
                ProgramCounter = 0
            };

            StaticData = new List<IConvertible>();
            PreviousStates = new Stack<MachineState>();
            SetDefaultSyscalls();
        }

        public void Reset()
        {
            CurrentState.ProgramCounter = 0;
            CurrentState.LocalSymbolTable.Clear();
            StaticData.Clear();
            PreviousStates.Clear();

            ClearPrimary();
            ClearSecondary();
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
                [Calls.PrintString] = SysCall.GetAction(Calls.PrintString)
            };
        }

        public void ExecuteInstructions(List<Instruction> list)
        {
            Reset();

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
                    foreach (IConvertible i in CurrentState.Stack0)
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
                    Jump(inst.Value.ToInt32(null));
                    break;

                case 0x0B:
                    Return();
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

                case 0xF0:
                    ClearPrimary();
                    break;

                case 0xF1:
                    ClearSecondary();
                    break;

                default:
                    Console.WriteLine("Unknown opcode: " + inst.Op);
                    break;
            }
        }

        // pop's top value from primary stack and pushes it to secondary stack
        public void PopVal()
        {
            CurrentState.Stack1.Push(CurrentState.Stack0.Pop());
        }

        // pushes given IConvertible value onto current data stack
        public void PushVal(IConvertible val)
        {
            CurrentState.Stack0.Push(val);
        }

        // performs action associated with top value on the stack
        public void Call()
        {
            var a = CurrentState.Stack0.Pop();
            ((Action)Syscalls[a])();
        }

        // dynamic data

        // looks up the value at the top of the current data stack
        // and stores it in the current local symbol table with the given key
        public void StoreVal(Object addr)
        {
            CurrentState.LocalSymbolTable[addr] = CurrentState.Stack0.Peek();
        }

        // same as StoreVal, but stores in the global symbol table
        public void StoreValGlobal(Object addr)
        {
            GlobalSymbolTable[addr] = CurrentState.Stack0.Peek();
        }

        // pushes the corresponding value from the current local symbol table onto the local data stack
        public void LoadValue(Object addr)
        {
            CurrentState.Stack0.Push((IConvertible)CurrentState.LocalSymbolTable[addr]);
        }

        // same as load value, but uses the global symbol table
        public void LoadValueGlobal(Object addr)
        {
            CurrentState.Stack0.Push((IConvertible)GlobalSymbolTable[addr]);
        }

        // static data

        // stores value on top of the stack in StaticData[key]
        public void StoreStatic(int key)
        {
            StaticData[key] = CurrentState.Stack0.Peek();
        }

        // pushes value of StaticData[key] onto stack
        public void LoadStatic(int key)
        {
            CurrentState.Stack0.Push(StaticData[key]);
        }

        // misc stack manipulations

        // duplicates the value on top of the stack
        public void Copy()
        {
            CurrentState.Stack0.Push(CurrentState.Stack0.Peek());
        }

        // swaps the top two values on the stack
        public void Swap()
        {
            var a = CurrentState.Stack0.Pop();
            var b = CurrentState.Stack0.Pop();
            CurrentState.Stack0.Push(a);
            CurrentState.Stack0.Push(b);
        }

        // pushes a copy of the second element onto the data stack
        public void Over()
        {
            var a = CurrentState.Stack0.Pop();
            var b = CurrentState.Stack0.Pop();
            CurrentState.Stack0.Push(b);
            CurrentState.Stack0.Push(a);
            CurrentState.Stack0.Push(b);
        }

        public void ClearPrimary()
        {
            CurrentState.Stack0.Clear();
        }

        public void ClearSecondary()
        {
            CurrentState.Stack1.Clear();
        }

        // switches to a new state
        // new state's main stack is previous state's secondary stack
        // new state's secondary stack is new
        // will be used for stuff like subroutines
        public void Jump(int pc)
        {
            PreviousStates.Push(CurrentState);
            CurrentState = new MachineState
            {
                Stack0 = PreviousStates.Peek().Stack1,
                Stack1 = new Stack<IConvertible>(),
                LocalSymbolTable = new Hashtable(),
                ProgramCounter = pc
            };
        }
        
        // clear's main stack and switches to previous state
        // will be used for returning from a subroutine
        public void Return()
        {
            CurrentState.Stack0.Clear();
            CurrentState = PreviousStates.Pop();
        }

        // Sets PC to given int if value on top is 0
        public void BranchZero(int pc)
        {
            if (CurrentState.Stack0.Pop().ToInt32(null) == 0)
            {
                CurrentState.ProgramCounter = pc;
            }
        }

        // Sets PC to given int if value on top is greater than 0
        public void BranchPositive(int pc)
        {
            if (CurrentState.Stack0.Pop().ToInt32(null) > 0)
            {
                CurrentState.ProgramCounter = pc;
            }
        }

        // Sets PC to given int if value on top is less than 0
        public void BranchNegative(int pc)
        {
            if (CurrentState.Stack0.Pop().ToInt32(null) < 0)
            {
                CurrentState.ProgramCounter = pc;
            }
        }

        // Int32 arithmetic
        public void AddInt()
        {
            CurrentState.Stack0.Push(CurrentState.Stack0.Pop().ToInt32(null) + CurrentState.Stack0.Pop().ToInt32(null));
        }

        public void SubInt()
        {
            var temp = CurrentState.Stack0.Pop().ToInt32(null);
            CurrentState.Stack0.Push(CurrentState.Stack0.Pop().ToInt32(null) - temp);
        }

        public void MulInt()
        {
            CurrentState.Stack0.Push(CurrentState.Stack0.Pop().ToInt32(null) * CurrentState.Stack0.Pop().ToInt32(null));
        }

        public void DivInt()
        {
            var a = CurrentState.Stack0.Pop().ToInt32(null);
            CurrentState.Stack0.Push(CurrentState.Stack0.Pop().ToInt32(null) / a);
        }

        public void ModInt()
        {
            var a = CurrentState.Stack0.Pop().ToInt32(null);
            CurrentState.Stack0.Push(CurrentState.Stack0.Pop().ToInt32(null) % a);
        }

        public void Inc()
        {
            CurrentState.Stack0.Push(CurrentState.Stack0.Pop().ToInt32(null) + 1);
        }

        public void Dec()
        {
            CurrentState.Stack0.Push(CurrentState.Stack0.Pop().ToInt32(null) - 1);
        }

        // Float32 arithmetic
        public void AddFloat()
        {
            CurrentState.Stack0.Push(CurrentState.Stack0.Pop().ToSingle(null) + CurrentState.Stack0.Pop().ToSingle(null));
        }

        public void SubFloat()
        {
            var temp = CurrentState.Stack0.Pop().ToSingle(null);
            CurrentState.Stack0.Push(CurrentState.Stack0.Pop().ToSingle(null) - temp);
        }

        public void MulFloat()
        {
            CurrentState.Stack0.Push(CurrentState.Stack0.Pop().ToSingle(null) * CurrentState.Stack0.Pop().ToSingle(null));
        }

        public void DivFloat()
        {
            var temp = CurrentState.Stack0.Pop().ToSingle(null);
            CurrentState.Stack0.Push(CurrentState.Stack0.Pop().ToSingle(null) / temp);
        }

        // bitwise operations
        public void Not()
        {
            CurrentState.Stack0.Push(~CurrentState.Stack0.Pop().ToUInt32(null));
        }

        public void And()
        {
            CurrentState.Stack0.Push(CurrentState.Stack0.Pop().ToUInt32(null) & CurrentState.Stack0.Pop().ToUInt32(null));
        }

        public void Or()
        {
            CurrentState.Stack0.Push(CurrentState.Stack0.Pop().ToUInt32(null) | CurrentState.Stack0.Pop().ToUInt32(null));
        }

        public void ShiftLeft(int val)
        {
            CurrentState.Stack0.Push(CurrentState.Stack0.Pop().ToUInt32(null) << val);
        }

        public void ShiftRight(int val)
        {
            CurrentState.Stack0.Push(CurrentState.Stack0.Pop().ToUInt32(null) >> val);
        }

        public void RotateLeft(int val)
        {
            var a = CurrentState.Stack0.Pop().ToUInt32(null);
            CurrentState.Stack0.Push((a << val) | (a >> -val));
        }

        public void RotateRight(int val)
        {
            var a = CurrentState.Stack0.Pop().ToUInt32(null);
            CurrentState.Stack0.Push((a >> val) | (a << -val));
        }
    }
}
