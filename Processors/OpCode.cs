namespace Sharp6502.Processors
{
    public readonly struct OpCode
    {
        #region Public

        #region Constructors
        internal OpCode(
            byte code,
            string name,
            AddressingMode mode,
            byte instructionBytes,
            byte cycles,
            byte flags = 0x00
        )
        {
            Code = code;
            Name = name;
            Mode = mode;
            InstructionBytes = instructionBytes;
            Cycles = cycles;
            Flags = flags;
        }
        #endregion

        #region Members
        public readonly string Name;
        public readonly byte Code;
        public readonly AddressingMode Mode;
        public readonly byte InstructionBytes;
        public readonly byte Cycles;
        public readonly byte Flags;
        #endregion

        #endregion
    }

    public enum OpCodeFlags : byte
    {
        Add1IfDecimal = 0x01,
        Add1IfPageBoundaryCrossed = 0x02,
        Branch = 0x04,
    }
}