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
            // Create a new Invokable object with the method that is going to run once the 'Login' button is pressed
            var loginInvoke = new Invokable<string, string>(LoginHandler);  //Check if we need to change back it later 
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
        //Just changed User to Guid userId
        private void LoginHandler(string userId, string password)
        {
            User? u = UserManager.Login(userId, password);
            if (u != null)
            {
                PageManager.SwitchUser(u);
                // Login successful, switch page and give feedback
                if (u is Client client)
                {
                    PageManager.SwitchPage(PageType.ClientDashboard);
                }
                else if (u is Admin admin)
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
