#r @"packages\GAF.2.3.1\lib\net461\GAF.461.dll"
#load "Populations.fs"
#load "K36.fs"
#load "Oracles.fs"
#load "GeneticOracles.fs"

open Admixture.Populations
open Admixture.K36
open Admixture.GeneticOracles

let _, populations = getPopulations ',' "D:\Populations\K36.csv"
    
let sample = 
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

sample |> convertToArray |> runGeneticOracle populations