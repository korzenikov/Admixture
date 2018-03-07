#load "Populations.fs"
#load "EthnoPlots.fs"

#r @"D:\Repos\Admixture\Admixture\packages\Accord.3.8.0\lib\net46\Accord.dll"
#r @"D:\Repos\Admixture\Admixture\packages\Accord.Math.3.8.0\lib\net46\Accord.Math.dll"
#r @"D:\Repos\Admixture\Admixture\packages\Accord.Math.3.8.0\lib\net46\Accord.Math.Core.dll"
#r @"D:\Repos\Admixture\Admixture\packages\Accord.Statistics.3.8.0\lib\net46\Accord.Statistics.dll"
#r @"D:\Repos\Admixture\Admixture\packages\FSharp.Charting.0.91.1\lib\net45\FSharp.Charting.dll"
#r @"D:\Repos\Admixture\Admixture\packages\Accord.MachineLearning.3.8.0\lib\net46\Accord.MachineLearning.dll"

open Accord.Statistics.Analysis;
open FSharp.Charting
open Admixture.Populations
open Accord.MachineLearning


let _, populations = getPopulations ',' "D:\Populations\K15.csv"

type K36 =
        Amerindian
        | Arabian
        | Armenian
        | Basque 
        | Central_African
        | Central_Euro
        | East_African
        | East_Asian
        | East_Balkan
        | East_Central_Asian
        | East_Central_Euro
        | East_Med
        | Eastern_Euro
        | Fennoscandian
        | French
        | Iberian
        | IndoChinese
        | Italian
        | Malayan
        | Near_Eastern
        | North_African
        | North_Atlantic
        | North_Caucasian
        | North_Sea
        | Northeast_African
        | Oceanian
        | Omotic
        | Pygmy
        | Siberian
        | South_Asian
        | South_Central_Asian
        | South_Chinese
        | VolgaUral
        | West_African
        | West_Caucasian
        | WestMed

let convertToArray components c = 
    let map = c |> Seq.map (fun (c, v) -> c.ToString(), v) |> Map.ofSeq 
    components |> Array.map (fun x -> 
                        match map.TryFind x with
                        | Some v -> v
                        | None -> 0.0)

let Andrei = 
    [
        Amerindian, 0.53
        Basque, 1.64
        Central_Euro, 10.14
        East_Balkan, 3.42
        East_Central_Euro, 23.31
        Eastern_Euro, 22.51
        Fennoscandian, 16.14
        Iberian, 1.13
        North_Atlantic, 8.1
        North_Caucasian, 0.47
        North_Sea, 5.15
        Omotic, 0.47
        Siberian, 0.29
        South_Asian, 1.20
        VolgaUral, 5.48
    ]

let Oxon = 
    [
        Amerindian, 1.76
        Basque, 2.47
        Central_Euro, 8.02
        East_Balkan, 1.63
        East_Central_Euro, 17.64
        Eastern_Euro, 20.59
        Fennoscandian, 19.78
        French, 3.50
        Italian, 0.62
        North_Atlantic, 8.22
        North_Caucasian, 5.14
        North_Sea, 8.43
        VolgaUral, 2.15
    ]

//let testPerson = (components, Andrei) ||> convertToArray

//let testPerson2 = (components, Oxon) ||> convertToArray

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


showEthnoPlotSampleClusterOnly 5 testPerson

showEthnoPlotSampleClusterOnly 5 testPerson2
