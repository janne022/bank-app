using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bank_app.Utility;

namespace bank_app.Models.Accounts
{
    public abstract class Account
    {
        public Guid AccountID { get; private set; }
        public Currency AccountCurrency { get; private set; }
        public decimal Balance { get; private set; }
        public string OwnerId { get; private set; }
        private readonly List<Transaction> _transactions = new List<Transaction>();
        public IReadOnlyList<Transaction> Transactions => _transactions;

        protected Account(Currency currency, decimal balance, string ownerId)
        {
            AccountID = Guid.NewGuid();
            AccountCurrency = currency;
            Balance = balance < 0 ? 0 : balance;
            OwnerId = ownerId;
        }
        /// <summary>
        /// Method that adds or subtracts balance in accounts local currency, while also saving all transactions made into a List. 
        /// </summary>
        internal void ApplyTransaction(Transaction transaction, decimal amount)
        {
            if (!CanApply(transaction))
            {
                throw new InvalidOperationException("Transaction cannot be applied");
            }

            switch (transaction.TransactionType)
            {
                case TransactionType.Deposit:
                    Balance += amount;
                    break;
                case TransactionType.Withdrawal:
                    Balance -= amount;
                    break;
                default:
                    Console.WriteLine("This transaction type is not an option");
                    break;
            }

            _transactions.Add(transaction);

        }

        public abstract bool CanApply(Transaction transaction);

    }
}
