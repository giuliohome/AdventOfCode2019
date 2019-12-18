﻿// Day 16 Part 2
//https://adventofcode.com/2019/day/16
// based on https://github.com/ephemient/aoc2019/blob/py/src/aoc2019/day16.py
open System
open System.IO
open Checked


let parse (str:string) : int list =
    str.ToCharArray()
    |> Array.map(fun c -> int <| string c)
    |> Array.toList

    

[<EntryPoint>]
let main _ =
    
    use sr = new StreamReader("C:\dev\FSharp\AdventOfCode\Day16\input.txt")
    let input = 
        sr.ReadLine()
        |> parse 

    let offset = 
        input
            |> List.take 7
            |> List.fold
                (fun acc x -> 10 * acc + x)
                0
    let n = 10000 * input.Length - offset
    let valuefolded =
        System.Collections.Generic.List(
            [0..( (n - (offset % input.Length) - 1) / input.Length)]
            |> List.fold
                (fun acc _ -> List.append acc input)
                (input
                |> List.skip (offset % input.Length)))
    for repeat in [0..99] do  
        let mutable acc = 0
        for i in ([0..(n-1)] |> List.rev) do
            valuefolded.[i] <- Math.Abs(acc + valuefolded.[i]) % 10
            acc <- valuefolded.[i]
        
    let answer2 = valuefolded |> Seq.take 8

    printfn "Answer Part 2 is %s" (String.Join("",answer2))
    
    Console.ReadKey() |> ignore
    0    