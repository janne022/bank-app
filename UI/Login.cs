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
            // Create new 3x3 grid
            Grid grid = new Grid(3, 3);
            // Add Menu with two inputfields and button inside to middle of grid
            GridCell cell = grid.GetGridCell(1, 1);
            cell.Justify = Justify.Center;
            cell.Align = Align.Middle;
            cell.OrderBy = OrderBy.Column;
            grid.AddGridComponent(1,1, new Menu(new List<UIComponent>{new InputField("Username",false,16),
                new InputField("Password",true,16),
                new Button()}));
            // Add text to middle top of grid
            cell = grid.GetGridCell(0, 1);
            cell.Justify = Justify.Center;
            cell.Align = Align.Middle;
            cell.OrderBy = OrderBy.Row;
            grid.AddGridComponent(0, 1, new Text("Login"));
            grid.AddGridComponent(0, 1, new Text("Hello"));
            // Add grid to layout and set rounded border style
            Layout layout = new Layout(grid, LayoutBorder.Rounded);
            layout.Render();
        }
    }
}
