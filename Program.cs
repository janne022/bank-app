using bank_app.Managers;
internal class Program
{
    private static void Main(string[] args)
    {
        // Starts the async method inside a background thread that needs to run without getting blocked from ex. UI
        Task.Run(BackgroundThread);
    }
    private static async Task BackgroundThread()
    {
        var timer = new PeriodicTimer(TimeSpan.FromMinutes(15));

        while (await timer.WaitForNextTickAsync())
        {
            TransactionManager.ProcessPendingTransactions();
        }
    }
}