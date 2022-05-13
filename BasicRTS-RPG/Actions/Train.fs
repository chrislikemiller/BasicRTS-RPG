module Train


open Population
open Player
open Person
open Resources

let matchPerson trainingType person =
    match trainingType with
    | HunterToUnemployed -> person = Hunter
    | WorkerToUnemployed -> person = Worker
    | GathererToUnemployed -> person = Gatherer
    | _ -> person = Unemployed

let trainPerson trainingType =
    match trainingType with
    | UnemployedToHunter -> Hunter
    | UnemployedToWorker -> Worker
    | UnemployedToGatherer -> Gatherer
    | _ -> Unemployed


let train from change n population = 
    population 
    |> List.mapFold (fun i person -> 
        if from person && i < n 
        then (change person, i + 1) 
        else (person, i)) 
        0
    |> fst


let canTrainPeople trainingType player amount  = 
    let result = match trainingType with
                 | UnemployedToHunter | UnemployedToWorker | UnemployedToGatherer 
                    -> player.Resources.Food >= trainingFoodRequirement * amount && player.Resources.Wood >= trainingWoodRequirement * amount
                 | _ -> true
    Some <| if result && amount <= (player.Population |> (matchPerson trainingType |> List.filter) |> List.length) 
            then Ok true else Error "not enough food or space"

let updateResources trainingType amount resources =
    match trainingType with 
    | UnemployedToHunter | UnemployedToWorker | UnemployedToGatherer 
        -> { resources with Food = resources.Food - amount * trainingFoodRequirement  
                            Wood = resources.Wood - amount * trainingWoodRequirement }
    | _ -> resources

let trainPeople trainingType amount player = 
     { player with Resources = updateResources trainingType amount player.Resources 
                   Population = train (fun person -> matchPerson trainingType person) 
                                      (fun _ -> trainPerson trainingType) 
                                      amount 
                                      player.Population } 
