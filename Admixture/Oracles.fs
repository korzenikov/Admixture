module Admixture.Oracles

open System
open Admixture.Populations

let distance (p1: float[]) (p2: float[]) = (p1, p2) ||> Array.map2 (fun p1 p2 -> (p1 - p2)*(p1 - p2)) |> Array.sum
 
let closestSamples n pops (unknown:float[]) = 
  pops 
    |> Seq.map (fun x -> x, distance x.Components unknown)
    |> Seq.sortBy snd
    |> Seq.take n 
    |> Seq.map (fun (p, d) -> p, Math.Sqrt(d))

let combineComponents (p:float[][]) =
    let len = p.Length |> float
    let seed : float array = Array.zeroCreate p.[0].Length
    let sum = p |> Array.fold (fun acc elem -> (acc, elem) ||> Array.map2 (+)) seed 
    sum |> Array.map (fun x -> x / len)

let rec getSeq k i n =
    let elements = seq { i .. n - 1 }
    if (k = 1) then
        elements |> Seq.map (fun x -> [ x ])
    else
        seq {
            for x in elements do 
                let tseq = getSeq (k-1) x n
                for t in tseq ->
                    x::t
         }

let onePopulationApproximation k populations sample =
    (populations, sample) ||> closestSamples k |>  Seq.iter (fun (p, d) -> printfn "%s @ %f" p.Label d)

let getKPopulationMix k (pops:_[]) =
    getSeq k 0 pops.Length 
    |> Seq.map (fun mix ->
                    let mixPops = mix |> Seq.map (fun i -> pops.[i])
                    {   Label = (" + ", mixPops |> Seq.map (fun x -> x.Label)) |> System.String.Join
                        Components = mixPops |> Seq.map (fun x -> x.Components) |> Seq.toArray |> combineComponents
                    }
                )
let populationApproximation k tp populations sample =
    let kPopulationMix = sample |> closestSamples populations tp |> Seq.map fst |> Seq.toArray |> getKPopulationMix k
    (kPopulationMix, sample) ||> closestSamples 40  |>  Seq.iter (fun (p, d) -> printfn "%s @ %f" p.Label d)

