using bank_app.Utility;
using bank_app.Utility.Components;
using bank_app.Utility.UI;
using bank_app.Utility.UI.Components;

namespace bank_app.UI.Pages
{
    internal class TransactionLogPage : Page
    {
        internal override UIComponent LoadPage()
        {
            Console.CursorVisible = false;
            var navbar = new Navbar(
                [
                new NavbarItem("Home", PageType.AdminDashboard),
                new NavbarItem("Create User", PageType.CreateUserPage),
                new NavbarItem("Transactions", PageType.TransactionLogPage),
                new NavbarItem("Rates", PageType.UpdateRatePage),
                new NavbarItem("Logout", PageType.LogOut)
                ]);
            Grid grid = new(3, 3);
            grid.AddGridComponent(0, 1, navbar);
            grid.AddGridComponent(1, 1, new Text("Home"));
            return new Panel(grid, LayoutBorder.Heavy);
        }
    }
}
