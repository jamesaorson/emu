namespace Sharp6502

type Register =
    | AccumulatorRegister of byte
    | XIndexRegister of byte
    | YIndexRegister of byte
    | InstructionRegister of byte
    | ProcessorStatusRegister of byte
    | StackPointerRegister of byte
