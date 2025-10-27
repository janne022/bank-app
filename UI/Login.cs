using bank_app.Utility;
using bank_app.Utility.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.UI
{
    public static class Login
    {
        public static void Menu()
        {
            Console.CursorVisible = false;
            // Create new 3x3 grid
            Grid grid = new Grid(3, 3);
            // Add Menu with two inputfields and button inside to center middle of grid. Note: Button is currently not finished, so it won't be rendered
            GridCell cell = grid.GetGridCell(1, 1);
            cell.Justify = Justify.Center;
            cell.Align = Align.Middle;
            cell.OrderBy = OrderBy.Column;
            grid.AddGridComponent(1,1, new Menu(new List<UIComponent>{new InputField("Username",false,16),
                new InputField("Password",true,16),
                new Button(LoginFunction, "Login")}, 30));
            // Add text to center bottom
            cell = grid.GetGridCell(0, 1);
            cell.Justify = Justify.Center;
            cell.Align = Align.Middle;
            cell.OrderBy = OrderBy.Column;
            //grid.AddGridComponent(0, 0, new AsciiArt(art));
            grid.AddGridComponent(0, 1, new Text("Welcome Back to Chas Bank. Securely access your account to manage your finances, pay bills, and track your transactions."));
            // Add grid to layout and set rounded border style
            Layout layout = new Layout(grid, LayoutBorder.Heavy);
            layout.Render();
        }

        public static void LoginFunction(string username, string password)
        {
            Console.Clear();
            Console.WriteLine($"Username: {username}, Password:{password}");
            Console.ReadLine();
        }
    }
}
