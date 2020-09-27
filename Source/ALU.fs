namespace Sharp6502

module ALU =
    open Sharp6502.Enums

    exception AndError of string
    exception OrError of string
    exception ProcessorStatusError of string

    // ALU operations
    let inline Add (value: byte) (register : Register) =
        match register with
        | AccumulatorRegister x -> AccumulatorRegister (x + value)
        | XIndexRegister x -> XIndexRegister (x + value)
        | YIndexRegister x -> YIndexRegister (x + value)
        | InstructionRegister x -> InstructionRegister (x + value)
        | ProcessorStatusRegister x -> ProcessorStatusRegister (x + value)
        | StackPointerRegister x -> StackPointerRegister (x + value)

    let inline And (value: byte) (register : Register) =
        match register with
        | AccumulatorRegister x -> AccumulatorRegister (x &&& value)
        | _ -> raise (AndError(sprintf "Unsupported And operation on %A" register))

    let inline Or (value: byte) (register : Register) =
        match register with
        | AccumulatorRegister x -> AccumulatorRegister (x &&& value)
        | _ -> raise (OrError(sprintf "Unsupported Or operation on %A" register))

    let inline ShiftLeft (value : byte) =
        value <<< 1

    // Status checks
    let inline private SetFlag (currentFlag : byte) (flag : ProcessorStatusFlags) =
        currentFlag ||| byte flag

    let inline private UnsetFlag (currentFlag : byte) (flag : ProcessorStatusFlags) =
        currentFlag &&& (byte flag ^^^ 0xFFuy)

    let inline CarryStatus (isCarry : bool) (processorStatus : Register) =
        match processorStatus with
               | ProcessorStatusRegister status ->
                   if isCarry then
                       ProcessorStatusRegister (SetFlag status ProcessorStatusFlags.Carry)
                   else
                       ProcessorStatusRegister (UnsetFlag status ProcessorStatusFlags.Carry)
               | _ -> raise (ProcessorStatusError(sprintf "Expected Processor Status Register, got %A" processorStatus))

    let inline NegativeStatus (value : byte) (processorStatus : Register) =
        match processorStatus with
        | ProcessorStatusRegister status ->
            if value &&& 0x80uy <> 0x00uy then
                ProcessorStatusRegister (SetFlag status ProcessorStatusFlags.Negative)
            else
                ProcessorStatusRegister (UnsetFlag status ProcessorStatusFlags.Negative)
        | _ -> raise (ProcessorStatusError(sprintf "Expected Processor Status Register, got %A" processorStatus))

    let inline ZeroStatus (value : byte) (processorStatus : Register) =
        match processorStatus with
        | ProcessorStatusRegister status ->
            if value = 0x00uy then
                ProcessorStatusRegister (SetFlag status ProcessorStatusFlags.Zero)
            else
                ProcessorStatusRegister (UnsetFlag status ProcessorStatusFlags.Zero)
        | _ -> raise (ProcessorStatusError(sprintf "Expected Processor Status Register, got %A" processorStatus))
