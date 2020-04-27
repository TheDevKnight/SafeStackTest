module ServiceClient

open Thoth.Fetch
open Shared

let getUrl url = sprintf "https://safestacktest.azurewebsites.net/%s" url

let getKonten () = Fetch.fetchAs<Konto list> (getUrl "konten")

// : Fable.Core.JS.Promise<Konto>
// let addKonto  (konto:Konto) = Fetch.post<unit> ((getUrl "konten"), konto) |> ignore

let addKonto (konto:Konto) =
    promise {
        do! Fetch.post ((getUrl "konten"), konto)
    }

// let updateKonto (konto:Konto) = Fetch.patch ((getUrl "konten"), konto) |> ignore

// let deleteKonto (konto:Konto) = Fetch.post ((getUrl "konten"), konto.Id) |> ignore