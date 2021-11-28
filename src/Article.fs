module Article

open System
open FSharp.Data

type Phrase = Phrase of string

type Atag = {
    Text: string
    Link: string
}

type LinkTextIndex = {
    Link: Atag
    Index: int
    Text: string
}

type PhraseTextIndex = {
    Phrase: Phrase
    Index: int
    Text: string
}

type ParagraphPhrase = {
    P: HtmlNode
    Phrase: Phrase
    Text: string
    Index: int
}

type LinkPhraseDistance = {
    Link: Atag
    Phrase: Phrase
    Distance: int
}

let findPhraseInText (phrase: string, text: string) =
    let index = text.ToLower().IndexOf(phrase)
    match index >= 0 with
    | false -> None
    | true -> Some {
            Phrase = Phrase(phrase)
            Text = text
            Index = index
        }

let findPhrasesInText (phrases: List<string>, text: string) =
    phrases
    |> Seq.choose(fun p -> findPhraseInText (p, text))


let findLinkInText (aTag: Atag, text: string) =
    let aTagIndex = text.IndexOf(aTag.Text)
    match aTagIndex >= 0 with 
    | false -> None
    | true -> Some {
            Link = aTag
            Text = text
            Index = aTagIndex
        }

let findLinksInText (aTags: List<Atag>, text: string) =
    aTags
    |> Seq.choose(fun a -> findLinkInText(a, text))

let getParagraphPhrases (htmlDocument: HtmlDocument, phrases: List<string>) =
    htmlDocument.Descendants ["p"]
    |> Seq.map (fun p -> 
        let text = p.InnerText()
        findPhrasesInText (phrases, text)
        |> Seq.map (fun x -> 
                {
                    P = p 
                    Phrase = x.Phrase
                    Text = text
                    Index = x.Index
                }
            )
        )
    |> Seq.reduce(fun a b -> Seq.append b a )


let getLinksFromPtag (p: HtmlNode) =
    p.Descendants ["a"]
    |> Seq.map(fun a -> {
        Text = a.InnerText()
        Link = a.AttributeValue("href")
        }
     )

let getSourceLinks (htmlDocument: HtmlDocument, phrases: List<string>) =
    getParagraphPhrases (htmlDocument, phrases)
    |> Seq.map(fun pp -> 
            getLinksFromPtag pp.P
            |> Seq.choose(fun l -> findLinkInText (l, pp.Text))
            |> Seq.map(fun lt -> {
                Link = lt.Link
                Phrase = pp.Phrase
                Distance = lt.Index - pp.Index |> Math.Abs
            })
            |> Seq.reduce(fun a b ->
                match a.Distance <= b.Distance with
                | true -> a
                | false -> b)
            )
    |> Seq.reduce(fun a b ->
        match a.Distance <= b.Distance with
        | true -> a
        | false -> b)
    
