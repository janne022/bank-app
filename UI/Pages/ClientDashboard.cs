using bank_app.Utility;
using bank_app.Utility.UI;
using bank_app.Utility.UI.Components;
using Figgle.Fonts;

namespace bank_app.UI.Pages
{
    public class ClientDashboard : Page
    {
        internal override UIComponent LoadPage()
        {
            Console.CursorVisible = false;
            var navbar = new Navbar(
                [
                new NavbarItem("Home", PageType.ClientDashboard),
                new NavbarItem("Transfer", PageType.TransferPage),
                new NavbarItem("Transaction", PageType.TransactionPage),
                new NavbarItem("Account", PageType.CreateAccountPage),
                new NavbarItem("Loan", PageType.LoanPage)
                ]);
            // Create a new Invokable object with the method that is going to run once the 'Login' button is pressed
            // Create new 3x3 grid
            Grid grid = new(3, 3);
            grid.AddGridComponent(0, 1, navbar)
                .SetJustify(Justify.Center)
                .SetAlign(Align.Top);
            grid.AddGridComponent(0, 1, new AsciiArt(AsciiType.String, FiggleFonts.Small.Render($"Welcome back {CurrentUser?.LegalName}")));
            // Add grid to layout and set rounded border style
            return new Panel(grid, LayoutBorder.Heavy);
        }
    }
}
