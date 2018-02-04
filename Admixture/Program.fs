open System
open System.IO

type Example = { Label:int; Pixels:int[] }

let split (s:string) = s.Split(',')

let dropHeader (x:_[]) = x.[1..]

let convert (s:string) = Convert.ToInt32(s)

let example (row:int[]) = { Label = row.[0]; Pixels = row.[1..]}

let path = "k36_v1_dots.txt"

File.ReadAllLines(path) 