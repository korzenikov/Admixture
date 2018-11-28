module Admixture.Phasing

open Admixture.RawData

let align a1 a2 offsetFunc =

    let a1 = a1 |> Array.sortBy offsetFunc
    let a2 = a2 |> Array.sortBy offsetFunc

    let rec alignRec i j =
        seq { 
            if (i < a1.Length && j < a2.Length) then
                let offsetI = offsetFunc a1.[i]
                let offsetJ = offsetFunc a2.[j]

                if offsetI < offsetJ then 
                    yield Some a1.[i], None
                    yield! alignRec (i + 1) j

                elif offsetI > offsetJ then
                    yield None, Some a2.[j]
                    yield! alignRec i (j + 1)
                else
                    yield Some a1.[i], Some a2.[j]
                    yield! alignRec (i + 1) (j + 1)

            elif (i < a1.Length) then
                yield! a1.[i..] |> Seq.map (fun x -> Some x, None)

            elif (j < a2.Length) then
                yield! a2.[j..] |> Seq.map (fun x ->  None, Some x)
        }

    alignRec 0 0

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
    |> Seq.collect (fun x-> align childSNPs.[x] parentSNPs.[x] (fun x -> x.Position) |> Seq.map (fun (ch, p) -> phaseSNP ch p) )
    |> Seq.where (fun x -> x.IsSome) 
    |> Seq.map (fun x -> x.Value)