module RedditAuth

open System

type RedditClientCredentials = {
    DeviceId: string
    ClientId: string
    ClientSecret: string
}

let getDeviceId = 
    match Environment.GetEnvironmentVariable "DEVICE_ID" with
    | null -> Error "Device Id is not defined"
    | deviceId -> Ok deviceId

let getRedditKey =
    match Environment.GetEnvironmentVariable "REDDIT_KEY" with
    | null -> Error "Reddit Key is not defined"
    | key -> Ok key

let getRedditSecret =
    match Environment.GetEnvironmentVariable "REDDIT_SECRET" with
    | null -> Error "Reddit Secret is not defined"
    | secret -> Ok secret


let getRedditClientCredentials = 
    match getDeviceId with
    | Error msg -> Error msg
    | Ok deviceId ->
        match getRedditKey with
        | Error msg -> Error msg
        | Ok redditKey ->
            match getRedditSecret with
            | Error msg -> Error msg
            | Ok redditSecret -> Ok {
                DeviceId = deviceId
                ClientId = redditKey
                ClientSecret = redditSecret
            }

let displayCredentials creds =
    printfn "DeviceId: %s\nKey: %s\n Secret: %s" creds.DeviceId creds.ClientId creds.ClientSecret 
    creds
