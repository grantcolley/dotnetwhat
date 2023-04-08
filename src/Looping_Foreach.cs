using System.Text;

namespace dotnetwhat.library
{
    public class Looping_Foreach
    {
        public string ForEach()
        {
            StringBuilder sb = new StringBuilder();

            var vals = new List<int> { 1, 2 };
            var funcs = new List<Func<int>>();

            foreach (int v in vals)
            {
                funcs.Add(() => v);
            }

            sb.Append(funcs[0]().ToString());
            sb.Append(funcs[1]().ToString());

            return sb.ToString();
        }
    }
}
