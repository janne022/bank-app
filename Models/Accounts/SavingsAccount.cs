using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Models.Accounts
{
    public class SavingsAccount : Account
    {
        

        public SavingsAccount(Currency currency, decimal balance)
            : base(currency, balance)
        {
        }
    }
}
