using System;
using System.Collections;
using System.Collections.Generic;

namespace GVM
{
    // each state needs to keep track of the current data stacks and a local symbol table
    internal struct MachineState
    {
        public Stack<IConvertible> Stack0; // primary data stack
        public Stack<IConvertible> Stack1; // secondary data stack used for arguments
        public Hashtable LocalSymbolTable;
        // not actually used yet
        public int ProgramCounter;
    }
}
