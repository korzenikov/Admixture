#load "Populations.fs"
#load "Oracles.fs"

#r @"D:\Repos\Admixture\Admixture\packages\Accord.3.8.0\lib\net46\Accord.dll"
#r @"D:\Repos\Admixture\Admixture\packages\Accord.Math.3.8.0\lib\net46\Accord.Math.dll"
#r @"D:\Repos\Admixture\Admixture\packages\Accord.Math.3.8.0\lib\net46\Accord.Math.Core.dll"
#r @"D:\Repos\Admixture\Admixture\packages\Accord.Statistics.3.8.0\lib\net46\Accord.Statistics.dll"
#r @"D:\Repos\Admixture\Admixture\packages\FSharp.Charting.0.91.1\lib\net45\FSharp.Charting.dll"

open Admixture.Populations
open Accord.Statistics.Analysis;
open FSharp.Charting


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

let components, populations = getPopulations ',' "D:\Populations\K13.csv"

let data = populations |> Array.map (fun x -> x.Components)

let pca = new PrincipalComponentAnalysis()

pca.Method <- PrincipalComponentMethod.Center
pca.Whiten <- true

let transform = pca.Learn(data) 

let output = pca.Transform(data)

Chart.Point(output |> Array.map (fun x -> x.[0], x.[1]), "PCA", "PCA", populations |> Array.map (fun x -> x.Label)) |> Chart.Show
//let populationByName n =
//    let index = populations |> Array.findIndex(fun x -> x.Label = n)
//    populations.[index]

//let convertToArray components c = 
//    let map = c |> Seq.map (fun (c, v) -> c.ToString(), v) |> Map.ofSeq 
//    components |> Array.map (fun x -> 
//                        match map.TryFind x with
//                        | Some v -> v
//                        | None -> 0.0)

//let Andrei = 
//    [
//        Amerindian, 0.53
//        Basque, 1.64
//        Central_Euro, 10.14
//        East_Balkan, 3.42
//        East_Central_Euro, 23.31
//        Eastern_Euro, 22.51
//        Fennoscandian, 16.14
//        Iberian, 1.13
//        North_Atlantic, 8.1
//        North_Caucasian, 0.47
//        North_Sea, 5.15
//        Omotic, 0.47
//        Siberian, 0.29
//        South_Asian, 1.20
//        VolgaUral, 5.48
//    ]

//(components, Andrei) ||> convertToArray |> Seq.iteri (fun i x -> printfn "%s\t%f" components.[i] x)
  

////Andrei |> populationApproximation 4 50


//let lith =  populationByName "Lithuanian"

//populations
//|> Seq.map (fun x -> x, distance x.Components lith.Components ) 
//|> Seq.sortBy snd
//|> Seq.map (fun (x, d) -> x.Label, Math.Sqrt(d))
//|> Seq.iter (fun (x, d) -> printfn "%s\t%f" x d)