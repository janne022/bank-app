using bank_app.Managers;
using bank_app.Models.Users;
using bank_app.Utility;
using bank_app.Utility.UI;
using bank_app.Utility.UI.Components;
using bank_app.Utility.UI.Invokables;
using Figgle.Fonts;


namespace bank_app.UI.Pages
{
    internal class ChangeUserInfoPage : Page
    {
        private Form? _changeUserInfoForm;
        internal override UIComponent LoadPage()
        {
            Console.CursorVisible = false;
            var navbar = new Navbar(
                [
                new("Home", PageType.ClientDashboard),
                new("Transfer", PageType.TransferPage),
                new("Transactions", PageType.TransactionPage),
                new("Account", PageType.CreateAccountPage),
                new("Loan", PageType.LoanPage),
                new NavbarItem("Change Info", PageType.ChangeUserInfoPage),
                new NavbarItem("Logout", PageType.LogOut)
                ]);

            var createAccountInvoke = new Invokable<string, string, string, string, string>(ChangeUserInfoHandler);

            Grid grid = new(3, 3);
            grid.AddGridComponent(0, 1, navbar)
                .SetJustify(Justify.Center)
                .SetAlign(Align.Top);
            grid.AddGridComponent(0, 1, new AsciiArt(AsciiType.String, FiggleFonts.Small.Render("Update Info"), ColourFG.Green));

            _changeUserInfoForm = new Form(
            [
                new InputField("FullName",InputFieldType.Normal,24),
                new InputField("Username",InputFieldType.Normal,24),
                new InputField("E-Mail",InputFieldType.Normal,24),
                new InputField("Phone number",InputFieldType.Number,16),
                new InputField("Password",InputFieldType.Password,24),
            ], new Button(createAccountInvoke, "Update", marginTop: 1));
            grid.AddGridComponent(1, 1, new Panel(_changeUserInfoForm, LayoutBorder.Rounded, "Update User Info", 55, 10, ColourFG.Green))
                .SetAlign(Align.Middle)
                .SetJustify(Justify.Center);

            return new Panel(grid, LayoutBorder.Heavy);
        }

        private void ChangeUserInfoHandler(string fullName, string username, string email, string phoneNumer, string password)
        {
            if (CurrentUser != null)
            {
                UserManager.ChangeUserInfo(CurrentUser, fullName, username, email, phoneNumer, password);
                PageManager.SwitchPage(PageType.ClientDashboard);
            }
            else
            {
                _changeUserInfoForm?.UpdateErrorMessage("Invalid User");
            }
        }
    }
}