#load "RawData.fs"
#load "Phasing.fs"

#r @"packages\FSharp.Data.2.3.2\lib\net40\FSharp.Data.dll"

open Admixture.RawData
open Admixture.Phasing
open FSharp.Data

let getSnp chr pos (snps:Map<string,_>) =
    snps.[chr] |> Seq.where (fun x -> x.Position = pos)

let noCall x = x.Allele1.IsNone || x.Allele2.IsNone


//let sonSNPs = readFromFile @"D:\DNA\Andrei.csv" |> Array.groupBy (fun x -> x.Chromosome) |> Map.ofArray

//let motherSNPs = readFromFile @"D:\Mother.csv" |> Array.groupBy (fun x -> x.Chromosome) |> Map.ofArray

//phase sonSNPs motherSNPs |> outputToFile @"D:\phased.csv"

let getSnpByRSID rsid (snps:Map<string,SNP>) =
    match snps.TryFind rsid with
    | Some x ->
        (x.Allele1, x.Allele2)
    | None ->
        (None, None)


let snps = readFromFile @"D:\DNA\Andrei.csv" |> Array.groupBy (fun x -> x.RSID) |> Array.map (fun (k, g) -> (k, g |> Array.exactlyOne)) |> Map.ofArray


type ReferenceFile = CsvProvider<"D:\DNA\Reference.csv">

let refSnps = ReferenceFile.Load("D:\DNA\Reference.csv")

refSnps.Rows 
    |> Seq.map (
        fun r -> 
            let allele = r.Allele.[0] |> parseNucleotide 
            let number = 
                match (snps |> getSnpByRSID r.SNP) with
                | None, None -> 
                    None
                | x, y -> 
                    Some ((if x = allele then 1 else 0) + (if y = allele then 1 else 0))
            r, number
      ) 
    |> Seq.iter (fun (r, x) -> printfn "%s\t\t%s\t%s" r.SNP r.Allele (if x.IsSome then x.Value.ToString() else "NA"))