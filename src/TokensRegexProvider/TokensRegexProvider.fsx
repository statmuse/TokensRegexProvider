#nowarn "211"
// Standard NuGet or Paket location
#I "."
#I "lib/net45/"

// Standard NuGet locations packages
#I "../Stanford.NLP.CoreNLP.3.6.0.0/lib/"
#I "../IKVM.8.1.5717.0/lib/"

// Standard Paket locations packages
#I "../Stanford.NLP.CoreNLP/lib/"
#I "../IKVM/lib/"

// Try various folders that people might like
#I "bin"
#I "../bin"
#I "../../bin"
#I "lib"

// Reference TokensRegexProvider and Stanford.NET.CoreNLP
#r "IKVM.Runtime.dll"
#r "IKVM.OpenJDK.Util.dll"
#r "IKVM.OpenJDK.Core.dll"
#r "stanford-corenlp-3.6.0.dll"
#r "TokensRegexProvider.dll"
