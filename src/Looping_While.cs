using System.Text;

namespace dotnetwhat.library
{
    public class Looping_While
    {
        public string While()
        {
            StringBuilder sb = new StringBuilder();

            var funcs = new List<Func<int>>(2);

            int i = 0;

            while(i < 2)
            {
                i++;
                funcs.Add(() => i);
            }

            sb.Append(funcs[0]().ToString());
            sb.Append(funcs[1]().ToString());

            return sb.ToString();
        }
    }
}
