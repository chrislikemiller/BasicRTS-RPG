module Util

open System
open Action
open GameWorld
open Player
open Person
open System.Drawing

let ( >>= ) x f = Result.bind f x
let log msg = printfn "\n%s" msg
let logError msg = 
    let currentColor = Console.ForegroundColor
    Console.ForegroundColor <- ConsoleColor.Red
    printfn "\n%s" msg
    Console.ForegroundColor <- currentColor

let logAny obj = printfn "%A" obj

let read _ = Console.ReadLine()
let readKey _ = Console.ReadKey().KeyChar |> string
let rec readAmount _ = 
    let input = read ()
    if String.length input > 0 && input |> Seq.forall Char.IsDigit 
        then let result = int input
             if result > 0 then result else logError "Enter a valid number!"; readAmount ()
        else logError "Please input a number!"; readAmount ()

let percentDone (now : DateTime) progressState = 
    match progressState with
    | NotStarted -> 0
    | Done -> 100
    | Started (start, stop) -> 100.0 * ((now.Subtract start).TotalSeconds / (stop.Subtract start).TotalSeconds) |> int |> min 100

let stringForPercent percent = sprintf "[%s%s] %s%%" (String.replicate (percent / 10) "=")
                                                     (String.replicate ((100 - percent) / 10) " ")
                                                     (string percent)

let progressReport now actions = 
    let result = match actions with 
                    | [] -> ""
                    | list -> List.map (fun state -> sprintf "%A (%i): %s" state.Type state.Amount (percentDone now state.Progress |> stringForPercent)) list 
                            |> String.concat "\n"
    match result with
        | "" -> "Nothing in progress.\n"
        | str -> "Progress report:\n" + str

let countPersonType targetPerson population = List.length <| List.filter (fun person -> person = targetPerson) population 

let playerReport (player : Player) = 
    $"Level {player.Level} ({player.XP} XP)
    Population: {player.Population.Length}/{player.PopulationCapacity} 
        Unemployed: {countPersonType Unemployed player.Population}
        Hunter: {countPersonType Hunter player.Population}
        Worker: {countPersonType Worker player.Population}
        Gatherer: {countPersonType Gatherer player.Population}
    Resources:
        Food: {player.Resources.Food}
        Wood: {player.Resources.Wood}
        Stone: {player.Resources.Stone}"