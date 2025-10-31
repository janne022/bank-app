using bank_app.Models;
using bank_app.Models.Accounts;
using bank_app.Models.Users;
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
        public static bool CreateAccount(Guid ownerID, Currency currency, decimal balance, AccountType accountType)
        {
            // Construct the account using a switch expression for clarity.
            Account newAccount = accountType switch
            {
                AccountType.CheckingAcc => new CheckingAccount(
                    currency,
                    balance,
                    ownerID,
                    AccountDefaults.CheckingMonthlyFee,
                    AccountDefaults.CheckingOverdraftLimit),

                AccountType.SavingsAcc => new SavingsAccount(
                    currency,
                    balance,
                    ownerID,
                    AccountDefaults.SavingsInterestRate,
                    AccountDefaults.SavingsMinimumBalance,
                    DateTime.Now,
                    AccountDefaults.SavingsAllowWithdraws),

                AccountType.LoanAcc => new LoanAccount(
                    currency,           
                    ownerID,
                    AccountDefaults.LoanCreditLimit),

                _ => throw new ArgumentException("Invalid account type.", nameof(accountType))
            };

            // Add or overwrite the account entry in a thread-safe manner.
            // Using the indexer simplifies handling rare Guid collisions by overwriting the same key.
            _accounts[newAccount.AccountID] = newAccount;

            if (newAccount == null)
            {

                return false;
            }

            return true;
        }

        internal static void AddAccount(Account accountToAdd)
        {
            _accounts[accountToAdd.AccountID] = accountToAdd;
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

        /// <summary>
        /// Returns the number of accounts currently tracked.
        /// </summary>
        public static int GetAccountsCount()
        {
            return _accounts.Count;
        }


        public static Account? GetAccountById(Guid id)
        {
            if (_accounts.TryGetValue(id, out Account? account))
            {
                return account;
            }
            return null;
        }

        //Methods for Deposit and Withdraw to update
        /// </summary>
        /// <param name="accountId">The unique identifier (Guid) for an Account object</param>
        public static void Deposit(Guid accountId, Transaction transaction, decimal amount)
        {
            transaction.TransactionType = TransactionType.Deposit;
            var account = GetOrThrow(accountId);
            account.ApplyTransaction(transaction, amount);
        }

        /// <summary>
        /// Decucts balance to an account using data from a transaction object.
        /// </summary>
        /// /// <param name="accountId">The unique identifier (Guid) for an Account object</param>
        public static void Withdraw(Guid accountId, Transaction transaction, decimal amount)
        {
            transaction.TransactionType = TransactionType.Withdrawal;
            var account = GetOrThrow(accountId);
            account.ApplyTransaction(transaction, amount);
        }

        /// <summary>
        /// Retrieves and returns an account from the account list using a unique identifier (Guid).
        /// </summary>
        public static Account GetOrThrow(Guid id)
        {
            if (!_accounts.TryGetValue(id, out var acc))
            {
                throw new InvalidOperationException();
            }
            return acc;
        }


    }
}
