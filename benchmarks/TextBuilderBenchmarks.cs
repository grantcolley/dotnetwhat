using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using dotnetwhat.library;

namespace dotnetwhat.benchmarks
{
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public class TextBuilderBenchmarks
    {
        private TextBuilder textBuilder;
        private static string sentence;

        [GlobalSetup]
        public void GlobalSetup()
        {
            textBuilder = new TextBuilder();
            sentence = "Hello world!";
        }

        [Benchmark]
        public void String_Concatenate_Two_Strings()
        {
            textBuilder.StringConcatenateTwoStrings(sentence);
        }

        [Benchmark]
        public void String_Concatenate_Five_Strings()
        {
            textBuilder.StringConcatenateFiveStrings(sentence);
        }

        [Benchmark]
        public void String_Concatenate_Ten_Strings()
        {
            textBuilder.StringConcatenateTenStrings(sentence);
        }

        [Benchmark]
        public void StringBuilder_Append_Two_Strings()
        {
            textBuilder.StringBuilderAppendTwoStrings(sentence);
        }

        [Benchmark]
        public void StringBuilder_Append_Five_Strings()
        {
            textBuilder.StringBuilderAppendFiveStrings(sentence);
        }

        [Benchmark]
        public void StringBuilder_Append_Ten_Strings()
        {
            textBuilder.StringBuilderAppendTenStrings(sentence);
        }
    }
}
