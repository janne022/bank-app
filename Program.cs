using bank_app.Managers;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var timer = new PeriodicTimer(TimeSpan.FromMinutes(15));

        while(await timer.WaitForNextTickAsync())
        {
            TransactionManager.ProcessPendingTransactions();
        }
    }
}