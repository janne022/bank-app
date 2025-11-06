using bank_app.Models;
using bank_app.Utility;

namespace bank_app.Managers
{
    public static class TransactionManager
    {
        private static List<Transaction> allTransactions = new List<Transaction>();

        /// <summary>
        /// Creates a new transaction object, marks it as PENDING and adds the transaction object
        /// to the transaction list containing all bank transactions. No funds will get transfered
        /// in this method, and pending transactions need to be processed.
        /// </summary>
        public static Transaction CreateNewTransaction(Guid senderId, Guid receiverId, decimal amount)
        {
            var transaction = new Transaction(senderId, receiverId, amount)
            {
                Status = TransferStatus.Pending
            };

            try
            {
                allTransactions.Add(transaction);
            }

            catch (Exception)
            {
                transaction.Status = TransferStatus.Failed;
                throw;
            }
            return transaction;
        }

        /// <summary>
        /// Method that performs the actual transaction, credits the sender account and debits the receiver account. 
        /// This method should be performed every 15 minutes using a timer. 
        /// </summary>
        public static void ProcessPendingTransactions()
        {
            //Finds all transaction that are still "pending"
            var transactionsToProcess = allTransactions
                .Where(t => t.Status == TransferStatus.Pending).ToList();

            foreach (var transaction in transactionsToProcess)
            {
                try
                {
                    var senderAccount = AccountManager.GetAccountById(transaction.SenderId);
                    var receiverAccount = AccountManager.GetAccountById(transaction.ReceiverId);
                    decimal transactionAmount = transaction.TransferAmount;

                    if (senderAccount.Balance < transaction.TransferAmount)
                    {
                        transaction.Status = TransferStatus.Failed;
                        continue;
                    }

                    AccountManager.Withdraw(transaction.SenderId, transaction, transactionAmount);

                    if (senderAccount.AccountCurrency != Currency.SEK)
                    {
                        transactionAmount = CurrencyExchange.ExchangeToSek(transactionAmount, senderAccount.AccountCurrency);
                    }

                    if (receiverAccount.AccountCurrency != Currency.SEK)
                    {
                        transactionAmount = CurrencyExchange.ExchangeFromSek(transactionAmount, receiverAccount.AccountCurrency);
                    }

                    AccountManager.Deposit(transaction.ReceiverId, transaction, transactionAmount);
                    transaction.Status = TransferStatus.Completed;
                }

                catch (Exception)
                {
                    transaction.Status = TransferStatus.Failed;
                }
            }
        }

        public static IReadOnlyList<Transaction> GetAllTransactions()
        {
            return allTransactions.OrderByDescending(t => t.TimeStamp).ToList();
        }

        public static IReadOnlyList<Transaction> GetAllTransactions(string userId)
        {
            var accountList = AccountManager.GetAllAccounts(userId);
            var accountIds = accountList.Select(a => a.AccountID).ToList();

            var userTransactions = allTransactions
                .Where(t => accountIds.Contains(t.SenderId) || accountIds.Contains(t.ReceiverId))
                .OrderByDescending(t => t.TimeStamp)
                .ToList();

            return userTransactions;
        }
    }
}
