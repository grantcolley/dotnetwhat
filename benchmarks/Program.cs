// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using dotnetwhat.benchmarks;

// https://www.codemag.com/Article/2209061/Benchmarking-.NET-6-Applications-Using-BenchmarkDotNet-A-Deep-Dive
// https://www.stevejgordon.co.uk/introduction-to-benchmarking-csharp-code-with-benchmark-dot-net
// https://www.stevejgordon.co.uk/an-introduction-to-optimising-code-using-span-t
// https://learn.microsoft.com/en-us/archive/msdn-magazine/2018/january/csharp-all-about-span-exploring-a-new-net-mainstay
// https://learn.microsoft.com/en-us/dotnet/api/system.span-1?view=net-7.0

var TextParserBenchmarkSummary = BenchmarkRunner.Run(typeof(TextParserBenchmarks));

Console.ReadLine();