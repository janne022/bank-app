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

        public readonly decimal interestRate = 0.05m;
        public decimal Interest { get; set; }

        public decimal Principal{ get; set; }

        public int Term { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }



        public Loan(decimal interest, decimal principal, int term, DateTime startDate,DateTime endDate)
        {
            Interest = interest;
            Principal = principal;
            Term = term;
            StartDate = startDate;
            EndDate = endDate;
        }



        

    }
}
