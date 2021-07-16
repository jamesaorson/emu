open EmuLib

let from whom =
    sprintf "from %s" whom

[<EntryPoint>]
let main argv =
    Say.hello(from "F#")
    0