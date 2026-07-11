using System.Text;

namespace dotnetwhat.library
{
    public class Looping_Foreach
    {
        public string Loop()
        {
            StringBuilder sb = new StringBuilder();

            var vals = new List<int> { 1, 2 };
            var funcs = new List<Func<int>>();

            foreach (int v in vals)
            {
                funcs.Add(() => v); // a fresh copy of the closed variable with each iteration
            }

            sb.Append(funcs[0]().ToString()); // closed variable evaluated when delegate is invoked
            sb.Append(funcs[1]().ToString()); // closed variable evaluated when delegate is invoked

            return sb.ToString(); // returns 12
        }
    }
}
