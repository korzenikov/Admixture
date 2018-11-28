#r @"packages\Accord.3.8.0\lib\net46\Accord.dll"
#r @"packages\Accord.Math.3.8.0\lib\net46\Accord.Math.dll"
#r @"packages\Accord.Math.3.8.0\lib\net46\Accord.Math.Core.dll"
#r @"packages\Accord.Statistics.3.8.0\lib\net46\Accord.Statistics.dll"
#r @"packages\Accord.MachineLearning.3.8.0\lib\net46\Accord.MachineLearning.dll"
#r @"packages\FSharp.Charting.2.1.0\lib\net45\FSharp.Charting.dll"

#load "Populations.fs"
#load "EthnoPlots.fs"
 
open System
open Admixture.Populations
open Admixture.EthnoPlots
open FSharp.Charting
open System.Windows.Forms

let _, populations = getPopulations ',' "D:\Populations\K15.csv"

let _, samples = getPopulations ',' "D:\Populations\K15samples.csv"

getEthnoPlot2DWithSamples 6 populations samples
|> Seq.groupBy(fun (_, _, _, c) -> c)
|> Seq.map 
      (fun (k, g) -> 
          let points = g |> Seq.map (fun (_, x, y, _) -> (x, y))
          let labels = g |> Seq.map (fun (l, _, _, _) -> l)
          Chart.Point(points, Labels = labels)
      )
|> Chart.Combine
|> Chart.WithXAxis(MajorGrid = ChartTypes.Grid(Enabled=false)) 
|> Chart.WithYAxis(MajorGrid = ChartTypes.Grid(Enabled=false))
|> Chart.Show