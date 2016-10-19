module TokensRegexProvider.TokenSequencePatternProviderConfig

open System
open System.Text.RegularExpressions
open ProviderImplementation.ProvidedTypes
open Microsoft.FSharp.Quotations
open edu.stanford.nlp.ling.tokensregex

let internal regex =
    new Regex(@"\(\?(?<name>\$[A-Z]?[0-9A-Z]+)",
              RegexOptions.Compiled ||| RegexOptions.IgnoreCase)

let internal getGroupName (pattern:string) :string[] =
    [|
        for x in regex.Matches(pattern) do
            yield x.Groups.["name"].Value
    |]

let internal createTypes () =
    let baseTy = typeof<obj>
    let tokenSeqType =
        ProvidedTypeDefinition(Constants.thisAssembly, Constants.rootNamespace, "TokenSequencePatternProvider", Some(baseTy))

    tokenSeqType.DefineStaticParameters(
        parameters = [ ProvidedStaticParameter ("pattern", typeof<string>)
                       ProvidedStaticParameter ("separator", typeof<string>, "\n") ],
        instantiationFunction = (fun typeName parameterValues ->
            match parameterValues with
            | [| :? string as pattern; :? string as separator |] ->
                let matcherType = ProvidedTypeDefinition("MatcherType", Some typeof<TokenSequenceMatcher>, IsErased = true)

                let completeMatch =
                    ProvidedProperty(
                        propertyName = "CompleteMatch",
                        propertyType = typeof<SequenceMatchResult.MatchedGroupInfo>,
                        GetterCode = (fun args -> <@@ let matcher = %%args.[0]:TokenSequenceMatcher
                                                      matcher.groupInfo(0) @@>)
                    )
                completeMatch.AddXmlDoc("Gets the complete pattern match")
                matcherType.AddMember completeMatch

                let patterns =
                    pattern.Split([|separator|], StringSplitOptions.None)
                    |> Array.map (fun x -> x.Trim())
                    |> Array.filter (String.IsNullOrWhiteSpace >> not)

                patterns
                |> Array.map getGroupName
                |> Array.concat
                |> List.ofArray
                |> List.distinct
                |> List.map (fun group ->
                    let property =
                        ProvidedProperty(
                            propertyName = group,
                            propertyType = typeof<SequenceMatchResult.MatchedGroupInfo>,
                            GetterCode = (fun args -> <@@ let matcher = %%args.[0]:TokenSequenceMatcher
                                                          matcher.groupInfo(group) @@>)
                        )
                    property.AddXmlDoc(sprintf @"Gets the '%s' group from this match" group)
                    property
                )
                |> matcherType.AddMembers

                let ty = ProvidedTypeDefinition(Constants.thisAssembly, Constants.rootNamespace, typeName, Some(baseTy), IsErased = true)
                ty.AddXmlDoc <| sprintf "A strongly typed interface to the regular TokenSequencePattern '%A'" patterns

                ty.AddMember matcherType

                let ctor = ProvidedConstructor([], InvokeCode = (fun args -> <@@ TokenSequencePattern.compile(patterns) @@>))
                ctor.AddXmlDoc "Initializes a TokenSequencePattern instance in new Env"
                ty.AddMember ctor

                let ctorEnv = ProvidedConstructor(
                                [ProvidedParameter("env", typeof<Env>)],
                                InvokeCode = (fun args ->
                                    <@@ let env = %%args.[0]:edu.stanford.nlp.ling.tokensregex.Env
                                        TokenSequencePattern.compile(env, patterns) @@>))
                ctorEnv.AddXmlDoc "Initializes a TokenSequencePattern instance in provided environment"
                ty.AddMember ctorEnv


                let getMatcherMethod =
                    ProvidedMethod(
                        methodName = "GetMatcher",
                        parameters = [ProvidedParameter("tokens", typeof<java.util.List>)],
                        returnType = matcherType,
                        InvokeCode = (fun args ->
                            <@@ let pattern = (%%args.[0]:obj) :?>TokenSequencePattern
                                let list = %%args.[1]:java.util.List
                                pattern.getMatcher(list) @@>))
                getMatcherMethod.AddXmlDoc "Indicates whether the TokenSequencePattern finds a match in the specified input tokens sequance"
                ty.AddMember getMatcherMethod

                ty
            | _ -> failwith "unexpected parameter values")
        )
    tokenSeqType
