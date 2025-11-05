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
    internal class CreateUserPage : Page
    {
        private Form? _createUserForm;
        internal override UIComponent LoadPage()
        {
            Console.CursorVisible = false;
            var navbar = new Navbar(new List<NavbarItem> { new NavbarItem("Home", PageType.AdminDashboard), new NavbarItem("Create User", PageType.CreateUser),
                new NavbarItem("Transactions", PageType.CheckTransactions), new NavbarItem("Rates", PageType.UpdateRates)});

            var loginInvoke = new Invokable<string, string, string, string, string, string>(CreateUserHandler);
            // Create a new Invokable object with the method that is going to run once the 'Login' button is pressed
            // Create new 3x3 grid
            Grid grid = new Grid(3, 3);
            grid.AddGridComponent(0, 1, navbar);
            grid.AddGridComponent(1, 1, new Text("Create a new user"));

            Flexbox cell = grid.GetGridCell(1, 1);
            cell.Justify = Justify.Center;
            cell.Align = Align.Middle;
            cell.OrderBy = OrderBy.Column;
            _createUserForm = new Form(new List<UIComponent>
            {
                new InputField("User Type",false,16),  //This will become dropdown menu later (choice client/admin)
                new InputField("Username",false,16),
                new InputField("Legal Name", false,16),
                new InputField("Password",true,16),
                new InputField("E-mail",false,16),
                new InputField("Phone number",false,16),
                }, new Button(loginInvoke, "Create User", marginTop: 1));
            grid.AddGridComponent(1, 1, new Panel(_createUserForm, LayoutBorder.Rounded, "Login", 40, 10, ColourFG.Red));
            grid.GetGridCell(1, 1).Align = Align.Top;

            return new Panel(grid, LayoutBorder.Heavy);
        }

        private void CreateUserHandler(string userTypeString, string userName, string legalName, string password, string email, string phone)
        {
            UserType userType;

            if (userTypeString ==  "Admin")
            {
                userType = UserType.Admin;
            }

            else 
            {
                userType = UserType.Client;
            }

            UserManager.CreateUser(userName, password, userType, email, phone, legalName);
            PageManager.SwitchPage(PageType.AdminDashboard);
        }
    }
}
