#r @"packages\XPlot.Plotly.1.4.2\lib\net45\XPlot.Plotly.dll"
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

let _, populations = getPopulations ',' "D:\Populations\K15.csv"

let _, samples = getPopulations ',' "D:\Populations\K15samples.csv"

let data =
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
        width = 1600.,
        height = 1200.
    )

(data, layout)
|> Chart.Plot
|> Chart.Show