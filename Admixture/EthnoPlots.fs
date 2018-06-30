module Admixture.EthnoPlots

open Accord.Statistics.Analysis
open FSharp.Charting
open Accord.MachineLearning
open Admixture.Populations

type Cluster = {
        x: seq<float>
        y: seq<float>
        z: seq<float>
        label: seq<string>
    }

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

let getTransform3D data =
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

    let clustering = data |> getClustering k
 
    let clusters = data |> clustering.Decide

    let labels = populations |> Array.map (fun x -> x.Label) |> Array.map (fun x -> abbreviations |> Seq.fold (fun (acc : string) (l, s) -> acc.Replace(l, s)) x)
    
    let cPoints = cdata |> transform.Transform |> getPoints scaleX scaleY

    (output, clusters)
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

let showEthnoPlot3D k populations =

    let data = populations |> Array.map (fun x -> x.Components)
    
    let transform = data |> getTransform3D

    let clustering = data |> getClustering k

    let clusters = data |> clustering.Decide

    let labels = populations |> Array.map (fun x -> x.Label)// |> Array.map (fun x -> abbreviations |> Seq.fold (fun (acc : string) (l, s) -> acc.Replace(l, s)) x)

    data |> transform.Transform |> Array.mapi (fun i x -> labels.[i], x.[0], x.[1], x.[2], clusters.[i])

let showEthnoPlotByClusters k populations =

    let scaleX = 1.0

    let scaleY = 1.0

    let data = populations |> Array.map (fun x -> x.Components)

    let transform = data |> getTransform

    let output = data |> transform.Transform |> getPoints scaleX scaleY

    let clustering = data |> getClustering k
 
    let clusters = data |> clustering.Decide

    let labels = populations |> Array.map (fun x -> x.Label) //|> Array.map (fun x -> abbreviations |> Seq.fold (fun (acc : string) (l, s) -> acc.Replace(l, s)) x)

    
    (output, clusters)
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

    let clustering = data |> getClustering k
 
    let clusters = data |> clustering.Decide

    let sampleData = samples |> Seq.toArray

    let sampleOutput = sampleData |> transform.Transform

    let labels = populations |> Array.map (fun x -> x.Label)  |> Array.map (fun x -> abbreviations |> Seq.fold (fun (acc : string) (l, s) -> acc.Replace(l, s)) x)
    
    let populationCharts = (output, clusters) ||> getCharts labels |> Seq.map snd

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


let showSamplesOnEthnoPlot3D k populations  (samples:seq<float[]>) =

    let data = populations |> Array.map (fun x -> x.Components)
    
    let transform = data |> getTransform3D

    let clustering = data |> getClustering k

    let clusters = data |> clustering.Decide

    let sampleData = samples |> Seq.toArray

    let sampleClusters = sampleData |> clustering.Decide

    let sampleLabels =
        sampleData |> Array.mapi (fun i _ -> sprintf "Test Person %d" ( i + 1))

    let labels = populations |> Array.map (fun x -> x.Label)// |> Array.map (fun x -> abbreviations |> Seq.fold (fun (acc : string) (l, s) -> acc.Replace(l, s)) x)

    let populationOutput = data |> transform.Transform |> Array.mapi (fun i x -> labels.[i], x.[0], x.[1], x.[2], clusters.[i])

    let sampleOutput = sampleData |> transform.Transform |> Array.mapi (fun i x -> sampleLabels.[i], x.[0], x.[1], x.[2], sampleClusters.[i])

    sampleOutput |> Seq.append populationOutput

let showSamplesOnEthnoPlotOwnClustersOnly k populations (samples:seq<float[]>) =

    let data = populations |> Array.map (fun x -> x.Components)

    let transform = data |> getTransform

    let output = data |> transform.Transform |> getPoints 1.0 1.0

    let clustering = data |> getClustering k
 
    let clusters = data |> clustering.Decide

    let sampleData = samples |> Seq.toArray

    let sampleOutput = sampleData |> transform.Transform |> getPoints 1.0 1.0

    let sampleClusters = sampleData |> clustering.Decide

    let labels = populations |> Array.map (fun x -> x.Label)

    let populationCharts = 
        (output, clusters) 
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

let convertToCluster g =
     {
        label = g |> Seq.map (fun (l, x, y, z, c) -> l)
        x = g |> Seq.map (fun (l, x, y, z, c) -> x)
        y = g |> Seq.map (fun (l, x, y, z, c) -> y)
        z = g |> Seq.map (fun (l, x, y, z, c) -> z)
     }

let getEthnoPlot3DClusters k populations =
    showEthnoPlot3D k populations 
    |> Seq.groupBy(fun (_, _, _, _, c) -> c)
    |> Seq.map (fun (_, g) -> g |> convertToCluster)

let getEthnoPlot3DWithSamplesClusters k populations (samples:seq<float[]>) =
    showSamplesOnEthnoPlot3D k populations samples 
    |> Seq.groupBy(fun (_, _, _, _, c) -> c)
    |> Seq.map (fun (_, g) -> g |> convertToCluster)
