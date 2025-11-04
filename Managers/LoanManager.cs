using bank_app.Models;
using bank_app.Models.Accounts;
using bank_app.Models.Users;
using bank_app.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Managers
{
    internal class LoanManager
    {

        private readonly List<Loan> _loans = new List<Loan>();


        public Loan DisburseLoan(string userId, decimal principal, Currency currency)
        {

            if (!ValidateLoanLimit(userId, principal))
            {
                throw new InvalidOperationException("Loan denied: exceeds allowed limit.");
            }


            var loanAccount = GetOrCreateLoanAccount(userId, currency);

            var loan = new Loan(userId, principal, AccountDefaults.LoanInterestRate, DateTime.Now);
            _loans.Add(loan);

            var transaction = CreateLoanTransaction(loanAccount, principal);
            loanAccount.ApplyTransaction(transaction, principal);

            return loan;

        }

        public LoanAccount GetOrCreateLoanAccount(string userId, Currency currency)
        {
            var loanAccount = AccountManager.GetAllAccounts(userId)
                .OfType<LoanAccount>()
                .FirstOrDefault();

            if (loanAccount == null)
            {
                loanAccount = new LoanAccount(
                    currency,
                    userId,
                    AccountDefaults.LoanCreditLimit
                );



                AccountManager.AddAccount(loanAccount);

            }

            return loanAccount;
        }


        public void AccrueInterestForAllLoans(DateTime currentDate)
        {
            foreach (var loan in _loans.Where(l => l.IsActive))
            {
                decimal before = loan.AccruedInterest;

                loan.ApplyInterest(currentDate);

                decimal added = loan.AccruedInterest - before;

                var loanAccount = AccountManager.GetAllAccounts(loan.UserId)
               .OfType<LoanAccount>()
               .FirstOrDefault();


                if (loanAccount != null && added > 0)
                {
                    var transaction = new Transaction(
                loanAccount.AccountID,
                loanAccount.AccountID,
                added)
                    {
                        TransactionType = TransactionType.LoanInterest,
                        Status = TransferStatus.Completed,

                    };
                    loanAccount.ApplyTransaction(transaction, added);
                }


            }
        }

        public bool RepayLoan(string userID, Guid loanID, decimal paymentAmount)
        {
            var loan = _loans.FirstOrDefault(l => l.LoanId == loanID && l.UserId == userID);

            if (loan == null) { return false; }

            loan?.ApplyPayment(paymentAmount);



            var loanAccount = AccountManager.GetAllAccounts(userID).OfType<LoanAccount>().FirstOrDefault();

            if (loanAccount != null)
            {

                var transaction = new Transaction(
               loanAccount.AccountID,
               loanAccount.AccountID,
               paymentAmount)
                {
                    TransactionType = TransactionType.LoanRepayment,
                    Status = TransferStatus.Completed,

                };
                loanAccount.ApplyTransaction(transaction, paymentAmount);
            }
            return true;

        }










        private bool ValidateLoanLimit(string userId, decimal requestedAmount)
        {


            decimal totalBalance = AccountManager.GetAllAccounts(userId)
                                   .Where(account => account is not LoanAccount)
                                   .Sum(account => account.Balance);
            if (totalBalance <= 0)
            {

                return false;
            }

            decimal existingDebt = _loans.Where(loan => loan.UserId == userId && loan.IsActive).Sum(loan => loan.OutstandingPrincipal);
         

            decimal maxAllowed = Math.Round(totalBalance * 5, 2);

            if ((requestedAmount + existingDebt) > maxAllowed)
            {
                return false;
            }
            return true;
        }

        private Transaction CreateLoanTransaction(LoanAccount loanAccount, decimal principal)
        {
            if (principal <= 0)
                throw new ArgumentOutOfRangeException(nameof(principal), "Loan amount must be positive.");

            var transaction = new Transaction(
                loanAccount.AccountID,
                loanAccount.AccountID,
                principal)
            {
                TransactionType = TransactionType.LoanDisbursement,
                Status = TransferStatus.Completed,

            };


            return transaction;
        }


    }
}
