#r @"packages\Google.DataTable.Net.Wrapper.3.1.2.0\lib\Google.DataTable.Net.Wrapper.dll"
#r @"packages\XPlot.GoogleCharts.1.5.0\lib\net45\XPlot.GoogleCharts.dll"

#r @"packages\Accord.3.8.0\lib\net46\Accord.dll"
#r @"packages\Accord.Math.3.8.0\lib\net46\Accord.Math.dll"
#r @"packages\Accord.Math.3.8.0\lib\net46\Accord.Math.Core.dll"
#r @"packages\Accord.Statistics.3.8.0\lib\net46\Accord.Statistics.dll"
#r @"packages\Accord.MachineLearning.3.8.0\lib\net46\Accord.MachineLearning.dll"

#load "Populations.fs"
#load "EthnoPlots.fs"
 
open XPlot.GoogleCharts
open System
open Admixture.Populations
open Admixture.EthnoPlots

let _, populations = getPopulations ',' "D:\Populations\K15.csv"

let options =
    Options(
        title = "PCA",
        width = 2400,
        height = 1400
    )

let data =
    getEthnoPlot2D 6 populations 
    |> Seq.groupBy(fun (_, _, _, c) -> c)
    |> Seq.map (fun (_, g) -> g |> Seq.map (fun (_, x, y, _) -> x, y ) )


let chart =
    data 
    |> Chart.Scatter
    |> Chart.WithOptions options
    |> Chart.Show