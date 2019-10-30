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
open Train


let player = { Name = ""; Level = 5; XP = 0; PopulationCapacity = 10; Population = []; Resources = { Wood = 0; Stone = 0; Food = 0} }
let initialWorld = { Player = player; OngoingActions = []}


let menuOptions = "Choose: \n\t1. Gather \n\t2. Hunt \n\t3. Build houses \n\t4. Increase population \n\t5. Train people \n\tQ. Exit"
let trainOptions = "1. Unemployed to worker \t\n2. Unemployed to hunter \t\n3. Hunter to worker \t\n4. Worker to hunter"
let gatheringOptions = "1. Wood \t\n2. Stone"

let printResult (player : Result<Player, Player * string>) = 
    match player with
        | Ok player -> logAny player; player
        | Error (player, error) -> log error; player


let rec userInput () : ActionState = 
    let str = read ()
    let actionType = match str with
                        | "1" -> log gatheringOptions
                                 let str = read ()
                                 match str with 
                                    | "1" -> Gathering GatheringType.WoodGathering
                                    | "2" -> Gathering GatheringType.StoneGathering
                                    | _ -> log "Invalid gathering type"
                                           RefreshMenu
                        | "2" -> Hunting
                        | "3" -> BuildHouses
                        | "4" -> IncreasePopulation
                        | "5" -> log trainOptions
                                 let str = read ()
                                 match str with 
                                    | "1" -> TrainPeople TrainingType.UnemployedToWorker
                                    | "2" -> TrainPeople TrainingType.UnemployedToHunter
                                    | "3" -> TrainPeople TrainingType.HunterToWorker
                                    | "4" -> TrainPeople TrainingType.WorkerToHunter
                                    | _ -> log "Invalid training type!"
                                           RefreshMenu
                        | "" -> RefreshMenu
                        | "q" | "Q" -> exit 0
                        | _ -> log "Invalid action type!"
                               RefreshMenu
    match actionType with
    | RefreshMenu -> { Type = RefreshMenu; Amount = 0; Duration = (DateTime.Now, DateTime.Now) }
    | _ ->  log "What's the amount?"
            let amount = int <| read ()
            { Type = actionType; Amount = amount; Duration = (DateTime.Now, DateTime.Now.AddSeconds <| getActionDuration actionType player) }
    
let isCompleted (state, _) = (snd state.Duration) <= DateTime.Now

let rec applyActions player actions = 
    match actions with 
    | (_, action) :: tail -> 
        applyActions (match action player with
                     | Ok p -> p
                     | Error (p, error) -> log error; p) 
                     tail
    | [] -> player

let rec game world =
    let (completed, ongoing) = List.partition isCompleted world.OngoingActions
    logAny ongoing
    log "\nProgress report:"
    log <| progressReport DateTime.Now ongoing 
    log "\n\nMenu:"
    log menuOptions
        
    game { world with Player = completed |> applyActions world.Player
                      OngoingActions = userInput () 
                                          |> act
                                          |> List.singleton
                                          |> List.append ongoing }

// test xp gain
// test levelup
let test : string list =
    [
        sprintf "gather 10 wood: %b\n" <| ((player |> gather wood 10)  = Result.Ok { player with Resources = { player.Resources with Wood = player.Resources.Wood + 10 } })
        sprintf "gather 10 stone: %b\n" <| ((player |> gather stone 10) = Result.Ok { player with Resources = { player.Resources with Stone = player.Resources.Stone + 10 } })
        sprintf "gather 10 food: %b\n" <| ((player |> hunt 10)  = Result.Ok { player with Resources = { player.Resources with Food = player.Resources.Food + 10 } })
        sprintf "can build 1 house: %b\n"
            <| match player 
                     |> gather wood 10 
                     >>= gather stone 10 
                     >>= buildHouse 1 with
                | Ok player -> player.PopulationCapacity = 15
                | Error (_, error) -> log error; false
        sprintf "can increase population by 10: %b\n"
            <| match player 
                     |> hunt 100  
                     >>= gather wood 20 
                     >>= gather stone 20
                     >>= buildHouse 2 
                     >>= increasePopulation 10 with
                | Ok player -> player.Population.Length = 10
                | Error (_, error) -> log error; false
        sprintf "can train 1 unemployed to worker: %b\n"
            <| match player 
                    |> hunt 10
                    >>= increasePopulation 1
                    >>= trainPeople TrainingType.UnemployedToWorker 1 with
                | Ok player -> player.Population.Head = Worker
                | Error (_, error) -> log error; false
        sprintf "can train 1 worker to hunter: %b\n"
            <| match player 
                    |> hunt 10
                    >>= gather wood 10
                    >>= increasePopulation 1
                    >>= trainPeople TrainingType.UnemployedToWorker 1 
                    >>= trainPeople TrainingType.WorkerToHunter 1 with
                | Ok newPlayer -> newPlayer.Population.Head = Hunter
                | Error (_, error) -> log error; false
    ]

[<EntryPoint>]
let main argv =
    //log test
    game initialWorld
    0

