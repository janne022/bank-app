using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bank_app.Utility;

namespace bank_app.Models.Accounts
{
    public class CheckingAccount : Account
    {

        //Monthly fee is the fee charged every month for maintaining the account, and logic will be implemented in Account Manager
        public decimal MonthlyFee { get; private set; }
        //Overdrat limist is the maximum negative balance allowed 
        public decimal OverdraftLimit { get; private set; }

        public CheckingAccount(Currency currency, decimal balance, string ownerId, decimal overdraftLimit, decimal monthlyFee) : base(currency, balance, ownerId)
        {
            OverdraftLimit = overdraftLimit > 0 ? 0 : overdraftLimit;
            MonthlyFee = monthlyFee < 0 ? 0 : monthlyFee;
        }

     

        //This method is looking to see if the transaction can be applied based on the type of transaction and the current balance and overdraft limit.
        public override bool CanApply(Transaction transaction)
        {

            if (transaction == null || transaction.TransferAmount <= 0)
            {
                return false;
            }

            if (transaction.TransactionType == TransactionType.Deposit)
            {
                return true;
            }

            if (transaction.TransactionType == TransactionType.Withdrawal)
            {
                return Balance - transaction.TransferAmount /*This will be change with transaction.Amount later */ >= -OverdraftLimit;
            }



            return false;
        }


    }
}
