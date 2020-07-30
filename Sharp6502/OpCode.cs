using System;
using System.Collections.Generic;

namespace Sharp6502
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
            Action<IList<byte>> action,
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
        public void Execute(IList<byte> instructionBytes)
        {
            if (instructionBytes.Count != InstructionBytes)
            {
                throw new Exception($"{Name} ({Code}): Expected {(uint)InstructionBytes} arguments, got {instructionBytes.Count}");
            }
            _action(instructionBytes);
        }
        #endregion

        #endregion

        #region Private

        #region Members
        private readonly Action<IList<byte>> _action;
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