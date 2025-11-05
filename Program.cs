using bank_app.UI;
using bank_app.Managers;
using bank_app.Utility;
using bank_app.Models.Users;
using bank_app.Models.Accounts;

internal class Program
{
    private static void Main(string[] args)
    {
        // Set the console to use UTF8 encoding to allow for more colors and characters
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;
        // Starts the async method inside a background thread that needs to run without getting blocked from ex. UI
        Task.Run(BackgroundThread);

        UserManager.CreateUser("admin", "admin", UserType.Admin, "admin@gmail.com", "0709491 135", "Creator");
        UserManager.CreateUser("vira", "hera123", UserType.Admin, "admina@gmail.com", "+46 709 421 135", "El Vira");
        UserManager.CreateUser("theo", "123", UserType.Client, "admina@gmail.com", "+46 709 421 135", "El Vira");

        // Start the first User Interface page
        PageManager.Start(PageType.LoginPage);
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