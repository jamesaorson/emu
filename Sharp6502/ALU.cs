using System.Runtime.CompilerServices;

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static byte AddToIndexRegisterX(byte value)
        {
            IndexRegisterX.Value += value;
            return IndexRegisterX.Value;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static byte BitwiseAndTransient(byte value) => (byte)(Accumulator.Value & value);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void BitwiseOr(byte value)
        {
            Accumulator.Value = BitwiseOrTransient(value);
            UpdateNegativeStatusFlag(Accumulator.Value);
            UpdateZeroStatusFlag(Accumulator.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static byte BitwiseOrTransient(byte value) => (byte)(Accumulator.Value | value);

        internal static void Initialize()
        {
            Accumulator = new Register();
            IndexRegisterX = new Register();
            IndexRegisterY = new Register();
            InstructionRegister = new Register();
            ProcessorStatusRegister = new Register();
            StackPointer = new Register();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void SetStatusFlag(ProcessorStatusFlags flag)
        {
            ProcessorStatusRegister.Value |= (byte)flag;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void UnsetStatusFlag(ProcessorStatusFlags flag)
        {
            ProcessorStatusRegister.Value &= (byte)((byte)flag ^ (byte)0xFF);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void UpdateNegativeStatusFlag(byte value)
        {
            if ((value & 0x80) != 0x00)
            {
                SetStatusFlag(ProcessorStatusFlags.Negative);
            }
            else
            {
                UnsetStatusFlag(ProcessorStatusFlags.Negative);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void UpdateZeroStatusFlag(byte value)
        {
            if (value == 0x00)
            {
                SetStatusFlag(ProcessorStatusFlags.Zero);
            }
            else
            {
                UnsetStatusFlag(ProcessorStatusFlags.Zero);
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