namespace Sharp6502

type Bus =
    | AddressBus of uint16
    | DataBus of uint16
