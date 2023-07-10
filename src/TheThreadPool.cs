namespace dotnetwhat.library
{
    public class TheThreadPool
    {
        public void RunThreadFromThreadPool()
        {
            var message = "Hello World!";

            ThreadPool.QueueUserWorkItem(WriteToConsole, message);
        }

        private static void WriteToConsole(object stateInfo)
        {
            Console.WriteLine(stateInfo);
        }
    }
}
