#r @"packages\FSharp.Data.2.4.6\lib\net45\FSharp.Data.dll"
#load "RawData.fs"

open Admixture.RawData
open FSharp.Data

let getSnpByRSID rsid (snps:Map<string,SNP>) =
    match snps.TryFind rsid with
    | Some x ->
        (x.Allele1, x.Allele2)
    | None ->
        (None, None)


let snps = readFromFile @"D:\DNA\Son.csv" |> Array.groupBy (fun x -> x.RSID) |> Array.map (fun (k, g) -> (k, g |> Array.exactlyOne)) |> Map.ofArray


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
