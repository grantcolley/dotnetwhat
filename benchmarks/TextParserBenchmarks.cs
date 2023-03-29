using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using dotnetwhat.library;

namespace dotnetwhat.benchmarks
{
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public class TextParserBenchmarks
    {
        private TextParser textParser;
        private static string paragraph;

        [GlobalSetup]
        public void GlobalSetup()
        {
            textParser = new TextParser();
            paragraph = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";
        }

        [Benchmark(Baseline = true)]
        public void Get_Last_Word_Using_LastOrDefault()
        {
            textParser.Get_Last_Word_Using_LastOrDefault(paragraph);
        }


        [Benchmark]
        public void Get_Last_Word_Using_Substring() 
        {
            textParser.Get_Last_Word_Using_Substring(paragraph);
        }

        [Benchmark]
        public void Get_Last_Word_Using_Span() 
        {
            textParser.Get_Last_Word_Using_Span(paragraph);
        }
    }
}
