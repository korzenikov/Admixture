#r @"packages\Accord.3.8.0\lib\net46\Accord.dll"
#r @"packages\Accord.Math.3.8.0\lib\net46\Accord.Math.dll"
#r @"packages\Accord.Math.3.8.0\lib\net46\Accord.Math.Core.dll"
#r @"packages\Accord.Statistics.3.8.0\lib\net46\Accord.Statistics.dll"
#r @"packages\FSharp.Charting.0.90.14\lib\net40\FSharp.Charting.dll"
#r @"packages\Accord.MachineLearning.3.8.0\lib\net46\Accord.MachineLearning.dll"

#load "Populations.fs"
#load "EthnoPlots.fs"
#load "K36.fs"

open System
open Admixture.Populations
open Admixture.EthnoPlots
open Admixture.K36

let components, populations = getPopulations ',' "D:\Populations\K15.csv"

populations |> getEthnoPlot3D 6 
    |> Seq.iter (fun (label, x, y, z, c) -> printfn "%d,\"%s\",%f,%f,%f" c label x y z)

//(components, populations) ||> showEthnoPlot 5 -1.0 -1.0

//populations |> showEthnoPlotByClusters 5

let testPersonK13 = 
    [|
        26.64
        49.97
        5.66
        4.29
        3.11
        1.20
        2.82
        0.0 
        4.93
        0.57
        0.0  
        0.61
        0.20
    |]

let testPersonK15 = 
    [|
        15.80
        18.44
        31.46
        24.02
        0.68
        2.52
        0.0   
        0.46
        2.16
        0.0    
        3.65
        0.14
        0.0    
        0.66
        0.0
    |]

let testPerson2K15 = 
    [|
         21.24
         15.18
         31.29
         21.55
         4.30
         2.35
         2.59
         0.0
         0.0
         0.0
         0.20
         0.09
         0.97
         0.0
         0.25
    |]

let testPerson3K15 = 
    [|
        10.64
        16.48
        27.76
        23.07
        0.0
        2.87
        0.0
        1.18
        5.64
        0.86
        6.18
        0.61
        0.0
        2.23
        2.47
    |]


let testPersonK36 = 
    [|
        0.54
        0.0
        0.0
        1.72
        0.0
        10.19
        0.0
        0.0
        3.41
        0.0
        23.37
        0.0
        22.47
        16.11
        0.0
        1.03
        0.0
        0.0
        0.0
        0.0
        0.0
        8.25
        0.5
        5.03
        0.0
        0.0
        0.49
        0.0
        0.29
        1.2
        0.0
        0.0
        5.4
        0.0
        0.0
        0.0
    |]

let testPerson2K36 = 
    [|
        0.0
        0.0
        0.0
        1.43
        0.0
        12.65
        0.0
        0.0
        5.35
        0.0
        24.12
        0.0
        20.32
        9.96
        0.0
        0.0
        0.0
        6.24
        0.0
        0.0
        0.0
        9.11
        0.0
        7.88
        0.0
        0.0
        0.0
        0.0
        0.0
        0.0
        0.0
        0.0
        2.93
        0.0
        0.0
        0.0
    |]

let testPerson3K36 = 
    [|
        0.77
        0.0
        0.0
        1.29
        0.0
        7.31
        1.31
        0.29
        2.02
        0.0
        20.48
        0.0
        21.75
        17.01
        0.0
        0.57
        0.0
        0.0
        0.0
        0.0
        0.0
        6.88
        0.0
        2.17
        0.0
        0.0
        1.24
        0.4
        2.19
        3.79
        2.39
        2.33
        4.2
        1.53
        0.0
        0.0
    |]

//(populations, testPersonK13) ||> showEthnoPlot 4

//(populations, testPersonK13) ||> showEthnoPlotSampleClusterOnly 5

[testPersonK15; testPerson2K15; testPerson3K15] |> showSamplesOnEthnoPlot 5 populations

//[testPersonK15; testPerson2K15; testPerson3K15] |> showSamplesOnEthnoPlotOwnClustersOnly 5 populations

//[testPersonK36; testPerson2K36; testPerson3K36] |> showSamplesOnEthnoPlot 5 populations

//[testPersonK36; testPerson2K36; testPerson3K36] |> showSamplesOnEthnoPlotOwnClustersOnly 5 populations


//[testPersonK36; testPerson2K36] |> showSamplesOnEthnoPlot3D 6 populations
//    |> Seq.iter (fun (label, x, y, z, c) -> printfn "%d,\"%s\",%f,%f,%f" c label x y z)

//[testPersonK15; testPerson2K15] |> showSamplesOnEthnoPlot3D 6 populations
//    |> Seq.iter (fun (label, x, y, z, c) -> printfn "%d,\"%s\",%f,%f,%f" c label x y z)

