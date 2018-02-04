
#r @"D:\Repos\Admixture\Admixture\packages\FSharp.Charting.0.91.1\lib\net45\FSharp.Charting.dll"

open System
open System.IO
open FSharp.Charting

type Population = { Label:string; Components:float[] }

let split (s:string) = s.Split(' ')

let convert (s:string) = Convert.ToDouble(s)

let population (row:string[]) = 
    {
        Label = row.[0]; 
        Components = row.[1..] |> Array.map convert 
    }

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

let path = @"D:\Repos\Admixture\Admixture\bin\Debug\k36_v1_dots.txt"

let lines = File.ReadAllLines(path) 

let components  =
    lines.[1..36]

//let componentByName n (s: _[]) =
//    let index = components |> Array.findIndex(fun x -> x = n)
//    s.[index]

let populations = 
    lines.[37..] 
    |> Array.map (split >> population)

//let drawPopulation p =
//    p.Components |> Seq.mapi (fun i x -> components.[i], x) |> Seq.where (fun (_, x) -> x <> 0.0) |> Chart.Column 


//let classify (unknown:float[]) =
//    closestSamples unknown
//    |> Seq.groupBy (fun x -> x.Label) 
//    |> Seq.maxBy (fun (_, values) -> Seq.length values) 
//    |> (fun (key, _) -> key)


//printfn "%A" options
    
//populations
//    |> Seq.where (fun x -> x.Label.Contains("Ukr"))
//    |> Seq.map drawPopulation
//    |> Chart.Combine
//    |> Chart.Show


//populations
//    |> Seq.map (fun x -> x.Label, x.Components |> componentByName "North_Caucasian", x.Components |> componentByName "North_Caucasian")
//    |> Seq.sortBy (fun (_, k1, k2) -> k1, k2)
//    |> Seq.where (fun (_, k1, k2) -> k1 >= 0.0 && k2 >= 4.0)
//    |> Seq.toArray
//    |> Seq.iter (fun (l, k1,k2) -> printfn "%s %f %f" l k1 k2)


//populations
//    |> Seq.map (fun x -> x.Label, x.Components |> componentByName "VolgaUral")
//    |> Seq.sortBy snd
//    |> Seq.where (fun (_, k) -> k > 5.0)
//    |> Seq.toArray
//    |> Chart.Column
//    |> Chart.Show
//    //|> Seq.iter (fun (l, k) -> printfn "%s %f" l k)


let distance (p1: float[]) (p2: float[]) = (p1, p2) ||> Array.map2 (fun p1 p2 -> (p1 - p2)*(p1 - p2)) |> Array.sum
 
let closestSamples pops n (unknown:float[]) = 
  pops 
    |> Seq.map (fun x -> x, distance x.Components unknown)
    |> Seq.sortBy snd
    |> Seq.take n 
    |> Seq.map (fun (p, d) -> p, Math.Sqrt(d))

let convertToArray c = 
    let map = c |> Seq.map (fun (c, v) -> c.ToString(), v) |> Map.ofSeq 
    components |> Array.map (fun x -> 
                        match map.TryFind x with
                        | Some v -> v
                        | None -> 0.0)



let onePopulationApproximation s =
    s |> convertToArray |> closestSamples populations 20  |>  Seq.iter (fun (p, d) -> printfn "%s @ %f" p.Label d)


let twoPopulations =
    seq { 
       for i in 0 .. populations.Length - 1 do
            for j in i .. populations.Length - 1 do
                let p1 = populations.[i]
                let p2 = populations.[j]
                yield  {
                    Label = (sprintf "%s + %s" p1.Label p2.Label)
                    Components = (p1.Components, p2.Components) ||> Array.map2 (fun c1 c2 -> 0.5*c1 + 0.5*c2)
                }
       }

let twoPopulationApproximation s =
    s |> convertToArray |> closestSamples twoPopulations 20  |>  Seq.iter (fun (p, d) -> printfn "%s @ %f" p.Label d)

let getFourPopulations (pops:_[]) =
    seq { 
       for i in 0 .. pops.Length - 1 do
            for j in i .. pops.Length - 1 do
                for k in j .. pops.Length - 1 do
                    for l in k .. pops.Length - 1 do
                        let p1 = pops.[i]
                        let p2 = pops.[j]
                        let p3 = pops.[k]
                        let p4 = pops.[l]
                        yield  {
                                Label = (sprintf "%s + %s + %s + %s" p1.Label p2.Label p3.Label p4.Label)
                                Components = [| for t in 0 .. p1.Components.Length - 1 -> 0.25 * p1.Components.[t] + 0.25 * p2.Components.[t] + 0.25 * p3.Components.[t] + 0.25 * p4.Components.[t] |]
                            }
       }

let fourPopulationApproximation s =
    let fourPopulations = s |> convertToArray |> closestSamples populations 80 |> Seq.map fst |> Seq.toArray |> getFourPopulations
    s |> convertToArray |> closestSamples fourPopulations 40  |>  Seq.iter (fun (p, d) -> printfn "%s @ %f" p.Label d)

