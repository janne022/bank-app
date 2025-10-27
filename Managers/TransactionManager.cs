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

                //Performs transaction, adds transaction to account transaction
                //list and marks transaction as completed. 
                sender.ApplyTransaction(TransactionType.Withdrawal, amount, transaction);
                receiver.ApplyTransaction(TransactionType.Deposit, amount, transaction);
                transaction.Status = TransferStatus.Completed;
            }

            catch (Exception)
            {
                transaction.Status = TransferStatus.Failed;
                throw;
            }

            allTransactions.Add(transaction);
            return transaction;


            /*For later use in UI when a transaction is performed:
             * - When a transaction is created using transaction manager,
             *   use transaction.Status to check if transfer was completed
             *   or not. 
             *   Example: if (transaction.Status == TransferStatus.Completed)
             *   same applies to check if transfer has failed.
             *   
             *   A Transaction object should only be created with the Transfer method!
             */
        }
    }
}
