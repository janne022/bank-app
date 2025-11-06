using bank_app.Managers;
using bank_app.Models.Users;
using bank_app.Utility;
using bank_app.Utility.Components;
using bank_app.Utility.UI;
using bank_app.Utility.UI.Components;
using Figgle.Fonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using bank_app.UI.Pages;

namespace bank_app.UI.Pages
{
    public class TransactionPage : Page
    {
        internal override UIComponent LoadPage()
        {
            Console.CursorVisible = false;
            var navbar = new Navbar(
                [
                new NavbarItem("Home", PageType.ClientDashboard),
                new NavbarItem("Transfer", PageType.TransferPage),
                new NavbarItem("Transaction", PageType.TransactionPage),
                new NavbarItem("Account", PageType.CreateAccountPage),
                new NavbarItem("Loan", PageType.LoanPage)
                ]);
            Grid grid = new(3, 3);
            grid.AddGridComponent(0, 1, navbar)
                .SetJustify(Justify.Center)
                .SetAlign(Align.Top);

            grid.AddGridComponent(0, 1, new AsciiArt(AsciiType.String, FiggleFonts.Small.Render("Transactions")));

            var _currentUser = PageManager.GetCurrentUser();


            if (_currentUser != null)
            {
                var allTransactions = new List<bank_app.Models.Transaction>();

                foreach (var account in AccountManager.GetAllAccounts(_currentUser.UserId))
                {
                    allTransactions.AddRange(AccountManager.PrintAllAccountTransactions(account.AccountID));
                }

                string[] _transactionIDs = new string[allTransactions.Count];
                string[] _senderIDs = new string[allTransactions.Count];
                string[] _receiverIDs = new string[allTransactions.Count];
                string[] _transferAmounts = new string[allTransactions.Count];
                string[] _statuses = new string[allTransactions.Count];
                string[] _transactionTypes = new string[allTransactions.Count];
                string[] _timeStamps = new string[allTransactions.Count];

                for (int i = 0; i < allTransactions.Count; i++)
                {
                    _transactionIDs[i] = allTransactions[i].TransactionId.ToString().Substring(0, 10);
                    _senderIDs[i] = allTransactions[i].SenderId.ToString().Substring(0, 8);
                    _receiverIDs[i] = allTransactions[i].ReceiverId.ToString().Substring(0, 8);
                    _transferAmounts[i] = allTransactions[i].TransferAmount.ToString();
                    _statuses[i] = allTransactions[i].Status.ToString();
                    _transactionTypes[i] = allTransactions[i].TransactionType.ToString();
                    _timeStamps[i] = allTransactions[i].TimeStamp.ToString();
                }

                FeedColumn _transactionIDColumn = new FeedColumn(_transactionIDs, "Transaction", true, true, true);
                FeedColumn _senderIDColumn = new FeedColumn(_senderIDs, "Sender", true, true, true);
                FeedColumn _receiverIDColumn = new FeedColumn(_receiverIDs, "Receiver", true, true, true);
                FeedColumn _transferAmountIDColumn = new FeedColumn(_transferAmounts, "Amount", true, true, true);
                FeedColumn _statusColumn = new FeedColumn(_statuses, "Status", true, true, true);
                FeedColumn _transactionTypeColumn = new FeedColumn(_transactionTypes, "Type", true, true, true);
                FeedColumn _timeStampColumn = new FeedColumn(_timeStamps, "Time", true, true, true);

                var transactionFeed = new Feed(12, true, true, marginTop: -18);

                grid.AddGridComponent(1, 1, transactionFeed)
                    .SetJustify(Justify.Center)
                    .SetAlign(Align.Middle);

                transactionFeed.AddColumn(_transactionIDColumn);
                transactionFeed.AddColumn(_senderIDColumn);
                transactionFeed.AddColumn(_receiverIDColumn);
                transactionFeed.AddColumn(_transferAmountIDColumn);
                transactionFeed.AddColumn(_statusColumn);
                transactionFeed.AddColumn(_transactionTypeColumn);
                transactionFeed.AddColumn(_timeStampColumn);

            }

            return new Panel(grid, LayoutBorder.Heavy);
        }
    }
}
