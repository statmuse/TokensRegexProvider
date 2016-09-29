namespace FSharpTokensRegexProvider

open System
open ProviderImplementation.ProvidedTypes
open Microsoft.FSharp.Core.CompilerServices
open TokensRegexProvider

[<TypeProvider>]
type FSharpTokensRegexProvider(config: TypeProviderConfig) as this =
    inherit TypeProviderForNamespaces()

    static do
      // When TokensRegexProvider is installed via NuGet/Paket, the IKVM assembly and
      // will appear typically in "../../*/lib/". To support this, we look at
      // TokensRegexProvider.dll.config which has this pattern in custom key "ProbingLocations".
      // Here, we resolve assemblies by looking into the specified search paths.
      AppDomain.CurrentDomain.add_AssemblyResolve(fun _ args ->
        TokensRegexProvider.Redirects.resolveReferencedAssembly args.Name)

    do this.RegisterRuntimeAssemblyLocationAsProbingFolder config
       this.AddNamespace (
           Constants.rootNamespace,
           [ TokenSequencePatternProviderConfig.createTypes() ])

[<TypeProviderAssembly()>]
do()