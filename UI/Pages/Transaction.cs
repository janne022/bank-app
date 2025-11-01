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
    public class Transaction : Page
    {
        internal override UIComponent LoadPage()
        {
            Console.CursorVisible = false;
            var navbar = new Navbar(new List<NavbarItem> { new NavbarItem("Home", PageType.ClientDashboard), new NavbarItem("Transfer", PageType.Transfer), new NavbarItem("Transaction", PageType.Transaction), new NavbarItem("Account", PageType.Account), new NavbarItem("Loan", PageType.Loan) });
            // Create a new Invokable object with the method that is going to run once the 'Login' button is pressed
            // Create new 3x3 grid
            Grid grid = new Grid(3, 3);
            grid.AddGridComponent(0, 1, navbar);
            grid.AddGridComponent(1, 1, new Text("Transaction"));
            // Add grid to layout and set rounded border style
            return new Layout(grid, LayoutBorder.Heavy);
        }
    }
}
