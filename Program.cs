using bank_app.UI;

internal class Program
{
    private static void Main(string[] args)
    {
        // Set the console to use UTF8 encoding to allow for more colors and characters
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;
        Login.Menu();
    }
}