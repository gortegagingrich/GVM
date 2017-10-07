namespace GVM
{
    /* This class doesn't actually do anything and should never actually be instantiated.
         * This is only here to provide a place to implement whatever syscalls I may find necessary.
         */
    public enum Calls
    {
        AddInt32 = 0,
        SubInt32 = 1,
        MulInt32 = 2,
        DivInt32 = 3,

        AddFloat32 = 4,
        SubFloat32 = 5,
        MulFloat32 = 6,
        DivFloat32 = 7,

        PrintInt32 = 8,
        PrintFloat32 = 9,
        PrintStringAddr = 10
    }
}
