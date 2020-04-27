module Client

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props
open Fetch.Types
open Thoth.Fetch
open Fulma
open Thoth.Json

open Shared
open ServiceClient

type Model =
    { Konten: Konto list }

type Msg =
    | GetKonten
    | GetKontenFinished of Konto list
    | AddKonto of Konto

let init(): Model * Cmd<Msg> = { Konten = [] }, Cmd.none

let update (msg: Msg) (currentModel: Model): Model * Cmd<Msg> =
    match msg with
    | GetKonten -> currentModel, Cmd.OfPromise.perform ServiceClient.getKonten () GetKontenFinished
    | GetKontenFinished konten ->
        let nextModel = { currentModel with Konten = konten }
        nextModel, Cmd.none
    | AddKonto konto ->
        currentModel, Cmd.OfPromise.perform ServiceClient.addKonto konto (fun _ -> GetKonten)

let getKontenTable (konten:Konto list) =
    Table.table [ Table.IsHoverable ]
            [ thead [ ]
                [ tr [ ]
                    [ th [ ] [ str "ID" ]
                      th [ ] [ str "Name" ]
                      th [ ] [ str "AppId" ]
                      th [ ] [ str "DevId" ]
                      th [ ] [ str "CertId" ] ] ]
              tbody [ ]
                [
                  for konto in konten do
                    tr [ ]
                     [ td [ ] [ str (konto.Id.ToString()) ]
                       td [ ] [ str konto.Name ]
                       td [ ] [ str konto.AppId ]
                       td [ ] [ str konto.DevId ]
                       td [ ] [ str konto.CertId ] ]
                       ] ]

let view (model: Model) (dispatch: Msg -> unit) =
    div []
        [

          getKontenTable model.Konten

          Container.container [ Container.IsFluid ]
              [ Content.content []
                    [ Button.button [ Button.OnClick(fun _ -> dispatch GetKonten) ] [ str ("Get Konten") ] ] ]
          Container.container [ Container.IsFluid ]
              [ Content.content []
                    [ Button.button
                        [ Button.OnClick(fun _ ->
                            dispatch
                                (AddKonto
                                    { Id = 0
                                      Name = "Test"
                                      AppId = "A"
                                      DevId = "B"
                                      CertId = "C" })) ] [ str ("Add Konto") ] ] ] ]

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactBatched "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
