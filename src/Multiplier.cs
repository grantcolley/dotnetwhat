namespace dotnetwhat.library
{
    public class Multiplier
    {
        public int Multiply(int value1, int value2)
        {
            Func<int, int, int> local = (v1, v2) => v1 * v2;

            return local(value1, value2);
        }
    }
}
