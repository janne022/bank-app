using bank_app.Models;
using bank_app.Models.Accounts;
using bank_app.Models.Users;
using bank_app.Utility;

namespace bank_app.Managers
{
    public static class AccountManager
    {
        // Use ConcurrentDictionary for thread-safety and better concurrency behavior.
        private static Dictionary<Guid, Account> _accounts = new Dictionary<Guid, Account>();

        public static decimal SumOfAmountsInSek(User user)
        {
            decimal totalSum = 0m;

            foreach (var account in GetAllAccounts(user))
            {
                if (account.AccountCurrency != Currency.SEK)
                {
                    totalSum += CurrencyExchange.ExchangeToSek(account.Balance, account.AccountCurrency);
                }
                else
                {
                    totalSum += account.Balance;
                }
            }
            return totalSum;
        }

        /// <summary>
        /// Creates an account of the specified type, registers it internally, and returns it.
        /// Throws ArgumentException for invalid inputs.
        /// </summary>
        public static bool CreateAccount(string ownerID, Currency currency, decimal balance, AccountType accountType)
        {
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
            AddAccount(newAccount);

            if (newAccount == null)
            {

                return false;
            }

            return true;
        }

        /// <summary>
        /// Adds account to account Dictionary. Returns true if added successfully. 
        /// </summary>
        internal static bool AddAccount(Account accountToAdd)
        {
            return _accounts.TryAdd(accountToAdd.AccountID, accountToAdd);
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
        public static List<Account> GetAllAccounts(string ownerId)
        {
            return _accounts.Values.Where(account => account.OwnerId == ownerId).ToList();
        }

        /// <summary>
        /// Overload method to only return accounts of a specific user.
        /// </summary>
        public static List<Account> GetAllAccounts(User user)
        {
            List<Account> specificUserAccounts = new List<Account>();

            foreach (var account in _accounts.Where(a => a.Value.OwnerId == user.UserId))
            {
                specificUserAccounts.Add(account.Value);
            }
            return specificUserAccounts;
        }

        /// <summary>
        /// Returns the number of accounts currently tracked.
        /// </summary>
        public static int GetAccountsCount()
        {
            return _accounts.Count;
        }

        /// <summary>
        /// Prints all transactions in a chosen accound, found by the account id. 
        /// </summary>
        public static IReadOnlyList<Transaction> PrintAllAccountTransactions(Guid accountId)
        {
            var account = GetAccountById(accountId);
            return account.Transactions;
        }

        /// <summary>
        /// Returns account object from account list based on the unique account id (Guid)
        /// </summary>
        public static Account GetAccountById(Guid accountId)
        {
            if (!_accounts.TryGetValue(accountId, out var account))
            {
                return null;
            }
            return account;
        }

        /// <summary>
        /// Adds balance to an account using data from a transaction object. 
        /// </summary>
        public static void Deposit(Guid accountId, Transaction transaction, decimal amount)
        {
            transaction.TransactionType = TransactionType.Deposit;
            var account = GetAccountById(accountId);
            account.ApplyTransaction(transaction, amount);
        }

        /// <summary>
        /// Deducts balance from account using unique identifier and data from transaction.
        /// </summary>
        public static void Withdraw(Guid accountId, Transaction transaction, decimal amount)
        {
            transaction.TransactionType = TransactionType.Withdrawal;
            var account = GetAccountById(accountId);
            account.ApplyTransaction(transaction, amount);
        }
    }
}
