open System
open Article
open FSharp.Data

let phrases = ["according to"; "reported"; "is reporting"; "published"]

[<EntryPoint>]
let main argsv : int =
    let link = "https://www.lehighvalleylive.com/news/2021/11/human-trafficking-lawsuit-alleges-pa-mennonite-church-subjected-boys-young-men-to-forced-labor.html"
    let htmlDocument = HtmlDocument.Load(link)
    
    let noop =
        getSourceLinks (htmlDocument, phrases)
        |> Seq.map(fun l ->  $"Phrase: %s{(string)l.Phrase} Text: %s{l.Link.Text} Distance: %i{l.Distance} Link: %s{l.Link.Link}")
        |> Seq.toArray
        |> Array.iter(fun i -> Console.WriteLine(i))
    0