#load "RawData.fs"
#load "Phasing.fs"

open Admixture.RawData
open Admixture.Phasing

let getSnp chr pos (snps:Map<string,_>) =
    snps.[chr] |> Seq.where (fun x -> x.Position = pos)

let noCall x = x.Allele1.IsNone || x.Allele2.IsNone


let sonSNPs = readFromFile @"D:\Son.csv" |> Array.groupBy (fun x -> x.Chromosome) |> Map.ofArray

let motherSNPs = readFromFile @"D:\Mother.csv" |> Array.groupBy (fun x -> x.Chromosome) |> Map.ofArray

phase sonSNPs motherSNPs |> outputToFile @"D:\phased.csv"