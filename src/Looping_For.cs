using System.Text;

namespace dotnetwhat.library
{
    public class Looping_For
    {
        public string For()
        {
            StringBuilder sb = new StringBuilder();

            var funcs = new List<Func<int>>(2);

            for(int i = 0; i < 2; i++)
            {
                funcs.Add(() => i);
            }

            sb.Append(funcs[0]().ToString());
            sb.Append(funcs[1]().ToString());

            return sb.ToString();
        }
    }
}
