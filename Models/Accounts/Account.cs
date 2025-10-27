using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Models.Accounts
{
    public abstract class Account
    {
        public Guid AccountID { get; private set; }

        public Currency AccountCurrency { get; private set; }

        public decimal Balance { get; private set; }

        private readonly List<Transaction> _transactions = new List<Transaction>();
        public IReadOnlyList<Transaction> Transactions => _transactions;


        protected Account(Currency currency, decimal balance)
        {

            AccountID = Guid.NewGuid();
            AccountCurrency = currency;
            Balance = balance< 0 ? 0 : balance;
           
        }
      
        public void ApplyTransaction(TransactionType transactionType, decimal amount, Transaction transaction) //This will be  change just to "Transaction transaction" later
        {
            if (!CanApply(transaction))
            {
                Console.WriteLine("This transaction cannot be applied.");
                return;
            }

            switch (transactionType)
            {
                case TransactionType.Deposit:
                   Balance+= amount;
                    break;
                case TransactionType.Withdrawal:
                    Balance-= amount;
                    break;
                default:
                    Console.WriteLine("This transaction type doest not an option");
                    break;
            }
            _transactions.Add(transaction);

        }


        public abstract bool CanApply(Transaction transaction);

        

        




    }
}
