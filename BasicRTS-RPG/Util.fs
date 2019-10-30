module Util

open System
open Action

let ( >>= ) x f = Result.bind f x

let read () = Console.ReadLine()
let log msg = printfn "%s" msg
let logAny obj = printfn "%A" obj
let random = new Random()


let percentDone (now : DateTime) ((start, stop) : DateTime * DateTime) = 
    let startToNow = now.Subtract start
    let startToStop = stop.Subtract start
    let divided = 100.0 * (startToNow.TotalSeconds / startToStop.TotalSeconds)
    min 100 <| int divided

let stringForPercent percent = sprintf "[%s%s] %s%%" (String.replicate (percent / 10) "=")
                                                     (String.replicate ((100 - percent) / 10) " ")
                                                     (string percent)

let progressReport now actions = 
    match actions with 
    | [] -> ""
    | list -> List.map (fun (state, _) -> sprintf "%A (%i): %s" state.Type state.Amount (stringForPercent <| percentDone now state.Duration)) list 
              |> String.concat "\n"

