module Article

open System
open FSharp.Data

let getLinks (htmlDocument: HtmlDocument) =
    htmlDocument.Descendants ["a"]

