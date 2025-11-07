using bank_app.Managers;
using bank_app.Models.Accounts;
using bank_app.Utility;
using bank_app.Utility.Components;
using bank_app.Utility.UI;
using bank_app.Utility.UI.Components;
using Figgle.Fonts;

namespace bank_app.UI.Pages
{
    public class TransferPage : Page
    {
        Form? form { get; set; }
        Text? balanceText { get; set; }

        internal override UIComponent LoadPage()
        {
            Console.CursorVisible = false;

            // Gets all checking accounts and saving accounts, then returns them as a DropdownItem with account id as name
            var accounts = AccountManager.GetAllAccounts(CurrentUser.UserId).Where(account => account is CheckingAccount || account is SavingsAccount);
            var accountsNavbarItems = accounts.Select(account => new DropdownItem<Account>(account.AccountID.ToString(), account)).ToList();
            var transferInvokable = new Invokable<Account, string, string>(TransferMoney);

            var accountDropdown = new Dropdown<Models.Accounts.Account>(accountsNavbarItems);
            accountDropdown.OnSelectionChanged = UpdateBalanceDisplay;

            var navbar = new Navbar(
            [
                new NavbarItem("Home", PageType.ClientDashboard),
                new NavbarItem("Transfer", PageType.TransferPage),
                new NavbarItem("Transaction", PageType.TransactionPage),
                new NavbarItem("Account", PageType.CreateAccountPage),
                new NavbarItem("Loan", PageType.LoanPage),
                new NavbarItem("Change Info", PageType.ChangeUserInfoPage),
                new NavbarItem("Logout", PageType.LogOut)
            ]);

            form = new Form(
                [
                    accountDropdown,
                    new InputField("Account ID", InputFieldType.Normal, 35),
                    new InputField("Amount", InputFieldType.Number, 24)
                ], new Button(transferInvokable, "Send", Justify.Center, marginTop: 1));

            balanceText = new Text($"Available balance: {accountsNavbarItems[0].DropDownItem.Balance:C}", marginBottom: 1);

            Flexbox transferGrid = new Flexbox(Justify.Center, Align.Top)
            .AddFlexComponent(balanceText)
            .AddFlexComponent(form);
            transferGrid.Width = 40;
            transferGrid.Height = 10;
            Grid grid = new(3, 3);
            grid.AddGridComponent(0, 1, navbar)
                .SetJustify(Justify.Center)
                .SetAlign(Align.Top);
            grid.AddGridComponent(0, 1, new AsciiArt(AsciiType.String, FiggleFonts.Small.Render("Transfer"), ColourFG.Green));
            grid.AddGridComponent(1, 1, new Panel(transferGrid, LayoutBorder.Rounded, width: 60, height: 10, borderColour: ColourFG.Green))
                .SetAlign(Align.Middle)
                .SetJustify(Justify.Center);
            return new Panel(grid, LayoutBorder.Heavy);
        }
        private void TransferMoney(Account senderAccount, string id, string amount)
        {
            if (Guid.TryParse(id, out Guid guid))
            {
                Account receiverAccount = AccountManager.GetAccountById(guid);
                if (receiverAccount != null)
                {
                    if (decimal.TryParse(amount, out decimal amountDecimal))
                    {
                        TransactionManager.CreateNewTransaction(senderAccount.AccountID, receiverAccount.AccountID, amountDecimal);
                        PageManager.SwitchPage(PageType.ClientDashboard);
                    }
                    else
                    {
                        form?.UpdateErrorMessage("Invalid amount");
                    }
                }
                else
                {
                    form?.UpdateErrorMessage("Account doesn't exist");
                }
            }
            else
            {
                form?.UpdateErrorMessage("Invalid Account ID");
            }
        }
        private void UpdateBalanceDisplay(Account account)
        {
            if (balanceText != null)
            {
                balanceText.UpdateText($"Available balance: {account.Balance} {account.AccountCurrency}");
            }
        }
    }
}
