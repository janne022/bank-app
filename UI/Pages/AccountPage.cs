using bank_app.Utility;
using bank_app.Utility.Components;
using bank_app.Utility.UI;
using bank_app.Utility.UI.Components;

namespace bank_app.UI.Pages
{
    public class AccountPage : Page
    {
        internal override UIComponent LoadPage()
        {
            Console.CursorVisible = false;
            var navbar = new Navbar(
                [
                new NavbarItem("Home", PageType.ClientDashboard),
                new NavbarItem("Transfer", PageType.TransferPage),
                new NavbarItem("Transaction", PageType.TransactionPage),
                new NavbarItem("Account", PageType.AccountPage),
                new NavbarItem("Loan", PageType.LoanPage)
                ]);
            Grid grid = new(3, 3);
            grid.AddGridComponent(0, 1, navbar);
            grid.AddGridComponent(1, 1, new Text("Account"));
            return new Panel(grid, LayoutBorder.Heavy);
        }
    }
}
