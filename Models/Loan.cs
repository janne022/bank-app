using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Models
{
    public class Loan
    {
        public Guid LoanId { get; }
        public Guid UserId { get; set; }
        public decimal  Principal { get; set; }
        public decimal  OutStandingPrincipal { get; set; }
        public decimal AccruedInterest { get; set; }
        public decimal  AnnualRate { get; set; }
        public  DateTime LastAccrualDate { get; set; }



        public Loan(Guid userId, decimal principal, decimal annualRate, DateTime startDate)
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
