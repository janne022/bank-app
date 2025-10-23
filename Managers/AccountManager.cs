using bank_app.Models;
using bank_app.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Managers
{
    public static class AccountManager
    {
        private static Dictionary<Guid, Account> _accounts = new Dictionary<Guid ,Account>();

        public static void CreateAccount(Account account)
        {
            if (account != null)
            {
                bool added = _accounts.TryAdd(account.AccountID, account);
                if (!added)
                {
                    Console.WriteLine("Account with the same ID already exists.");
                }
                else
                {
                    Console.WriteLine("Account added ");
                }

            }
        }

        public static int GetAccountsCount()
        {
            return _accounts.Count;
        }

        public static bool RemoveAccount(Guid accountID)
        {

            return _accounts.Remove(accountID);
        }


        public static List<Account> GetAllAccounts()
        {
           return _accounts.Values.ToList();
        }
    }
}
