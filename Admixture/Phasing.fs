module Admixture.Phasing

open Admixture.RawData

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

let phaseSNP ch p =
    match ch, p with
    | None, _ -> None
    | Some x, None -> Some x
    | Some x, Some y ->
        let (a1, a2) = phaseAlleles (x.Allele1, x.Allele2) (y.Allele1, y.Allele2)
        Some 
            {
                x with Allele1 = a1; Allele2 = a2
            }

let phase (childSNPs:Map<_,_>) (parentSNPs:Map<_,_>) =
    [1..22] 
    |> Seq.map (fun x -> x.ToString()) 
    |> Seq.collect (fun x-> align childSNPs.[x] parentSNPs.[x] |> Seq.map (fun (ch, p) -> phaseSNP ch p) )
    |> Seq.where (fun x -> x.IsSome) 
    |> Seq.map (fun x -> x.Value)