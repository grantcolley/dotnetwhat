namespace dotnetwhat.library
{
    public class CapturedVariable
    {
        public int IncrementLocalVariable()
        {
            int myLocalValue = 0;

            Func<int> increment = () => myLocalValue++;

            increment();

            return myLocalValue;
        }
    }
}
