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
            UserManager.CreateUser("janne","jan",UserType.Admin);
            // Create a new Invokable object with the method that is going to run once the 'Login' button is pressed
            var loginInvoke = new Invokable<string, string>(LoginHandler);
            // Create new 3x3 grid
            Grid grid = new Grid(3, 3);
            // Add Menu with two inputfields and button inside to center middle of grid. Note: Button is currently not finished, so it won't be rendered
            Flexbox cell = grid.GetGridCell(1, 1);
            cell.Justify = Justify.Center;
            cell.Align = Align.Middle;
            cell.OrderBy = OrderBy.Column;
            _loginForm = new Form(new List<UIComponent>{new InputField("Username",false,16),
                new InputField("Password",true,16),
                }, new Button(loginInvoke, "Login", marginTop: 2));
            grid.AddGridComponent(1, 1, new Panel(_loginForm, LayoutBorder.Rounded, "Login", 40, 10, ColourFG.Yellow));
            // Add text to center bottom
            cell = grid.GetGridCell(0, 1);
            cell.Justify = Justify.Center;
            cell.Align = Align.Middle;
            cell.OrderBy = OrderBy.Column;
            grid.AddGridComponent(0, 1, new AsciiArt("Assets/Ascii/bank.txt"));
            // Add grid to layout and set rounded border style
            return new Panel(grid, LayoutBorder.Heavy);
        }

        private void LoginHandler(string username, string password)
        {
            User? user = UserManager.Login(username, password);
            if (user != null)
            {
                PageManager.SwitchUser(user);
                // Login successful, switch page and give feedback
                if (user is Client)
                {
                    PageManager.SwitchPage(PageType.ClientDashboard);
                }
                else if (user is Admin)
                {
                    PageManager.SwitchPage(PageType.AdminDashboard);
                }
            }
            else
            {
                _loginForm.UpdateErrorMessage("Wrong username or password");
            }
        }
    }
}
