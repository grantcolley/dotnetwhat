namespace dotnetwhat.library
{
    public class CapturedVariable
    {
        public int IncrementLocalVariable()
        {
            int myLocalValue = 0;

            Func<int> increment = () => myLocalValue++;

            // NOTE: myLocalValue++ is a post-increment
            //       so calling increment() will return
            //       0 to variable stillZero:
            //       i.e. stillZero = 0
            //
            //       whereas myLocalValue will equal 1
            //       after the post-increment kicks in
            //       i.e. myLocalValue = 1

            int stillZero = increment();

            return myLocalValue;
        }
    }
}
