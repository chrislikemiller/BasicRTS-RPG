module GameWorld

open Player
open Action

type World =
    {
        Player : Player
        OngoingActions : (ActionState * (Player -> Result<Player, Player * string>)) list
    }