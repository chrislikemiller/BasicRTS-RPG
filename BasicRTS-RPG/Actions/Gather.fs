module Gather

open Player
open Resources


let gather gatherThis amount player = Result.Ok { player with Resources = gatherThis player.Resources amount }

let wood resources amount = { resources with Wood = resources.Wood + amount }
let stone resources amount = { resources with Stone = resources.Stone + amount }

let getGatherer gatherType = 
    match gatherType with
        | WoodGathering -> gather wood
        | StoneGathering -> gather stone 

let hunt amount player = Result.Ok { player with Resources = { player.Resources with Food = player.Resources.Food + amount } }