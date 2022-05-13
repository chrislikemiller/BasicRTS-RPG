module Resources

let peoplePerHouse = 5
let trainingFoodRequirement = 10
let trainingWoodRequirement = 10
let personFoodRequirement = 10
let houseWoodRequirement = 10
let houseStoneRequirement = 10

type Resources =
    {
        Food  : int
        Wood  : int
        Stone : int
    }

type GatheringType = 
    | WoodGathering 
    | StoneGathering

type TrainingType =
    | UnemployedToWorker
    | UnemployedToHunter
    | UnemployedToGatherer
    | HunterToUnemployed
    | WorkerToUnemployed
    | GathererToUnemployed
