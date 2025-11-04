using bank_app.Managers;
using bank_app.Models.Users;
using bank_app.Utility;
using bank_app.Utility.UI;
using bank_app.Utility.UI.Components;
using bank_app.Utility.UI.Invokables;

namespace bank_app.UI.Pages
{
    public class Login : Page
    {
        private Form? _loginForm;
        internal override UIComponent LoadPage()
        {
            var loginInvoke = new Invokable<string, string>(LoginHandler);
            Grid grid = new(3, 3);
            Flexbox cell = grid.GetGridCell(1, 1);
            cell.Justify = Justify.Center;
            cell.Align = Align.Middle;
            cell.OrderBy = OrderBy.Column;
            _loginForm = new Form(
                [
                new InputField("Username",InputFieldType.Normal, 24, 0),
                new InputField("Password",InputFieldType.Password,24, 0),
                ], new Button(loginInvoke, "Login", marginTop: 2));
            grid.AddGridComponent(1, 1, new Panel(_loginForm, LayoutBorder.Rounded, "Login", 40, 10, ColourFG.Yellow));
            cell = grid.GetGridCell(0, 1).SetAlign(Align.Middle).SetJustify(Justify.Center);
            grid.AddGridComponent(0, 1, new AsciiArt("Assets/Ascii/bank.txt"));
            return new Panel(grid, LayoutBorder.Heavy);
        }

        private void LoginHandler(string userId, string password)
        {
            User? u = UserManager.Login(userId, password);
            if (u != null)
            {
                PageManager.SwitchUser(u);
                // Login successful, switch page and give feedback
                if (u is Client)
                {
                    PageManager.SwitchPage(PageType.ClientDashboard);
                }
                else if (u is Admin)
                {
                    PageManager.SwitchPage(PageType.AdminDashboard);
                }
            }
            else if (UserManager.GetUser(userId) != null && UserManager.GetUser(userId).CurrentAccountStatus == AccountStatus.Locked)
            {
                _loginForm?.UpdateErrorMessage("Account is locked");
            }
            else
            {
                _loginForm?.UpdateErrorMessage("Wrong username or password");
            }
        }
    }
}
