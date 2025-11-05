using bank_app.Managers;
using bank_app.Models.Accounts;
using bank_app.Models.Users;
using bank_app.Utility;
using bank_app.Utility.Components;
using bank_app.Utility.UI;
using bank_app.Utility.UI.Components;
using bank_app.Utility.UI.Invokables;
using Figgle.Fonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.UI.Pages
{
    public class Transfer : Page
    {
        Form? form { get; set; }
        internal override UIComponent LoadPage()
        {
            Console.CursorVisible = false;

            // Gets all checking accounts and saving accounts, then returns them as a DropdownItem with account id as name
            var accounts = AccountManager.GetAllAccounts(CurrentUser.UserId).Where(account => account is CheckingAccount || account is SavingsAccount);
            var accountsNavbarItems = accounts.Select(account => new DropdownItem<Models.Accounts.Account>(account.AccountID.ToString(), account)).ToList();
            var transferInvokable = new Invokable<Models.Accounts.Account, string, string>(TransferMoney);
            var navbar = new Navbar(
                [
                new NavbarItem("Home", PageType.ClientDashboard),
                new NavbarItem("Transfer", PageType.Transfer),
                new NavbarItem("Transaction", PageType.Transaction),
                new NavbarItem("Account", PageType.Account),
                new NavbarItem("Loan", PageType.Loan)
                ]);

            form = new Form(
                [
                new Dropdown<Models.Accounts.Account>(accountsNavbarItems),
                new InputField("Account ID", InputFieldType.Normal, 35, 0),
                new InputField("Amount", InputFieldType.Number, 24, 0)
                ], new Button(transferInvokable, "Send"));

            Flexbox transferGrid = new Flexbox(Justify.Center, Align.Top)
                .AddFlexComponent(new Text($"Avaliable balance: "))
                .AddFlexComponent(form);
            transferGrid.Width = 40;
            transferGrid.Height = 10;
            Grid grid = new(3, 3);
            grid.AddGridComponent(0, 1, navbar)
                .SetJustify(Justify.Center)
                .SetAlign(Align.Top);
            grid.AddGridComponent(0, 1, new AsciiArt(AsciiType.String, FiggleFonts.Small.Render("Transfer")));
            grid.AddGridComponent(1, 1, new Panel(transferGrid, LayoutBorder.Rounded, width: 70, height: 15))
                .SetAlign(Align.Middle)
                .SetJustify(Justify.Center);
            return new Panel(grid, LayoutBorder.Heavy);
        }

        private void TransferMoney(Models.Accounts.Account account, string id, string amount)
        {
            if (Guid.TryParse(id, out Guid guid))
            {
                Models.Accounts.Account sendAccount = AccountManager.GetAccountById(guid);
                if (sendAccount != null)
                {
                    if (Decimal.TryParse(amount, out decimal amountDouble))
                    {
                        TransactionManager.CreateNewTransaction(account.AccountID, sendAccount.AccountID, amountDouble);
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
    }
}
