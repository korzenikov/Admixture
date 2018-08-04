// Learn more about F# at http://fsharp.org

open System
open AdmixtureLib

[<EntryPoint>]
let main argv =
    let components, populations = Populations.getPopulations ',' "D:\Populations\K15.csv"
    printfn "%d" Clustering.train 
    0 // return an integer exit code
