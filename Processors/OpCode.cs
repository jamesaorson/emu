using System;

namespace Sharp6502.Processors
{
    public class OpCode
    {
        #region Public

        #region Constructors
        internal OpCode(
            byte code,
            string name,
            AddressingMode mode,
            byte instructionBytes,
            byte cycles,
            Action<byte[]> action,
            byte flags = 0x00
        )
        {
            if (action == null)
            {
                throw new Exception("OpCode action must be non-null");
            }
            _action = action;

            Code = code;
            Cycles = cycles;
            Flags = flags;
            InstructionBytes = instructionBytes;
            Mode = mode;
            Name = name;
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

        #region Member Methods
        public void Execute(params byte[] instructionBytes)
        {
            if (instructionBytes.Length != InstructionBytes)
            {
                throw new Exception($"{Name} ({Code}): Expected {(uint)InstructionBytes} arguments, got {instructionBytes.Length}");
            }
            _action(instructionBytes);
        }
        #endregion

        #endregion

        #region Private

        #region Members
        private readonly Action<byte[]> _action;
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