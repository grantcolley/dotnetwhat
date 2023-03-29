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
            paragraph = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
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

        [Benchmark]
        public void Get_Last_Word_Using_Array()
        {
            textParser.Get_Last_Word_Using_Array(paragraph);
        }
    }
}
