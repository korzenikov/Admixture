module Admixture.Populations

open System
open System.IO

type Population = { Label:string; Components:float[] }

let convert (s:string) = Convert.ToDouble(s)

let population (row:string[]) = 
    {
        Label = row.[0]; 
        Components = row.[1..] |> Array.map convert 
    }

let getPopulations (separtor:char) path = 
    let lines = File.ReadAllLines(path) 
    let components = lines.[0].Split(separtor).[1..]
    let populations = lines.[1..] |> Array.map (fun s -> s.Split(separtor) |> population)
    components, populations