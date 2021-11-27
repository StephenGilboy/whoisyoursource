module ArticleTests

open System
open System.IO
open FSharp.Data
open Xunit

let getHtmlDocument filename =
    let reader = new StreamReader filename
    HtmlDocumen.load(reader)

let 