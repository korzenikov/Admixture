﻿#load "ComponentParser.fs"

open System.IO
open Admixture.ComponentParser

let getRefComponents path = 
    let lines = File.ReadAllLines(path) 
    let numOfComponents = lines.[0].Split([|' '|]).[0] |> int
    lines.[1..numOfComponents]

let outputToFile (referenceFile:string) inputFile (outputFile:string) = 
    use sw = new StreamWriter(outputFile)
    match getComponents inputFile |> checkSum with
    | Ok components ->
        let refComponents = getRefComponents referenceFile
        match components |> getInvalidComponents refComponents with
        | None ->
            sw.WriteLine "Person"
            sw.WriteLine referenceFile
            sw.WriteLine "Output.txt"
            sw.WriteLine "20     {Quantity of approximations}"
            sw.WriteLine "0.25    {Threshold of components to ignore noice (-1 for autodetect)}"
            sw.WriteLine "1      {1 for enable gaussian method or 0 for disable}"
        
            let componentMap = components |> Map.ofArray
            for n in refComponents do
                let pct =
                    match componentMap.TryFind n with
                    | Some v ->
                        v
                    | None ->
                        0M
                sw.WriteLine (sprintf "%M     %s" pct n)
            Ok "Done"
        | Some invalidComponents -> 
            Error (sprintf "Invalid components: %s" <| (invalidComponents |> String.concat ", "))
             
    | Error e -> Error e

outputToFile "D:\Admix4\k36_v1.txt" "D:\Admix4\components.txt" "D:\Admix4\input.txt"