#load "RawData.fs"
#load "Phasing.fs"

open Admixture.RawData
open Admixture.Phasing

let sonSNPs = readFromFile @"D:\DNA\Son.csv" |> Array.groupBy (fun x -> x.Chromosome) |> Map.ofArray

let fatherSNPs = readFromFile @"D:\DNA\Father.csv" |> Array.groupBy (fun x -> x.Chromosome) |> Map.ofArray

phase sonSNPs fatherSNPs |> outputToFile @"D:\DNA\phased.csv"

