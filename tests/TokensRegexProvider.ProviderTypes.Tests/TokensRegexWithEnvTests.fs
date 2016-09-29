module TokensRegexWithEnvTests

open edu.stanford.nlp.ling.tokensregex

open NUnit.Framework
open FsUnitTyped
open Helpers


open TokensRegexProvider

type MyPattern = TokenSequencePatternProvider<"(?$all /archbishop/ /of/ /canterbury/)">

[<Test>]
let ``Use custom Env with TP`` () =
    let env = TokenSequencePattern.getNewEnv();
    env.setDefaultStringPatternFlags(NodePattern.CASE_INSENSITIVE);

    let pattern = MyPattern(env)

    let tokens = tokenize "Mellitus was the first Bishop of London, the third Archbishop of Canterbury, and a member of the Gregorian mission  sent to England to convert the Anglo-Saxons. He arrived in 601 AD, and was consecrated as Bishop of London in 604."
    let matcher = pattern.GetMatcher(tokens)

    matcher.find() |> shouldEqual true
    matcher.``$all``.text |> shouldEqual "Archbishop of Canterbury"
    matcher.CompleteMatch.text |> shouldEqual "Archbishop of Canterbury"

    matcher.find() |> shouldEqual false


