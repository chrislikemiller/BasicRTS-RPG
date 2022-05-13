open System

open Player
open GameWorld
open Person
open GameLogic
open Resources
open Gather
open Util
open Population
open Action
open Strings
open System


// todos:
//      - if tasks cant be completed, check it before the timer starts
//      - tie down people who are in some tasks
//             i.e. if I send 5 people to hunt, I have 5 less hunters temporarily


let player = { Name = ""; Level = 5; XP = 0; PopulationCapacity = 10; Population = [Unemployed]; Resources = { Wood = 20; Stone = 20; Food = 100} }
let initialWorld = { Player = player; OngoingActions = []}
let refreshMenuAction = { Type = RefreshMenu; Amount = 0; CanExecute = None; Execute = None; Progress = Done; Duration = 0 };

let printResult (player : Result<Player, Player * string>) = 
    match player with
        | Ok player -> logAny player; player
        | Error (player, error) -> log error; player

let matchAction str = 
    match str with
    | "\r" -> Ok RefreshMenu
    | "1" -> log gatheringOptions
             let str = readKey ()
             match str with 
                | "1" -> Ok <| Gathering WoodGathering
                | "2" -> Ok <| Gathering StoneGathering
                | _ -> Error "Invalid gathering type"
    | "2" -> Ok Hunting
    | "3" -> Ok BuildHouses
    | "4" -> Ok IncreasePopulation
    | "5" -> log trainOptions
             let str = readKey ()
             match str with 
                | "1" -> Ok <| TrainPeople UnemployedToWorker
                | "2" -> Ok <| TrainPeople UnemployedToHunter
                | "3" -> Ok <| TrainPeople UnemployedToGatherer
                | "4" -> Ok <| TrainPeople WorkerToUnemployed
                | "5" -> Ok <| TrainPeople HunterToUnemployed
                | "6" -> Ok <| TrainPeople GathererToUnemployed
                | _ -> Error "Invalid training type!"
    | "q" | "Q" -> exit 0
    | _ -> Error "Invalid action type!"


let userInput (player : Player) : GameAction = 
    let input = readKey ()
    match matchAction input with
    | Ok RefreshMenu -> refreshMenuAction
    | Ok action -> log "Amount:" 
                   let amount = readAmount ()
                   { Type = action; Amount = amount; CanExecute = None; Execute = None; Duration = getActionDuration action player; Progress = NotStarted }
    | Error str -> logError str; refreshMenuAction


let rec updatePlayer (actions : GameAction list) player = 
    match actions with 
    | [] -> player
    | gameAction :: tail -> updatePlayer tail <| match gameAction.Execute with 
                                                 | None -> player 
                                                 | Some action -> action player


let isCompleted gameAction = match gameAction.Progress with 
                                  | NotStarted -> false
                                  | Done -> true
                                  | Started (_, endTime) -> endTime <= DateTime.Now

let startProgress (gameAction : GameAction) = 
    match gameAction.Progress with
    | NotStarted -> { gameAction with Progress = Started (DateTime.Now, DateTime.Now.AddSeconds gameAction.Duration)}
    | _ -> gameAction

let rec game world =
    let (completedActions, ongoingActions) = List.partition isCompleted world.OngoingActions
    let newPlayer = updatePlayer completedActions world.Player 
    log "Player report:"
    log <| playerReport newPlayer
    log <| progressReport DateTime.Now ongoingActions 
    log "---------------------------------"
    log "Menu:"
    log menuOptions

    let gameAction = act newPlayer <| userInput newPlayer
    Console.Clear()
    // todo: if CanExecute is true, take away the resources
    // todo: when people do activities, make them unavailable
    //   i.e. remove them from the population into a different list
    game { world with Player = newPlayer
                      OngoingActions = match gameAction.CanExecute with 
                                       | None -> [] 
                                       | Some x -> match x with
                                                   | Ok _ -> startProgress gameAction |> List.singleton 
                                                   | Error str -> logError str; [] 
                                       |> List.append ongoingActions }


[<EntryPoint>]
let main _ =
    game initialWorld
    0

