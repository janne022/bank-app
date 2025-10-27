using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bank_app.Models;
using bank_app.Models.Accounts;
using bank_app.Utility;

namespace bank_app.Managers
{
    public class TransactionManager
    {
        static List<Transaction> allTransactions = new List<Transaction>();
        /// <summary>
        /// Performs a transfer of selected balance amount between two Account objects.
        /// Returns the Transaction object for success validation. 
        /// </summary>
        public Transaction Transfer(Account sender, Account receiver, decimal amount)
        {
            var transaction = new Transaction(sender, receiver, amount)
            {
                Status = TransferStatus.Pending
            };

            try
            {
                //Fails transaction if amount is greater than account balance
                if (amount > sender.Balance)
                {
                    transaction.Status = TransferStatus.Failed;
                    return transaction;
                }

                /*Adds the pending transaction to the transactionlist for tracking. Transaction must be
                  completed by ProcessTransaction method */
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
        /// Processes all transactions that have been pending for at least 15 minutes.
        /// Only works together with a timer that calls the method ex. every minute while
        /// application is running. 
        /// </summary>
        public void ProcessPendingTransactions()
        {
            //Finds all transaction that are still "pending" and have waited for >= 15 minutes
            var transactionToProcess = allTransactions
                .Where(t => t.Status == TransferStatus.Pending &&
                    (DateTime.Now - t.TimeStamp).TotalMinutes >= 15).ToList();

            foreach (var transaction in transactionToProcess)
            {
                try
                {
                    //Makes one more check to make sure sender balance has not changed 
                    if (transaction.TransferAmount > transaction.Sender.Balance)
                    {
                        transaction.Status = TransferStatus.Failed;
                        continue;
                    }

                    //Performs the transaction and marks it as completed
                    transaction.Sender.ApplyTransaction(TransactionType.Withdrawal, transaction.TransferAmount, transaction);
                    transaction.Receiver.ApplyTransaction(TransactionType.Deposit, transaction.TransferAmount, transaction);
                    transaction.Status = TransferStatus.Completed;
                }

                catch (Exception)
                {
                    transaction.Status = TransferStatus.Failed;
                }
            }

            //This function will only work if being continously called by a timer (ex. every minute)
       
        }
    }
}
