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

            var navbar = new Navbar(
                [
                new("Home", PageType.ClientDashboard),
                new("Transfer", PageType.TransferPage),
                new("Transactions", PageType.TransactionPage),
                new("Account", PageType.CreateAccountPage),
                new("Loan", PageType.LoanPage),
                new NavbarItem("Logout", PageType.LogOut)
                ]);

            var createAccountInvoke = new Invokable<string>(CreateLoanHandler);

            Grid grid = new(3, 3);
            grid.AddGridComponent(0, 1, navbar)
                .SetJustify(Justify.Center)
                .SetAlign(Align.Top);
            grid.AddGridComponent(0, 1, new AsciiArt(AsciiType.String, FiggleFonts.Small.Render("Loans")));

            _createLoanForm = new Form(
            [
                new InputField("Loan Amount (SEK)",InputFieldType.Number,24),
            ], new Button(createAccountInvoke, "Take Loan", marginTop: 2));
            grid.AddGridComponent(1, 1, new Panel(_createLoanForm, LayoutBorder.Rounded, "Take new Loan", 55, 10, ColourFG.Red))
                .SetAlign(Align.Middle)
                .SetJustify(Justify.Center);

            return new Panel(grid, LayoutBorder.Heavy);
        }

        private void CreateLoanHandler(string stringAmount)
        {
            var loanManager = new LoanManager();
            bool canConvert = decimal.TryParse(stringAmount, out decimal loanAmount);
            string label = "";
            try
            {
                loanManager.DisburseLoan(CurrentUser.UserId, loanAmount, Currency.SEK, label);
                PageManager.SwitchPage(PageType.LoanConfirmPage);
            }

            catch (Exception)
            {
                _createLoanForm?.UpdateErrorMessage("Loan exceeds limit");
            }
        }
    }
}
