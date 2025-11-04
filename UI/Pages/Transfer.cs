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
        internal override UIComponent LoadPage()
        {
            Console.CursorVisible = false;
            List<Models.Accounts.Account> accounts = AccountManager.GetAllAccounts(CurrentUser.UserId).Where(account => account is CheckingAccount || account is SavingsAccount).ToList();
            var transferInvokable = new Invokable<string, string>(TransferMoney);
            var navbar = new Navbar(new List<NavbarItem> { new NavbarItem("Home", PageType.ClientDashboard), new NavbarItem("Transfer", PageType.Transfer), new NavbarItem("Transaction", PageType.Transaction), new NavbarItem("Account", PageType.Account), new NavbarItem("Loan", PageType.Loan) });
            // Create a new Invokable object with the method that is going to run once the 'Login' button is pressed
            var transferGrid = new Flexbox(Justify.Center, Align.Top);
            transferGrid.AddFlexComponent(new Text($"Avaliable balance: "));
            transferGrid.AddFlexComponent(new Form(new List<UIComponent> {new InputField("Account ID", false, 24), new InputField("Amount", false, 24) }, new Button(transferInvokable, "Send")));
            transferGrid.Width = 40;
            transferGrid.Height = 10;
            // Create new 3x3 grid
            Grid grid = new Grid(3, 3);
            grid.AddGridComponent(0, 1, navbar);
            grid.AddGridComponent(1,1, new Panel(transferGrid, LayoutBorder.Rounded,width: 40, height: 10));
            // Add grid to layout and set rounded border style
            return new Panel(grid, LayoutBorder.Heavy);
        }

        private void TransferMoney(string id, string amount)
        {

        }
    }
}
