module Gather

open Player
open Resources
open Common


let canGather player amount = Some <| if amount <= countProfession player.Population Person.Gatherer then Ok true else Error "not enough gatherers"
let canHunt player amount = Some <| if amount <= countProfession player.Population Person.Hunter then Ok true else Error "not enough hunters"

let doGather gatherfunc amount player = { player with Resources = gatherfunc player.Resources amount }


// todo: later determine how much each unit of hunting/hunting takes
// adjust time proportionately
// for now, use static 50

let hunt amount player = { player with Resources = { player.Resources with Food = player.Resources.Food +  amount * 50 } } 

let wood resources amount = { resources with Wood = resources.Wood + amount * 50 }
let stone resources amount = { resources with Stone = resources.Stone + amount * 50}

let gather gatherType = 
    match gatherType with
        | WoodGathering -> doGather wood
        | StoneGathering -> doGather stone 
