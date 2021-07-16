open Sharp6502.CPU
open Sharp6502.Enums

[<EntryPoint>]
let main argv =
    CPU6502(ClockSpeed.OneMegahertz).Run("./Examples/nestest.nes")
    0