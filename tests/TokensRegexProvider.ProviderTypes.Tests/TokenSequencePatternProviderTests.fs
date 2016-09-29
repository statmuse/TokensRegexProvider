module TokenSequencePatternProviderTests

open NUnit.Framework
open FsUnitTyped
open Helpers

open TokensRegexProvider

type FiveOrSeqPattern = TokenSequencePatternProvider<"(?$my five|5|5x|5.0|-5|5L) (?$or or)">

[<Test>]
let ``Match text by FiveOrSeqPattern`` () =
    let tokens = tokenize "the number is five or 5 or 5.0 or but not 5x or -5 or 5L."
    let pattern = FiveOrSeqPattern()
    let matcher = pattern.GetMatcher(tokens)

    matcher.find() |> shouldEqual true
    matcher.``$my``.text |> shouldEqual "five"
    matcher.``$or``.text |> shouldEqual "or"
    matcher.CompleteMatch.text |> shouldEqual "five or"

    matcher.find() |> shouldEqual true
    matcher.``$my``.text |> shouldEqual "5"
    matcher.``$or``.text |> shouldEqual "or"
    matcher.CompleteMatch.text |> shouldEqual "5 or"

    matcher.find() |> shouldEqual true
    matcher.``$my``.text |> shouldEqual "5.0"
    matcher.``$or``.text |> shouldEqual "or"
    matcher.CompleteMatch.text |> shouldEqual "5.0 or"

    matcher.find() |> shouldEqual true
    matcher.``$my``.text |> shouldEqual "5x"
    matcher.``$or``.text |> shouldEqual "or"
    matcher.CompleteMatch.text |> shouldEqual "5x or"

    matcher.find() |> shouldEqual true
    matcher.``$my``.text |> shouldEqual "-5"
    matcher.``$or``.text |> shouldEqual "or"
    matcher.CompleteMatch.text |> shouldEqual "-5 or"

    matcher.find() |> shouldEqual false


