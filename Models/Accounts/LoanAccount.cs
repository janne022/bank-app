using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bank_app.Utility;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace bank_app.Models.Accounts
{
    public class LoanAccount : Account
    {
        
        public decimal CreditLimit { get; private set; }

        public LoanAccount(Currency currency, string ownerId, decimal creditLimit)
            : base(currency, 0, ownerId)
        {
           
          
            CreditLimit = creditLimit < 0 ? throw new ArgumentOutOfRangeException(nameof(creditLimit), "Credit limit cannot be negative.") : creditLimit;

        }

        public override bool CanApply(Transaction transaction)
        {

            if (transaction==null || transaction.TransferAmount<=0)
            {
                return false;
            }

            if (transaction.TransactionType == TransactionType.Deposit)
            {
                return true;
            }

            if (transaction.TransactionType == TransactionType.Withdrawal)
            {
                return Balance - transaction.TransferAmount /*This will be change with transaction.Amount later */ >= -CreditLimit;
            }



            return false;
        }
    }
}
