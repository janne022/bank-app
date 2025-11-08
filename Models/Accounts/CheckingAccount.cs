using bank_app.Utility;

namespace bank_app.Models.Accounts
{
    public class CheckingAccount : Account
    {
        //Monthly fee is the fee charged every month for maintaining the account, and logic will be implemented in Account Manager
        public decimal MonthlyFee { get; private set; }
        //Overdrat limist is the maximum negative balance allowed 
        public decimal OverdraftLimit { get; private set; }

        public CheckingAccount(Currency currency, decimal balance, string ownerId, decimal overdraftLimit, decimal monthlyFee, AccountType accountType, string label) 
            : base(currency, balance, ownerId, accountType, label)
        {
            OverdraftLimit = overdraftLimit > 0 ? 0 : overdraftLimit;
            MonthlyFee = monthlyFee < 0 ? 0 : monthlyFee;
        }

        /// <summary>
        /// This method is looking to see if the transaction can be applied based on the type of transaction and the current balance and overdraft limit.
        /// </summary>
        public override bool CanApply(Transaction transaction)
        {
            if (transaction == null || transaction.TransferAmount <= 0)
            {
                return false;
            }

            if (transaction.TransactionType == TransactionType.Deposit)
            {
                return true;
            }

            if (transaction.TransactionType == TransactionType.Withdrawal)
            {
                return Balance - transaction.TransferAmount >= -OverdraftLimit;
            }
            return false;
        }
    }
}
