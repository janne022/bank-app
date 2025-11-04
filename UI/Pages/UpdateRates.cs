using bank_app.Managers;
using bank_app.Models;
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

namespace bank_app.UI.Pages
{
    internal class UpdateRates : Page
    {
        private Form? _rateForm;

        internal override UIComponent LoadPage()
        {
            Console.CursorVisible = false;
            var navbar = new Navbar(new List<NavbarItem> { new NavbarItem("Home", PageType.AdminDashboard), new NavbarItem("Create User", PageType.CreateUser),
                new NavbarItem("Transactions", PageType.CheckTransactions), new NavbarItem("Rates", PageType.UpdateRates)});

            var loginInvoke = new Invokable<string, string>(UpdateRateHandler);
            // Create a new Invokable object with the method that is going to run once the 'Login' button is pressed
            // Create new 3x3 grid
            Grid grid = new Grid(3, 3);
            grid.AddGridComponent(0, 1, navbar);
            grid.AddGridComponent(1, 1, new Text("Update the daily exchanges"));

            Flexbox cell = grid.GetGridCell(1, 1);
            cell.Justify = Justify.Center;
            cell.Align = Align.Middle;
            cell.OrderBy = OrderBy.Column;
            _rateForm = new Form(new List<UIComponent>
            {
                new InputField("Currency",false,16),  //This will become dropdown menu later (choice currency)
                new InputField("New rate",false,16),

                }, new Button(loginInvoke, "Apply", marginTop: 2));
            grid.AddGridComponent(1, 1, new Panel(_rateForm, LayoutBorder.Rounded, "Login", 40, 10, ColourFG.GreenBright));
            grid.GetGridCell(1, 1).Align = Align.Top;

            return new Panel(grid, LayoutBorder.Heavy);
        }

        private void UpdateRateHandler(string currencyString, string rate)
        {

            bool isConvert = decimal.TryParse(rate, out decimal decimalRate);


            if (Enum.TryParse<Currency>(currencyString, ignoreCase: true, out Currency currency))
            {

                if (isConvert)
                {
                    CurrencyExchange.UpdateRate(currency, decimalRate);
                }
                else
                {
                    throw new Exception("Invalid format");
                }

            }
            else
            {

                throw new Exception("Invalid currency");
            }

            

            PageManager.SwitchPage(PageType.AdminDashboard);
        }

    }
}
