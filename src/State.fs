module MatoFS.State

open Elmish
open MatoFS.Types
open System

open MatoFS.Constants

let isInsideGameArea (Pos (x, y)) =
    x >= 0 && x < gameWidth && y >= 0 && y < gameHeight

let containsPos (worm:Pos list) (pos:Pos) =
    List.contains pos worm

let isCrashed = function
    | [] -> false
    | (hd::tail) -> List.contains hd tail

let r = Random()

let randomPos () = Pos (r.Next gameWidth, r.Next gameHeight)

let rec randomPosNotIn positions = 
    let p = randomPos ()
    if List.contains p positions then 
        randomPosNotIn positions
    else 
        p

let init _ = NotStarted, Cmd.ofMsg Update

let startGame () = 
    Game { worm = [ Pos (10, 10) ]; direction = Up; length = initialLength; food = randomPos () }

let nextPos (Pos (cx, cy)) direction : Pos =
    match direction with
    | Up -> Pos (cx, cy - 1)
    | Down -> Pos (cx, cy + 1)
    | Left -> Pos (cx - 1, cy)
    | Right -> Pos (cx + 1, cy)


let move model = 
    let pos = List.head model.worm
    let newWorm =  nextPos pos model.direction :: model.worm
    let newWorm' = if List.length newWorm < model.length then
                    newWorm
                   else 
                    List.take model.length newWorm
    { model with worm = newWorm' }

let eat model =
    let pos = List.head model.worm 
    if pos = model.food then
        { model with food = randomPosNotIn model.worm ; length = model.length + growth }
    else 
        model    

let isGameOver gs = 
    let pos = List.head gs.worm
    not (isInsideGameArea pos) || isCrashed gs.worm

let updateGame msg game = 
    match msg with
    | ChangeDirection d ->
        Game { game with direction = d }
    | Update ->
        let newGameState = game |> move |> eat
        if isGameOver newGameState then
            GameOver ((game.length-5) * scorePerLength)
        else 
            Game newGameState
    | _ -> Game game

let update msg model =
    match model with 
    | Game (gs:GameState) ->
        updateGame msg gs, []
    | _ -> // NotStarted or GameOver
        match msg with
        | StartGame -> 
            startGame (), []
        | _ -> model, []
    
  