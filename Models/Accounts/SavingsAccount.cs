using bank_app.Utility;

namespace bank_app.Models.Accounts
{
    public class SavingsAccount : Account
    {
        //% per annum
        public decimal InterestRate { get; private set; }
        //Minimum balance required to avoid penalties
        public decimal MinimumBalance { get; private set; }
        //Last date when interest was applied
        public DateTime LastInterestDate { get; private set; }
        //Indicates whether withdrawals are allowed
        public bool AllowWithdrawals { get; private set; }

        public SavingsAccount(Currency currency, decimal balance, string ownerId, decimal interestRate, decimal minimalBalance, DateTime lastInterestDate, bool allowWithdrawals, AccountType accountType)
            : base(currency, balance, ownerId, accountType)
        {
            InterestRate = interestRate < 0 ? 0 : interestRate;
            MinimumBalance = minimalBalance < 0 ? 0 : minimalBalance;
            LastInterestDate = lastInterestDate;
            AllowWithdrawals = allowWithdrawals;
        }

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
                if (!AllowWithdrawals)
                {
                    return false;
                }
                return Balance - transaction.TransferAmount >= MinimumBalance;
            }

            return false;
        }

        public decimal CalculateInterest(int days)
        {
            if (Balance <= 0 || InterestRate <= 0)
            {
                return 0;
            }

            decimal annualRate = InterestRate / 100m;
            decimal fractionalRate = days / 365m;
            decimal interest = Balance * annualRate * fractionalRate;
            return interest;
        }

        public void ApplyInterest(int days)
        {
            if (days <= 0)
            {
                return;
            }

            decimal interest = CalculateInterest(days);

            if (interest <= 0)
            {
                return;
            }

            interest = Math.Round(interest, 2, MidpointRounding.AwayFromZero);
            //We use this.AccountID for both sender and receiver since interest is being added to the same account, because we calculate interest based on current balance.
            var interestTransaction = new Transaction(this.AccountID, this.AccountID, interest)
            {
                TransactionType = TransactionType.Deposit,
                Status = TransferStatus.Completed
            };

            ApplyTransaction(interestTransaction, interest);

            LastInterestDate = LastInterestDate.AddDays(days);
        }
    }
}