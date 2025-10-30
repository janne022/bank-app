using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace bank_app.Models.Accounts
{
    public class LoanAccount : Account
    {

        public decimal InterestRate { get; private set; } //% per annum

        public decimal CreditLimit { get; private set; }

        //This will be implemented in the User data model
        //private List<Loan> loans = new List<Loan>();
        //public IReadOnlyList<Loan> Loans => loans;

        public LoanAccount(Currency currency, decimal balance, decimal interestRate, decimal creditLimit)
            : base(currency, balance)
        {
            InterestRate = interestRate < 0 ? 0 : interestRate;
            CreditLimit = creditLimit < 0 ? 0 : creditLimit;

        }

        public override bool CanApply(Transaction transaction)
        {

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
