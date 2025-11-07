using bank_app.Models;
using bank_app.Utility;
using bank_app.Utility.UI;
using bank_app.Utility.UI.Components;
using bank_app.Utility.UI.Invokables;
using Figgle.Fonts;

namespace bank_app.UI.Pages
{
    internal class UpdateRatePage : Page
    {
        private Form? _rateForm;

        internal override UIComponent LoadPage()
        {
            Console.CursorVisible = false;
            var navbar = new Navbar(
                [
                new("Home", PageType.AdminDashboard),
                new("Create User", PageType.CreateUserPage),
                new("Transactions", PageType.TransactionLogPage),
                new("Rates", PageType.UpdateRatePage),
                new NavbarItem("Logout", PageType.LogOut)
                ]);

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
            Grid grid = new(3, 3);
            grid.AddGridComponent(0, 1, navbar)
                .SetJustify(Justify.Center)
                .SetAlign(Align.Top);
            grid.AddGridComponent(0, 1, new AsciiArt(AsciiType.String, FiggleFonts.Small.Render("Update the daily exchanges")));

            _rateForm = new Form(
            [
                new Dropdown<Currency>(currencyItems),
                new InputField("New rate",InputFieldType.Normal, 16),

            ], new Button(loginInvoke, "Apply", marginTop: 2));
            grid.AddGridComponent(1, 1, new Panel(_rateForm, LayoutBorder.Rounded, "Update Rates", 40, 10, ColourFG.GreenBright));

            return new Panel(grid, LayoutBorder.Heavy);
        }

        private void UpdateRateHandler(Currency currency, string rate)
        {
            if (decimal.TryParse(rate, out decimal decimalRate))
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
