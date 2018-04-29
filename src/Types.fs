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

type GameState = {
  worm: Pos list
  direction: Direction
  length: int
  food: Pos
}
 
type Model =
  | NotStarted 
  | Game of GameState
  | GameOver of int