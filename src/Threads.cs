namespace dotnetwhat.library
{
    public class Threads
    {
        public void RunThread()
        {
            var message = "Hello World!";

            var thread = new Thread(WriteToConsole);
            thread.IsBackground = true;
            thread.Start(message);            
        }

        private static void WriteToConsole(object stateInfo)
        {
            Console.WriteLine(stateInfo);
        }
    }
}
