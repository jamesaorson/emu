namespace Sharp6502

module InstructionSet =

    open Sharp6502.Enums
    open Sharp6502.Instruction

    type Operation =
        {
            Cycles: InstructionCycles;
            Flags: InstructionFlags;
            Mode: AddressingMode;
            Name: InstructionName;
            Size: InstructionSize;
        }

    let table = Map([
            (0x00uy, {
                Name = InstructionName.BRK;
                Mode = AddressingMode.Implied;
                // BRK is documented as a 1 byte opcode, but it really
                // exists as a 1 byte opcode with a padding byte.
                // Interrupt routines called by BRK always return 2 bytes;
                // so treating BRK as a 2 byte opcode resolves this issue.
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Seven;
                Flags = InstructionFlags.None;
            })
            (0x01uy, {
                Name = InstructionName.ORA;
                Mode = AddressingMode.IndexedIndirect;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Six;
                Flags = InstructionFlags.None;
            })
            (0x04uy, {
                Name = InstructionName.TSB;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0x05uy, {
                Name = InstructionName.ORA;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Three;
                Flags = InstructionFlags.None;
            })
            (0x06uy, {
                Name = InstructionName.ASL;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0x07uy, {
                Name = InstructionName.RMB0;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0x08uy, {
                Name = InstructionName.PHP;
                Mode = AddressingMode.Implied;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Three;
                Flags = InstructionFlags.None;
            })
            (0x09uy, {
                Name = InstructionName.ORA;
                Mode = AddressingMode.Immediate;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0x0Auy, {
                Name = InstructionName.ASL;
                Mode = AddressingMode.Accumulator;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0x0Cuy, {
                Name = InstructionName.TSB;
                Mode = AddressingMode.Absolute;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Six;
                Flags = InstructionFlags.None;
            })
            (0x0Duy, {
                Name = InstructionName.ORA;
                Mode = AddressingMode.Absolute;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0x0Euy, {
                Name = InstructionName.ASL;
                Mode = AddressingMode.Absolute;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Six;
                Flags = InstructionFlags.None;
            })
            (0x0Fuy, {
                Name = InstructionName.BBR0;
                Mode = AddressingMode.ZeroPageRelative;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.Branch;
            })
            
            (0x10uy, {
                Name = InstructionName.BPL;
                Mode = AddressingMode.Relative;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.Branch;
            })
            (0x11uy, {
                Name = InstructionName.ORA;
                Mode = AddressingMode.IndirectIndexed;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.Add1IfPageBoundaryCrossed;
            })
            (0x12uy, {
                Name = InstructionName.ORA;
                Mode = AddressingMode.Indirect;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0x14uy, {
                Name = InstructionName.TRB;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0x15uy, {
                Name = InstructionName.ORA;
                Mode = AddressingMode.ZeroPageIndexedX;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0x16uy, {
                Name = InstructionName.ASL;
                Mode = AddressingMode.ZeroPageIndexedX;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Six;
                Flags = InstructionFlags.None;
            })
            (0x17uy, {
                Name = InstructionName.RMB1;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0x18uy, {
                Name = InstructionName.CLC;
                Mode = AddressingMode.Implied;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0x19uy, {
                Name = InstructionName.ORA;
                Mode = AddressingMode.AbsoluteIndexedY;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.Add1IfPageBoundaryCrossed;
            })
            (0x1Auy, {
                Name = InstructionName.INC;
                Mode = AddressingMode.Accumulator;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0x1Cuy, {
                Name = InstructionName.TRB;
                Mode = AddressingMode.Absolute;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Six;
                Flags = InstructionFlags.None;
            })
            (0x1Duy, {
                Name = InstructionName.ORA;
                Mode = AddressingMode.AbsoluteIndexedX;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.Add1IfPageBoundaryCrossed;
            })
            (0x1Euy, {
                Name = InstructionName.ASL;
                Mode = AddressingMode.AbsoluteIndexedX;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Seven;
                Flags = InstructionFlags.None;
            })
            (0x1Fuy, {
                Name = InstructionName.BBR1;
                Mode = AddressingMode.ZeroPageRelative;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.Branch;
            })
            
            (0x20uy, {
                Name = InstructionName.JSR;
                Mode = AddressingMode.Absolute;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Six; 
                Flags = InstructionFlags.None;
            })
            (0x21uy, {
                Name = InstructionName.AND;
                Mode = AddressingMode.IndexedIndirect;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Six; 
                Flags = InstructionFlags.None;
            })
            (0x24uy, {
                Name = InstructionName.BIT;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Three;
                Flags = InstructionFlags.None;
            })
            (0x25uy, {
                Name = InstructionName.AND;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Three;
                Flags = InstructionFlags.None;
            })
            (0x26uy, {
                Name = InstructionName.ROL;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0x27uy, {
                Name = InstructionName.RMB2;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0x28uy, {
                Name = InstructionName.PLP;
                Mode = AddressingMode.Implied;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0x29uy, {
                Name = InstructionName.AND;
                Mode = AddressingMode.Immediate;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0x2Auy, {
                Name = InstructionName.ROL;
                Mode = AddressingMode.Accumulator;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0x2Cuy, {
                Name = InstructionName.BIT;
                Mode = AddressingMode.Absolute;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0x2Duy, {
                Name = InstructionName.AND;
                Mode = AddressingMode.Absolute;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0x2Euy, {
                Name = InstructionName.ROL;
                Mode = AddressingMode.Absolute;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Six;
                Flags = InstructionFlags.None;
            })
            (0x2Fuy, {
                Name = InstructionName.BBR2;
                Mode = AddressingMode.ZeroPageRelative;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.Branch;
            })
            
            (0x30uy, {
                Name = InstructionName.BMI;
                Mode = AddressingMode.Relative;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.Branch;
            })
            (0x31uy, {
                Name = InstructionName.AND;
                Mode = AddressingMode.IndirectIndexed;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.Add1IfPageBoundaryCrossed;
            })
            (0x32uy, {
                Name = InstructionName.AND;
                Mode = AddressingMode.Indirect;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0x34uy, {
                Name = InstructionName.BIT;
                Mode = AddressingMode.ZeroPageIndexedX;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0x35uy, {
                Name = InstructionName.AND;
                Mode = AddressingMode.ZeroPageIndexedX;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0x36uy, {
                Name = InstructionName.ROL;
                Mode = AddressingMode.ZeroPageIndexedX;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Six;
                Flags = InstructionFlags.None;
            })
            (0x37uy, {
                Name = InstructionName.RMB3;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0x38uy, {
                Name = InstructionName.SEC;
                Mode = AddressingMode.Implied;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0x39uy, {
                Name = InstructionName.AND;
                Mode = AddressingMode.AbsoluteIndexedY;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.Add1IfPageBoundaryCrossed;
            })
            (0x3Auy, {
                Name = InstructionName.DEC;
                Mode = AddressingMode.Accumulator;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0x3Cuy, {
                Name = InstructionName.BIT;
                Mode = AddressingMode.AbsoluteIndexedX;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.Add1IfPageBoundaryCrossed;
            })
            (0x3Duy, {
                Name = InstructionName.AND;
                Mode = AddressingMode.AbsoluteIndexedX;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.Add1IfPageBoundaryCrossed;
            })
            (0x3Euy, {
                Name = InstructionName.ROL;
                Mode = AddressingMode.AbsoluteIndexedX;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Seven;
                Flags = InstructionFlags.None;
            })
            (0x3Fuy, {
                Name = InstructionName.BBR3;
                Mode = AddressingMode.ZeroPageRelative;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.Branch;
            })
            
            (0x40uy, {
                Name = InstructionName.RTI;
                Mode = AddressingMode.Implied;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Six;
                Flags = InstructionFlags.None;
            })
            (0x41uy, {
                Name = InstructionName.EOR;
                Mode = AddressingMode.IndexedIndirect;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Six;
                Flags = InstructionFlags.None;
            })
            (0x45uy, {
                Name = InstructionName.EOR;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Three;
                Flags = InstructionFlags.None;
            })
            (0x46uy, {
                Name = InstructionName.LSR;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0x47uy, {
                Name = InstructionName.RMB4;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0x48uy, {
                Name = InstructionName.PHA;
                Mode = AddressingMode.Implied;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Three;
                Flags = InstructionFlags.None;
            })
            (0x49uy, {
                Name = InstructionName.EOR;
                Mode = AddressingMode.Immediate;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0x4Auy, {
                Name = InstructionName.LSR;
                Mode = AddressingMode.Accumulator;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0x4Cuy, {
                Name = InstructionName.JMP;
                Mode = AddressingMode.Absolute;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Three;
                Flags = InstructionFlags.None;
            })
            (0x4Duy, {
                Name = InstructionName.EOR;
                Mode = AddressingMode.Absolute;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0x4Euy, {
                Name = InstructionName.LSR;
                Mode = AddressingMode.Absolute;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Six;
                Flags = InstructionFlags.None;
            })
            (0x4Fuy, {
                Name = InstructionName.BBR4;
                Mode = AddressingMode.ZeroPageRelative;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.Branch;
            })
            
            (0x50uy, {
                Name = InstructionName.BVC;
                Mode = AddressingMode.Relative;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.Branch;
            })
            (0x51uy, {
                Name = InstructionName.EOR;
                Mode = AddressingMode.IndirectIndexed;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.Add1IfPageBoundaryCrossed;
            })
            (0x52uy, {
                Name = InstructionName.EOR;
                Mode = AddressingMode.Indirect;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0x55uy, {
                Name = InstructionName.EOR;
                Mode = AddressingMode.ZeroPageIndexedX;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0x56uy, {
                Name = InstructionName.LSR;
                Mode = AddressingMode.ZeroPageIndexedX;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Six;
                Flags = InstructionFlags.None;
            })
            (0x57uy, {
                Name = InstructionName.RMB5;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0x58uy, {
                Name = InstructionName.CLI;
                Mode = AddressingMode.Implied;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0x59uy, {
                Name = InstructionName.EOR;
                Mode = AddressingMode.AbsoluteIndexedY;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.Add1IfPageBoundaryCrossed;
            })
            (0x5Auy, {
                Name = InstructionName.PHY;
                Mode = AddressingMode.Implied;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Three;
                Flags = InstructionFlags.None;
            })
            (0x5Duy, {
                Name = InstructionName.EOR;
                Mode = AddressingMode.AbsoluteIndexedX;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.Add1IfPageBoundaryCrossed;
            })
            (0x5Euy, {
                Name = InstructionName.LSR;
                Mode = AddressingMode.AbsoluteIndexedX;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Seven;
                Flags = InstructionFlags.None;
            })
            (0x5Fuy, {
                Name = InstructionName.BBR5;
                Mode = AddressingMode.ZeroPageRelative;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.Branch;
            })
            
            (0x60uy, {
                Name = InstructionName.RTS;
                Mode = AddressingMode.Implied;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Six;
                Flags = InstructionFlags.None;
            })
            (0x61uy, {
                Name = InstructionName.ADC;
                Mode = AddressingMode.IndexedIndirect;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Six;
                Flags = InstructionFlags.Add1IfDecimal;
            })
            (0x64uy, {
                Name = InstructionName.STZ;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Three;
                Flags = InstructionFlags.None;
            })
            (0x65uy, {
                Name = InstructionName.ADC;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Three;
                Flags = InstructionFlags.Add1IfDecimal;
            })
            (0x66uy, {
                Name = InstructionName.ROR;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0x67uy, {
                Name = InstructionName.RMB6;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0x68uy, {
                Name = InstructionName.PLA;
                Mode = AddressingMode.Implied;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0x69uy, {
                Name = InstructionName.ADC;
                Mode = AddressingMode.Immediate;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.Add1IfDecimal;
            })
            (0x6Auy, {
                Name = InstructionName.ROR;
                Mode = AddressingMode.Accumulator;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0x6Cuy, {
                Name = InstructionName.JMP;
                Mode = AddressingMode.AbsoluteIndirect;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Six;
                Flags = InstructionFlags.None;
            })
            (0x6Duy, {
                Name = InstructionName.ADC;
                Mode = AddressingMode.Absolute;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.Add1IfDecimal;
            })
            (0x6Euy, {
                Name = InstructionName.ROR;
                Mode = AddressingMode.Absolute;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Six;
                Flags = InstructionFlags.None;
            })
            (0x6Fuy, {
                Name = InstructionName.BBR7;
                Mode = AddressingMode.ZeroPageRelative;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.Branch;
            })
            
            (0x70uy, {
                Name = InstructionName.BVS;
                Mode = AddressingMode.Relative;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Three;
                Flags = InstructionFlags.Add1IfPageBoundaryCrossed;
            })
            (0x71uy, {
                Name = InstructionName.ADC;
                Mode = AddressingMode.IndirectIndexed;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.Add1IfDecimalOrPageBoundaryCrossed;
            })
            (0x72uy, {
                Name = InstructionName.ADC;
                Mode = AddressingMode.Indirect;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.Add1IfDecimal;
            })
            (0x74uy, {
                Name = InstructionName.STZ;
                Mode = AddressingMode.ZeroPageIndexedX;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0x75uy, {
                Name = InstructionName.ADC;
                Mode = AddressingMode.ZeroPageIndexedX;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.Add1IfDecimal;
            })
            (0x76uy, {
                Name = InstructionName.ROR;
                Mode = AddressingMode.ZeroPageIndexedX;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Six;
                Flags = InstructionFlags.None;
            })
            (0x77uy, {
                Name = InstructionName.RMB7;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0x78uy, {
                Name = InstructionName.SEI;
                Mode = AddressingMode.Implied;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0x79uy, {
                Name = InstructionName.ADC;
                Mode = AddressingMode.AbsoluteIndexedY;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.Add1IfDecimalOrPageBoundaryCrossed;
            })
            (0x7Auy, {
                Name = InstructionName.PLY;
                Mode = AddressingMode.Implied;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0x7Cuy, {
                Name = InstructionName.JMP;
                Mode = AddressingMode.IndexedAbsoluteIndirect;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Six;
                Flags = InstructionFlags.None;
            })
            (0x7Duy, {
                Name = InstructionName.ADC;
                Mode = AddressingMode.AbsoluteIndexedX;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.Add1IfDecimalOrPageBoundaryCrossed;
            })
            (0x7Euy, {
                Name = InstructionName.ROR;
                Mode = AddressingMode.AbsoluteIndexedX;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Seven;
                Flags = InstructionFlags.None;
            })
            (0x7Fuy, {
                Name = InstructionName.BBR7;
                Mode = AddressingMode.ZeroPageRelative;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.Branch;
            })
            
            (0x80uy, {
                Name = InstructionName.BRA;
                Mode = AddressingMode.Relative;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Three;
                Flags = InstructionFlags.Add1IfPageBoundaryCrossed;
            })
            (0x81uy, {
                Name = InstructionName.STA;
                Mode = AddressingMode.IndexedIndirect;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Six;
                Flags = InstructionFlags.None;
            })
            (0x84uy, {
                Name = InstructionName.STY;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Three;
                Flags = InstructionFlags.None;
            })
            (0x85uy, {
                Name = InstructionName.STA;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Three;
                Flags = InstructionFlags.None;
            })
            (0x86uy, {
                Name = InstructionName.STX;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Three;
                Flags = InstructionFlags.None;
            })
            (0x87uy, {
                Name = InstructionName.SMB0;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0x88uy, {
                Name = InstructionName.DEY;
                Mode = AddressingMode.Implied;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0x89uy, {
                Name = InstructionName.BIT;
                Mode = AddressingMode.Immediate;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0x8Auy, {
                Name = InstructionName.TXA;
                Mode = AddressingMode.Implied;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0x8Cuy, {
                Name = InstructionName.STY;
                Mode = AddressingMode.Absolute;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0x8Duy, {
                Name = InstructionName.STA;
                Mode = AddressingMode.Absolute;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0x8Euy, {
                Name = InstructionName.STX;
                Mode = AddressingMode.Absolute;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0x8Fuy, {
                Name = InstructionName.BBS0;
                Mode = AddressingMode.ZeroPageRelative;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.Branch;
            })
            
            (0x90uy, {
                Name = InstructionName.BCC;
                Mode = AddressingMode.Relative;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.Branch;
            })
            (0x91uy, {
                Name = InstructionName.STA;
                Mode = AddressingMode.IndirectIndexed;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Six;
                Flags = InstructionFlags.None;
            })
            (0x92uy, {
                Name = InstructionName.STA;
                Mode = AddressingMode.Indirect;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0x94uy, {
                Name = InstructionName.STY;
                Mode = AddressingMode.ZeroPageIndexedX;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0x95uy, {
                Name = InstructionName.STA;
                Mode = AddressingMode.ZeroPageIndexedX;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0x96uy, {
                Name = InstructionName.STX;
                Mode = AddressingMode.ZeroPageIndexedY;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0x97uy, {
                Name = InstructionName.SMB1;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0x98uy, {
                Name = InstructionName.TYA;
                Mode = AddressingMode.Implied;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0x99uy, {
                Name = InstructionName.STA;
                Mode = AddressingMode.AbsoluteIndexedY;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0x9Auy, {
                Name = InstructionName.TXS;
                Mode = AddressingMode.Implied;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0x9Cuy, {
                Name = InstructionName.STZ;
                Mode = AddressingMode.Absolute;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0x9Duy, {
                Name = InstructionName.STA;
                Mode = AddressingMode.AbsoluteIndexedX;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0x9Euy, {
                Name = InstructionName.STZ;
                Mode = AddressingMode.AbsoluteIndexedX;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0x9Fuy, {
                Name = InstructionName.BBS1;
                Mode = AddressingMode.ZeroPageRelative;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.Branch;
            })
            
            (0xA0uy, {
                Name = InstructionName.LDY;
                Mode = AddressingMode.Immediate;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0xA1uy, {
                Name = InstructionName.LDA;
                Mode = AddressingMode.IndexedIndirect;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Six;
                Flags = InstructionFlags.None;
            })
            (0xA2uy, {
                Name = InstructionName.LDX;
                Mode = AddressingMode.Immediate;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0xA4uy, {
                Name = InstructionName.LDY;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Three;
                Flags = InstructionFlags.None;
            })
            (0xA5uy, {
                Name = InstructionName.LDA;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Three;
                Flags = InstructionFlags.None;
            })
            (0xA6uy, {
                Name = InstructionName.LDX;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Three;
                Flags = InstructionFlags.None;
            })
            (0xA7uy, {
                Name = InstructionName.SMB2;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0xA8uy, {
                Name = InstructionName.TAY;
                Mode = AddressingMode.Implied;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0xA9uy, {
                Name = InstructionName.LDA;
                Mode = AddressingMode.Immediate;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0xAAuy, {
                Name = InstructionName.TAX;
                Mode = AddressingMode.Implied;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0xACuy, {
                Name = InstructionName.LDY;
                Mode = AddressingMode.Absolute;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0xADuy, {
                Name = InstructionName.LDA;
                Mode = AddressingMode.Absolute;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0xAEuy, {
                Name = InstructionName.LDX;
                Mode = AddressingMode.Absolute;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0xAFuy, {
                Name = InstructionName.BBS2;
                Mode = AddressingMode.ZeroPageRelative;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.Branch;
            })
            
            (0xB0uy, {
                Name = InstructionName.BCS;
                Mode = AddressingMode.Relative;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.Branch;
            })
            (0xB1uy, {
                Name = InstructionName.LDA;
                Mode = AddressingMode.IndirectIndexed;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.Add1IfPageBoundaryCrossed;
            })
            (0xB2uy, {
                Name = InstructionName.LDA;
                Mode = AddressingMode.Indirect;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0xB4uy, {
                Name = InstructionName.LDY;
                Mode = AddressingMode.ZeroPageIndexedX;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0xB5uy, {
                Name = InstructionName.LDA;
                Mode = AddressingMode.ZeroPageIndexedX;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0xB6uy, {
                Name = InstructionName.LDX;
                Mode = AddressingMode.ZeroPageIndexedY;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0xB7uy, {
                Name = InstructionName.SMB3;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0xB8uy, {
                Name = InstructionName.CLV;
                Mode = AddressingMode.Implied;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0xB9uy, {
                Name = InstructionName.LDA;
                Mode = AddressingMode.AbsoluteIndexedY;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.Add1IfPageBoundaryCrossed;
            })
            (0xBAuy, {
                Name = InstructionName.TSX;
                Mode = AddressingMode.Implied;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0xBCuy, {
                Name = InstructionName.LDY;
                Mode = AddressingMode.AbsoluteIndexedX;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.Add1IfPageBoundaryCrossed;
            })
            (0xBDuy, {
                Name = InstructionName.LDA;
                Mode = AddressingMode.AbsoluteIndexedX;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.Add1IfPageBoundaryCrossed;
            })
            (0xBEuy, {
                Name = InstructionName.LDX;
                Mode = AddressingMode.AbsoluteIndexedY;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.Add1IfPageBoundaryCrossed;
            })
            (0xBFuy, {
                Name = InstructionName.BBS3;
                Mode = AddressingMode.ZeroPageRelative;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.Branch;
            })
            
            (0xC0uy, {
                Name = InstructionName.CPY;
                Mode = AddressingMode.Immediate;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0xC1uy, {
                Name = InstructionName.CMP;
                Mode = AddressingMode.IndexedIndirect;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Six;
                Flags = InstructionFlags.None;
            })
            (0xC4uy, {
                Name = InstructionName.CPY;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Three;
                Flags = InstructionFlags.None;
            })
            (0xC5uy, {
                Name = InstructionName.CMP;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Three;
                Flags = InstructionFlags.None;
            })
            (0xC6uy, {
                Name = InstructionName.DEC;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0xC7uy, {
                Name = InstructionName.SMB4;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0xC8uy, {
                Name = InstructionName.INY;
                Mode = AddressingMode.Implied;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0xC9uy, {
                Name = InstructionName.CMP;
                Mode = AddressingMode.Immediate;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0xCAuy, {
                Name = InstructionName.DEX;
                Mode = AddressingMode.Implied;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0xCCuy, {
                Name = InstructionName.CPY;
                Mode = AddressingMode.Absolute;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0xCDuy, {
                Name = InstructionName.CMP;
                Mode = AddressingMode.Absolute;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0xCEuy, {
                Name = InstructionName.DEC;
                Mode = AddressingMode.Absolute;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Six;
                Flags = InstructionFlags.None;
            })
            (0xCFuy, {
                Name = InstructionName.BBS4;
                Mode = AddressingMode.ZeroPageRelative;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.Branch;
            })
            
            (0xD0uy, {
                Name = InstructionName.BNE;
                Mode = AddressingMode.Relative;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.Branch;
            })
            (0xD1uy, {
                Name = InstructionName.CMP;
                Mode = AddressingMode.IndirectIndexed;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.Add1IfPageBoundaryCrossed;
            })
            (0xD2uy, {
                Name = InstructionName.CMP;
                Mode = AddressingMode.Indirect;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0xD5uy, {
                Name = InstructionName.CMP;
                Mode = AddressingMode.ZeroPageIndexedX;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0xD6uy, {
                Name = InstructionName.DEC;
                Mode = AddressingMode.ZeroPageIndexedX;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Six;
                Flags = InstructionFlags.None;
            })
            (0xD7uy, {
                Name = InstructionName.SMB5;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0xD8uy, {
                Name = InstructionName.CLD;
                Mode = AddressingMode.Implied;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0xD9uy, {
                Name = InstructionName.CMP;
                Mode = AddressingMode.AbsoluteIndexedY;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.Add1IfPageBoundaryCrossed;
            })
            (0xDAuy, {
                Name = InstructionName.PHX;
                Mode = AddressingMode.Implied;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Three;
                Flags = InstructionFlags.None;
            })
            (0xDDuy, {
                Name = InstructionName.CMP;
                Mode = AddressingMode.AbsoluteIndexedX;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.Add1IfPageBoundaryCrossed;
            })
            (0xDEuy, {
                Name = InstructionName.DEC;
                Mode = AddressingMode.AbsoluteIndexedX;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Seven;
                Flags = InstructionFlags.None;
            })
            (0xDFuy, {
                Name = InstructionName.BBS5;
                Mode = AddressingMode.ZeroPageRelative;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.Branch;
            })
            
            (0xE0uy, {
                Name = InstructionName.CPX;
                Mode = AddressingMode.Immediate;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0xE1uy, {
                Name = InstructionName.SBC;
                Mode = AddressingMode.IndexedIndirect;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Six;
                Flags = InstructionFlags.Add1IfDecimal;
            })
            (0xE4uy, {
                Name = InstructionName.CPX;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Three;
                Flags = InstructionFlags.None;
            })
            (0xE5uy, {
                Name = InstructionName.SBC;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Three;
                Flags = InstructionFlags.Add1IfDecimal;
            })
            (0xE6uy, {
                Name = InstructionName.INC;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0xE7uy, {
                Name = InstructionName.SMB6;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0xE8uy, {
                Name = InstructionName.INX;
                Mode = AddressingMode.Implied;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0xE9uy, {
                Name = InstructionName.SBC;
                Mode = AddressingMode.Immediate;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.Add1IfDecimal;
            })
            (0xEAuy, {
                Name = InstructionName.NOP;
                Mode = AddressingMode.Implied;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0xECuy, {
                Name = InstructionName.CPX;
                Mode = AddressingMode.Absolute;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0xEDuy, {
                Name = InstructionName.SBC;
                Mode = AddressingMode.Absolute;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.Add1IfDecimal;
            })
            (0xEEuy, {
                Name = InstructionName.INC;
                Mode = AddressingMode.Absolute;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Six;
                Flags = InstructionFlags.None;
            })
            (0xEFuy, {
                Name = InstructionName.BBS6;
                Mode = AddressingMode.ZeroPageRelative;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.Branch;
            })
            
            (0xF0uy, {
                Name = InstructionName.BEQ;
                Mode = AddressingMode.Relative;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.Branch;
            })
            (0xF1uy, {
                Name = InstructionName.SBC;
                Mode = AddressingMode.IndirectIndexed;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.Add1IfDecimalOrPageBoundaryCrossed;
            })
            (0xF2uy, {
                Name = InstructionName.SBC;
                Mode = AddressingMode.Indirect;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.Add1IfDecimalOrPageBoundaryCrossed;
            })
            (0xF5uy, {
                Name = InstructionName.SBC;
                Mode = AddressingMode.ZeroPageIndexedX;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.Add1IfDecimal;
            })
            (0xF6uy, {
                Name = InstructionName.INC;
                Mode = AddressingMode.ZeroPageIndexedX;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Six;
                Flags = InstructionFlags.None;
            })
            (0xF7uy, {
                Name = InstructionName.SMB7;
                Mode = AddressingMode.ZeroPage;
                Size = InstructionSize.Two;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.None;
            })
            (0xF8uy, {
                Name = InstructionName.SED;
                Mode = AddressingMode.Implied;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Two;
                Flags = InstructionFlags.None;
            })
            (0xF9uy, {
                Name = InstructionName.SBC;
                Mode = AddressingMode.AbsoluteIndexedY;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.Add1IfDecimalOrPageBoundaryCrossed;
            })
            (0xFAuy, {
                Name = InstructionName.PLX;
                Mode = AddressingMode.Implied;
                Size = InstructionSize.One;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.None;
            })
            (0xFDuy, {
                Name = InstructionName.SBC;
                Mode = AddressingMode.AbsoluteIndexedX;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Four;
                Flags = InstructionFlags.Add1IfDecimalOrPageBoundaryCrossed;
            })
            (0xFEuy, {
                Name = InstructionName.INC;
                Mode = AddressingMode.AbsoluteIndexedX;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Seven;
                Flags = InstructionFlags.None;
            })
            (0xFFuy, {
                Name = InstructionName.BBS7;
                Mode = AddressingMode.ZeroPageRelative;
                Size = InstructionSize.Three;
                Cycles = InstructionCycles.Five;
                Flags = InstructionFlags.Branch;
            })
        ]
    )
