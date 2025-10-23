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


        protected Account(Currency currency, decimal balance)
        {

            Id = GenerateAccountId();
            AccountCurrency = currency;

            SetBalance(balance);
            _transactions = new List<Transaction>();
        }
        public void SetBalance(decimal amount)
        {
            if (amount < 0)
            {
                Balance = 0;
            }
            else
            {
                Balance = amount;
            }
        
        }
        public static string GenerateAccountId()
        {
            return Guid.NewGuid().ToString();
        }
        public void AddTransaction(Transaction transaction)
        {
            _transactions.Add(transaction);
        }

        public void UpdateBalance(decimal amount)
        {
            Balance += amount;
        }


        public bool Deposit(decimal amount)
        {

            if (amount > 0)
            {
                Balance += amount;
                return true;
            }
            else
            {

                return false;
            }
        }
        public virtual void Withdraw(decimal amount)
        {
            if (CanWithdraw(amount))
            {
                Balance -= amount;
            }
            else
            {
                Console.WriteLine("Impossible to do thatr");
            }
        }

        public virtual bool CanWithdraw(decimal amount)
        {
            return Balance >= amount;
        }


    }
}
