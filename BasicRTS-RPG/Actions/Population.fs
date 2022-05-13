module Population

open Person
open Player
open Resources
open Common


let canBuildHouse player amount = 
    Some <| if amount <= countProfession player.Population Worker && player.Resources.Wood >= (amount * houseWoodRequirement) && player.Resources.Stone >= (amount * houseStoneRequirement)
            then Ok true
            else Error "not enough wood or stone"


let canIncreasePopulation player numberOfPeople  = 
    Some <| if player.PopulationCapacity >= player.Population.Length + numberOfPeople && player.Resources.Food >= personFoodRequirement * numberOfPeople 
        then Ok true 
        else Error "not enough food or space"


let buildHouse numberOfHouses player =
        { player with PopulationCapacity = player.PopulationCapacity + peoplePerHouse * numberOfHouses
                      Resources = { player.Resources with Wood = player.Resources.Wood - houseWoodRequirement * numberOfHouses; 
                                                          Stone = player.Resources.Stone - houseStoneRequirement * numberOfHouses } }


let increasePopulation numberOfPeople player = { player with Population = List.append player.Population <| List.init numberOfPeople (fun _ -> Unemployed)
                                                             Resources = { player.Resources with Food = player.Resources.Food - personFoodRequirement * numberOfPeople} }
    