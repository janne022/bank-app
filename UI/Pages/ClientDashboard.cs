using bank_app.Managers;
using bank_app.Models.Users;
using bank_app.Utility;
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
    public class ClientDashboard : Page
    {
        internal override UIComponent LoadPage()
        {
            Console.CursorVisible = false;
            var navbar = new Navbar(new List<NavbarItem> { new NavbarItem("Home", PageType.Login), new NavbarItem("Transfer", PageType.AdminDashboard) });
            // Create a new Invokable object with the method that is going to run once the 'Login' button is pressed
            // Create new 3x3 grid
            Grid grid = new Grid(3, 3);
            // Add Menu with two inputfields and button inside to center middle of grid. Note: Button is currently not finished, so it won't be rendered
            GridCell cell = grid.GetGridCell(1, 1);
            cell.Justify = Justify.Center;
            cell.Align = Align.Middle;
            cell.OrderBy = OrderBy.Column;
            grid.AddGridComponent(1, 1, new Layout(new Form(new List<UIComponent>{new InputField("Username",false,16),
                new InputField("Password",true,16)}), LayoutBorder.Rounded, "Login", 40, 10, ColourFG.Yellow));
            grid.AddGridComponent(0, 1, navbar);
            // Add grid to layout and set rounded border style
            return new Layout(grid, LayoutBorder.Heavy);

        }
    }
}
