using bank_app.Managers;
using bank_app.Models.Accounts;
using bank_app.Utility;
using bank_app.Utility.UI;
using bank_app.Utility.UI.Components;
using bank_app.Utility.UI.Invokables;
using Figgle.Fonts;

namespace bank_app.UI.Pages
{
    public class LoanPage : Page
    {
        private Form? _createLoanForm;
        internal override UIComponent LoadPage()
        {
            Console.CursorVisible = false;

            var accounts = AccountManager.GetAllAccounts(CurrentUser.UserId).Where(account => account is LoanAccount);
            var accountsNavbarItems = accounts.Select(account => new DropdownItem<Models.Accounts.Account>(account.AccountID.ToString(), account)).ToList();
            var accountDropdown = new Dropdown<Models.Accounts.Account>(accountsNavbarItems);

            var navbar = new Navbar(
                [
                new("Home", PageType.ClientDashboard),
                new("Transfer", PageType.TransferPage),
                new("Transactions", PageType.TransactionPage),
                new("Account", PageType.CreateAccountPage),
                new("Loan", PageType.LoanPage)
                ]);

            var createAccountInvoke = new Invokable<Account, string>(CreateLoanHandler);

            Grid grid = new(3, 3);
            grid.AddGridComponent(0, 1, navbar)
                .SetJustify(Justify.Center)
                .SetAlign(Align.Top);
            grid.AddGridComponent(0, 1, new AsciiArt(AsciiType.String, FiggleFonts.Small.Render("Loans")));

            _createLoanForm = new Form(
            [
                accountDropdown,
                new InputField("Loan Amount",InputFieldType.Number,24, 0),
            ], new Button(createAccountInvoke, "Take Loan", marginTop: 1));
            grid.AddGridComponent(1, 1, new Panel(_createLoanForm, LayoutBorder.Rounded, "Take new Loan", 70, 15, ColourFG.Red))
                .SetAlign(Align.Middle)
                .SetJustify(Justify.Center);

            return new Panel(grid, LayoutBorder.Heavy);
        }

        private void CreateLoanHandler(Account loanAccount, string stringAmount)
        {
            var loanManager = new LoanManager();
            bool canConvert = decimal.TryParse(stringAmount, out decimal loanAmount);

            try 
            {
                loanManager.DisburseLoan(CurrentUser.UserId, loanAmount, loanAccount.AccountCurrency);
                PageManager.SwitchPage(PageType.LoanConfirmPage);
            }
            
            catch (Exception)
            {
                _createLoanForm?.UpdateErrorMessage("Loan exceeds limit");
            }
        }
    }
}
