using bank_app.Managers;
using bank_app.Models.Users;
using bank_app.Utility;
using bank_app.Utility.Components;
using bank_app.Utility.UI;
using bank_app.Utility.UI.Components;
using bank_app.Utility.UI.Invokables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.UI.Pages
{
    public class AdminDashboard : Page
    {
        internal override UIComponent LoadPage()
        {
            var user = PageManager.GetCurrentUser();

            Console.CursorVisible = false;
            var navbar = new Navbar(
                [
                new NavbarItem("Home", PageType.AdminDashboard),
                new NavbarItem("Create User", PageType.CreateUser),
                new NavbarItem("Transactions", PageType.CheckTransactions),
                new NavbarItem("Rates", PageType.UpdateRates)
                ]);
            Grid grid = new(3, 3);
            grid.AddGridComponent(0, 1, navbar);
            grid.AddGridComponent(1, 1, new Text($"Welcome back {user?.UserId}!"));
            grid.GetGridCell(1, 1).SetAlign(Align.Middle).SetJustify(Justify.Center);
            return new Panel(grid, LayoutBorder.Heavy);
        }
    }
}
