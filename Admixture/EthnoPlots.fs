module Admixture.EthnoPlots

open Accord.Statistics.Analysis
open FSharp.Charting
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

let getTransform data =
    let pca = new PrincipalComponentAnalysis()

    pca.Method <- PrincipalComponentMethod.Standardize
    pca.Whiten <- true
    pca.NumberOfOutputs <- 2;
    pca.Learn(data) 

let getClusters (k:int) data =
    Accord.Math.Random.Generator.Seed <- new System.Nullable<int>(0);
    
    let kmeans = BalancedKMeans k
    kmeans.MaxIterations <- 10000

    kmeans.Learn(data)

let getPoints scaleX scaleY (s:_[][]) =
    s |> Array.map (fun x -> x.[0] * scaleX, x.[1] * scaleY)

let getCharts (labels:string[]) (output:(float*float)[]) (clusters:int[]) =
    output
    |> Array.mapi (fun i x -> (i, x))
    |> Seq.groupBy (fun (i, _) -> clusters.[i])
    |> Seq.map 
        (fun (k, g) -> 
            let ls = g |> Seq.map (fun (i, _) -> labels.[i])
            k, Chart.Point(g |> Seq.map snd, Labels = ls)
        )

let showEthnoPlot k scaleX scaleY components populations =

    let data = populations |> Array.map (fun x -> x.Components)

    let cdata = components |> Array.mapi (fun ci _ -> [| for i in 0..components.Length - 1 -> if i = ci then 100.0 else 0.0 |])

    let transform = data |> Array.append cdata |> getTransform

    let output = data |> transform.Transform |> getPoints scaleX scaleY

    let clusters = data |> getClusters k
 
    let clusterLabels = data |> clusters.Decide

    let labels = populations |> Array.map (fun x -> x.Label)  |> Array.map (fun x -> abbreviations |> Seq.fold (fun (acc : string) (l, s) -> acc.Replace(l, s)) x)
    
    let cPoints = cdata |> transform.Transform |> getPoints scaleX scaleY

    (output, clusterLabels)
    ||> getCharts labels 
    |> Seq.map snd
    |> Seq.append
        [
            Chart.Point(cPoints, Labels = components) |> Chart.WithSeries.Style(System.Drawing.Color.Red) 
        ]
    |> Chart.Combine
    |> Chart.WithXAxis(MajorGrid = ChartTypes.Grid(Enabled=false)) 
    |> Chart.WithYAxis(MajorGrid = ChartTypes.Grid(Enabled=false))
    |> Chart.Show

let showEthnoPlotByClusters k populations =

    let scaleX = 1.0

    let scaleY = 1.0

    let data = populations |> Array.map (fun x -> x.Components)

    let transform = data |> getTransform

    let output = data |> transform.Transform |> getPoints scaleX scaleY

    let clusters = data |> getClusters k
 
    let clusterLabels = data |> clusters.Decide


    let labels = populations |> Array.map (fun x -> x.Label)  |> Array.map (fun x -> abbreviations |> Seq.fold (fun (acc : string) (l, s) -> acc.Replace(l, s)) x)

    
    (output, clusterLabels)
    ||> getCharts labels
    |> Seq.sortBy fst
    |> Seq.iter (fun (k, c) -> 
            c 
            |> Chart.WithTitle(Text = sprintf "Cluster %d" (k+1)) 
            |> Chart.WithXAxis(MajorGrid = ChartTypes.Grid(Enabled=false)) 
            |> Chart.WithYAxis(MajorGrid = ChartTypes.Grid(Enabled=false))
            |> Chart.Show)

let showSamplesOnEthnoPlot k populations (samples:seq<float[]>) =

    let data = populations |> Array.map (fun x -> x.Components)

    let transform = getTransform data

    let output = data |> transform.Transform |> getPoints 1.0 1.0

    let clusters = data |> getClusters k
 
    let clusterLabels = data |> clusters.Decide

    let sampleData = samples |> Seq.toArray

    let sampleOutput = sampleData |> transform.Transform

    let labels = populations |> Array.map (fun x -> x.Label)  |> Array.map (fun x -> abbreviations |> Seq.fold (fun (acc : string) (l, s) -> acc.Replace(l, s)) x)
    
    let populationCharts = (output, clusterLabels) ||> getCharts labels |> Seq.map snd

    let samplePoints =
        sampleOutput |> Seq.map (fun x -> x.[0], x.[1])

    let sampleLabels =
        sampleOutput |> Seq.mapi (fun i _ -> sprintf "Test Person %d" ( i + 1))

    [
        Chart.Point(samplePoints, Labels = sampleLabels) |> Chart.WithSeries.Style(System.Drawing.Color.Red)
    ]
    |> Seq.append populationCharts 
    |> Chart.Combine
    |> Chart.WithXAxis(MajorGrid = ChartTypes.Grid(Enabled=false)) |> Chart.WithYAxis(MajorGrid = ChartTypes.Grid(Enabled=false))
    |> Chart.Show

let showSamplesOnEthnoPlotOwnClustersOnly k populations (samples:seq<float[]>) =

    let data = populations |> Array.map (fun x -> x.Components)

    let transform = getTransform data

    let output = data |> transform.Transform |> getPoints 1.0 1.0

    let clusters = data |> getClusters k
 
    let clusterLabels = data |> clusters.Decide

    let sampleData = samples |> Seq.toArray

    let sampleOutput = sampleData |> transform.Transform |> getPoints 1.0 1.0

    let sampleClusters = sampleData |> clusters.Decide

    let labels = populations |> Array.map (fun x -> x.Label)

    let populationCharts = 
        (output, clusterLabels) 
        ||> getCharts labels
        |> Seq.where (fun (k, _) -> sampleClusters |> Seq.contains k) |> Seq.map snd

    let sampleLabels =
        sampleOutput |> Seq.mapi (fun i _ -> sprintf "Test Person %d" ( i + 1))

    [
        Chart.Point(sampleOutput, Labels = sampleLabels) |> Chart.WithSeries.Style(System.Drawing.Color.Red)
    ]
    |> Seq.append populationCharts
    |> Chart.Combine
    |> Chart.WithXAxis(MajorGrid = ChartTypes.Grid(Enabled=false)) |> Chart.WithYAxis(MajorGrid = ChartTypes.Grid(Enabled=false))
    |> Chart.Show