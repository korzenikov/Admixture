module Admixture.ComponentParser

open System.IO
open System

let getComponents path =
    let lines = File.ReadAllLines(path) 
    lines |> Array.map (fun s -> 
                            let x = s.Split([|' '; '\t'|], StringSplitOptions.RemoveEmptyEntries)
                            x.[0], (if x.[1] = "-" then 0.0M else x.[1] |> decimal)
                       )

let checkSum components =
    let sum = components |> Seq.map snd |> Seq.sum 
    if (sum > 100.02M) then
        Error (sprintf "The sum %M is greater than 100" sum)
    elif (sum < 99.0M) then
        Error (sprintf "The sum %M is less than 100" sum)
    else
        Ok components

let getInvalidComponents refComponents components =
    let refComponentsSet = refComponents |> Set.ofArray
    let invalidComponents = components |> Array.where(fun (n, _) -> refComponentsSet.Contains n |> not) |> Array.map fst
    match invalidComponents with
    | [||] -> None
    | _ -> Some invalidComponents
