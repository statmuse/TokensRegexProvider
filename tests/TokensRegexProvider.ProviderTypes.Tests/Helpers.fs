module Helpers

open edu.stanford.nlp.pipeline
open edu.stanford.nlp.ling

let private pipeline =
    let res = new AnnotationPipeline()
    res.addAnnotator(new TokenizerAnnotator(false, "en"))
    res

let private cls = CoreAnnotations.TokensAnnotation().getClass()

let tokenize (text:string) =
    let annotation = new Annotation(text)
    pipeline.annotate(annotation)
    annotation.get(cls) :?> java.util.List