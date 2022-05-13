module Action

open Resources
open System

type ActionType =
    | Gathering of GatheringType
    | Hunting
    | BuildHouses
    | IncreasePopulation
    | TrainPeople of TrainingType
    | RefreshMenu

type ProgressState =
    | NotStarted
    | Started of DateTime * DateTime
    | Done

type GatheringAction =
    {
        Type  : GatheringType
        Level : int
    }

type TrainingAction =
    {
        Type  : TrainingType
        Level : int
    }

//type Action =
//    {
//        Type : ActionType
//        Level : int
//    }
