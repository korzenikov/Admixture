module Admixture.K36

open Microsoft.FSharp.Reflection

type K36 =
        Amerindian
        | Arabian
        | Armenian
        | Basque 
        | Central_African
        | Central_Euro
        | East_African
        | East_Asian
        | East_Balkan
        | East_Central_Asian
        | East_Central_Euro
        | East_Med
        | Eastern_Euro
        | Fennoscandian
        | French
        | Iberian
        | IndoChinese
        | Italian
        | Malayan
        | Near_Eastern
        | North_African
        | North_Atlantic
        | North_Caucasian
        | North_Sea
        | Northeast_African
        | Oceanian
        | Omotic
        | Pygmy
        | Siberian
        | South_Asian
        | South_Central_Asian
        | South_Chinese
        | VolgaUral
        | West_African
        | West_Caucasian
        | WestMed

let components = FSharpType.GetUnionCases typeof<K36> |> Array.map (fun x -> x.Name)

let convertToArray c = 
    let map = c |> Seq.map (fun (c, v) -> c.ToString(), v) |> Map.ofSeq 
    components |> Array.map (fun x -> 
                        match map.TryFind x with
                        | Some v -> v
                        | None -> 0.0)