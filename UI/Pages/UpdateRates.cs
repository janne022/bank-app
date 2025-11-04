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
            var navbar = new Navbar([ new("Home", PageType.AdminDashboard), new("Create User", PageType.CreateUser),
                new("Transactions", PageType.CheckTransactions), new("Rates", PageType.UpdateRates)]);

            List<DropdownItem<Currency>> currencyItems = [
                new DropdownItem<Currency>("SEK", Currency.SEK),
                new DropdownItem<Currency>("SLC", Currency.SLC),
                new DropdownItem<Currency>("USD", Currency.USD),
                new DropdownItem<Currency>("EUR", Currency.EUR),
                new DropdownItem<Currency>("GBP", Currency.GBP),
                new DropdownItem<Currency>("JPY", Currency.JPY),
                new DropdownItem<Currency>("AUD", Currency.AUD),
                new DropdownItem<Currency>("CAD", Currency.CAD),
                new DropdownItem<Currency>("CHF", Currency.CHF),
                new DropdownItem<Currency>("CNY", Currency.CNY),
                new DropdownItem<Currency>("NZD", Currency.NZD)
            ];
            var loginInvoke = new Invokable<Currency, string>(UpdateRateHandler);
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
                new Dropdown<Currency>(currencyItems),
                new InputField("New rate",false,16),

                }, new Button(loginInvoke, "Apply", marginTop: 2));
            grid.AddGridComponent(1, 1, new Panel(_rateForm, LayoutBorder.Rounded, "Login", 40, 10, ColourFG.GreenBright));
            grid.GetGridCell(1, 1).Align = Align.Top;

            return new Panel(grid, LayoutBorder.Heavy);
        }

        private void UpdateRateHandler(Currency currency, string rate)
        {

            bool isConvert = decimal.TryParse(rate, out decimal decimalRate);

            if (isConvert)
            {
                CurrencyExchange.UpdateRate(currency, decimalRate);
            }
            else
            {
                throw new Exception("Invalid format");
            }



            PageManager.SwitchPage(PageType.AdminDashboard);
        }

    }
}
