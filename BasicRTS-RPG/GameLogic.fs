module GameLogic

open Action
open Player
open Gather
open Population
open Train

let getActionDuration actionType player =
    match actionType with
    | _ -> 15.0

let getActiontype actionState =
    match actionState.Type with
        | RefreshMenu -> (fun _ player -> Ok player)
        | Hunting -> hunt
        | Gathering gatheringType -> getGatherer gatheringType
        | BuildHouses -> buildHouse
        | TrainPeople trainingType -> trainPeople trainingType
        | IncreasePopulation -> increasePopulation


let act actionState =
    let action = getActiontype actionState
    (actionState, action actionState.Amount)