namespace bank_app.Models.Accounts
{
    public static class AccountDefaults
    {
        //This class holds default values for different account types

        //Checking Account
        public const decimal CheckingMonthlyFee = 10m;
        public const decimal CheckingOverdraftLimit = 500m;

        //Savings Account
        public const decimal SavingsInterestRate = 5m;
        public const decimal SavingsMinimumBalance = 100m;
        public const bool SavingsAllowWithdraws = true;

        //Loan Account
        public const decimal LoanInterestRate = 7m;
        public const decimal LoanCreditLimit = 10000000m;
        public const int MaxLoansPerUser = 5;

    }
}
