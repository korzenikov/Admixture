module Admixture.RawData

open System.IO
open System

type Nucleotide = A | G | C | T

type SNP = 
    { 
        RSID: string
        Chromosome : string
        Position: int
        Allele1: Nucleotide option
        Allele2: Nucleotide option
    }

let split (s:string) = s.Split(',')

let unquote (row:string[]) = 
    row |> Array.map (fun x -> if x.StartsWith("\"") then x.Substring(1, x.Length - 2) else x)

let parseNucleotide c =
    match c with
    | 'A' -> Some A
    | 'G' -> Some G
    | 'C' -> Some C
    | 'T' -> Some T
    | _ -> None

let nucleotideToChar n =
    match n with
    | Some A -> 'A'
    | Some C -> 'C'
    | Some G -> 'G'
    | Some T -> 'T'
    | None -> '-'

let convert (row:string[]) = 
    {
        RSID = row.[0]
        Chromosome = row.[1] 
        Position = row.[2] |> Convert.ToInt32
        Allele1 = parseNucleotide row.[3].[0]
        Allele2 = parseNucleotide row.[3].[1]
    }

let readFromFile path = 
    let lines = File.ReadAllLines(path)
    lines |> Array.map (split >> unquote) |> Array.where(fun x -> x.[0].StartsWith("rs")) |> Array.map convert

let outputToFile (path:string) snps =
    use streamWriter = new StreamWriter(path)
    streamWriter.WriteLine "RSID,CHROMOSOME,POSITION,RESULT"
    snps |> Seq.iter (fun x-> streamWriter.WriteLine (sprintf "\"%s\",\"%s\",\"%d\",\"%c%c\"" x.RSID x.Chromosome x.Position (x.Allele1 |> nucleotideToChar) (x.Allele2 |> nucleotideToChar)))
