using bank_app.Managers;
using bank_app.Models;
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
         
            var monthlyPayment = LoanManager.CalcculateMonthlyPayment(CurrentUser!.UserId, AccountDefaults.MonthsToPay);

            string displayInterestText = $"Your interest is: {AccountDefaults.LoanInterestRate}%";
            string displayPaymentText = $"Your monthly payment is: {monthlyPayment:F2} SEK";

            var continueInvoke = new Invokable(NavigationHandler);

            Grid grid = new(3, 3);
            grid.AddGridComponent(0, 1, new AsciiArt(AsciiType.String, FiggleFonts.Small.Render("Loan Confirmed"), ColourFG.Green))
                .SetJustify(Justify.Center);
            grid.AddGridComponent(1, 1, new Text(displayInterestText))
                .SetJustify(Justify.Center);
            grid.AddGridComponent(1, 1, new Text(displayPaymentText))
                .SetJustify(Justify.Center);
            grid.AddGridComponent(2, 1, new Button(continueInvoke, "Continue"))
                .SetAlign(Align.Middle);

            return new Panel(grid, LayoutBorder.Heavy);
        }

        private void NavigationHandler()
        {
            PageManager.SwitchPage(PageType.ClientDashboard);
        }
    }
}
