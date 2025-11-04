using bank_app.Managers;
using bank_app.Models.Accounts;
using bank_app.Utility;
using bank_app.Utility.Components;
using bank_app.Utility.UI;
using bank_app.Utility.UI.Components;
using bank_app.Utility.UI.Invokables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.UI.Pages
{
    public class Transfer : Page
    {
        Form? form {  get; set; }
        internal override UIComponent LoadPage()
        {
            Console.CursorVisible = false;

            // Gets all checking accounts and saving accounts, then returns them as a DropdownItem with account id as name
            List<DropdownItem<Models.Accounts.Account>> accounts = AccountManager.GetAllAccounts(CurrentUser.UserId).Where(account => account is CheckingAccount || account is SavingsAccount).Select(account => new DropdownItem<Models.Accounts.Account>(account.AccountID.ToString(), account)).ToList();
            var transferInvokable = new Invokable<Models.Accounts.Account,string, string>(TransferMoney);
            var navbar = new Navbar(new List<NavbarItem> { new NavbarItem("Home", PageType.ClientDashboard), new NavbarItem("Transfer", PageType.Transfer), new NavbarItem("Transaction", PageType.Transaction), new NavbarItem("Account", PageType.Account), new NavbarItem("Loan", PageType.Loan) });
            // Create a new Invokable object with the method that is going to run once the 'Login' button is pressed
            var transferGrid = new Flexbox(Justify.Center, Align.Top);
            transferGrid.AddFlexComponent(new Text($"Avaliable balance: "));
            form = new Form(new List<UIComponent> { new Dropdown<Models.Accounts.Account>(accounts), new InputField("Account ID", false, 35), new InputField("Amount", false, 24) }, new Button(transferInvokable, "Send"));
            transferGrid.AddFlexComponent(form);
            transferGrid.Width = 40;
            transferGrid.Height = 10;
            // Create new 3x3 grid
            Grid grid = new Grid(3, 3);
            grid.AddGridComponent(0, 1, navbar);
            grid.AddGridComponent(1,1, new Panel(transferGrid, LayoutBorder.Rounded,width: 70, height: 15));
            var gridCell = grid.GetGridCell(1,1);
            gridCell.Justify = Justify.Center;
            gridCell.Align = Align.Middle;
            // Add grid to layout and set rounded border style
            return new Panel(grid, LayoutBorder.Heavy);
        }

        private void TransferMoney(Models.Accounts.Account account,string id, string amount)
        {
            bool success = Guid.TryParse(id, out Guid guid);
            bool successAmount = Decimal.TryParse(amount, out decimal amountDouble);
            if (success)
            {
                Models.Accounts.Account sendAccount = AccountManager.GetAccountById(guid);
                if (sendAccount != null)
                {
                    if (successAmount)
                    {
                        TransactionManager.CreateNewTransaction(account.AccountID, sendAccount.AccountID, amountDouble);
                        Console.WriteLine("TRANSCATION WORKED");
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
                form?.UpdateErrorMessage("Invalid account id");
            }
        }
    }
}
