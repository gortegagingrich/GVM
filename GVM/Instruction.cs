using System;

namespace GVM
{
    [Serializable]
    internal struct Instruction
    {
        public Byte Op;
        public IConvertible Value;

        public Instruction(Byte op, IConvertible val)
        {
            /* Each instruction contains two parts: a 1 byte opcode and an IConvertible value that can be null
             * 
             * Overview of opcodes:
             * 0x00 = PushVal(Value)
             * 0x01 = PopVal
             * 0x02 = Call
             * 0x03 = StoreVal(Value)
             * 0x04 = StoreValGlobal(Value)
             * 0x05 = LoadValue(Value)
             * 0x06 = LoadValueGlobal(Value)
             * 0x07 = Copy()
             * 0x08 = Swap()
             * 0x09 = Over()
             * 
             * 0x0A = Jump(Value)
             * 0x0B = Return()
             * 0x0C = BEZ(Value)
             * 0x0D = BLZ(Value)
             * 0x0E = BGZ(Value)
             * 
             * 0x10 = AddInt()
             * 0x11 = SubInt()
             * 0x12 = MulInt()
             * 0x13 = DivInt()
             * 0x14 = AddFloat()
             * 0x15 = SubFloat()
             * 0x16 = MulFloat()
             * 0x17 = DivFloat()
             * 
             * 0x18 = ModInt()
             * 0x19 = Inc()
             * 0x1A = Dec()
             * 
             * 0x20 = Not()
             * 0x21 = And()
             * 0x22 = Or()
             * 0x23 = ShiftLeft(Value)
             * 0x24 = ShiftRight(Value)
             * 0x25 = RotateLeft(Value)
             * 0x26 = RotateRight(Value)
             * 
             * 0x30 = StoreStatic(Value)
             * 0x31 = LoadStatic(Value)
             * 
             * // use top value on stack as address
             * 0x40 = StoreLocal()
             * 0x41 = StoreGlobal()
             * 0x42 = LoadLocal()
             * 0x43 = LoadGlobal()
             * 
             * 0xF0 = ClearPrimary()
             * 0xF1 = ClearSecondary()
             * 0xFF = END
             */
            Op = op;
            Value = val;
        }

        public static Byte StringToOp(string str)
        {
            switch (str)
            {
                case "push":
                    return 0x00;

                case "pop":
                    return 0x01;

                case "call":
                    return 0x02;

                case "store_local":
                    return 0x03;

                case "store_global":
                    return 0x04;

                case "load_local":
                    return 0x05;

                case "load_global":
                    return 0x06;

                case "copy":
                    return 0x07;

                case "swap":
                    return 0x08;

                case "over":
                    return 0x09;

                case "jump":
                    return 0x0A;

                case "return":
                    return 0x0B;

                case "bez":
                    return 0x0C;

                case "blz":
                    return 0x0D;

                case "bgz":
                    return 0x0E;

                case "add_int":
                    return 0x10;

                case "sub_int":
                    return 0x11;

                case "mul_int":
                    return 0x12;

                case "div_int":
                    return 0x13;

                case "add_float":
                    return 0x14;

                case "sub_float":
                    return 0x15;

                case "mul_float":
                    return 0x16;

                case "div_float":
                    return 0x17;

                case "mod":
                    return 0x18;

                case "inc":
                    return 0x19;

                case "dec":
                    return 0x1A;

                case "not":
                    return 0x20;

                case "and":
                    return 0x21;

                case "or":
                    return 0x22;

                case "shift_left":
                    return 0x23;

                case "shift_right":
                    return 0x24;

                case "rot_left":
                    return 0x25;

                case "rot_right":
                    return 0x26;

                case "store_static":
                    return 0x30;

                case "load_static":
                    return 0x31;

                case "load_local_stack":
                    return 0x40;

                case "load_global_stack":
                    return 0x41;

                case "store_local_stack":
                    return 0x42;

                case "store_global_stack":
                    return 0x43;

                case "clear_primary":
                    return 0xF0;

                case "clear_secondary":
                    return 0xF1;

                case "end":
                default:
                    return 0xFF;
            }
        }
    }
}
