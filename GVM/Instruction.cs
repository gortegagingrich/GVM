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
             * 0x0A = PushState(Value)
             * 0x0B = PopState()
             * 0x0C = If(Value)
             */
            Op = op;
            Value = val;
        }
    }
}