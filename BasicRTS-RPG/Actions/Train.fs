module Train


open Population
open Player
open Person
open Resources

let matchPerson trainingType person =
    match trainingType with
    | UnemployedToHunter -> person = Unemployed
    | UnemployedToWorker -> person = Unemployed
    | HunterToWorker -> person = Hunter
    | WorkerToHunter -> person = Worker

let trainPerson trainingType =
    match trainingType with
    | UnemployedToHunter -> Hunter
    | UnemployedToWorker -> Worker
    | HunterToWorker -> Worker
    | WorkerToHunter -> Hunter


let train from change n population = 
    population 
    |> List.mapFold (fun i person -> 
        if from person && i < n 
        then (change person, i + 1) 
        else (person, i)) 
        0
    |> fst


let meetsResourceRequirement trainingType player = 
    match trainingType with
    | HunterToWorker -> player.Resources.Food >= 10 && player.Resources.Wood >= 10
    | WorkerToHunter -> player.Resources.Food >= 10 && player.Resources.Wood >= 10
    | _ -> true

let updateResources trainingType count resources =
    match trainingType with 
    | HunterToWorker -> { resources with Food = resources.Food - count * retrainingFoodRequirement  
                                         Wood = resources.Wood - count * retrainingWoodRequirement }
    | WorkerToHunter -> { resources with Food = resources.Food - count * retrainingFoodRequirement  
                                         Wood = resources.Wood - count * retrainingWoodRequirement }
    | _ -> resources

let trainPeople trainingType count player =
    if meetsResourceRequirement trainingType player
       && count <= (player.Population 
                    |> (List.filter <| matchPerson trainingType) 
                    |> List.length) 
    then Ok { player with Population = train (fun person -> matchPerson trainingType person) 
                                             (fun _ -> trainPerson trainingType) 
                                             count 
                                             player.Population 
                          Resources = updateResources trainingType count player.Resources } 
    else Error (player, "Cannot train, not enough space or not enough food.") 
