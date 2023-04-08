using System.Text;

namespace dotnetwhat.library
{
    public class Looping_For
    {
        public string Loop()
        {
            StringBuilder sb = new StringBuilder();

            var funcs = new List<Func<int>>(2);

            for(int i = 0; i < 2; i++)
            {
                funcs.Add(() => i); // same copy of the closed variable is updated
            }

            sb.Append(funcs[0]().ToString()); // closed variable evaluated when delegate is invoked
            sb.Append(funcs[1]().ToString()); // closed variable evaluated when delegate is invoked

            return sb.ToString(); // returns 22
        }
    }
}
