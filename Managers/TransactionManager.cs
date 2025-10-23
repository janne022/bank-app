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
        /// <summary>
        /// Performs a transfer of selected balance amount between two Account objects.
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
                if (!sender.CanWithdraw(amount))
                {
                    transaction.Status = TransferStatus.Failed;
                    return transaction;
                }

                //Performs transaction, adds transaction to account transaction
                //list and marks transaction as completed. 
                sender.Withdraw(amount);
                receiver.Deposit(amount);
                sender.AddTransaction(transaction);
                receiver.AddTransaction(transaction);
                transaction.Status = TransferStatus.Completed;
            }

            catch (Exception)
            {
                transaction.Status = TransferStatus.Failed;
                throw;
            }

            return transaction;
        }
    }
}
