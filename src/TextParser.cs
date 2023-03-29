using System;
using System.Linq;

namespace dotnetwhat.library
{
    public class TextParser
    {
        public string Get_Last_Word_Using_LastOrDefault(string paragraph)
        {
            var words = paragraph.Split(" ");

            var lastWord = words.LastOrDefault();

            return lastWord?.Substring(0, lastWord.Length - 1) ?? string.Empty;
        }

        public string Get_Last_Word_Using_Substring(string paragraph)
        {
            var lastSpaceIndex = paragraph.LastIndexOf(" ", StringComparison.Ordinal);

            var position = lastSpaceIndex + 1;
            var wordLength = paragraph.Length - position - 1;

            return lastSpaceIndex == -1
                ? string.Empty
                : paragraph.Substring(position, wordLength);
        }

        public ReadOnlySpan<char> Get_Last_Word_Using_Span(ReadOnlySpan<char> paragraph)
        {
            var lastSpaceIndex = paragraph.LastIndexOf(' ');

            var position = lastSpaceIndex + 1;
            var wordLength = paragraph.Length - position - 1;

            return lastSpaceIndex == -1
                ? ReadOnlySpan<char>.Empty
                : paragraph.Slice(position, wordLength);
        }

        public ArraySegment<char> Get_Last_Word_Using_Array(string paragraph)
        {
            var arrayParagraph = paragraph.ToCharArray();
            var lastSpaceIndex = Array.LastIndexOf(arrayParagraph, ' ');

            var position = lastSpaceIndex + 1;
            var wordLength = paragraph.Length - position - 1;

            return new ArraySegment<char>(arrayParagraph).Slice(position, wordLength);
        }
    }
}
