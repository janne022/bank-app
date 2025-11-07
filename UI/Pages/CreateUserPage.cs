using bank_app.Managers;
using bank_app.Utility;
using bank_app.Utility.UI;
using bank_app.Utility.UI.Components;
using bank_app.Utility.UI.Invokables;
using Figgle.Fonts;


namespace bank_app.UI.Pages
{
    internal class CreateUserPage : Page
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

            List<DropdownItem<bool>> authTypeItems =
                [
                new DropdownItem<bool>("2-Step Disabled", false),
                new DropdownItem<bool>("2-Step Enabled", true)
                ];

            var navbar = new Navbar(
                [
                new("Home", PageType.AdminDashboard),
                new("User", PageType.CreateUserPage),
                new("Transactions", PageType.TransactionLogPage),
                new("Rates", PageType.UpdateRatePage),
                new NavbarItem("Logout", PageType.LogOut)
                ]);

            var userNavbar = new Navbar(
                [
                new("Create User", PageType.CreateUserPage),
                new("Unlock User", PageType.UnlockUserPage),
            ]);

            Grid grid = new(1, 1);
            grid.AddGridComponent(0, 0, navbar)
                .SetJustify(Justify.Center)
                .SetAlign(Align.Top);
            grid.AddGridComponent(0, 0, userNavbar);
            grid.AddGridComponent(0, 0, new AsciiArt(AsciiType.String, FiggleFonts.Small.Render("Create a new user")));

            var loginInvoke = new Invokable<UserType, string, string, string, string, string, bool>(CreateUserHandler);

            _createUserForm = new Form(
            [
                new Dropdown<UserType>(userTypeItems),
                new InputField("Username",InputFieldType.Normal,24),
                new InputField("Legal Name", InputFieldType.Normal, 24),
                new InputField("Password", InputFieldType.Password,24),
                new InputField("E-mail",InputFieldType.Normal, 24),
                new InputField("Phone number",InputFieldType.Number, 15),
                new Dropdown<bool>(authTypeItems),
            ], new Button(loginInvoke, "Create User", marginTop: 1));
            grid.AddGridComponent(0, 0, new Panel(_createUserForm, LayoutBorder.Rounded, "Create User", 55, 13, ColourFG.Red));

            return new Panel(grid, LayoutBorder.Heavy);
        }

        /// <summary>
        /// Invoked by a button connected to a form. Uses the data from the form to create a new User object using methods from UserManager.cs. 
        /// </summary>
        private void CreateUserHandler(UserType userType, string userName, string legalName, string password, string email, string phone, bool twoFactorEnabled)
        {
            if (UserManager.GetUser(userName) != null)
            {
                _createUserForm?.UpdateErrorMessage("Username already exists");
            }

            else
            {
                try
                {
                    UserManager.CreateUser(userName, password, userType, email, phone, legalName, twoFactorEnabled);
                    PageManager.SwitchPage(PageType.AdminDashboard);
                }

                catch (Exception)
                {
                    _createUserForm?.UpdateErrorMessage("An error occured");
                }
            }
        }
    }
}
