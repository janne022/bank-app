using bank_app.Managers;
using bank_app.Models.Users;
using bank_app.Utility;
using bank_app.Utility.Components;
using bank_app.Utility.UI;
using bank_app.Utility.UI.Components;
using bank_app.Utility.UI.Invokables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bank_app.UI;


namespace bank_app.UI.Pages
{
    internal class CreateUser : Page
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
                new("Create User", PageType.CreateUser),
                new("Transactions", PageType.CheckTransactions),
                new("Rates", PageType.UpdateRates)
                ]);

            var loginInvoke = new Invokable<UserType, string, string, string, string, string>(CreateUserHandler);

            Grid grid = new(3, 3);
            grid.AddGridComponent(0, 1, navbar);
            grid.AddGridComponent(0, 1, new Text("Create a new user"));

            _createUserForm = new Form(
            [
                new Dropdown<UserType>(userTypeItems),
                new InputField("Username",InputFieldType.Normal,24, 0),
                new InputField("Legal Name", InputFieldType.Normal, 24, 0),
                new InputField("Password", InputFieldType.Password,24, 0),
                new InputField("E-mail",InputFieldType.Normal, 24, 0),
                new InputField("Phone number",InputFieldType.Number, 15, 0),
            ], new Button(loginInvoke, "Create User", marginTop: 1));
            grid.AddGridComponent(1, 1, new Panel(_createUserForm, LayoutBorder.Rounded, "Login", 70, 15, ColourFG.Red))
                .SetAlign(Align.Middle)
                .SetJustify(Justify.Center);

            return new Panel(grid, LayoutBorder.Heavy);
        }

        private void CreateUserHandler(UserType userType, string userName, string legalName, string password, string email, string phone)
        {
            UserManager.CreateUser(userName, password, userType, email, phone, legalName);
            PageManager.SwitchPage(PageType.AdminDashboard);
        }
    }
}
