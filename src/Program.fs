open System
open RedditAuth

[<EntryPoint>]
let main argsv : int =
    let creds = getRedditClientCredentials

    match creds with
    | Ok c -> displayCredentials c |> ignore
    | Error msg -> printfn "Error: %s" msg
    
    0
