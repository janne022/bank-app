using bank_app.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Models
{
    public class Transaction
    {
        public string? TransactionId { get; set; }
        public Account Sender { get; set; }
        public Account Reciever { get; set; }
        public decimal TransferAmount { get; set; }

        public Transaction(Account sender, Account reciever, decimal transferAmount)
        {
            TransactionId = CreateTransactionId();
            Sender = sender;
            Reciever = reciever;
            TransferAmount = transferAmount;
        }

        public static string CreateTransactionId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
