namespace Sharp6502
{
    public static class ALU
    {
        #region Public

        #region Static Fields
        public static Register Accumulator { get; private set; }
        public static Register IndexRegisterX { get; private set; }
        public static Register IndexRegisterY { get; private set; }
        public static Register InstructionRegister { get; private set; }
        public static Register ProcessorStatusRegister { get; private set; }
        public static Register StackPointer { get; private set; }
        #endregion

        #endregion

        #region Internal

        #region Static Methods
        internal static void BitwiseOr(byte value)
        {
            Accumulator.Value |= (byte)value;
            UpdateNegativeStatusFlag(Accumulator.Value);
            UpdateZeroStatusFlag(Accumulator.Value);
        }

        internal static void Initialize()
        {
            Accumulator = new Register();
            IndexRegisterX = new Register();
            IndexRegisterY = new Register();
            InstructionRegister = new Register();
            ProcessorStatusRegister = new Register();
            StackPointer = new Register();
        }

        internal static void SetStatusFlag(ProcessorStatusFlags flag)
        {
            ProcessorStatusRegister.Value |= (byte)flag;
        }

        internal static void UpdateNegativeStatusFlag(byte value)
        {
            if ((value & 0x80) != 0x00)
            {
                SetStatusFlag(ProcessorStatusFlags.Negative);
            }
        }

        internal static void UpdateZeroStatusFlag(byte value)
        {
            if (value == 0x00)
            {
                SetStatusFlag(ProcessorStatusFlags.Zero);
            }
        }
        #endregion

        #endregion

        static ALU()
        {
            Initialize();
        }
    }
}