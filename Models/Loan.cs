namespace bank_app.Models
{
    public class Loan
    {
        public Guid LoanId { get; }
        public Guid UserId { get; private set; }
        //Original amount borrowed
        public decimal Principal { get; private set; }
        //How much is left to pay
        public decimal OutstandingPrincipal { get; private set; }
        //Interest that has accrued but not yet paid
        public decimal AccruedInterest { get; private set; }
        public decimal AnnualRate { get; private set; }
        public DateTime LastAccrualDate { get; private set; }

        public bool IsActive => OutstandingPrincipal > 0 || AccruedInterest > 0;

        public Loan(Guid userId, decimal principal, decimal annualRate, DateTime startDate)
        {


            LoanId = Guid.NewGuid();
            UserId = userId;
            Principal = principal < 0 ? throw new ArgumentOutOfRangeException(nameof(principal)) : principal;
            OutstandingPrincipal = principal < 0 ? throw new ArgumentOutOfRangeException(nameof(principal)) : principal;
            AnnualRate = annualRate < 0 ? throw new ArgumentOutOfRangeException(nameof(annualRate)) : annualRate;
            LastAccrualDate = startDate.Date;


        }


        public decimal CalculateInterest(int days)
        {
            if (days <= 0 || AnnualRate <= 0 || OutstandingPrincipal <= 0)
            {
                return 0;
            }

            decimal dailyRate = AnnualRate / 100m / 365m;
            decimal interest = OutstandingPrincipal * dailyRate * days;
            return interest;

        }

        //Can change to return type decimal if it's needed
        public void ApplyInterest(DateTime currentDate)
        {
            int daysToAccrue = (currentDate - LastAccrualDate).Days;
            if (daysToAccrue <= 0) {   return; }

            decimal interest = CalculateInterest(daysToAccrue);
            if (interest < 0)
            {
                return;
            }
            interest = Math.Round(interest, 2, MidpointRounding.AwayFromZero);

            AccruedInterest += interest;
            LastAccrualDate = currentDate.Date;


        }


        public void ApplyPayment(decimal amount )
        {
            if (amount<=0)
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

            if (OutstandingPrincipal <=0 && AccruedInterest<=0)
            {
                OutstandingPrincipal = 0;
                AccruedInterest = 0;
                
            }

            
        }

    }
}
