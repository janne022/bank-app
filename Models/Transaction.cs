using bank_app.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using bank_app.Utility;

namespace bank_app.Models
{
    public class Transaction
    {
        public TransactionType TransactionType { get; internal set; }
        public Guid TransactionId = Guid.NewGuid();
        public Account Sender { get; set; }
        public Account Receiver { get; set; }
        public decimal TransferAmount { get; set; }
        public Currency TransferCurrency { get; set; }
        public DateTime TimeStamp { get; set; }
        public TransferStatus Status { get; set; }

        public Transaction(Account sender, Account receiver, decimal transferAmount)
        {
            TimeStamp = DateTime.Now;
            Sender = sender;
            Receiver = receiver;
            TransferAmount = transferAmount;
        }

        //Overloaded constructor for other currencies. Feature not implemented yet. 
        public Transaction(Account sender, Account receiver, decimal transferAmount, Currency transferCurrency)
        {
            TransactionId = Guid.NewGuid();
            TimeStamp = DateTime.Now;
            Sender = sender;
            Receiver = receiver;
            TransferAmount = transferAmount;
            TransferCurrency = transferCurrency;
        }
    }
}
