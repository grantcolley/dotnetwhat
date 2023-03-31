using System.Text;

namespace dotnetwhat.library
{
    public class TextBuilder
    {
        public string StringConcatenateTwoStrings(string sentence)
        {
            sentence += sentence;

            return sentence;
        }

        public string StringConcatenateFiveStrings(string sentence)
        {
            for (int i = 0; i < 5; i++) 
            {
                sentence += sentence;
            }

            return sentence;
        }

        public string StringConcatenateTenStrings(string sentence)
        {
            for (int i = 0; i < 10; i++)
            {
                sentence += sentence;
            }

            return sentence;
        }

        public string StringBuilderAppendTwoStrings(string sentence)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(sentence);
            stringBuilder.Append(sentence);

            return stringBuilder.ToString();
        }

        public string StringBuilderAppendFiveStrings(string sentence)
        {
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < 5; i++)
            {
                stringBuilder.Append(sentence);
            }

            return stringBuilder.ToString();
        }

        public string StringBuilderAppendTenStrings(string sentence)
        {
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < 10; i++)
            {
                stringBuilder.Append(sentence);
            }

            return stringBuilder.ToString();
        }
    }
}
