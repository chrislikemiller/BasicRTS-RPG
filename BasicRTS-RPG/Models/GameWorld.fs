module GameWorld

open Player
open Action


type GameAction = 
    {
        Type : ActionType
        Progress : ProgressState
        CanExecute : Result<bool,string> option
        Execute : (Player -> Player) option
        Duration : int
        Amount : int
    }

type World =
    {
        Player : Player
        OngoingActions : GameAction list
    }