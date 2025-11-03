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
            _accounts[ownerID] = newAccount;

            if (newAccount == null)
            {

                return false;
            }

            return true;
        }

        internal static bool AddAccount(Account accountToAdd)
        {
         
           return  _accounts.TryAdd(accountToAdd.AccountID, accountToAdd);
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
        public static List<Account> GetAllAccounts(Guid ownerId)
        {
            return _accounts.Values.Where(account=>account.OwnerId==ownerId).ToList();
        }

        // Overload method to only return accounts of a specific user
        public static List<Account> GetAllAccounts(User user)
        {
            List<Account> specificUserAccounts = new List<Account>();

            // Loops through the account list, and checks the inputted user's ID with the accounts saved userIDs...
            foreach(var account in _accounts.Where(a => a.Value.OwnerId == user.UserId))
            {
                //... And adds those accounts to a list of accounts
                specificUserAccounts.Add(account.Value);
            }

            // finally returns list of that user's accounts
            return specificUserAccounts;
        }


        /// <summary>
        /// Adds balance to account using unique identifier and data from transaction.

        /// <summary>
        /// Returns the number of accounts currently tracked.
        /// </summary>
        public static int GetAccountsCount()
        {
            return _accounts.Count;
        }

        public static IReadOnlyList<Transaction> PrintAllAccountTransactions(Guid id)
        {
            var account = GetAccountById(id);
            return account.Transactions;
        }

        /// <summary>
        /// Returns account object from account list based on the unique account id (Guid)
        /// </summary>
        public static Account GetAccountById(Guid id)
        {
            if (!_accounts.TryGetValue(id, out var account))
            {
                throw new InvalidOperationException();
            }
            return account;
        }

        /// <summary>
        /// Adds balance to an account using data from a transaction object. 
        /// </summary>
        /// <param name="accountId">The unique identifier (Guid) for an Account object</param>
        public static void Deposit(Guid accountId, Transaction transaction, decimal amount)
        {
            transaction.TransactionType = TransactionType.Deposit;
            var account = GetAccountById(accountId);
            account.ApplyTransaction(transaction, amount);
        }

        /// <summary>
        /// Deducts balance from account using unique identifier and data from transaction.
        /// </summary>
        /// /// /// <param name="accountId">The unique identifier (Guid) for an Account object</param>
        public static void Withdraw(Guid accountId, Transaction transaction, decimal amount)
        {
            transaction.TransactionType = TransactionType.Withdrawal;
            var account = GetAccountById(accountId);
            account.ApplyTransaction(transaction, amount);
        }
    }
}
