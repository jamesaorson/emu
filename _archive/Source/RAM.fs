namespace Sharp6502

module RAM =
    type MemoryAddress = MemoryAddress of uint16

    type RAM64k =
        static member public Length = int System.UInt16.MaxValue

        val private Data : byte array

        new() =
            {
                Data = Array.create RAM64k.Length 0x00uy;
            }

        member public this.GetData (address: MemoryAddress) =
            match address with
            | MemoryAddress a -> this.Data.[int a]

        member public this.SetData (address: MemoryAddress) (data: byte) =
            match address with
            | MemoryAddress a -> this.Data.[int a] <- data
