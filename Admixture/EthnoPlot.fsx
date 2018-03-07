#r @"D:\Repos\Admixture\Admixture\packages\Accord.3.8.0\lib\net46\Accord.dll"
#r @"D:\Repos\Admixture\Admixture\packages\Accord.Math.3.8.0\lib\net46\Accord.Math.dll"
#r @"D:\Repos\Admixture\Admixture\packages\Accord.Math.3.8.0\lib\net46\Accord.Math.Core.dll"
#r @"D:\Repos\Admixture\Admixture\packages\Accord.Statistics.3.8.0\lib\net46\Accord.Statistics.dll"
#r @"D:\Repos\Admixture\Admixture\packages\FSharp.Charting.0.91.1\lib\net45\FSharp.Charting.dll"
#r @"D:\Repos\Admixture\Admixture\packages\Accord.MachineLearning.3.8.0\lib\net46\Accord.MachineLearning.dll"

#load "Populations.fs"
#load "EthnoPlots.fs"

open Admixture.Populations
open Admixture.EthnoPlots

let _, populations = getPopulations ',' "D:\Populations\K15.csv"


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

let testPerson = 
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

let testPerson2 = 
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


showEthnoPlotOnly 5 populations

(populations, testPerson) ||> showEthnoPlot 5 

(populations, testPerson) ||> showEthnoPlotSampleClusterOnly 5

(populations, testPerson2) ||> showEthnoPlotSampleClusterOnly 5

