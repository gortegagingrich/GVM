using System;
using System.Collections;
using System.Collections.Generic;

namespace GVM
{
    // each state needs to keep track of the current data stacks and a local symbol table
    internal struct MachineState
    {
        internal Stack<IConvertible> Stack0; // primary data stack
        internal Stack<IConvertible> Stack1; // secondary data stack used for arguments
        internal Hashtable LocalSymbolTable; // used for local variables
        internal int ProgramCounter; // instruction index for current frame
    }
}
