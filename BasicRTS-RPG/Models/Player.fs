module Player

open Resources
open Person
open System.Threading.Tasks
open Action

//[<StructuralEquality>]
type Player =
    {
        Name : string
        Level : int
        XP : int
        Population : Person list
        PopulationCapacity : int
        Resources : Resources
    }

