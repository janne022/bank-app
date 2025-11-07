using bank_app.Managers;
using bank_app.Models.Users;
using bank_app.Utility;
using bank_app.Utility.UI;
using bank_app.Utility.UI.Components;
using bank_app.Utility.UI.Invokables;
using Figgle.Fonts;


namespace bank_app.UI.Pages
{
    internal class UnlockUserPage : Page
    {
        private Form? _createUserForm;
        internal override UIComponent LoadPage()
        {
            Console.CursorVisible = false;
            List<DropdownItem<UserType>> userTypeItems =
                [
                new DropdownItem<UserType>("Admin", UserType.Admin),
                new DropdownItem<UserType>("Client", UserType.Client)
                ];
            var navbar = new Navbar(
                [
                new("Home", PageType.AdminDashboard),
                new("Create User", PageType.CreateUserPage),
                new("Transactions", PageType.TransactionLogPage),
                new("Rates", PageType.UpdateRatePage),
                new NavbarItem("Logout", PageType.LogOut)
                ]);

            var userNavbar = new Navbar(
                [
                new("Create User", PageType.CreateUserPage),
                new("Unlock User", PageType.TransactionLogPage),
            ]);

            var loginInvoke = new Invokable<UserType, string, string, string, string, string, bool>(UnlockUserHandler);

            Grid grid = new(3, 3);
            grid.AddGridComponent(0, 1, navbar)
                .SetJustify(Justify.Center)
                .SetAlign(Align.Top);
            grid.AddGridComponent(0, 1, userNavbar);
            grid.AddGridComponent(0, 1, new AsciiArt(AsciiType.String, FiggleFonts.Small.Render("Create a new user")));

            _createUserForm = new Form(
            [
                new Dropdown<UserType>(userTypeItems),
                new InputField("Username",InputFieldType.Normal,24),
                new InputField("Legal Name", InputFieldType.Normal, 24),
                new InputField("Password", InputFieldType.Password,24),
                new InputField("E-mail",InputFieldType.Normal, 24),
                new InputField("Phone number",InputFieldType.Number, 15),
            ], new Button(loginInvoke, "Unlock User", marginTop: 1));
            grid.AddGridComponent(1, 1, new Panel(_createUserForm, LayoutBorder.Rounded, "Login", 55, 13, ColourFG.Red))
                .SetAlign(Align.Middle)
                .SetJustify(Justify.Center);

            return new Panel(grid, LayoutBorder.Heavy);
        }

        private void UnlockUserHandler(User user)
        {
            try
            {
                UserManager.UnlockAccount(user);
                PageManager.SwitchPage(PageType.AdminDashboard);
            }

            catch (Exception)
            {
                _createUserForm?.UpdateErrorMessage("An error occured");
            }
        }
    }
}
