module Resources

let peoplePerHouse = 5
let retrainingFoodRequirement = 10
let retrainingWoodRequirement = 10
let personFoodRequirement = 10
let houseWoodRequirement = 10
let houseStoneRequirement = 10


type ResourceType = Wood | Stone | Food

type Resources =
    {
        Food  : int
        Wood  : int
        Stone : int
    }

type GatheringType = WoodGathering | StoneGathering

type TrainingType =
    | UnemployedToWorker
    | UnemployedToHunter
    | HunterToWorker
    | WorkerToHunter
