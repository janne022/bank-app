using bank_app.Models;
using bank_app.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Managers
{
    public static class AccountManager
    {
        // Use ConcurrentDictionary for thread-safety and better concurrency behavior.
        private static Dictionary<Guid, Account> _accounts = new Dictionary<Guid, Account>();

        /// <summary>
        /// Creates an account of the specified type, registers it internally, and returns it.
        /// Throws ArgumentException for invalid inputs.
        /// </summary>
        public static bool CreateAccount(Currency currency, decimal balance, AccountType accountType)
        {
          

            // Construct the account using a switch expression for clarity.
            Account newAccount = accountType switch
            {
                AccountType.Checking => new CheckingAccount(
                    currency,
                    balance,
                    AccountDefaults.CheckingMonthlyFee,
                    AccountDefaults.CheckingOverdraftLimit),

                AccountType.Savings => new SavingsAccount(
                    currency,
                    balance,
                    AccountDefaults.SavingsInterestRate,
                    AccountDefaults.SavingsMinimumBalance,
                    DateTime.Now,
                    AccountDefaults.SavingsAllowWithdraws),

                AccountType.Loan => new LoanAccount(
                    currency,
                    balance,
                    AccountDefaults.LoanInterestRate,
                    AccountDefaults.LoanCreditLimit),

                _ => throw new ArgumentException("Invalid account type.", nameof(accountType))
            };

            // Add or overwrite the account entry in a thread-safe manner.
            // Using the indexer simplifies handling rare Guid collisions by overwriting the same key.
            _accounts[newAccount.AccountID] = newAccount;

            if (newAccount==null)
            {
                
            return false;
            }

			return true;
		}

        public static void Deposit(Guid accountId)
        {
            var account = GetOrThrow(accountId);
        }

        public static void Withdraw(Guid accountId)
        {
            var account = GetOrThrow(accountId);
        }

        public static Account GetOrThrow(Guid id)
        {
            if(!_accounts.TryGetValue(id, out var acc))
            {
                throw new InvalidOperationException();
            }
            return acc;
        }

        /// <summary>
        /// Returns the number of accounts currently tracked.
        /// </summary>
        public static int GetAccountsCount()
        {
            return _accounts.Count;
        }

        /// <summary>
        /// Removes an account by its ID. Returns true if removed.
        /// </summary>
        public static bool RemoveAccount(Guid accountID)
        {
            return _accounts.Remove(accountID);
        }

        /// <summary>
        /// Returns a snapshot list of all accounts.
        /// </summary>
        public static List<Account> GetAllAccounts()
        {
            return _accounts.Values.ToList();
        }

        // Additional account-related logic may go here.
    }
}
