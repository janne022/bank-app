using bank_app.Utility;

namespace bank_app.Models.Accounts
{
    public class LoanAccount : Account
    {
        public decimal CreditLimit { get; private set; }
        public LoanAccount(Currency currency, string ownerId, decimal creditLimit, AccountType accountType, string label)
            : base(currency, 0, ownerId, accountType,label)
        {
            CreditLimit = creditLimit < 0 ? throw new ArgumentOutOfRangeException(nameof(creditLimit), "Credit limit cannot be negative.") : creditLimit;
        }

        public override bool CanApply(Transaction transaction)
        {

            if (transaction == null || transaction.TransferAmount <= 0)
            {
                return false;
            }

            if (transaction.TransactionType == TransactionType.Deposit || transaction.TransactionType == TransactionType.LoanRepayment)
            {
                return true;
            }

            if (transaction.TransactionType == TransactionType.Withdrawal || transaction.TransactionType == TransactionType.LoanInterest || transaction.TransactionType == TransactionType.LoanDisbursement)
            {
                return Balance - transaction.TransferAmount >= -CreditLimit;
            }

            return false;
        }
    }
}
