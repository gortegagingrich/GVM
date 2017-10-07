using System;
using System.Collections;
using System.Collections.Generic;

namespace GVM
{
    // each state needs to keep track of the current data stack and a local symbol table
    internal struct MachineState
    {
        public Stack<IConvertible> DataStack;
        public Hashtable LocalSymbolTable;
        // not actually used yet
        public int ProgramCounter;
    }
}
