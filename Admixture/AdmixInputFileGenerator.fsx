open System.IO
open System

let getRefComponents path = 
    let lines = File.ReadAllLines(path) 
    let numOfComponents = lines.[0].Split([|' '|]).[0] |> Convert.ToInt32
    lines.[1..numOfComponents]

let getComponents path =
    let lines = File.ReadAllLines(path) 
    lines |> Array.map (fun s -> 
                            let x = s.Split([|' '; '\t'|], StringSplitOptions.RemoveEmptyEntries)
                            x.[0], (if x.[1] = "-" then "0" else x.[1])
                       )

let outputToFile (outputFile:string) inputFile (referenceFile:string) = 
    use streamWriter = new StreamWriter(outputFile)
    let components = getComponents inputFile
    let sum = components |> Seq.map(fun (_, v) -> v |> Convert.ToDecimal) |> Seq.sum 
    if (sum > 100.01M) then
        printfn "The sum %M is greater than 100" sum
    elif (sum < 99.0M) then
        printfn "The sum %M is less than 100" sum
    else
        let refComponents = getRefComponents referenceFile

        let refComponentsSet = refComponents |> Set.ofArray

        let invalidComponents = components |> Array.where(fun (n, _) -> refComponentsSet.Contains n |> not) |> Array.map fst
        if invalidComponents.Length <> 0 then
            printfn "Invalid components:"
            for c in invalidComponents do
                printfn "%s" c
        else
            streamWriter.WriteLine "Person"
            streamWriter.WriteLine referenceFile
            streamWriter.WriteLine "Output.txt"
            streamWriter.WriteLine "20     {Quantity of approximations}"
            streamWriter.WriteLine "0.25    {Threshold of components to ignore noice (-1 for autodetect)}"
            streamWriter.WriteLine "1      {1 for enable gaussian method or 0 for disable}"
        
            let componentMap = components |> Map.ofArray
            for n in refComponents do
                let pct =
                    match componentMap.TryFind n with
                    | Some v ->
                        v
                    | None ->
                        "0"
                streamWriter.WriteLine (sprintf "%s     %s" pct n)
            printfn "Done"

outputToFile "D:\Admix4\input.txt" "D:\Admix4\components.txt" "D:\Admix4\k36_v1.txt"