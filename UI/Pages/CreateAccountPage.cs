using bank_app.Managers;
using bank_app.Models.Users;
using bank_app.UI;
using bank_app.Utility;
using bank_app.Utility.Components;
using bank_app.Utility.UI;
using bank_app.Utility.UI.Components;
using bank_app.Utility.UI.Invokables;
using Figgle.Fonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace bank_app.UI.Pages
{
    internal class CreateAccountPage : Page
    {
        private Form? _createAccountForm;
        internal override UIComponent LoadPage()
        {
            Console.CursorVisible = false;
            List<DropdownItem<AccountType>> accountTypeItems =
                [
                new DropdownItem<AccountType>("Checking Account", AccountType.CheckingAcc),
                new DropdownItem<AccountType>("Savings Account", AccountType.SavingsAcc),
                new DropdownItem<AccountType>("Loan Account", AccountType.LoanAcc)
                ];

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
            var navbar = new Navbar(
                [
                new("Home", PageType.ClientDashboard),
                new("Transfer", PageType.TransferPage),
                new("Transactions", PageType.TransactionPage),
                new("Create Account", PageType.CreateAccountPage),
                new("Loan", PageType.LoanPage)
                ]);

            var loginInvoke = new Invokable<string, string, string>(CreateAccountHandler);

            Grid grid = new(3, 3);
            grid.AddGridComponent(0, 1, navbar)
                .SetJustify(Justify.Center)
                .SetAlign(Align.Top);
            grid.AddGridComponent(0, 1, new AsciiArt(AsciiType.String, FiggleFonts.Small.Render("Create a new user")));

            _createAccountForm = new Form(
            [
                new Dropdown<AccountType>(accountTypeItems),
                new Dropdown<Currency>(currencyItems),
                new InputField("Deposit amount",InputFieldType.Number,24, 0),
            ], new Button(loginInvoke, "Create", marginTop: 1));
            grid.AddGridComponent(1, 1, new Panel(_createAccountForm, LayoutBorder.Rounded, "Login", 70, 15, ColourFG.Red))
                .SetAlign(Align.Middle)
                .SetJustify(Justify.Center);

            return new Panel(grid, LayoutBorder.Heavy);
        }

        private void CreateAccountHandler(string accountType, string currency, string depositAmount)
        {
           
        }
    }
}