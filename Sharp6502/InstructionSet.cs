using System.Collections.Generic;
using System.Linq;

namespace Sharp6502
{
    internal static class InstructionSet
    {
        #region Public

        #region Static Members
        public static readonly IDictionary<byte, OpCode> OpCodeMap;
        #endregion

        #region Static Member Methods
        public static OpCode ConvertToOpCode(byte opCode)
        {
            if (!OpCodeMap.TryGetValue(opCode, out OpCode outValue))
            {
                return null;
            }
            return outValue;
        }

        public static OpCode ConvertToOpCode(string opName) => OpCodeMap.Values.Where(opCode => opCode.Name == opName)?.ToList().FirstOrDefault();
        #endregion

        #region Constants
        public const string ADC = "ADC";
        public const string AND = "AND";
        public const string ASL = "ASL";
        public const string BBR0 = "BBR0";
        public const string BBR1 = "BBR1";
        public const string BBR2 = "BBR2";
        public const string BBR3 = "BBR3";
        public const string BBR4 = "BBR4";
        public const string BBR5 = "BBR5";
        public const string BBR6 = "BBR6";
        public const string BBR7 = "BBR7";
        public const string BBS0 = "BBS0";
        public const string BBS1 = "BBS1";
        public const string BBS2 = "BBS2";
        public const string BBS3 = "BBS3";
        public const string BBS4 = "BBS4";
        public const string BBS5 = "BBS5";
        public const string BBS6 = "BBS6";
        public const string BBS7 = "BBS7";
        public const string BCC = "BCC";
        public const string BCS = "BCS";
        public const string BEQ = "BEQ";
        public const string BIT = "BIT";
        public const string BMI = "BMI";
        public const string BNE = "BNE";
        public const string BPL = "BPL";
        public const string BRA = "BRA";
        public const string BRK = "BRK";
        public const string BVC = "BVC";
        public const string BVS = "BVS";
        public const string CLC = "CLC";
        public const string CLD = "CLD";
        public const string CLI = "CLI";
        public const string CLV = "CLV";
        public const string CMP = "CMP";
        public const string CPX = "CPX";
        public const string CPY = "CPY";
        public const string DEC = "DEC";
        public const string DEX = "DEX";
        public const string DEY = "DEY";
        public const string EOR = "EOR";
        public const string INC = "INC";
        public const string INX = "INX";
        public const string INY = "INY";
        public const string JMP = "JMP";
        public const string JSR = "JSR";
        public const string LDA = "LDA";
        public const string LDX = "LDX";
        public const string LDY = "LDY";
        public const string LSR = "LSR";
        public const string NOP = "NOP";
        public const string ORA = "ORA";
        public const string PHA = "PHA";
        public const string PHP = "PHP";
        public const string PHX = "PHX";
        public const string PHY = "PHY";
        public const string PLA = "PLA";
        public const string PLP = "PLP";
        public const string PLX = "PLX";
        public const string PLY = "PLY";
        public const string RMB0 = "RMB0";
        public const string RMB1 = "RMB1";
        public const string RMB2 = "RMB2";
        public const string RMB3 = "RMB3";
        public const string RMB4 = "RMB4";
        public const string RMB5 = "RMB5";
        public const string RMB6 = "RMB6";
        public const string RMB7 = "RMB7";
        public const string ROL = "ROL";
        public const string ROR = "ROR";
        public const string RTI = "RTI";
        public const string RTS = "RTS";
        public const string SBC = "SBC";
        public const string SEC = "SEC";
        public const string SED = "SED";
        public const string SEI = "SEI";
        public const string SMB0 = "SMB0";
        public const string SMB1 = "SMB1";
        public const string SMB2 = "SMB2";
        public const string SMB3 = "SMB3";
        public const string SMB4 = "SMB4";
        public const string SMB5 = "SMB5";
        public const string SMB6 = "SMB6";
        public const string SMB7 = "SMB7";
        public const string STA = "STA";
        public const string STX = "STX";
        public const string STY = "STY";
        public const string STZ = "STZ";
        public const string TAX = "TAX";
        public const string TAY = "TAY";
        public const string TRB = "TRB";
        public const string TSB = "TSB";
        public const string TSX = "TSX";
        public const string TXA = "TXA";
        public const string TXS = "TXS";
        public const string TYA = "TYA";
        #endregion

        #endregion

        static InstructionSet()
        {
            OpCodeMap = new Dictionary<byte, OpCode>
            {
                [0x00] = new OpCode(
                    0x00,
                    BRK,
                    AddressingMode.Implied,
                     // BRK is documented as a 1 byte opcode, but it really
                     // exists as a 1 byte opcode with a padding byte.
                     // Interrupt routines called by BRK always return 2 bytes,
                     // so treating BRK as a 2 byte opcode resolves this issue.
                    instructionBytes: 2,
                    cycles: 7,
                    command: (instructionBytes) => {
                        CPU.SetStatusFlag(ProcessorStatusFlags.B);
                        // TODO: RTI
                    }
                ),
                [0x01] = new OpCode(
                    0x01,
                    ORA,
                    AddressingMode.IndexedIndirect,
                    instructionBytes: 2,
                    cycles: 6,
                    command: (instructionBytes) => {}
                ),
                [0x04] = new OpCode(
                    0x04,
                    TSB,
                    AddressingMode.ZeroPage,
                    instructionBytes: 2,
                    cycles: 5,
                    command: (instructionBytes) => {}
                ),
                [0x05] = new OpCode(
                    0x05,
                    ORA,
                    AddressingMode.ZeroPage,
                    instructionBytes: 2,
                    cycles: 3,
                    command: (instructionBytes) => {}
                ),
                [0x06] = new OpCode(
                    0x06,
                    ASL,
                    AddressingMode.ZeroPage,
                    instructionBytes: 2,
                    cycles: 5,
                    command: (instructionBytes) => {}
                ),
                [0x07] = new OpCode(
                    0x07,
                    RMB0,
                    AddressingMode.ZeroPage,
                    instructionBytes: 2,
                    cycles: 5,
                    command: (instructionBytes) => {}
                ),
                [0x08] = new OpCode(
                    0x08,
                    PHP,
                    AddressingMode.Implied,
                    instructionBytes: 1,
                    cycles: 3,
                    command: (instructionBytes) => {}
                ),
                [0x09] = new OpCode(
                    0x09,
                    ORA,
                    AddressingMode.Immediate,
                    instructionBytes: 2,
                    cycles: 2,
                    command: (instructionBytes) => {}
                ),
                [0x0A] = new OpCode(
                    0x0A,
                    ASL,
                    AddressingMode.Accumulator,
                    instructionBytes: 1,
                    cycles: 2,
                    command: (instructionBytes) => {}
                ),
                [0x0C] = new OpCode(
                    0x0C,
                    TSB,
                    AddressingMode.Absolute,
                    instructionBytes: 3,
                    cycles: 6,
                    command: (instructionBytes) => {}
                ),
                [0x0D] = new OpCode(
                    0x0D,
                    ORA,
                    AddressingMode.Absolute,
                    instructionBytes: 3,
                    cycles: 4,
                    command: (instructionBytes) => {}
                ),
                [0x0E] = new OpCode(
                    0x0E,
                    ASL,
                    AddressingMode.Absolute,
                    instructionBytes: 3,
                    cycles: 6,
                    command: (instructionBytes) => {}
                ),
                [0x0F] = new OpCode(
                    0x0F,
                    BBR0,
                    AddressingMode.ZeroPageRelative,
                    instructionBytes: 3,
                    cycles: 5,
                    command: (instructionBytes) => {},
                    flags: (byte)OpCodeFlags.Branch
                ),
                
                [0x10] = new OpCode(0x10, BPL, AddressingMode.Relative, instructionBytes: 2, cycles: 2, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Branch),
                [0x11] = new OpCode(0x11, ORA, AddressingMode.IndirectIndexed, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfPageBoundaryCrossed),
                [0x12] = new OpCode(0x12, ORA, AddressingMode.Indirect, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}),
                [0x14] = new OpCode(0x14, TRB, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}),
                [0x15] = new OpCode(0x15, ORA, AddressingMode.ZeroPageIndexedX, instructionBytes: 2, cycles: 4, command: (instructionBytes) => {}),
                [0x16] = new OpCode(0x16, ASL, AddressingMode.ZeroPageIndexedX, instructionBytes: 2, cycles: 6, command: (instructionBytes) => {}),
                [0x17] = new OpCode(0x17, RMB1, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}),
                [0x18] = new OpCode(0x18, CLC, AddressingMode.Implied, instructionBytes: 1, cycles: 2, command: (instructionBytes) => {}),
                [0x19] = new OpCode(0x19, ORA, AddressingMode.AbsoluteIndexedY, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfPageBoundaryCrossed),
                [0x1A] = new OpCode(0x1A, INC, AddressingMode.Accumulator, instructionBytes: 1, cycles: 2, command: (instructionBytes) => {}),
                [0x1C] = new OpCode(0x1C, TRB, AddressingMode.Absolute, instructionBytes: 3, cycles: 6, command: (instructionBytes) => {}),
                [0x1D] = new OpCode(0x1D, ORA, AddressingMode.AbsoluteIndexedX, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfPageBoundaryCrossed),
                [0x1E] = new OpCode(0x1E, ASL, AddressingMode.AbsoluteIndexedX, instructionBytes: 3, cycles: 7, command: (instructionBytes) => {}),
                [0x1F] = new OpCode(0x1F, BBR1, AddressingMode.ZeroPageRelative, instructionBytes: 3, cycles: 5, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Branch),
                
                [0x20] = new OpCode(0x20, JSR, AddressingMode.Absolute, instructionBytes: 3, cycles: 6, command: (instructionBytes) => {}),
                [0x21] = new OpCode(0x21, AND, AddressingMode.IndexedIndirect, instructionBytes: 2, cycles: 6, command: (instructionBytes) => {}),
                [0x24] = new OpCode(0x24, BIT, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 3, command: (instructionBytes) => {}),
                [0x25] = new OpCode(0x25, AND, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 3, command: (instructionBytes) => {}),
                [0x26] = new OpCode(0x26, ROL, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}),
                [0x27] = new OpCode(0x27, RMB2, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}),
                [0x28] = new OpCode(0x28, PLP, AddressingMode.Implied, instructionBytes: 1, cycles: 4, command: (instructionBytes) => {}),
                [0x29] = new OpCode(0x29, AND, AddressingMode.Immediate, instructionBytes: 2, cycles: 2, command: (instructionBytes) => {}),
                [0x2A] = new OpCode(0x2A, ROL, AddressingMode.Accumulator, instructionBytes: 1, cycles: 2, command: (instructionBytes) => {}),
                [0x2C] = new OpCode(0x2C, BIT, AddressingMode.Absolute, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}),
                [0x2D] = new OpCode(0x2D, AND, AddressingMode.Absolute, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}),
                [0x2E] = new OpCode(0x2E, ROL, AddressingMode.Absolute, instructionBytes: 3, cycles: 6, command: (instructionBytes) => {}),
                [0x2F] = new OpCode(0x2F, BBR2, AddressingMode.ZeroPageRelative, instructionBytes: 3, cycles: 5, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Branch),
                
                [0x30] = new OpCode(0x30, BMI, AddressingMode.Relative, instructionBytes: 2, cycles: 2, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Branch),
                [0x31] = new OpCode(0x31, AND, AddressingMode.IndirectIndexed, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfPageBoundaryCrossed),
                [0x32] = new OpCode(0x32, AND, AddressingMode.Indirect, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}),
                [0x34] = new OpCode(0x34, BIT, AddressingMode.ZeroPageIndexedX, instructionBytes: 2, cycles: 4, command: (instructionBytes) => {}),
                [0x35] = new OpCode(0x35, AND, AddressingMode.ZeroPageIndexedX, instructionBytes: 2, cycles: 4, command: (instructionBytes) => {}),
                [0x36] = new OpCode(0x36, ROL, AddressingMode.ZeroPageIndexedX, instructionBytes: 2, cycles: 6, command: (instructionBytes) => {}),
                [0x37] = new OpCode(0x37, RMB3, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}),
                [0x38] = new OpCode(0x38, SEC, AddressingMode.Implied, instructionBytes: 1, cycles: 2, command: (instructionBytes) => {}),
                [0x39] = new OpCode(0x39, AND, AddressingMode.AbsoluteIndexedY, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfPageBoundaryCrossed),
                [0x3A] = new OpCode(0x3A, DEC, AddressingMode.Accumulator, instructionBytes: 1, cycles: 2, command: (instructionBytes) => {}),
                [0x3C] = new OpCode(0x3C, BIT, AddressingMode.AbsoluteIndexedX, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfPageBoundaryCrossed),
                [0x3D] = new OpCode(0x3D, AND, AddressingMode.AbsoluteIndexedX, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfPageBoundaryCrossed),
                [0x3E] = new OpCode(0x3E, ROL, AddressingMode.AbsoluteIndexedX, instructionBytes: 3, cycles: 7, command: (instructionBytes) => {}),
                [0x3F] = new OpCode(0x3F, BBR3, AddressingMode.ZeroPageRelative, instructionBytes: 3, cycles: 5, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Branch),
                
                [0x40] = new OpCode(0x40, RTI, AddressingMode.Implied, instructionBytes: 1, cycles: 6, command: (instructionBytes) => {}),
                [0x41] = new OpCode(0x41, EOR, AddressingMode.IndexedIndirect, instructionBytes: 2, cycles: 6, command: (instructionBytes) => {}),
                [0x45] = new OpCode(0x45, EOR, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 3, command: (instructionBytes) => {}),
                [0x46] = new OpCode(0x46, LSR, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}),
                [0x47] = new OpCode(0x47, RMB4, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}),
                [0x48] = new OpCode(0x48, PHA, AddressingMode.Implied, instructionBytes: 1, cycles: 3, command: (instructionBytes) => {}),
                [0x49] = new OpCode(0x49, EOR, AddressingMode.Immediate, instructionBytes: 2, cycles: 2, command: (instructionBytes) => {}),
                [0x4A] = new OpCode(0x4A, LSR, AddressingMode.Accumulator, instructionBytes: 1, cycles: 2, command: (instructionBytes) => {}),
                [0x4C] = new OpCode(0x4C, JMP, AddressingMode.Absolute, instructionBytes: 3, cycles: 3, command: (instructionBytes) => {}),
                [0x4D] = new OpCode(0x4D, EOR, AddressingMode.Absolute, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}),
                [0x4E] = new OpCode(0x4E, LSR, AddressingMode.Absolute, instructionBytes: 3, cycles: 6, command: (instructionBytes) => {}),
                [0x4F] = new OpCode(0x4F, BBR4, AddressingMode.ZeroPageRelative, instructionBytes: 3, cycles: 5, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Branch),
                
                [0x50] = new OpCode(0x50, BVC, AddressingMode.Relative, instructionBytes: 2, cycles: 2, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Branch),
                [0x51] = new OpCode(0x51, EOR, AddressingMode.IndirectIndexed, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfPageBoundaryCrossed),
                [0x52] = new OpCode(0x52, EOR, AddressingMode.Indirect, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}),
                [0x55] = new OpCode(0x55, EOR, AddressingMode.ZeroPageIndexedX, instructionBytes: 2, cycles: 4, command: (instructionBytes) => {}),
                [0x56] = new OpCode(0x56, LSR, AddressingMode.ZeroPageIndexedX, instructionBytes: 2, cycles: 6, command: (instructionBytes) => {}),
                [0x57] = new OpCode(0x57, RMB5, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}),
                [0x58] = new OpCode(0x58, CLI, AddressingMode.Implied, instructionBytes: 1, cycles: 2, command: (instructionBytes) => {}),
                [0x59] = new OpCode(0x59, EOR, AddressingMode.AbsoluteIndexedY, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfPageBoundaryCrossed),
                [0x5A] = new OpCode(0x5A, PHY, AddressingMode.Implied, instructionBytes: 1, cycles: 3, command: (instructionBytes) => {}),
                [0x5D] = new OpCode(0x5D, EOR, AddressingMode.AbsoluteIndexedX, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfPageBoundaryCrossed),
                [0x5E] = new OpCode(0x5E, LSR, AddressingMode.AbsoluteIndexedX, instructionBytes: 3, cycles: 7, command: (instructionBytes) => {}),
                [0x5F] = new OpCode(0x5F, BBR5, AddressingMode.ZeroPageRelative, instructionBytes: 3, cycles: 5, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Branch),
                
                [0x60] = new OpCode(0x60, RTS, AddressingMode.Implied, instructionBytes: 1, cycles: 6, command: (instructionBytes) => {}),
                [0x61] = new OpCode(0x61, ADC, AddressingMode.IndexedIndirect, instructionBytes: 2, cycles: 6, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfDecimal),
                [0x64] = new OpCode(0x64, STZ, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 3, command: (instructionBytes) => {}),
                [0x65] = new OpCode(0x65, ADC, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 3, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfDecimal),
                [0x66] = new OpCode(0x66, ROR, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}),
                [0x67] = new OpCode(0x67, RMB6, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}),
                [0x68] = new OpCode(0x68, PLA, AddressingMode.Implied, instructionBytes: 1, cycles: 4, command: (instructionBytes) => {}),
                [0x69] = new OpCode(0x69, ADC, AddressingMode.Immediate, instructionBytes: 2, cycles: 2, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfDecimal),
                [0x6A] = new OpCode(0x6A, ROR, AddressingMode.Accumulator, instructionBytes: 1, cycles: 2, command: (instructionBytes) => {}),
                [0x6C] = new OpCode(0x6C, JMP, AddressingMode.AbsoluteIndirect, instructionBytes: 3, cycles: 6, command: (instructionBytes) => {}),
                [0x6D] = new OpCode(0x6D, ADC, AddressingMode.Absolute, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfDecimal),
                [0x6E] = new OpCode(0x6E, ROR, AddressingMode.Absolute, instructionBytes: 3, cycles: 6, command: (instructionBytes) => {}),
                [0x6F] = new OpCode(0x6F, BBR7, AddressingMode.ZeroPageRelative, instructionBytes: 3, cycles: 5, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Branch),
                
                [0x70] = new OpCode(0x70, BVS, AddressingMode.Relative, instructionBytes: 2, cycles: 3, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfPageBoundaryCrossed),
                [0x71] = new OpCode(0x71, ADC, AddressingMode.IndirectIndexed, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfDecimal | (byte)OpCodeFlags.Add1IfPageBoundaryCrossed),
                [0x72] = new OpCode(0x72, ADC, AddressingMode.Indirect, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfDecimal),
                [0x74] = new OpCode(0x74, STZ, AddressingMode.ZeroPageIndexedX, instructionBytes: 2, cycles: 4, command: (instructionBytes) => {}),
                [0x75] = new OpCode(0x75, ADC, AddressingMode.ZeroPageIndexedX, instructionBytes: 2, cycles: 4, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfDecimal),
                [0x76] = new OpCode(0x76, ROR, AddressingMode.ZeroPageIndexedX, instructionBytes: 2, cycles: 6, command: (instructionBytes) => {}),
                [0x77] = new OpCode(0x77, RMB7, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}),
                [0x78] = new OpCode(0x78, SEI, AddressingMode.Implied, instructionBytes: 1, cycles: 2, command: (instructionBytes) => {}),
                [0x79] = new OpCode(0x79, ADC, AddressingMode.AbsoluteIndexedY, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfDecimal | (byte)OpCodeFlags.Add1IfPageBoundaryCrossed),
                [0x7A] = new OpCode(0x7A, PLY, AddressingMode.Implied, instructionBytes: 1, cycles: 4, command: (instructionBytes) => {}),
                [0x7C] = new OpCode(0x7C, JMP, AddressingMode.IndexedAbsoluteIndirect, instructionBytes: 3, cycles: 6, command: (instructionBytes) => {}),
                [0x7D] = new OpCode(0x7D, ADC, AddressingMode.AbsoluteIndexedX, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfDecimal | (byte)OpCodeFlags.Add1IfPageBoundaryCrossed),
                [0x7E] = new OpCode(0x7E, ROR, AddressingMode.AbsoluteIndexedX, instructionBytes: 3, cycles: 7, command: (instructionBytes) => {}),
                [0x7F] = new OpCode(0x7F, BBR7, AddressingMode.ZeroPageRelative, instructionBytes: 3, cycles: 5, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Branch),
                
                [0x80] = new OpCode(0x80, BRA, AddressingMode.Relative, instructionBytes: 2, cycles: 3, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfPageBoundaryCrossed),
                [0x81] = new OpCode(0x81, STA, AddressingMode.IndexedIndirect, instructionBytes: 2, cycles: 6, command: (instructionBytes) => {}),
                [0x84] = new OpCode(0x84, STY, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 3, command: (instructionBytes) => {}),
                [0x85] = new OpCode(0x85, STA, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 3, command: (instructionBytes) => {}),
                [0x86] = new OpCode(0x86, STX, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 3, command: (instructionBytes) => {}),
                [0x87] = new OpCode(0x87, SMB0, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}),
                [0x88] = new OpCode(0x88, DEY, AddressingMode.Implied, instructionBytes: 1, cycles: 2, command: (instructionBytes) => {}),
                [0x89] = new OpCode(0x89, BIT, AddressingMode.Immediate, instructionBytes: 2, cycles: 2, command: (instructionBytes) => {}),
                [0x8A] = new OpCode(0x8A, TXA, AddressingMode.Implied, instructionBytes: 1, cycles: 2, command: (instructionBytes) => {}),
                [0x8C] = new OpCode(0x8C, STY, AddressingMode.Absolute, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}),
                [0x8D] = new OpCode(0x8D, STA, AddressingMode.Absolute, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}),
                [0x8E] = new OpCode(0x8E, STX, AddressingMode.Absolute, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}),
                [0x8F] = new OpCode(0x8F, BBS0, AddressingMode.ZeroPageRelative, instructionBytes: 3, cycles: 5, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Branch),
                
                [0x90] = new OpCode(0x90, BCC, AddressingMode.Relative, instructionBytes: 2, cycles: 2, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Branch),
                [0x91] = new OpCode(0x91, STA, AddressingMode.IndirectIndexed, instructionBytes: 2, cycles: 6, command: (instructionBytes) => {}),
                [0x92] = new OpCode(0x92, STA, AddressingMode.Indirect, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}),
                [0x94] = new OpCode(0x94, STY, AddressingMode.ZeroPageIndexedX, instructionBytes: 2, cycles: 4, command: (instructionBytes) => {}),
                [0x95] = new OpCode(0x95, STA, AddressingMode.ZeroPageIndexedX, instructionBytes: 2, cycles: 4, command: (instructionBytes) => {}),
                [0x96] = new OpCode(0x96, STX, AddressingMode.ZeroPageIndexedY, instructionBytes: 2, cycles: 4, command: (instructionBytes) => {}),
                [0x97] = new OpCode(0x97, SMB1, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}),
                [0x98] = new OpCode(0x98, TYA, AddressingMode.Implied, instructionBytes: 1, cycles: 2, command: (instructionBytes) => {}),
                [0x99] = new OpCode(0x99, STA, AddressingMode.AbsoluteIndexedY, instructionBytes: 3, cycles: 5, command: (instructionBytes) => {}),
                [0x9A] = new OpCode(0x9A, TXS, AddressingMode.Implied, instructionBytes: 1, cycles: 2, command: (instructionBytes) => {}),
                [0x9C] = new OpCode(0x9C, STZ, AddressingMode.Absolute, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}),
                [0x9D] = new OpCode(0x9D, STA, AddressingMode.AbsoluteIndexedX, instructionBytes: 3, cycles: 5, command: (instructionBytes) => {}),
                [0x9E] = new OpCode(0x9E, STZ, AddressingMode.AbsoluteIndexedX, instructionBytes: 3, cycles: 5, command: (instructionBytes) => {}),
                [0x9F] = new OpCode(0x9F, BBS1, AddressingMode.ZeroPageRelative, instructionBytes: 3, cycles: 5, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Branch),
                
                [0xA0] = new OpCode(0xA0, LDY, AddressingMode.Immediate, instructionBytes: 2, cycles: 2, command: (instructionBytes) => {}),
                [0xA1] = new OpCode(0xA1, LDA, AddressingMode.IndexedIndirect, instructionBytes: 2, cycles: 6, command: (instructionBytes) => {}),
                [0xA2] = new OpCode(0xA2, LDX, AddressingMode.Immediate, instructionBytes: 2, cycles: 2, command: (instructionBytes) => {}),
                [0xA4] = new OpCode(0xA4, LDY, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 3, command: (instructionBytes) => {}),
                [0xA5] = new OpCode(0xA5, LDA, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 3, command: (instructionBytes) => {}),
                [0xA6] = new OpCode(0xA6, LDX, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 3, command: (instructionBytes) => {}),
                [0xA7] = new OpCode(0xA7, SMB2, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}),
                [0xA8] = new OpCode(0xA8, TAY, AddressingMode.Implied, instructionBytes: 1, cycles: 2, command: (instructionBytes) => {}),
                [0xA9] = new OpCode(0xA9, LDA, AddressingMode.Immediate, instructionBytes: 2, cycles: 2, command: (instructionBytes) => {}),
                [0xAA] = new OpCode(0xAA, TAX, AddressingMode.Implied, instructionBytes: 1, cycles: 2, command: (instructionBytes) => {}),
                [0xAC] = new OpCode(0xAC, LDY, AddressingMode.Absolute, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}),
                [0xAD] = new OpCode(0xAD, LDA, AddressingMode.Absolute, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}),
                [0xAE] = new OpCode(0xAE, LDX, AddressingMode.Absolute, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}),
                [0xAF] = new OpCode(0xAF, BBS2, AddressingMode.ZeroPageRelative, instructionBytes: 3, cycles: 5, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Branch),
                
                [0xB0] = new OpCode(0xB0, BCS, AddressingMode.Relative, instructionBytes: 2, cycles: 2, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Branch),
                [0xB1] = new OpCode(0xB1, LDA, AddressingMode.IndirectIndexed, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfPageBoundaryCrossed),
                [0xB2] = new OpCode(0xB2, LDA, AddressingMode.Indirect, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}),
                [0xB4] = new OpCode(0xB4, LDY, AddressingMode.ZeroPageIndexedX, instructionBytes: 2, cycles: 4, command: (instructionBytes) => {}),
                [0xB5] = new OpCode(0xB5, LDA, AddressingMode.ZeroPageIndexedX, instructionBytes: 2, cycles: 4, command: (instructionBytes) => {}),
                [0xB6] = new OpCode(0xB6, LDX, AddressingMode.ZeroPageIndexedY, instructionBytes: 2, cycles: 4, command: (instructionBytes) => {}),
                [0xB7] = new OpCode(0xB7, SMB3, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}),
                [0xB8] = new OpCode(0xB8, CLV, AddressingMode.Implied, instructionBytes: 1, cycles: 2, command: (instructionBytes) => {}),
                [0xB9] = new OpCode(0xB9, LDA, AddressingMode.AbsoluteIndexedY, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfPageBoundaryCrossed),
                [0xBA] = new OpCode(0xBA, TSX, AddressingMode.Implied, instructionBytes: 1, cycles: 2, command: (instructionBytes) => {}),
                [0xBC] = new OpCode(0xBC, LDY, AddressingMode.AbsoluteIndexedX, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfPageBoundaryCrossed),
                [0xBD] = new OpCode(0xBD, LDA, AddressingMode.AbsoluteIndexedX, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfPageBoundaryCrossed),
                [0xBE] = new OpCode(0xBE, LDX, AddressingMode.AbsoluteIndexedY, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfPageBoundaryCrossed),
                [0xBF] = new OpCode(0xBF, BBS3, AddressingMode.ZeroPageRelative, instructionBytes: 3, cycles: 5, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Branch),
                
                [0xC0] = new OpCode(0xC0, CPY, AddressingMode.Immediate, instructionBytes: 2, cycles: 2, command: (instructionBytes) => {}),
                [0xC1] = new OpCode(0xC1, CMP, AddressingMode.IndexedIndirect, instructionBytes: 2, cycles: 6, command: (instructionBytes) => {}),
                [0xC4] = new OpCode(0xC4, CPY, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 3, command: (instructionBytes) => {}),
                [0xC5] = new OpCode(0xC5, CMP, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 3, command: (instructionBytes) => {}),
                [0xC6] = new OpCode(0xC6, DEC, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}),
                [0xC7] = new OpCode(0xC7, SMB4, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}),
                [0xC8] = new OpCode(0xC8, INY, AddressingMode.Implied, instructionBytes: 1, cycles: 2, command: (instructionBytes) => {}),
                [0xC9] = new OpCode(0xC9, CMP, AddressingMode.Immediate, instructionBytes: 2, cycles: 2, command: (instructionBytes) => {}),
                [0xCA] = new OpCode(0xCA, DEX, AddressingMode.Implied, instructionBytes: 1, cycles: 2, command: (instructionBytes) => {}),
                [0xCC] = new OpCode(0xCC, CPY, AddressingMode.Absolute, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}),
                [0xCD] = new OpCode(0xCD, CMP, AddressingMode.Absolute, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}),
                [0xCE] = new OpCode(0xCE, DEC, AddressingMode.Absolute, instructionBytes: 3, cycles: 6, command: (instructionBytes) => {}),
                [0xCF] = new OpCode(0xCF, BBS4, AddressingMode.ZeroPageRelative, instructionBytes: 3, cycles: 5, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Branch),
                
                [0xD0] = new OpCode(0xD0, BNE, AddressingMode.Relative, instructionBytes: 2, cycles: 2, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Branch),
                [0xD1] = new OpCode(0xD1, CMP, AddressingMode.IndirectIndexed, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfPageBoundaryCrossed),
                [0xD2] = new OpCode(0xD2, CMP, AddressingMode.Indirect, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}),
                [0xD5] = new OpCode(0xD5, CMP, AddressingMode.ZeroPageIndexedX, instructionBytes: 2, cycles: 4, command: (instructionBytes) => {}),
                [0xD6] = new OpCode(0xD6, DEC, AddressingMode.ZeroPageIndexedX, instructionBytes: 2, cycles: 6, command: (instructionBytes) => {}),
                [0xD7] = new OpCode(0xD7, SMB5, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}),
                [0xD8] = new OpCode(0xD8, CLD, AddressingMode.Implied, instructionBytes: 1, cycles: 2, command: (instructionBytes) => {}),
                [0xD9] = new OpCode(0xD9, CMP, AddressingMode.AbsoluteIndexedY, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfPageBoundaryCrossed),
                [0xDA] = new OpCode(0xDA, PHX, AddressingMode.Implied, instructionBytes: 1, cycles: 3, command: (instructionBytes) => {}),
                [0xDD] = new OpCode(0xDD, CMP, AddressingMode.AbsoluteIndexedX, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfPageBoundaryCrossed),
                [0xDE] = new OpCode(0xDE, DEC, AddressingMode.AbsoluteIndexedX, instructionBytes: 3, cycles: 7, command: (instructionBytes) => {}),
                [0xDD] = new OpCode(0xDD, BBS5, AddressingMode.ZeroPageRelative, instructionBytes: 3, cycles: 5, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Branch),
                
                [0xE0] = new OpCode(0xE0, CPX, AddressingMode.Immediate, instructionBytes: 2, cycles: 2, command: (instructionBytes) => {}),
                [0xE1] = new OpCode(0xE1, SBC, AddressingMode.IndexedIndirect, instructionBytes: 2, cycles: 6, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfDecimal),
                [0xE4] = new OpCode(0xE4, CPX, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 3, command: (instructionBytes) => {}),
                [0xE5] = new OpCode(0xE5, SBC, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 3, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfDecimal),
                [0xE6] = new OpCode(0xE6, INC, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}),
                [0xE7] = new OpCode(0xE7, SMB6, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}),
                [0xE8] = new OpCode(0xE8, INX, AddressingMode.Implied, instructionBytes: 1, cycles: 2, command: (instructionBytes) => {}),
                [0xE9] = new OpCode(0xE9, SBC, AddressingMode.Immediate, instructionBytes: 2, cycles: 2, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfDecimal),
                [0xEA] = new OpCode(0xEA, NOP, AddressingMode.Implied, instructionBytes: 1, cycles: 2, command: (instructionBytes) => {}),
                [0xEC] = new OpCode(0xEC, CPX, AddressingMode.Absolute, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}),
                [0xED] = new OpCode(0xED, SBC, AddressingMode.Absolute, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfDecimal),
                [0xEE] = new OpCode(0xEE, INC, AddressingMode.Absolute, instructionBytes: 3, cycles: 6, command: (instructionBytes) => {}),
                [0xEF] = new OpCode(0xEF, BBS6, AddressingMode.ZeroPageRelative, instructionBytes: 3, cycles: 5, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Branch),
                
                [0xF0] = new OpCode(0xF0, BEQ, AddressingMode.Relative, instructionBytes: 2, cycles: 2, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Branch),
                [0xF1] = new OpCode(0xF1, SBC, AddressingMode.IndirectIndexed, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfDecimal | (byte)OpCodeFlags.Add1IfPageBoundaryCrossed),
                [0xF2] = new OpCode(0xF2, SBC, AddressingMode.Indirect, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfDecimal | (byte)OpCodeFlags.Add1IfPageBoundaryCrossed),
                [0xF5] = new OpCode(0xF5, SBC, AddressingMode.ZeroPageIndexedX, instructionBytes: 2, cycles: 4, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfDecimal),
                [0xF6] = new OpCode(0xF6, INC, AddressingMode.ZeroPageIndexedX, instructionBytes: 2, cycles: 6, command: (instructionBytes) => {}),
                [0xF7] = new OpCode(0xF7, SMB7, AddressingMode.ZeroPage, instructionBytes: 2, cycles: 5, command: (instructionBytes) => {}),
                [0xF8] = new OpCode(0xF8, SED, AddressingMode.Implied, instructionBytes: 1, cycles: 2, command: (instructionBytes) => {}),
                [0xF9] = new OpCode(0xF9, SBC, AddressingMode.AbsoluteIndexedY, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfDecimal | (byte)OpCodeFlags.Add1IfPageBoundaryCrossed),
                [0xFA] = new OpCode(0xFA, PLX, AddressingMode.Implied, instructionBytes: 1, cycles: 4, command: (instructionBytes) => {}),
                [0xFD] = new OpCode(0xFD, SBC, AddressingMode.AbsoluteIndexedX, instructionBytes: 3, cycles: 4, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Add1IfDecimal | (byte)OpCodeFlags.Add1IfPageBoundaryCrossed),
                [0xFE] = new OpCode(0xFE, INC, AddressingMode.AbsoluteIndexedX, instructionBytes: 3, cycles: 7, command: (instructionBytes) => {}),
                [0xFF] = new OpCode(0xFF, BBS7, AddressingMode.ZeroPageRelative, instructionBytes: 3, cycles: 5, command: (instructionBytes) => {}, flags: (byte)OpCodeFlags.Branch),
            };
        }
    }
}