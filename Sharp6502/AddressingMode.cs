namespace Sharp6502
{
    public enum AddressingMode
    {
        Absolute,                 // ABS
        AbsoluteIndexedX,         // ABS, X
        AbsoluteIndexedY,         // ABS, Y
        AbsoluteIndirect,        // (ABS)
        Accumulator,             // Accum
        Immediate,               // IMM
        Implied,                 // Implied
        IndexedAbsoluteIndirect, // (ABS, X)
        IndexedIndirect,         // (IND, X)
        Indirect,                // (IND)
        IndirectIndexed,         // (IND), Y
        Relative,                // Relative
        ZeroPage,                // ZP
        ZeroPageIndexedX,        // ZP, X
        ZeroPageIndexedY,        // ZP, Y
        ZeroPageRelative,        // ZP REL
    }
}