namespace System
open System.Reflection
open System.Runtime.CompilerServices

[<assembly: AssemblyTitleAttribute("TokensRegexProvider")>]
[<assembly: AssemblyProductAttribute("TokensRegexProvider")>]
[<assembly: AssemblyDescriptionAttribute("F# Type Provider for Stanford NLP TokensRegex - http://nlp.stanford.edu/software/tokensregex.html")>]
[<assembly: AssemblyVersionAttribute("0.0.2")>]
[<assembly: AssemblyFileVersionAttribute("0.0.2")>]
[<assembly: InternalsVisibleToAttribute("TokensRegexProvider.ProviderTypes.Tests")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.0.2"
    let [<Literal>] InformationalVersion = "0.0.2"
