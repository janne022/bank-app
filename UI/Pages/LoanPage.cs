using bank_app.Utility;
using bank_app.Utility.Components;
using bank_app.Utility.UI;
using bank_app.Utility.UI.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.UI.Pages
{
    public class LoanPage : Page
    {
        internal override UIComponent LoadPage()
        {
            Console.CursorVisible = false;
            var navbar = new Navbar(
                [new NavbarItem("Home", PageType.ClientDashboard),
                new NavbarItem("Transfer", PageType.Transfer),
                new NavbarItem("Transaction", PageType.Transaction),
                new NavbarItem("Account", PageType.Account),
                new NavbarItem("Loan", PageType.Loan)
                ]);
            Grid grid = new(3, 3);
            grid.AddGridComponent(0, 1, navbar);
            grid.AddGridComponent(1, 1, new Text("Loan"));
            return new Panel(grid, LayoutBorder.Heavy);
        }
    }
}
