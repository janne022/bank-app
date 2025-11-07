using bank_app.Managers;
using bank_app.Models.Accounts;
using bank_app.Utility;
using bank_app.Utility.Components;
using bank_app.Utility.UI;
using bank_app.Utility.UI.Components;
using Figgle.Fonts;

namespace bank_app.UI.Pages
{
    internal class TransactionLogPage : Page
    {
        internal override UIComponent LoadPage()
        {
            Console.CursorVisible = false;
            var navbar = new Navbar(
                [
                new NavbarItem("Home", PageType.AdminDashboard),
                new NavbarItem("User", PageType.CreateUserPage),
                new NavbarItem("Transactions", PageType.TransactionLogPage),
                new NavbarItem("Rates", PageType.UpdateRatePage),
                new NavbarItem("Logout", PageType.LogOut)
                ]);
            Grid grid = new(3, 3);
            grid.AddGridComponent(0, 1, navbar)
                .SetAlign(Align.Top)
                .SetJustify(Justify.Center);

            grid.AddGridComponent(0, 1, new AsciiArt(AsciiType.String, FiggleFonts.Small.Render("All Transactions"), ColourFG.Red))
                .SetAlign(Align.Top)
                .SetJustify(Justify.Center);


            var allTransactions = TransactionManager.GetAllTransactions();

            string[] _transactionIDs = new string[allTransactions.Count];
            string[] _senderIDs = new string[allTransactions.Count];
            string[] _receiverIDs = new string[allTransactions.Count];
            string[] _transferAmounts = new string[allTransactions.Count];
            string[] _transactionTypes = new string[allTransactions.Count];
            string[] _timeStamps = new string[allTransactions.Count];

            for (int i = 0; i < allTransactions.Count; i++)
            {
                _transactionIDs[i] = allTransactions[i].TransactionId.ToString().Substring(0, 10);
                _senderIDs[i] = allTransactions[i].SenderId.ToString().Substring(0, 8);
                _receiverIDs[i] = allTransactions[i].ReceiverId.ToString().Substring(0, 8);
                var senderAccount = AccountManager.GetAccountById(allTransactions[i].SenderId);
                string senderCurrency = senderAccount.AccountCurrency.ToString();
                _transferAmounts[i] = allTransactions[i].TransferAmount.ToString() + " " + senderCurrency;
                _timeStamps[i] = allTransactions[i].TimeStamp.ToString();
            }
            
            FeedColumn _transactionIDColumn = new FeedColumn(_transactionIDs, "Transaction", true, true, true, textAlign: TextAlign.Center);
            FeedColumn _senderIDColumn = new FeedColumn(_senderIDs, "Sender", true, true, true, textAlign: TextAlign.Center);
            FeedColumn _receiverIDColumn = new FeedColumn(_receiverIDs, "Receiver", true, true, true, textAlign: TextAlign.Center);
            FeedColumn _transferAmountIDColumn = new FeedColumn(_transferAmounts, "Amount", true, true, true, textAlign: TextAlign.Center);
            FeedColumn _timeStampColumn = new FeedColumn(_timeStamps, "Time", true, true, true, textAlign: TextAlign.Center);

            var transactionFeed = new Feed(12, true, true, marginTop: -8);

            grid.AddGridComponent(1, 1, transactionFeed)
                .SetJustify(Justify.Center)
                .SetAlign(Align.Middle);

            transactionFeed.AddColumn(_transactionIDColumn);
            transactionFeed.AddColumn(_senderIDColumn);
            transactionFeed.AddColumn(_receiverIDColumn);
            transactionFeed.AddColumn(_transferAmountIDColumn);
            transactionFeed.AddColumn(_timeStampColumn);






            return new Panel(grid, LayoutBorder.Heavy);
        }
    }
}
