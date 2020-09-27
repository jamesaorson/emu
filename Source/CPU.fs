namespace Sharp6502

module CPU =
    open System.Diagnostics

    open Sharp6502.Enums
    open Sharp6502.Instruction
    open Sharp6502.RAM

    [<AutoOpen>]
    module private DomainErrors =
        exception ReduceZeroCycleError of string
        exception Invalid6502OpCodeError of string

    [<AutoOpen>]
    module private DomainFunctions =
            
        let Cycle cycles =
            match cycles with
            | InstructionCycles.One -> InstructionCycles.Zero
            | InstructionCycles.Two -> InstructionCycles.One
            | InstructionCycles.Three -> InstructionCycles.Two
            | InstructionCycles.Four -> InstructionCycles.Three
            | InstructionCycles.Five -> InstructionCycles.Four
            | InstructionCycles.Six -> InstructionCycles.Five
            | InstructionCycles.Seven -> InstructionCycles.Six
            | InstructionCycles.Zero -> raise (ReduceZeroCycleError("Can't reduce 0 cycles"))
            | _ -> raise (System.IndexOutOfRangeException(sprintf "Unknown instruction cycles: %A" cycles))
        
        let FetchOperation (programCounter : MemoryAddress) (ram : RAM64k) =
            let opCode = ram.GetData programCounter
            match InstructionSet.table.TryFind opCode with
            | Some instruction -> instruction
            | None -> raise (Invalid6502OpCodeError(sprintf "Invalid opCode: %X" opCode))
            //| None -> InstructionSet.table.[0x00uy]

        let HighByte (value : uint16) =
            byte ((value >>> 0x0008) &&& 0x00FFus)

        let LoadAddress address =
            match address with
            | MemoryAddress a -> AddressBus a

        let LoadProgram filePath =
            System.IO.File.ReadAllBytes(filePath)
        
        let LowByte (value : uint16) =
            byte (value &&& 0x00FFus)

        let Step programCounter (operation : InstructionSet.Operation) =
            let operationSize =
                match operation.Size with
                | InstructionSize.One -> 1us
                | InstructionSize.Two -> 2us
                | InstructionSize.Three -> 3us
                | _ -> raise (System.Exception("Unknown instruction size"))
        
            match programCounter with
                | MemoryAddress address -> MemoryAddress (address + operationSize)
        
        let SynchronizeTick (clock : Stopwatch) (speed : ClockSpeed) =
            while clock.ElapsedTicks < int64 speed do
                ()

        let Tick ticks =
            ticks + 1UL

    type CPU6502 =
        val private Speed : ClockSpeed
        val mutable private IsPoweredOn : bool
        val mutable private Accumulator : Register
        val mutable private XIndex : Register
        val mutable private YIndex : Register
        val mutable private Instruction : Register
        val mutable private ProcessorStatus : Register
        val mutable private StackPointer : Register

        new() = CPU6502(ClockSpeed.OneMegahertz)
        
        new(speed : ClockSpeed) = {
            Speed = speed;
            IsPoweredOn = false;
            Accumulator = AccumulatorRegister 0x00uy;
            XIndex = XIndexRegister 0x00uy;
            YIndex = YIndexRegister 0x00uy;
            Instruction = InstructionRegister 0x00uy;
            ProcessorStatus = ProcessorStatusRegister 0x00uy;
            StackPointer = StackPointerRegister 0x00uy;
        }

        member public this.Run filePath =
            this.IsPoweredOn <- true
            let mutable ticks = 0UL
            let mutable ram = RAM64k()
            let mutable programCounter = MemoryAddress 0us
            let mutable remainingCycles = Instruction.InstructionCycles.Zero
            let mutable addressBus = (Bus.AddressBus 0us)
            let mutable dataBus = (Bus.DataBus 0us)
            
            let fileBytes = LoadProgram filePath
            for i = 0 to (min RAM64k.Length fileBytes.Length) - 1 do
                ram.SetData (MemoryAddress (uint16 i)) fileBytes.[i]
            
            // printfn "Press any key to start %s!" filePath
            // System.Console.ReadKey(true) |> ignore
            // printfn "Press any key to stop execution..."
            // printfn "Starting in 3 seconds!"
            // System.Threading.Thread.Sleep(1000)
            // printfn "Starting in 2 seconds!"
            // System.Threading.Thread.Sleep(1000)
            // printfn "Starting in 1 second!"
            // System.Threading.Thread.Sleep(1000)
            printfn "Start!"

            let totalClock = Stopwatch.StartNew()
            let clock = Stopwatch.StartNew()

            // Fetch first operation without incrementing program counter
            let mutable operation = FetchOperation programCounter ram
            addressBus <- LoadAddress programCounter
            remainingCycles <- operation.Cycles
            
            // TODO: Consider wrapping all of this logic in a function to remove all mutable state.
            while (this.IsPoweredOn) do                
                SynchronizeTick clock this.Speed
                clock.Restart()
                ticks <- Tick ticks
                remainingCycles <- Cycle remainingCycles
                
                match remainingCycles with
                | InstructionCycles.Zero -> do
                    printfn "Executing: %A" operation

                    programCounter <- this.ExecuteOperation programCounter ram operation

                    operation <- FetchOperation programCounter ram
                    addressBus <- LoadAddress programCounter
                    remainingCycles <- operation.Cycles
                | _ -> ()
                
                this.IsPoweredOn <- not System.Console.KeyAvailable
            
            totalClock.Stop()
            clock.Stop()

            printfn "CPU uptime: %A" totalClock.Elapsed
            printfn "CPU ticks:  %i" ticks

            ()

        member private this.ExecuteOperation (programCounter : MemoryAddress) (ram : RAM64k) (operation : InstructionSet.Operation) =
            let opCode = ram.GetData programCounter
            
            match opCode with
            | 0x00uy ->
                // BRK
                () // TODO: RTI interrupt
            | 0x01uy ->
                // ORA
                let value = 0x00uy // TODO: AddressingMode.IndexedIndirect
                this.Accumulator <- ALU.Or value this.Accumulator
                ()
            | 0x04uy ->
                // TSB
                let value = 0x00uy // TODO: AddressingMode.ZeroPage
                let testValue =
                    match ALU.And value this.Accumulator with
                    | AccumulatorRegister test -> test
                    | _ -> raise (System.IndexOutOfRangeException())
                this.ProcessorStatus <- ALU.ZeroStatus testValue this.ProcessorStatus
                ()
            | 0x05uy ->
                // ORA
                let value = 0x00uy // TODO: AddressingMode.ZeroPage
                this.Accumulator <- ALU.Or value this.Accumulator
                ()
            | 0x06uy ->
                // ASL
                let value = 0x00uy // TODO: AddressingMode.ZeroPage
                let hasCarry = (0x40uy &&& value) = 0x40uy
                ALU.ShiftLeft value |> ignore // TODO: Send value to memory or accumulator
                this.ProcessorStatus <- ALU.CarryStatus hasCarry this.ProcessorStatus
                ()
            | 0x07uy -> ()
            | 0x08uy -> ()
            | 0x09uy -> ()
            | 0x0Auy -> ()
            | 0x0Cuy -> ()
            | 0x0Duy -> ()
            | 0x0Euy -> ()
            | 0x0Fuy -> ()
            
            | 0x10uy -> ()
            | 0x11uy -> ()
            | 0x12uy -> ()
            | 0x14uy -> ()
            | 0x15uy -> ()
            | 0x16uy -> ()
            | 0x17uy -> ()
            | 0x18uy -> ()
            | 0x19uy -> ()
            | 0x1Auy -> ()
            | 0x1Cuy -> ()
            | 0x1Duy -> ()
            | 0x1Euy -> ()
            | 0x1Fuy -> ()
            
            | 0x20uy -> ()
            | 0x21uy -> ()
            | 0x24uy -> ()
            | 0x25uy -> ()
            | 0x26uy -> ()
            | 0x27uy -> ()
            | 0x28uy -> ()
            | 0x29uy -> ()
            | 0x2Auy -> ()
            | 0x2Cuy -> ()
            | 0x2Duy -> ()
            | 0x2Euy -> ()
            | 0x2Fuy -> ()
            
            | 0x30uy -> ()
            | 0x31uy -> ()
            | 0x32uy -> ()
            | 0x34uy -> ()
            | 0x35uy -> ()
            | 0x36uy -> ()
            | 0x37uy -> ()
            | 0x38uy -> ()
            | 0x39uy -> ()
            | 0x3Auy -> ()
            | 0x3Cuy -> ()
            | 0x3Duy -> ()
            | 0x3Euy -> ()
            | 0x3Fuy -> ()
            
            | 0x40uy -> ()
            | 0x41uy -> ()
            | 0x45uy -> ()
            | 0x46uy -> ()
            | 0x47uy -> ()
            | 0x48uy -> ()
            | 0x49uy -> ()
            | 0x4Auy -> ()
            | 0x4Cuy -> ()
            | 0x4Duy -> ()
            | 0x4Euy -> ()
            | 0x4Fuy -> ()
            
            | 0x50uy -> ()
            | 0x51uy -> ()
            | 0x52uy -> ()
            | 0x55uy -> ()
            | 0x56uy -> ()
            | 0x57uy -> ()
            | 0x58uy -> ()
            | 0x59uy -> ()
            | 0x5Auy -> ()
            | 0x5Duy -> ()
            | 0x5Euy -> ()
            | 0x5Fuy -> ()
            
            | 0x60uy -> ()
            | 0x61uy -> ()
            | 0x64uy -> ()
            | 0x65uy -> ()
            | 0x66uy -> ()
            | 0x67uy -> ()
            | 0x68uy -> ()
            | 0x69uy -> ()
            | 0x6Auy -> ()
            | 0x6Cuy -> ()
            | 0x6Duy -> ()
            | 0x6Euy -> ()
            | 0x6Fuy -> ()
            
            | 0x70uy -> ()
            | 0x71uy -> ()
            | 0x72uy -> ()
            | 0x74uy -> ()
            | 0x75uy -> ()
            | 0x76uy -> ()
            | 0x77uy -> ()
            | 0x78uy -> ()
            | 0x79uy -> ()
            | 0x7Auy -> ()
            | 0x7Cuy -> ()
            | 0x7Duy -> ()
            | 0x7Euy -> ()
            | 0x7Fuy -> ()
            
            | 0x80uy -> ()
            | 0x81uy -> ()
            | 0x84uy -> ()
            | 0x85uy -> ()
            | 0x86uy -> ()
            | 0x87uy -> ()
            | 0x88uy -> ()
            | 0x89uy -> ()
            | 0x8Auy -> ()
            | 0x8Cuy -> ()
            | 0x8Duy -> ()
            | 0x8Euy -> ()
            | 0x8Fuy -> ()
            
            | 0x90uy -> ()
            | 0x91uy -> ()
            | 0x92uy -> ()
            | 0x94uy -> ()
            | 0x95uy -> ()
            | 0x96uy -> ()
            | 0x97uy -> ()
            | 0x98uy -> ()
            | 0x99uy -> ()
            | 0x9Auy -> ()
            | 0x9Cuy -> ()
            | 0x9Duy -> ()
            | 0x9Euy -> ()
            | 0x9Fuy -> ()
            
            | 0xA0uy -> ()
            | 0xA1uy -> ()
            | 0xA2uy -> ()
            | 0xA4uy -> ()
            | 0xA5uy -> ()
            | 0xA6uy -> ()
            | 0xA7uy -> ()
            | 0xA8uy -> ()
            | 0xA9uy -> ()
            | 0xAAuy -> ()
            | 0xACuy -> ()
            | 0xADuy -> ()
            | 0xAEuy -> ()
            | 0xAFuy -> ()
            
            | 0xB0uy -> ()
            | 0xB1uy -> ()
            | 0xB2uy -> ()
            | 0xB4uy -> ()
            | 0xB5uy -> ()
            | 0xB6uy -> ()
            | 0xB7uy -> ()
            | 0xB8uy -> ()
            | 0xB9uy -> ()
            | 0xBAuy -> ()
            | 0xBCuy -> ()
            | 0xBDuy -> ()
            | 0xBEuy -> ()
            | 0xBFuy -> ()
            
            | 0xC0uy -> ()
            | 0xC1uy -> ()
            | 0xC4uy -> ()
            | 0xC5uy -> ()
            | 0xC6uy -> ()
            | 0xC7uy -> ()
            | 0xC8uy -> ()
            | 0xC9uy -> ()
            | 0xCAuy -> ()
            | 0xCCuy -> ()
            | 0xCDuy -> ()
            | 0xCEuy -> ()
            | 0xCFuy -> ()
            
            | 0xD0uy -> ()
            | 0xD1uy -> ()
            | 0xD2uy -> ()
            | 0xD5uy -> ()
            | 0xD6uy -> ()
            | 0xD7uy -> ()
            | 0xD8uy -> ()
            | 0xD9uy -> ()
            | 0xDAuy -> ()
            | 0xDDuy -> ()
            | 0xDEuy -> ()
            | 0xDFuy -> ()
            
            | 0xE0uy -> ()
            | 0xE1uy -> ()
            | 0xE4uy -> ()
            | 0xE5uy -> ()
            | 0xE6uy -> ()
            | 0xE7uy -> ()
            | 0xE8uy -> ()
            | 0xE9uy -> ()
            | 0xEAuy -> ()
            | 0xECuy -> ()
            | 0xEDuy -> ()
            | 0xEEuy -> ()
            | 0xEFuy -> ()
            
            | 0xF0uy -> ()
            | 0xF1uy -> ()
            | 0xF2uy -> ()
            | 0xF5uy -> ()
            | 0xF6uy -> ()
            | 0xF7uy -> ()
            | 0xF8uy -> ()
            | 0xF9uy -> ()
            | 0xFAuy -> ()
            | 0xFDuy -> ()
            | 0xFEuy -> ()
            | 0xFFuy -> ()

            | _ -> raise (Invalid6502OpCodeError(sprintf "Invalid opCode: %X" opCode))

            Step programCounter operation

