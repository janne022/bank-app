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
        private static Dictionary<string, Account> _accounts = new Dictionary<string, Account>();

        public static void CreateAccount(Account account)
        {
            if (account != null)
            {
                bool added = _accounts.TryAdd(account.Id, account);
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

        public static bool RemoveAccount(string id)
        {

            return _accounts.Remove(id);
        }


        public static void GetAllAccounts()
        {
            foreach (var account in _accounts.Values)
            {
                Console.WriteLine($"Account ID: {account.Id}, Balance: {account.Balance} {account.AccountCurrency}");
            }
        }
    }
}
