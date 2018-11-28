#r @"packages\XPlot.Plotly.1.5.0\lib\net45\XPlot.Plotly.dll"
#r @"packages\Accord.3.8.0\lib\net46\Accord.dll"
#r @"packages\Accord.Math.3.8.0\lib\net46\Accord.Math.dll"
#r @"packages\Accord.Math.3.8.0\lib\net46\Accord.Math.Core.dll"
#r @"packages\Accord.Statistics.3.8.0\lib\net46\Accord.Statistics.dll"
#r @"packages\Accord.MachineLearning.3.8.0\lib\net46\Accord.MachineLearning.dll"

#load "Populations.fs"
#load "EthnoPlots.fs"
 
open XPlot.Plotly
open System
open Admixture.Populations
open Admixture.EthnoPlots

type Cluster2D = {
        index: int
        x: seq<float>
        y: seq<float>
        label: seq<string>
    }

type Cluster3D = {
        index: int
        x: seq<float>
        y: seq<float>
        z: seq<float>
        label: seq<string>
    }
    
let convertToCluster2D i g =
     {
        index = i
        label = g |> Seq.map (fun (l, x, y, c) -> l)
        x = g |> Seq.map (fun (l, x, y, c) -> x)
        y = g |> Seq.map (fun (l, x, y, c) -> y)
     }

let convertToCluster3D i g =
     {
        index = i
        label = g |> Seq.map (fun (l, x, y, z, c) -> l)
        x = g |> Seq.map (fun (l, x, y, z, c) -> x)
        y = g |> Seq.map (fun (l, x, y, z, c) -> y)
        z = g |> Seq.map (fun (l, x, y, z, c) -> z)
     }

let getEthnoPlot3DClusters k populations =
    getEthnoPlot3D k populations 
    |> Seq.groupBy(fun (_, _, _, _, c) -> c)
    |> Seq.map (fun (i, g) -> g |> convertToCluster3D i)

let getEthnoPlot2DWithSamplesClusters k populations samples =
    getEthnoPlot2DWithSamples k populations samples
    |> Seq.groupBy(fun (_, _, _, c) -> c)
    |> Seq.map (fun (i, g) -> g |> convertToCluster2D i)

let getEthnoPlot3DWithSamplesClusters k populations samples =
    getEthnoPlot3DWithSamples k populations samples
    |> Seq.groupBy(fun (_, _, _, _, c) -> c)
    |> Seq.map (fun (i, g) -> g |> convertToCluster3D i)


let _, populations = getPopulations ',' "D:\Populations\K36.csv"

let _, samples = getPopulations ',' "D:\Populations\K36samples.csv"

let data2D =
    getEthnoPlot2DWithSamplesClusters 6 populations samples
    |> Seq.mapi (fun i c -> 
                    let clusterName = if c.index = -1 then "Samples" else (sprintf "C %d" (i+1))
                    Scatter(
                        x = c.x,
                        y = c.y,
                        mode = "markers+text",
                        name = clusterName,
                        text = c.label,
                        marker = Marker(size = 6.)
                    )
                )

let data3D =
    getEthnoPlot3DWithSamplesClusters 6 populations samples
    |> Seq.mapi (fun i c -> 
                    let clusterName = if c.index = -1 then "Samples" else (sprintf "C %d" (i+1))
                    Scatter3d(
                        x = c.x,
                        y = c.y,
                        z = c.z,
                        mode = "markers+text",
                        name = clusterName,
                        text = c.label,
                        marker = Marker(size = 6.)
                    )
                )

let layout =
    Layout(
        legend =
            Legend(
                y = 0.5,
                font =
                    Font(
                        family = "Arial; sans-serif",
                        color = "grey"
                    )
            ),
        title ="PCA 3D",
        width = 1800.,
        height = 1200.
    )

(data2D, layout)
|> Chart.Plot
|> Chart.Show