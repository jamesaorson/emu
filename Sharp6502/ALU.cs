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

        #region Static Methods

        public static void SetStatusFlag(ProcessorStatusFlags flag)
        {
            ProcessorStatusRegister.Value |= (byte)flag;
        }
        #endregion

        #endregion

        #region Internal

        #region Static Methods
        internal static void Initialize()
        {
            Accumulator = new Register();
            IndexRegisterX = new Register();
            IndexRegisterY = new Register();
            InstructionRegister = new Register();
            ProcessorStatusRegister = new Register();
            StackPointer = new Register();
        }
        #endregion

        #endregion

        static ALU()
        {
            Initialize();
        }
    }
}