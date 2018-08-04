#r @"packages\XPlot.Plotly.1.4.2\lib\net45\XPlot.Plotly.dll"
#r @"packages\Accord.3.8.0\lib\net46\Accord.dll"
#r @"packages\Accord.Math.3.8.0\lib\net46\Accord.Math.dll"
#r @"packages\Accord.Math.3.8.0\lib\net46\Accord.Math.Core.dll"
#r @"packages\Accord.Statistics.3.8.0\lib\net46\Accord.Statistics.dll"
#r @"packages\Accord.MachineLearning.3.8.0\lib\net46\Accord.MachineLearning.dll"

#load "Populations.fs"
#load "EthnoPlots.fs"
#load "K36.fs"
 
open XPlot.Plotly



open System
open Admixture.Populations
open Admixture.EthnoPlots

let components, populations = getPopulations ',' "D:\Populations\K15.csv"

 
//let trace1 =
//    Scatter(
//        x = [1; 2; 3; 4; 5],
//        y = [1; 6; 3; 6; 1],
//        mode = "markers+text",
//        name = "Team A",
//        text = ["A-1"; "A-2"; "A-3"; "A-4"; "A-5"],
//        textposition = "top center",
//        textfont = Textfont(family = "Raleway; sans-serif"),
//        marker = Marker(size = 12.)
//    )

//let trace2 =
//    Scatter(
//        x = [1.5; 2.5; 3.5; 4.5; 5.5],
//        y = [4; 1; 7; 1; 4],
//        mode = "markers+text",
//        name = "Team B",
//        text = ["B-a"; "B-b"; "B-c"; "B-d"; "B-e"],
//        textfont = Textfont(family = "Times New Roman"),
//        textposition = "bottom center",
//        marker = Marker(size = 12.)
//    )

//let data =
//    getEthnoPlot3DClusters 6 populations
//    |> Seq.mapi (fun i c -> 
//                    Scatter3d(
//                        x = c.x,
//                        y = c.y,
//                        z = c.z,
//                        mode = "markers",
//                        name = sprintf "C %d" (i+1),
//                        text = c.label,
//                        marker = Marker(size = 6.)

//                    )
                            
//                )

let data =
    getEthnoPlot3DClusters 6 populations
    |> Seq.mapi (fun i c -> 
                    Scatter(
                        x = c.x,
                        y = c.y,
                        mode = "markers",
                        name = sprintf "C %d" (i+1),
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
        title ="Data Labels on the Plot",
        width = 1600.,
        height = 1200.
    )

(data, layout)
|> Chart.Plot
|> Chart.Show