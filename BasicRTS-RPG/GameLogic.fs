module GameLogic

open Action
open Player
open Gather
open Population
open Train
open GameWorld

let getActionDuration actionType player =
    match actionType with
    | _ -> 5

let act (player : Player) (gameAction : GameAction) : GameAction =
    match gameAction.Type with
        | RefreshMenu -> gameAction
        | Gathering gatheringType -> { gameAction with CanExecute = canGather player gameAction.Amount; Execute = Some <| gather gatheringType gameAction.Amount }
        | Hunting -> { gameAction with CanExecute = canHunt player gameAction.Amount; Execute = Some <| hunt gameAction.Amount }
        | BuildHouses -> { gameAction with CanExecute = canBuildHouse player gameAction.Amount; Execute = Some <| buildHouse gameAction.Amount }
        | TrainPeople trainingType -> { gameAction with CanExecute = canTrainPeople trainingType player gameAction.Amount; Execute = Some <| trainPeople trainingType gameAction.Amount }
        | IncreasePopulation -> { gameAction with CanExecute = canIncreasePopulation player gameAction.Amount; Execute = Some <| increasePopulation gameAction.Amount }

