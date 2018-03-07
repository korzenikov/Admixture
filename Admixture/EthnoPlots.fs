module Admixture.EthnoPlots

open Accord.Statistics.Analysis
open FSharp.Charting
open Accord.MachineLearning
open Admixture.Populations

let getTransform data =
    let pca = new PrincipalComponentAnalysis()

    pca.Method <- PrincipalComponentMethod.Standardize
    pca.Whiten <- true
    pca.NumberOfOutputs <- 2;
    pca.Learn(data) 

let getClusters k data =
    Accord.Math.Random.Generator.Seed <- new System.Nullable<int>(0);
    let kmeans = new KMeans(k);
    kmeans.Learn(data);


let getCharts (labels:string[]) (output:float[][]) (clusters:int[]) =
    output
    |> Array.mapi (fun i x -> (i, x))
    |> Seq.groupBy (fun (i, _) -> clusters.[i])
    |> Seq.map 
        (fun (k, g) -> 
            let ls = g |> Seq.map (fun (i, _) -> labels.[i])
            k, Chart.Point(g |> Seq.map (fun (_, x) -> -x.[0], -x.[1]), Labels = ls)
        )

let showEthnoPlotOnly k populations =

    let data = populations |> Array.map (fun x -> x.Components)

    let transform = getTransform data

    let output = transform.Transform(data)

    let clusters = getClusters k data
 
    let clusterLabels = clusters.Decide(data);

    let abbreviations =
        [ 
            ("Russian", "RU")
            ("Belarusian", "BY")
            ("Belarussian", "BY")
            ("Ukrainian", "UA")
            ("North_", "N. ")
            ("Southwest_", "SW. ")
            ("West_", "W. ")
        ]

    let labels = populations |> Array.map (fun x -> x.Label)  |> Array.map (fun x -> abbreviations |> Seq.fold (fun (acc : string) (l, s) -> acc.Replace(l, s)) x)
    
    (output, clusterLabels)
    ||> getCharts labels
    |> Seq.map snd
    |> Chart.Combine
    |> Chart.WithXAxis(MajorGrid = ChartTypes.Grid(Enabled=false)) 
    |> Chart.WithYAxis(MajorGrid = ChartTypes.Grid(Enabled=false))
    |> Chart.Show

let showEthnoPlot k populations (sample:float[]) =

    let data = populations |> Array.map (fun x -> x.Components)

    let transform = getTransform data

    let output = transform.Transform(data)

    let point = transform.Transform(sample)

    let clusters = getClusters k data
 
    let clusterLabels = clusters.Decide(data);

    let abbreviations =
        [ 
            ("Russian", "RU")
            ("Belarusian", "BY")
            ("Belarussian", "BY")
            ("Ukrainian", "UA")
            ("North_", "N. ")
            ("Southwest_", "SW. ")
            ("West_", "W. ")
        ]

    let labels = populations |> Array.map (fun x -> x.Label)  |> Array.map (fun x -> abbreviations |> Seq.fold (fun (acc : string) (l, s) -> acc.Replace(l, s)) x)
    
    (output, clusterLabels)
    ||> getCharts labels
    |> Seq.map snd
    |> Seq.append
        [
            Chart.Point([(-point.[0], -point.[1])], Labels = ["Me"]) |> Chart.WithSeries.Style(System.Drawing.Color.Red)
        ]
    |> Chart.Combine
    |> Chart.WithXAxis(MajorGrid = ChartTypes.Grid(Enabled=false)) |> Chart.WithYAxis(MajorGrid = ChartTypes.Grid(Enabled=false))
    |> Chart.Show

let showEthnoPlotSampleClusterOnly k populations (sample:float[]) =

    let data = populations |> Array.map (fun x -> x.Components)

    let transform = getTransform data

    let output = transform.Transform(data)

    let point = transform.Transform(sample)

    let clusters = getClusters k data

    let clusterLabels = clusters.Decide(data);

    let labels = populations |> Array.map (fun x -> x.Label)

    let sampleCluster = clusters.Decide(sample)

    (output, clusterLabels)
    ||> getCharts labels
    |> Seq.where (fun (k, _) -> k = sampleCluster)
    |> Seq.map snd
    |> Seq.append
        [
            Chart.Point([(-point.[0], -point.[1])], Labels = ["Me"]) |> Chart.WithSeries.Style(System.Drawing.Color.Red)
        ]
    |> Chart.Combine
    |> Chart.WithXAxis(MajorGrid = ChartTypes.Grid(Enabled=false)) |> Chart.WithYAxis(MajorGrid = ChartTypes.Grid(Enabled=false))
    |> Chart.Show



