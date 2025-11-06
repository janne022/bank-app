using bank_app.Managers;
using bank_app.Utility;
using bank_app.Utility.Components;
using bank_app.Utility.UI;
using bank_app.Utility.UI.Components;
using Figgle.Fonts;

namespace bank_app.UI.Pages
{
    public class ClientDashboard : Page
    {
        internal override UIComponent LoadPage()
        {
            Console.CursorVisible = false;

            // -------------------------------------- NAVBAR AND MESSAGE SECTION ----------------------------- //
            var navbar = new Navbar(
                [
                new NavbarItem("Home", PageType.ClientDashboard),
                new NavbarItem("Transfer", PageType.TransferPage),
                new NavbarItem("Transaction", PageType.TransactionPage),
                new NavbarItem("Account", PageType.CreateAccountPage),
                new NavbarItem("Loan", PageType.LoanPage),
                new NavbarItem("Logout", PageType.LogOut)
                ]);
            // Create a new Invokable object with the method that is going to run once the 'Login' button is pressed
            // Create new 3x3 grid
            Grid grid = new(3, 3);
            grid.AddGridComponent(0, 1, navbar)
                .SetJustify(Justify.Center)
                .SetAlign(Align.Top);
            grid.AddGridComponent(0, 1, new AsciiArt(AsciiType.String, FiggleFonts.Small.Render($"Welcome back, {CurrentUser?.LegalName}")));


            // --------------------------------------- AVAILABLE AMOUNT SECTION ------------------------------ //
            string textToDisplay = $"Available Amount in SEK: <{AccountManager.SumOfAmountsInSek(CurrentUser.UserId).ToString("F2")}>";

            var availableAmount = new Text(textToDisplay);

            // --------------------------------------- ACCOUNT TABLE SECTION -------------------------------- //

            // creates a table of account information
            var accountFeed = new Feed(4, false, false);
            string[] column1 = new string[AccountManager.GetAllAccounts(CurrentUser.UserId).Count];
            string[] column2 = new string[AccountManager.GetAllAccounts(CurrentUser.UserId).Count];

            // loops as many times as there are accounts...
            for (int i = 0; i < AccountManager.GetAllAccounts(CurrentUser.UserId).Count; i++)
            {
                // adds info about the accounts first in first column, then second...
                column1[i] = $"Account {i + 1} <{AccountManager.GetAllAccounts(CurrentUser.UserId)[i].GetType().Name.Substring(0, AccountManager.GetAllAccounts(CurrentUser.UserId)[i].GetType().Name.Length - 7)}>: ";

                if (AccountManager.GetAllAccounts(CurrentUser.UserId)[i].AccountCurrency != Currency.SLC)
                {
                    column2[i] = $"{AccountManager.GetAllAccounts(CurrentUser.UserId)[i].Balance.ToString("F2")} {AccountManager.GetAllAccounts(CurrentUser.UserId)[i].AccountCurrency}";
                }
                else
                {
                    column2[i] = $"{AccountManager.GetAllAccounts(CurrentUser.UserId)[i].Balance} {AccountManager.GetAllAccounts(CurrentUser.UserId)[i].AccountCurrency}";
                }

            }

            // ... then adds those columns into a left and right column...
            FeedColumn leftColumn = new FeedColumn(column1, "Vira was here >:3", false, false);
            FeedColumn rightColumn = new FeedColumn(column2, "Vira was also here, hehe c:", false, false);

            //...and finally adds the columns into the table
            accountFeed.AddColumn(leftColumn);
            accountFeed.AddColumn(rightColumn);

            var flexbox = new Flexbox(Justify.Center, Align.Top).AddFlexComponent(availableAmount).AddFlexComponent(accountFeed);

            // adds border around the feed
            grid.AddGridComponent(1, 1, new Panel(flexbox, LayoutBorder.Rounded, "", 55, 8, ColourFG.GreenBright)).SetAlign(Align.Middle).SetJustify(Justify.Center);


            // Add grid to layout and set rounded border style
            return new Panel(grid, LayoutBorder.Heavy);
        }
    }
}
