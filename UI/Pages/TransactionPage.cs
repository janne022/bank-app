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
using bank_app.Managers;

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
                new NavbarItem("Loan", PageType.LoanPage),
                new NavbarItem("Logout", PageType.LogOut)
                ]);
            Grid grid = new(3, 3);
            grid.AddGridComponent(0, 1, navbar)
                .SetJustify(Justify.Center)
                .SetAlign(Align.Top);

            grid.AddGridComponent(0, 1, new AsciiArt(AsciiType.String, FiggleFonts.Small.Render("Transactions"), ColourFG.Green));

            var _currentUser = PageManager.GetCurrentUser();

            if (_currentUser != null)
            {
                var allTransactions = new List<bank_app.Models.Transaction>();

                allTransactions.AddRange(bank_app.Managers.TransactionManager.GetAllTransactions(_currentUser.UserId));

                if (allTransactions.Count == 0)
                {
                    var errorText = new Text("There are no transactions to be displayed yet.", TextAlign.Center);
                    grid.AddGridComponent(1, 1, errorText)
                    .SetJustify(Justify.Center)
                    .SetAlign(Align.Middle);
                    return new Panel(grid, LayoutBorder.Heavy);
                }

                string[] _transactionIDs = new string[allTransactions.Count];
                string[] _senderIDs = new string[allTransactions.Count];
                string[] _receiverIDs = new string[allTransactions.Count];
                string[] _transferAmounts = new string[allTransactions.Count];
                string[] _statuses = new string[allTransactions.Count];
                string[] _timeStamps = new string[allTransactions.Count];
                string[] _senderLegalNames = new string[allTransactions.Count];
                string[] _receiverLegalNames = new string[allTransactions.Count];

                for (int i = 0; i < allTransactions.Count; i++)
                {
                    _senderIDs[i] = allTransactions[i].SenderId.ToString().Substring(0, 8);
                    _receiverIDs[i] = allTransactions[i].ReceiverId.ToString().Substring(0, 8);
                    var senderAccount = AccountManager.GetAccountById(allTransactions[i].SenderId);
                    string senderCurrency = senderAccount.AccountCurrency.ToString();
                    _transferAmounts[i] = allTransactions[i].TransferAmount.ToString() + " " + senderCurrency;
                    _statuses[i] = allTransactions[i].Status.ToString();
                    _timeStamps[i] = allTransactions[i].TimeStamp.ToString();
                    _senderLegalNames[i] = UserManager.GetUser(AccountManager.GetAccountById(allTransactions[i].SenderId).OwnerId).LegalName;
                    _receiverLegalNames[i] = UserManager.GetUser(AccountManager.GetAccountById(allTransactions[i].ReceiverId).OwnerId).LegalName;
                }

                FeedColumn _senderIDColumn = new FeedColumn(_senderIDs, "Sender", true, true, true, textAlign: TextAlign.Center);
                FeedColumn _receiverIDColumn = new FeedColumn(_receiverIDs, "Receiver", true, true, true, textAlign: TextAlign.Center);
                FeedColumn _transferAmountIDColumn = new FeedColumn(_transferAmounts, "Amount", true, true, true, textAlign: TextAlign.Center);
                FeedColumn _statusColumn = new FeedColumn(_statuses, "Status", true, true, true, textAlign: TextAlign.Center);
                FeedColumn _timeStampColumn = new FeedColumn(_timeStamps, "Time", true, true, true, textAlign: TextAlign.Center);
                FeedColumn _senderLegalColumn = new FeedColumn(_senderLegalNames, "Sender", true, true, true, textAlign: TextAlign.Center);
                FeedColumn _receiverLegalColumn = new FeedColumn(_receiverLegalNames, "Receiver", true, true, true, textAlign: TextAlign.Center);

                var transactionFeed = new Feed(12, true, true, marginTop: -10);

                grid.AddGridComponent(1, 1, transactionFeed)
                    .SetJustify(Justify.Center)
                    .SetAlign(Align.Middle);

                transactionFeed.AddColumn(_senderLegalColumn);
                transactionFeed.AddColumn(_receiverLegalColumn);
                transactionFeed.AddColumn(_transferAmountIDColumn);
                transactionFeed.AddColumn(_statusColumn);
                transactionFeed.AddColumn(_timeStampColumn);

            }
            else
            {
                var errorText = new Text("Technical error when finding your data. Please try again later.", TextAlign.Center);
                grid.AddGridComponent(1, 1, errorText)
                .SetJustify(Justify.Center)
                .SetAlign(Align.Middle);
                return new Panel(grid, LayoutBorder.Heavy);
            }
                return new Panel(grid, LayoutBorder.Heavy);
        }
    }
}
