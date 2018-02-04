open System
open System.IO

type Nucleotide = A | G | C | T

type SNP = 
    { 
        RSID:string
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

let getSnp chr pos (snps:Map<string,_>) =
    snps.[chr] |> Seq.where (fun x -> x.Position = pos)

let noCall x = x.Allele1.IsNone || x.Allele2.IsNone

let getSnps path = 
    let lines = File.ReadAllLines(path)     
    lines |> Array.map (split >> unquote) |> Array.where(fun x -> x.[0].StartsWith("rs")) |> Array.map convert


let align a1 a2 =
    let a1 = a1 |> Array.sortBy (fun x -> x.Position)
    let a2 = a2 |> Array.sortBy (fun x -> x.Position)
    seq { 
        let mutable i = 0
        let mutable j = 0
        
        while (i < a1.Length && j < a2.Length) do
            if a1.[i].Position < a2.[j].Position then 
                yield Some a1.[i], None
                i <- i + 1
            elif a1.[i].Position > a2.[j].Position then
                yield None, Some a2.[j]
                j <- j + 1
            else
                yield Some a1.[i], Some a2.[j]
                i <- i + 1
                j <- j + 1

        while (i < a1.Length) do
            yield Some a1.[i], None
            i <- i + 1

        while (j < a2.Length) do
            yield None, Some a2.[j]
            j <- j + 1
    }

let phaseAlleles ch p =
    match ch with
    | (Some x, Some y ) ->
        if x = y then 
            ch
        else
            match p with
            | (Some a, Some b ) when a = b -> 
                if a = x then
                    (Some y, Some y)
                elif a = y then 
                    (Some x, Some x)
                else
                    ch
             | _ -> ch
    | _ -> ch

let phaseSnp ch p =
    match ch, p with
    | None, _ -> None
    | Some x, None -> Some x
    | Some x, Some y ->
        let (a1, a2) = phaseAlleles (x.Allele1, x.Allele2) (y.Allele1, y.Allele2)
        Some 
            {
                x with Allele1 = a1; Allele2 = a2
            }

let createFile (fileName:string) snps =
    use streamWriter = new StreamWriter(fileName)
    streamWriter.WriteLine "RSID,CHROMOSOME,POSITION,RESULT"
    snps |> Seq.iter (fun x-> streamWriter.WriteLine (sprintf "\"%s\",\"%s\",\"%d\",\"%c%c\"" x.RSID x.Chromosome x.Position (x.Allele1 |> nucleotideToChar) (x.Allele2 |> nucleotideToChar)))

let chromosomes = ([1..22] |> Seq.map (fun x -> x.ToString()) |> Seq.toList)

let sonSNPs = getSnps @"D:\Son.csv" |> Array.groupBy (fun x -> x.Chromosome) |> Map.ofArray

let motherSNPs = getSnps @"D:\Mother.csv" |> Array.groupBy (fun x -> x.Chromosome) |> Map.ofArray

let phased = chromosomes |> Seq.collect (fun x-> align sonSNPs.[x] motherSNPs.[x] |> Seq.map (fun (ch, p) -> phaseSnp ch p) )  |> Seq.where (fun x -> x.IsSome) |> Seq.map (fun x -> x.Value)

phased |> createFile @"D:\phased.csv" 

//let merged = chromosomes |> Seq.collect (fun x-> mergeSnps FTDNA.[x] MH.[x]) 

//merged |> createFile @"D:\Merged.csv" 




