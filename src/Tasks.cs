namespace dotnetwhat.library
{
    public class Tasks
    {
        public void RunTask()
        {
            var message = "Hello World!";

            _ = Task.Run(() => WriteToConsole(message));

            // this does the same thing as Task.Run()
            _ = Task.Factory.StartNew(() => WriteToConsole(message),
                    CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
        }

        private static void WriteToConsole(string stateInfo)
        {
            Console.WriteLine(stateInfo);
        }
    }
}
