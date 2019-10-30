module Population

open Person
open Player
open Resources


let canBuildHouse numberOfHouses resources =
    resources.Wood >= (numberOfHouses * houseWoodRequirement) && resources.Stone >= (numberOfHouses * houseStoneRequirement)


let canIncreasePopulation numberOfPeople player =
    player.PopulationCapacity >= player.Population.Length + numberOfPeople
    && player.Resources.Food >= personFoodRequirement * numberOfPeople


let buildHouse numberOfHouses player =
    let actuallyBuildHouse p =
        { p with PopulationCapacity = p.PopulationCapacity + peoplePerHouse * numberOfHouses
                 Resources = { p.Resources with Wood = p.Resources.Wood - houseWoodRequirement * numberOfHouses; Stone = p.Resources.Wood - houseStoneRequirement * numberOfHouses } }
    if canBuildHouse numberOfHouses player.Resources
        then Result.Ok <| actuallyBuildHouse player
        else Result.Error (player, "not enough wood or stone")


let increasePopulation numberOfPeople player =
    let actuallyIncreasePopulation p = 
        { p with Population = List.append p.Population <| List.init numberOfPeople (fun _ -> Unemployed)}
    if canIncreasePopulation numberOfPeople player
        then Ok <| actuallyIncreasePopulation player
        else Error (player, "not enough food or space")