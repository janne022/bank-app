using bank_app.UI;
using bank_app.Managers;
internal class Program
{
    private static void Main(string[] args)
    {
        // Set the console to use UTF8 encoding to allow for more colors and characters
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;
        // Starts the async method inside a background thread that needs to run without getting blocked from ex. UI
        Task.Run(BackgroundThread);

        // Start the first User Interface page
        Login.Menu();
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