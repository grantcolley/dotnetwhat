﻿// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using dotnetwhat.benchmarks;

var TextParserBenchmarkSummary = BenchmarkRunner.Run(typeof(TextParserBenchmarks));

Console.ReadLine();