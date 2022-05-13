module Test

//open Player
//open GameWorld
//open Person
//open GameLogic
//open Resources
//open Gather
//open Util
//open Population
//open Action
//open Train

//let player = { Name = ""; Level = 5; XP = 0; PopulationCapacity = 10; Population = []; Resources = { Wood = 0; Stone = 0; Food = 0} }


//// test xp gain
//// test levelup
//let test : string list =
//    [
//        sprintf "gather 10 wood: %b\n" <| ((player |> doGather wood 10)  = Result.Ok { player with Resources = { player.Resources with Wood = player.Resources.Wood + 10 } })
//        sprintf "gather 10 stone: %b\n" <| ((player |> doGather stone 10) = Result.Ok { player with Resources = { player.Resources with Stone = player.Resources.Stone + 10 } })
//        sprintf "gather 10 food: %b\n" <| ((player |> hunt 10)  = Result.Ok { player with Resources = { player.Resources with Food = player.Resources.Food + 10 } })
//        sprintf "can build 1 house: %b\n"
//            <| match player 
//                     |> doGather wood 10 
//                     >>= doGather stone 10 
//                     >>= buildHouse 1 with
//                | Ok player -> player.PopulationCapacity = 15
//                | Error (_, error) -> log error; false
//        sprintf "can increase population by 10: %b\n"
//            <| match player 
//                     |> hunt 100  
//                     >>= doGather wood 20 
//                     >>= doGather stone 20
//                     >>= buildHouse 2 
//                     >>= increasePopulation 10 with
//                | Ok player -> player.Population.Length = 10
//                | Error (_, error) -> log error; false
//        sprintf "can train 1 unemployed to worker: %b\n"
//            <| match player 
//                    |> hunt 10
//                    >>= increasePopulation 1
//                    >>= trainPeople TrainingType.UnemployedToWorker 1 with
//                | Ok player -> player.Population.Head = Worker
//                | Error (_, error) -> log error; false
//        sprintf "can train 1 worker to hunter: %b\n"
//            <| match player 
//                    |> hunt 10
//                    >>= doGather wood 10
//                    >>= increasePopulation 1
//                    >>= trainPeople TrainingType.UnemployedToWorker 1 
//                    >>= trainPeople TrainingType.WorkerToUnemployed 1 with
//                | Ok newPlayer -> newPlayer.Population.Head = Hunter
//                | Error (_, error) -> log error; false
//    ]