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
        /// Creates a new transaction object, marks it as PENDING and adds the transaction object
        /// to the transaction list containing all bank transactions. 
        /// </summary>
        public Transaction CreateNewTransaction(Guid senderId, Guid receiverId, decimal amount)
        {
            var transaction = new Transaction(senderId, receiverId, amount)
            {
                Status = TransferStatus.Pending
            };

            try
            {
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
                    //Performs the transaction and marks it as completed
                    AccountManager.Withdraw(transaction.SenderId, transaction);
                    AccountManager.Deposit(transaction.ReceiverId, transaction);
                    transaction.Status = TransferStatus.Completed;
                }

                catch (Exception)
                {
                    transaction.Status = TransferStatus.Failed;
                }
            }
        }
    }
}
