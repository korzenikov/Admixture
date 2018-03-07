#r @"D:\Repos\Admixture\Admixture\packages\GAF.2.3.1\lib\net461\GAF.461.dll"

open System
open System.IO
open GAF
open GAF.Operators

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

let path = @"D:\Repos\Admixture\Admixture\k36_v1_dots.txt"

let lines = File.ReadAllLines(path) 

let components  =
    lines.[1..36]

let populations = 
    lines.[37..] 
    |> Array.map (split >> population)

let distance (p1: float[]) (p2: float[]) = (p1, p2) ||> Array.map2 (fun p1 p2 -> (p1 - p2)*(p1 - p2)) |> Array.sum


let convertToArray c = 
    let map = c |> Seq.map (fun (c, v) -> c.ToString(), v) |> Map.ofSeq 
    components |> Array.map (fun x -> 
                        match map.TryFind x with
                        | Some v -> v
                        | None -> 0.0)


let combineComponents (p:float[][]) =
    let len = p.Length |> float
    let seed : float array = Array.zeroCreate p.[0].Length
    let sum = p |> Array.fold (fun acc elem -> (acc, elem) ||> Array.map2 (+)) seed 
    sum |> Array.map (fun x -> x / len)


let Andrei = 
    [
        Amerindian, 0.53
        Basque, 1.64
        Central_Euro, 10.14
        East_Balkan, 3.42
        East_Central_Euro, 23.31
        Eastern_Euro, 22.51
        Fennoscandian, 16.14
        Iberian, 1.13
        North_Atlantic, 8.1
        North_Caucasian, 0.47
        North_Sea, 5.15
        Omotic, 0.47
        Siberian, 0.29
        South_Asian, 1.20
        VolgaUral, 5.48
    ]

let Oxon = 
    [
        Amerindian, 1.76
        Basque, 2.47
        Central_Euro, 8.02
        East_Balkan, 1.63
        East_Central_Euro, 17.64
        Eastern_Euro, 20.59
        Fennoscandian, 19.78
        French, 3.50
        Italian, 0.62
        North_Atlantic, 8.22
        North_Caucasian, 5.14
        North_Sea, 8.43
        VolgaUral, 2.15
    ]

let WindBringer = 
    [
        East_Central_Euro, 25.38
        Eastern_Euro, 20.79
        Fennoscandian, 12.03
        North_Sea, 8.62
        Iberian, 7.63
        Central_Euro,  7.29
        East_Balkan, 6.93
        French, 3.59
        VolgaUral, 2.95
        North_Caucasian, 2.88
        North_Atlantic, 1.71
        Central_African, 0.21
    ]

let sample = WindBringer |> convertToArray

let pop = new GAF.Population()

for p in 0 .. populations.Length - 1 do
    let chromosome = new Chromosome()
    chromosome.Genes.AddRange(seq { 1.. 8} |> Seq.map (fun _ -> new Gene(p)))
    pop.Solutions.Add(chromosome)

let elite = new Elite(1)

let crossover = new Crossover(0.8)
crossover.CrossoverType <- CrossoverType.SinglePoint

let mutate = new BinaryMutate(0.02)

let getPopulationsFromChromosome (ch:Chromosome) =
    ch.Genes |> Seq.map (fun x -> populations.[Math.Abs((int)x.RealValue) % populations.Length])
    
let evaluateResult (ch:Chromosome) (unknown:float[]) =
    let comps = ch |> getPopulationsFromChromosome |> Seq.map (fun x -> x.Components) |> Seq.toArray |> combineComponents
    Math.Sqrt(distance comps unknown)

let fitness (ch:Chromosome) = 
    1.0 - (evaluateResult ch sample)/600.0

let ga = new GeneticAlgorithm(pop, new FitnessFunction(fitness))

let OnRunComplete (e : GaEventArgs) =
    let fittest = e.Population.GetTop(1).[0]
    let pops = fittest |> getPopulationsFromChromosome |> Seq.toArray
    let label = (" + ", pops |> Array.map (fun x -> x.Label) |> Array.groupBy id |> Array.map (fun (k, g) -> k, g.Length) |> Array.sortByDescending snd |> Array.map (fun (k, c) -> sprintf "%d/%d %s" c pops.Length k)) |> System.String.Join
    let dist = evaluateResult fittest sample
    printfn "%s @ %f" label dist

let OnGenerationComplete (e : GaEventArgs) =
    if e.Generation % 100 = 0 then
        let fittest = e.Population.GetTop(1).[0]
        let dist = evaluateResult fittest sample
        printfn "Generation: %i, Fitness: %f, Distance: @ %f" e.Generation fittest.Fitness dist

ga.OnRunComplete.Add(OnRunComplete)
ga.OnGenerationComplete.Add(OnGenerationComplete)

ga.Operators.Add(elite)
ga.Operators.Add(crossover)
ga.Operators.Add(mutate)

ga.Run(new TerminateFunction(fun _ g _ -> g > 1000))

 //populations |> Array.indexed  |> Array.where (fun (i, x) -> x.Label.StartsWith("Russia")) 