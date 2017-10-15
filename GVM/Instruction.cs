using System;

namespace GVM
{
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

                case "store_global":
                    return 0x04;

                case "jump":
                    return 0x0A;

                case "return":
                    return 0x0B;

                case "add_int":
                    return 0x10;

                case "end":
                default:
                    return 0xFF;
            }

            return 0xFF;
        }
    }
}
