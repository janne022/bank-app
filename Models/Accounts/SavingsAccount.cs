using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Models.Accounts
{
    public class SavingsAccount : Account
    {
        //% per annum
        public decimal InterestRate { get; private set; }
        //Minimum balance required to avoid penalties
        public decimal MinimumBalance { get; private set; }
        //Last date when interest was applied
        public DateTime LastInterestDate { get; private set; }
        //Indicates whether withdrawals are allowed
        public bool AllowWithdrawals { get; private set; } 

        public SavingsAccount(Currency currency, decimal balance, decimal interestRate, decimal minimalBalance,DateTime lastInterestDate, bool allowWithdrawals)
            : base(currency, balance)
        {
            InterestRate = interestRate < 0 ? 0 : interestRate;
            MinimumBalance = minimalBalance < 0 ? 0 : minimalBalance;
            LastInterestDate = lastInterestDate;
            AllowWithdrawals = allowWithdrawals;
        }

        public override bool CanApply(Transaction transaction)
        {
            decimal amount = 0;
            if (transaction.TransactionType == TransactionType.Deposit)
            {
                return true;
            }
            if (transaction.TransactionType == TransactionType.Withdrawal)
            {

                if (!AllowWithdrawals)
                {
                    return false;
                }

                return Balance - amount >= MinimumBalance;
            }



            return false;

        }

        public decimal CalculateInterest(int days)
        {
            if (Balance<=0 || InterestRate<=0)
            {
                return 0;
            }


            decimal annualRate= InterestRate / 100m;
            decimal fractionalRate= days / 365m;
            decimal interest= Balance * annualRate * fractionalRate;
            return interest;
        }

        public void ApplyInterest(int days)
        {
           
            decimal interest = CalculateInterest(days);
            if (interest > 0)
            {
                //var tx= new Transaction(TransactionType.Deposit, interest, currentDate, "Interest Payment");
                //ApplyTransaction(tx);
                LastInterestDate = DateTime.Now;
            }
        }
    }
}
