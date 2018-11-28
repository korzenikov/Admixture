#load "ComponentParser.fs"

open System.IO
open Admixture.ComponentParser

let getRefComponents path = 
    let lines = File.ReadAllLines(path) 
    lines.[0].Split([|','|]).[1..]

let outputToFile (referenceFile:string) inputFile (outputFile:string) = 
    use sw = new StreamWriter(outputFile)
    match getComponents inputFile |> checkSum with
    | Ok components ->
        let refComponents = getRefComponents referenceFile
        match components |> getInvalidComponents refComponents with
        | None ->
            refComponents 
            |> Seq.append ["Population"]
            |> String.concat ","
            |> sw.WriteLine
        
            let componentMap = components |> Map.ofArray
            let sampleComponents = 
                refComponents |> Seq.map(fun n -> 
                    match componentMap.TryFind n with
                    | Some v ->
                        v
                    | None ->
                        0M)
            
            sampleComponents 
            |> Seq.map string 
            |> Seq.append ["Sample"] 
            |> String.concat "," 
            |> sw.WriteLine
            Ok "Done"
        | Some invalidComponents -> 
            Error (sprintf "Invalid components: %s" <| (invalidComponents |> String.concat ", "))
             
    | Error e -> Error e

outputToFile @"D:\nMonte\k36.csv" @"D:\nMonte\components.txt" @"D:\nMonte\k36sample.csv"

getRefComponents @"D:\nMonte\k36.csv"