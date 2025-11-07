namespace bank_app.Models
{
    public class Loan
    {
        public Guid LoanId { get; }
        public string UserId { get; private set; }
        //Original amount borrowed
        public decimal Principal { get; private set; }
        //How much is left to pay
        public decimal OutstandingPrincipal { get; private set; }
        //Interest that has accrued but not yet paid
        public decimal AccruedInterest { get; private set; }
        public decimal AnnualRate { get; private set; }
        public DateTime LastAccrualDate { get; private set; }

        public bool IsActive => OutstandingPrincipal > 0 || AccruedInterest > 0;

        public Loan(string userId, decimal principal, decimal annualRate, DateTime startDate)
        {


            LoanId = Guid.NewGuid();
            UserId = userId;
            Principal = principal < 0 ? throw new ArgumentOutOfRangeException(nameof(principal)) : principal;
            OutstandingPrincipal = principal < 0 ? throw new ArgumentOutOfRangeException(nameof(principal)) : principal;
            AnnualRate = annualRate < 0 ? throw new ArgumentOutOfRangeException(nameof(annualRate)) : annualRate;
            LastAccrualDate = startDate.Date;


        }


      

        public void ApplyPayment(decimal amount)
        {
            if (amount <= 0)
            {
                return;
            }
            //Pay % 
            decimal payInterest = Math.Min(amount, AccruedInterest);
            AccruedInterest -= payInterest;
            amount -= payInterest;


            //Pay Loan 
            decimal payPrincipal = Math.Min(amount, OutstandingPrincipal);
            OutstandingPrincipal -= payPrincipal;
            amount -= payPrincipal;

            if (OutstandingPrincipal <= 0 && AccruedInterest <= 0)
            {
                OutstandingPrincipal = 0;
                AccruedInterest = 0;

            }


        }

    }
}
