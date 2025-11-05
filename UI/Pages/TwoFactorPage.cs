using bank_app.Managers;
using bank_app.Models.Users;
using bank_app.Utility;
using bank_app.Utility.UI;
using bank_app.Utility.UI.Components;
using bank_app.Utility.UI.Invokables;
using Figgle.Fonts;

namespace bank_app.UI.Pages
{
    public class TwoFactorPage : Page
    {
        private Form? _loginForm;
        internal override UIComponent LoadPage()
        {
            var loginInvoke = new Invokable<string>(LoginHandler);
            Grid grid = new(3, 3);
            Flexbox cell = grid.GetGridCell(1, 1);
            cell.Justify = Justify.Center;
            cell.Align = Align.Middle;
            cell.OrderBy = OrderBy.Column;
            _loginForm = new Form(
                [
                new InputField("Code",InputFieldType.Number, 6, 0),
                ], new Button(loginInvoke, "Submit", marginTop: 2));
            grid.AddGridComponent(1, 1, new Panel(_loginForm, LayoutBorder.Rounded, "Authentication Code", 40, 10, ColourFG.Yellow));
            cell = grid.GetGridCell(0, 1).SetAlign(Align.Middle).SetJustify(Justify.Center);
            grid.AddGridComponent(0, 1, new AsciiArt(AsciiType.String, FiggleFonts.Small.Render("Authentication")));
            return new Panel(grid, LayoutBorder.Heavy);
        }

        private void LoginHandler(string code)
        {
            User? u = CurrentUser;
            bool success = UserManager.TwoFactorAuth(u.UserId, code);
            if (success)
            {
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
            else
            {
                _loginForm?.UpdateErrorMessage("Wrong code");
            }
        }
    }
}
