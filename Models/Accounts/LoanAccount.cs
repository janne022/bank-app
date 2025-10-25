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

        public decimal InterestRate { get; set; } //% per annum
        public bool IsDisbusrsed { get; set; }

        public Status Status { get; set; } = Status.Active;
        public DateOnly LastInterestAppliedPeriod { get; set; }

        private List<Loan> loans = new List<Loan>();
        public IReadOnlyList<Loan> Loans => loans;
        public LoanAccount(Currency currency, decimal balance, decimal interestRate)
            : base(currency, balance)
        {
            InterestRate = interestRate < 0 ? 0 : interestRate;

        }

       



    }
}
