using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Models.Accounts
{
    public class SavingsAccount : Account
    {
        public decimal InterestRate { get; set; }

        public SavingsAccount(Currency currency, decimal balance, decimal interestRate)
            : base(currency, balance)
        {
            InterestRate = interestRate < 0 ? 0 : interestRate;
        }




        public override void Withdraw(decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}
