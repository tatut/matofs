module App

open Elmish
open Fable.Core.JsInterop
open Fable.Import.Browser
open MatoFS.Types
open MatoFS.State
open MatoFS.View

open Elmish.React

let keyDirection = function
    | "ArrowLeft" -> Some Left
    | "ArrowRight" -> Some Right
    | "ArrowUp" -> Some Up
    | "ArrowDown" -> Some Down
    | _ -> None

let timerAndKeyboard _ =
    let sub dispatch =
        window.setInterval ((fun _ -> dispatch Update), 100) |> ignore
        let handler = fun e ->
            if !!e?key = " " then
                dispatch StartGame |> ignore
            else 
                match keyDirection !!e?key with
                | Some d -> dispatch (ChangeDirection d) |> ignore
                | None -> None |> ignore
        window.addEventListener("keydown", unbox handler ) |> ignore
    Cmd.ofSub sub


// App
Program.mkProgram init update gameView
|> Program.withReact "elmish-app"
|> Program.withSubscription timerAndKeyboard
|> Program.run
