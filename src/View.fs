module MatoFS.View

open Fable.Helpers.React
open Types

open MatoFS.Constants

module P = Fable.Helpers.React.Props

let inline wormPos (Pos (x,y)) =
    rect [P.Key (x.ToString() + "_" + y.ToString())
          P.Fill "red"
          P.Width width
          P.Height height
          P.X (x * width)
          P.Y (y * height)] []

let wormPositions (positions: Pos list) =
    g []
      [for p in positions do yield wormPos p]

let food (Pos (x, y)) = 
    circle [P.Key "food"
            P.Fill "green"
            P.R (width/2)
            P.Cx (x*width + width/2)
            P.Cy (y*height + height/2)] []

let score { length = len } = (len - initialLength) * scorePerLength

let gameView model _ =
    match model with
    | NotStarted -> div [] [str "Press space to start new game"]
    | GameOver score -> div [] [str ("Game Over, score: " + string score)]
    | Game gs ->
        div [ ]
            [ svg [P.Width (width * gameWidth)
                   P.Height (height * gameHeight)]
                  [ wormPositions gs.worm 
                    food gs.food]
              div [] [str ("Score: " + string (score gs))] ]
