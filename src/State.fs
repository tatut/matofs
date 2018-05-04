module MatoFS.State

open System
open Elmish
open MatoFS.Types
open MatoFS.Constants

let isInsideGameArea (Pos (x, y)) =
    x >= 0 && x < gameWidth 
           && y >= 0 
           && y < gameHeight

let containsPos (worm:Pos list) (pos:Pos) =
    List.contains pos worm

let isCrashed = function
    | [] -> false
    | hd::tail -> List.contains hd tail

let r = Random()

let randomPos () = 
    Pos (r.Next gameWidth, r.Next gameHeight)

let rec randomPosNotIn positions = 
    let p = randomPos ()
    if List.contains p positions 
    then randomPosNotIn positions
    else p

let init _ = NotStarted, Cmd.ofMsg Update

let startGame () = 
    Game { Worm = [ Pos (10, 10) ]
           Direction = Up
           Length = initialLength
           Food = randomPos () }

let nextPos (Pos (cx, cy)) direction =
    match direction with
    | Up -> Pos (cx, cy - 1)
    | Down -> Pos (cx, cy + 1)
    | Left -> Pos (cx - 1, cy)
    | Right -> Pos (cx + 1, cy)

let move model = 
    let pos = List.head model.Worm
    let newWorm = nextPos pos model.Direction :: model.Worm
    let newWorm' = 
        if List.length newWorm < model.Length
        then newWorm
        else List.take model.Length newWorm
    { model with Worm = newWorm' }

let eat model =
    let pos = List.head model.Worm 
    if pos = model.Food 
    then
        { model with 
            Food = randomPosNotIn model.Worm
            Length = model.Length + growth }
    else 
        model    

let isGameOver gs = 
    let pos = List.head gs.Worm
    not (isInsideGameArea pos) || isCrashed gs.Worm

let updateGame msg game = 
    match msg with
    | ChangeDirection d ->
        Game { game with Direction = d }
    | Update ->
        let newGameState = game |> move |> eat
        if isGameOver newGameState 
        then GameOver ((game.Length - 5) * scorePerLength)
        else Game newGameState
    | _ -> Game game

let update msg model =
    match model with 
    | Game gs ->
        updateGame msg gs, []
    | _ -> // NotStarted or GameOver
        match msg with
        | StartGame -> 
            startGame (), []
        | _ -> model, []
    
  