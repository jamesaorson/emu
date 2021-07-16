namespace Sharp6502.Enums

type ProcessorStatusFlags =
    | Negative = 0x80         // N
    | Overflow = 0x40         // V
    | Unused = 0x20
    | B = 0x10                // B
    | DecimalMode = 0x08      // D
    | InterruptDisable = 0x04 // I
    | Zero = 0x02             // Z
    | Carry = 0x01            // C
