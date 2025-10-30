namespace bank_app.Models
{
    public class Loan
    {
        public Guid LoanId { get; }
        public Guid UserId { get; set; }
        public decimal Principal { get; set; }
        public decimal OutStandingPrincipal { get; set; }
        public decimal AccruedInterest { get; set; }
        public decimal AnnualRate { get; set; }
        public DateTime LastAccrualDate { get; set; }



        public Loan(Guid userId, decimal principal, decimal annualRate, DateTime startDate)
        public decimal Principal { get; set; }

        public int Term { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }



        public Loan(decimal interest, decimal principal, int term, DateTime startDate, DateTime endDate)
        {
            LoanId = Guid.NewGuid();
            UserId = userId;
            Principal = principal;
            OutStandingPrincipal = principal;
            AnnualRate = annualRate;
            LastAccrualDate = startDate;


        }


    }
}
