namespace Sharp6502

module Instruction =
    type InstructionCycles =
        | Zero = 0x00uy
        | One = 0x01uy
        | Two = 0x02uy
        | Three = 0x03uy
        | Four = 0x04uy
        | Five = 0x05uy
        | Six = 0x06uy
        | Seven = 0x07uy

    type InstructionFlags =
        | None = 0x00uy
        | Add1IfDecimal = 0x01uy
        | Add1IfPageBoundaryCrossed = 0x02uy
        | Add1IfDecimalOrPageBoundaryCrossed = 0x03uy
        | Branch = 0x04uy

    type InstructionName =
        | ADC
        | AND
        | ASL
        | BBR0
        | BBR1
        | BBR2
        | BBR3
        | BBR4
        | BBR5
        | BBR6
        | BBR7
        | BBS0
        | BBS1
        | BBS2
        | BBS3
        | BBS4
        | BBS5
        | BBS6
        | BBS7
        | BCC
        | BCS
        | BEQ
        | BIT
        | BMI
        | BNE
        | BPL
        | BRA
        | BRK
        | BVC
        | BVS
        | CLC
        | CLD
        | CLI
        | CLV
        | CMP
        | CPX
        | CPY
        | DEC
        | DEX
        | DEY
        | EOR
        | INC
        | INX
        | INY
        | JMP
        | JSR
        | LDA
        | LDX
        | LDY
        | LSR
        | NOP
        | ORA
        | PHA
        | PHP
        | PHX
        | PHY
        | PLA
        | PLP
        | PLX
        | PLY
        | RMB0
        | RMB1
        | RMB2
        | RMB3
        | RMB4
        | RMB5
        | RMB6
        | RMB7
        | ROL
        | ROR
        | RTI
        | RTS
        | SBC
        | SEC
        | SED
        | SEI
        | SMB0
        | SMB1
        | SMB2
        | SMB3
        | SMB4
        | SMB5
        | SMB6
        | SMB7
        | STA
        | STX
        | STY
        | STZ
        | TAX
        | TAY
        | TRB
        | TSB
        | TSX
        | TXA
        | TXS
        | TYA

    type InstructionSize =
        | One = 0x01uy
        | Two = 0x02uy
        | Three = 0x03uy
