module TokensRegexWithEnvTests

open edu.stanford.nlp.ling.tokensregex
open edu.stanford.nlp.pipeline
open edu.stanford.nlp.ling
open NUnit.Framework
open FsUnitTyped
open TokensRegexProvider

let private cls = CoreAnnotations.TokensAnnotation().getClass()

let private pipeline =
    let temp = System.Environment.CurrentDirectory
    System.Environment.CurrentDirectory
        <- __SOURCE_DIRECTORY__ + @"/../../paket-files/nlp.stanford.edu/stanford-corenlp-full-2015-12-09/models/"

    let res = new AnnotationPipeline()
    res.addAnnotator(new TokenizerAnnotator(false, "en"))
    res.addAnnotator(new WordsToSentencesAnnotator(false))
    res.addAnnotator(new POSTaggerAnnotator(false))

    System.Environment.CurrentDirectory <- temp

    res

let tokenize (text:string) =
    let annotation = new Annotation(text)
    pipeline.annotate(annotation)
    annotation.get(cls) :?> java.util.List

let tokens =
    tokenize "Mellitus was the first Bishop of London, the third Archbishop of Canterbury, and a member of the Gregorian mission  sent to England to convert the Anglo-Saxons. He arrived in 601 AD, and was consecrated as Bishop of London in 604."


type MyPattern = TokenSequencePatternProvider<"(?$all /archbishop/ /of/ /canterbury/)">

[<Test>]
let ``Use custom Env with TP`` () =
    let env = TokenSequencePattern.getNewEnv();
    env.setDefaultStringPatternFlags(NodePattern.CASE_INSENSITIVE);

    let pattern = MyPattern(env)

    let matcher = pattern.GetMatcher(tokens)

    matcher.find() |> shouldEqual true
    matcher.``$all``.text |> shouldEqual "Archbishop of Canterbury"
    matcher.CompleteMatch.text |> shouldEqual "Archbishop of Canterbury"

    matcher.find() |> shouldEqual false


type MyPattern2 = TokenSequencePatternProvider<" $NNP [ /is|was/ ] []*? $NNP+ [ \"of\" ] $NNP+ ">

[<Test>]
let ``Use custom Env with TP binded inside`` () =
    let nnpPattern = TokenSequencePattern.compile("[ { tag:\"NNP\" } ]");
    let env = TokenSequencePattern.getNewEnv()
    env.bind("$NNP", nnpPattern)

    let pattern = MyPattern2(env)
    let matcher = pattern.GetMatcher(tokens)

    matcher.find() |> shouldEqual true
    matcher.CompleteMatch.text
    |> shouldEqual "Mellitus was the first Bishop of London"
