module Admixture.GeneticOracles

open System
open GAF
open GAF.Operators
open Admixture.Oracles
open Admixture.Populations

let runGeneticOracle (populations:_[]) sample =
    
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
