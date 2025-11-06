namespace bank_app.Models.Accounts
{
    public static class AccountDefaults
    {
        //This class holds default values for different account types

        //Checking Account
        public const decimal CheckingMonthlyFee = 20m;
        public const decimal CheckingOverdraftLimit = 1000m;

        //Savings Account
        public const decimal SavingsInterestRate = 0.1m;
        public const decimal SavingsMinimumBalance = 1000m;
        public const bool SavingsAllowWithdraws = true;

        //Loan Account
        public const decimal LoanInterestRate = 6m;
        public const decimal LoanCreditLimit = 10000000m;
        public const int MaxLoansPerUser = 5;

    }
}
