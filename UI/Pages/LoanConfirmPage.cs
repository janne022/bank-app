using bank_app.Managers;
using bank_app.Models.Accounts;
using bank_app.Utility;
using bank_app.Utility.Components;
using bank_app.Utility.UI;
using bank_app.Utility.UI.Components;
using bank_app.Utility.UI.Invokables;
using Figgle.Fonts;

namespace bank_app.UI.Pages
{
    internal class LoanConfirmPage : Page
    {
        internal override UIComponent LoadPage()
        {
            Console.CursorVisible = false;
            string currenctUserId = PageManager.GetCurrentUser()!.UserId;
            
            var latestUserLoan = LoanManager.GetLatestLoan(currenctUserId);
            decimal latestLoanAmount = latestUserLoan.Principal;
            decimal monthlyInterestRate = latestUserLoan.AnnualRate;
            decimal monthlyPayment = (latestLoanAmount * (monthlyInterestRate/100)) / 12;

            var navbar = new Navbar(
                [
                    new("Continue", PageType.ClientDashboard),
                ]);

            var continueInvoke = new Invokable(NavigationHandler);

            Grid grid = new(3, 3);
            grid.AddGridComponent(0, 1, navbar)
                .SetJustify(Justify.Center)
                .SetAlign(Align.Top);
            grid.AddGridComponent(0, 1, new AsciiArt(AsciiType.String, FiggleFonts.Small.Render("Loan Confirmed")));

            grid.AddGridComponent(1, 1, new Text($"Your interest is: {monthlyInterestRate}%"));
            grid.AddGridComponent(1, 1, new Text($"Your monthly payment is: {monthlyPayment:F2}"));

            return new Panel(grid, LayoutBorder.Heavy);
        }

        private void NavigationHandler()
        {
            PageManager.SwitchPage(PageType.ClientDashboard);
        }
    }
}
