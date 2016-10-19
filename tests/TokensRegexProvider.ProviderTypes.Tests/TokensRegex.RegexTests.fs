module TokenSequencePatternProviderConfigTests

open TokensRegexProvider
open NUnit.Framework
open FsUnitTyped

[<Test>]
let ``simple pattern with 2 groups`` () =
    "(?$my five|5|5x|5.0|-5|5L)"
    |> TokenSequencePatternProviderConfig.getGroupName
    |> shouldEqual [|"$my"|]


[<Test>]
let ``simple pattern with 3 groups`` () =
    "(?$my five|5|5x|5.0|-5|5L) (?$or or)"
    |> TokenSequencePatternProviderConfig.getGroupName
    |> shouldEqual [|"$my"; "$or"|]
