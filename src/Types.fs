module MatoFS.Types

type Pos = Pos of int * int

type Direction =
    | Up
    | Down
    | Left
    | Right

type Msg =
    | ChangeDirection of Direction
    | Update
    | StartGame

type GameState = 
    { Worm: Pos list
      Direction: Direction
      Length: int
      Food: Pos }
 
type Model =
    | NotStarted 
    | Game of GameState
    | GameOver of int