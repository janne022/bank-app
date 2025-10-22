using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Models.Accounts
{
    public abstract class Account
    {
        public string Id { get; set; }

        public Currency AccountCurrency { get; private set; }

        public decimal Balance { get; private set; }

        private readonly List<Transaction> _transactions = new List<Transaction>();
        public IReadOnlyList<Transaction> Transactions => _transactions;


        protected Account( Currency currency, decimal balance)
        {
                 
            Id = GenerateAccountId();
            AccountCurrency = currency;
            Balance = balance;
            _transactions= new List<Transaction>();
        }


        public static string GenerateAccountId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
