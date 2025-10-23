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

                sender.Withdraw(amount);
                receiver.Deposit(amount);
                transaction.Status = TransferStatus.Completed;
                sender.AddTransaction(transaction);
                receiver.AddTransaction(transaction);
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
