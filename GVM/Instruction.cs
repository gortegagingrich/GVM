﻿using System;

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
             * 0x13 = DivInt() // sets GlobalSymbolTable["Remainder"] to remainder
             * 0x14 = AddFloat()
             * 0x15 = SubFloat()
             * 0x16 = MulFloat()
             * 0x17 = DivFloat()
             * 0x18 = Inc() NOT IMPLEMENTED
             * 0x19 = Dec() NOT IMPLEMENTED
             * 
             * 0x20 = Not() NOT IMPLEMENTED
             * 0x21 = And() NOT IMPLEMENTED
             * 0x22 = Or() NOT IMPLEMENTED
             * 0x23 = ShiftLeft(Value) NOT IMPLEMENTED
             * 0x24 = ShiftRight(Value) NOT IMPLEMENTED
             * 0x25 = RotateLeft(Value) NOT IMPLEMENTED
             * 0x26 = RotateRight(Value) NOT IMPLEMENTED
             * 
             * 0xFF = END
             */
            Op = op;
            Value = val;
        }
    }
}
