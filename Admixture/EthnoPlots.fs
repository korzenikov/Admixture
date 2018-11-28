module Admixture.EthnoPlots

open Accord.Statistics.Analysis
open Accord.MachineLearning
open Admixture.Populations

let abbreviations =
        [ 
            ("Russian", "RU")
            ("Belarusian", "BY")
            ("Belarussian", "BY")
            ("Belorussian", "BY")
            ("Ukrainian", "UA")
            ("Southwest", "SW")
            ("Southeast", "SE")
            ("Northwest", "NW")
            ("Northeast", "NE")
            ("North", "N")
            ("South", "S")
            ("West", "W")
            ("East", "E")
        ]

let getTransform k data =
    let pca = new PrincipalComponentAnalysis()

    pca.Method <- PrincipalComponentMethod.Standardize
    pca.Whiten <- true
    pca.NumberOfOutputs <- 3;
    pca.Learn(data) 

let getClustering (k:int) data =
    Accord.Math.Random.Generator.Seed <- new System.Nullable<int>(0);
    
    let kmeans = BalancedKMeans k
    kmeans.MaxIterations <- 10000

    kmeans.Learn(data)

let getEthnoPlot2D k populations =

    let data = populations |> Array.map (fun x -> x.Components)
    
    let transform = data |> getTransform 2

    let clustering = data |> getClustering k

    let clusters = data |> clustering.Decide

    let labels = populations |> Array.map (fun x -> x.Label) |> Array.map (fun x -> abbreviations |> Seq.fold (fun (acc : string) (l, s) -> acc.Replace(l, s)) x)

    data |> transform.Transform |> Array.mapi (fun i x -> labels.[i], x.[0], x.[1], clusters.[i])

let getEthnoPlot3D k populations =

    let data = populations |> Array.map (fun x -> x.Components)
    
    let transform = data |> getTransform 3

    let clustering = data |> getClustering k

    let clusters = data |> clustering.Decide

    let labels = populations |> Array.map (fun x -> x.Label) |> Array.map (fun x -> abbreviations |> Seq.fold (fun (acc : string) (l, s) -> acc.Replace(l, s)) x)

    data |> transform.Transform |> Array.mapi (fun i x -> labels.[i], x.[0], x.[1], x.[2], clusters.[i])

let getEthnoPlot2DWithSamples k populations samples =

    let data = populations |> Array.map (fun x -> x.Components)
    
    let transform = data |> getTransform 2

    let clustering = data |> getClustering k

    let clusters = data |> clustering.Decide

    let sampleData = samples |> Array.map (fun x -> x.Components)

    let sampleLabels = samples |> Array.map (fun x -> x.Label)

    let labels = populations |> Array.map (fun x -> x.Label) |> Array.map (fun x -> abbreviations |> Seq.fold (fun (acc : string) (l, s) -> acc.Replace(l, s)) x)

    let populationOutput = data |> transform.Transform |> Array.mapi (fun i x -> labels.[i], x.[0], x.[1], clusters.[i])

    let sampleOutput = sampleData |> transform.Transform |> Array.mapi (fun i x -> sampleLabels.[i], x.[0], x.[1], -1)

    sampleOutput |> Seq.append populationOutput

let getEthnoPlot3DWithSamples k populations samples =

    let data = populations |> Array.map (fun x -> x.Components)
    
    let transform = data |> getTransform 3

    let clustering = data |> getClustering k

    let clusters = data |> clustering.Decide

    let sampleData = samples |> Array.map (fun x -> x.Components)

    let sampleLabels = samples |> Array.map (fun x -> x.Label)

    let labels = populations |> Array.map (fun x -> x.Label) |> Array.map (fun x -> abbreviations |> Seq.fold (fun (acc : string) (l, s) -> acc.Replace(l, s)) x)

    let populationOutput = data |> transform.Transform |> Array.mapi (fun i x -> labels.[i], x.[0], x.[1], x.[2], clusters.[i])

    let sampleOutput = sampleData |> transform.Transform |> Array.mapi (fun i x -> sampleLabels.[i], x.[0], x.[1], x.[2], -1)

    sampleOutput |> Seq.append populationOutput