using bank_app.Utility;

namespace bank_app.Models
{
    public class Transaction
    {
        public Guid TransactionId { get; private set; } = Guid.NewGuid();
        public DateTime TimeStamp { get; private set; } = DateTime.Now;
        public Guid SenderId { get; private set; }
        public Guid ReceiverId { get; private set; }
        public decimal TransferAmount { get; private set; }
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
