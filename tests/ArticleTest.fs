module ArticleTests

open System
open System.IO
open Article
open FSharp.Data
open Xunit

let getHtmlDocument (filename: string) =
    let reader = new StreamReader(filename)
    HtmlDocument.Load(reader)

[<Fact>]
let ``findPhraseInText should return Some PhraseTextIndex when it contains a given pharse`` () =
    let phrase = "according to"
    let text = "Something is making the news according to the AP."
    let want = {
        Phrase = Phrase(phrase)
        Text = text
        Index = 29
    }
    let pti = findPhraseInText (phrase, text)
    match pti with
    | Some got -> 
        Assert.Equal(want, got)
    | None -> Assert.True(false, "findPhraseInText returned None")

[<Fact>]
let ``findLinkInText should Some LinkTextIndex the link text is found in the text`` () =

    let text = "Something is making the news says the AP."
    let want = {
        Link = {
            Text = "the AP"
            Link = "https://www.ap.org"
        }
        Text = text
        Index = 34
    }
    let lti = findLinkInText (want.Link, text)
    match lti with
    | None -> Assert.True(false, "Link was not found in the text")
    | Some got -> 
        Assert.Equal(want, got)

[<Fact>]
let ``getSourceLink should return the link that the article is using for a source`` () =
    let sourcePhrases = ["according to"; "reported"; "is reporting"; "published"]
    let htmlDocument = getHtmlDocument "./test-data/accordingto.html"
    let want = {
        Link = {
            Text = "Local News 11"
            Link = "https://localnews11.com/original-story"
        }
        Phrase = Phrase("according to")
        Distance = 13
    }
    let lpd = getSourceLinks (htmlDocument, sourcePhrases)
    Assert.Equal(want, lpd)

