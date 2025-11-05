using bank_app.Managers;
using bank_app.Models.Users;
using bank_app.Utility;
using bank_app.Utility.Components;
using bank_app.Utility.UI;
using bank_app.Utility.UI.Components;
using bank_app.Utility.UI.Invokables;
using Figgle.Fonts;
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
                new NavbarItem("Create User", PageType.CreateUserPage),
                new NavbarItem("Transactions", PageType.TransactionLogPage),
                new NavbarItem("Rates", PageType.UpdateRatePage)
                ]);
            Grid grid = new(3, 3);
            grid.AddGridComponent(0, 1, navbar)
                .SetAlign(Align.Top)
                .SetJustify(Justify.Center)
                .SetOrderBy(OrderBy.Column);
            grid.AddGridComponent(0, 1, new AsciiArt(AsciiType.String, FiggleFonts.Small.Render($"Welcome back {user?.LegalName}!")));
            return new Panel(grid, LayoutBorder.Heavy);
        }
    }
}
