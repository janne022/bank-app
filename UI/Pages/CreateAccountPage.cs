using bank_app.Managers;
using bank_app.Models.Users;
using bank_app.Utility;
using bank_app.Utility.UI;
using bank_app.Utility.UI.Components;
using bank_app.Utility.UI.Invokables;
using Figgle.Fonts;


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
                new("Account", PageType.CreateAccountPage),
                new("Loan", PageType.LoanPage),
                new NavbarItem("Logout", PageType.LogOut)
                ]);

            var createAccountInvoke = new Invokable<AccountType, Currency, string, string>(CreateAccountHandler);

            Grid grid = new(3, 3);
            grid.AddGridComponent(0, 1, navbar)
                .SetJustify(Justify.Center)
                .SetAlign(Align.Top);
            grid.AddGridComponent(0, 1, new AsciiArt(AsciiType.String, FiggleFonts.Small.Render("Create a new account")));

            _createAccountForm = new Form(
            [
                new Dropdown<AccountType>(accountTypeItems),
                new Dropdown<Currency>(currencyItems),
                new InputField("Deposit amount",InputFieldType.Normal,24),
                new InputField("Account Label",InputFieldType.Normal,16),
            ], new Button(createAccountInvoke, "Create", marginTop: 1));
            grid.AddGridComponent(1, 1, new Panel(_createAccountForm, LayoutBorder.Rounded, "Create New Account", 55, 10, ColourFG.Red))
                .SetAlign(Align.Middle)
                .SetJustify(Justify.Center);

            return new Panel(grid, LayoutBorder.Heavy);
        }

        /// <summary>
        /// Invoked by a button connected to a form. Uses the data from the form to create a new Account object using methods from AccountManager.cs. 
        /// </summary>
        private void CreateAccountHandler(AccountType accountType, Currency currency, string stringAmount, string label)
        {
            User? currentUser = PageManager.GetCurrentUser();
            bool canConvert = decimal.TryParse(stringAmount, out decimal decimalAmount);

            AccountManager.CreateAccount(currentUser.UserId, currency, decimalAmount, accountType, label);

            if (accountType == AccountType.CheckingAcc)
            {
                PageManager.SwitchPage(PageType.CheckingAccountPageConfirm);
            }
            else if (accountType == AccountType.SavingsAcc)
            {
                PageManager.SwitchPage(PageType.SavingsAccountConfirmPage);
            }
        }
    }
}