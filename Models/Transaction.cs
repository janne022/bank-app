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
        public Guid TransactionId = Guid.NewGuid();
        public DateTime TimeStamp = DateTime.Now;
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public decimal TransferAmount { get; set; }
        public TransferStatus Status { get; set; }
        public TransactionType TransactionType { get; internal set; }

        public Transaction(Guid senderId, Guid receiverId, decimal transferAmount)
        {
            SenderId = senderId;
            ReceiverId = receiverId;
            TransferAmount = transferAmount;
        }
    }
}
