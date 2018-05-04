module App

open Fable.Core.JsInterop
open Fable.Import.Browser
open Elmish
open Elmish.React
open MatoFS.Types
open MatoFS.State
open MatoFS.View

let (|ArrowKey|_|) keyCode =
    match keyCode with
    | 37 -> Some Left
    | 39 -> Some Right
    | 38 -> Some Up
    | 40 -> Some Down
    | _ -> None

let (|SpaceKey|_|) keyCode = 
    if keyCode = 32 then Some () else None

let timerAndKeyboard _ =
    let sub dispatch =
        window.setInterval ((fun _ -> dispatch Update), 100) |> ignore
        let handler = fun e ->
            match !!e?keyCode with
            | ArrowKey d -> dispatch (ChangeDirection d) |> ignore
            | SpaceKey -> dispatch StartGame |> ignore
            | _ -> None |> ignore
        window.addEventListener("keydown", unbox handler) |> ignore
    Cmd.ofSub sub

// App
Program.mkProgram init update gameView
|> Program.withReact "elmish-app"
|> Program.withSubscription timerAndKeyboard
|> Program.run
