#r @"D:\Repos\Admixture\Admixture\packages\Accord.3.8.0\lib\net46\Accord.dll"
#r @"D:\Repos\Admixture\Admixture\packages\Accord.Math.3.8.0\lib\net46\Accord.Math.dll"
#r @"D:\Repos\Admixture\Admixture\packages\Accord.Math.3.8.0\lib\net46\Accord.Math.Core.dll"
#r @"D:\Repos\Admixture\Admixture\packages\Accord.Statistics.3.8.0\lib\net46\Accord.Statistics.dll"
#r @"D:\Repos\Admixture\Admixture\packages\FSharp.Charting.0.91.1\lib\net45\FSharp.Charting.dll"
#r @"D:\Repos\Admixture\Admixture\packages\Accord.MachineLearning.3.8.0\lib\net46\Accord.MachineLearning.dll"

#load "Populations.fs"
#load "EthnoPlots.fs"
#load "K36.fs"

open System
open Admixture.Populations
open Admixture.EthnoPlots
open Admixture.K36

let components, populations = getPopulations ',' "D:\Populations\K15.csv"

    