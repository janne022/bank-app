using bank_app.Models;
using bank_app.Models.Accounts;
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


        public Loan DisburseLoan(Guid userId, decimal principal, Currency currency)
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

        public LoanAccount GetOrCreateLoanAccount(Guid userId, Currency currency)
        {
            var loanAccount = AccountManager.GetAllAccounts()
                .OfType<LoanAccount>()
                .FirstOrDefault(acc => acc.OwnerId == userId);

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
        private bool ValidateLoanLimit(Guid userId, decimal requestedAmount)
        {
           

            decimal totalBalance = AccountManager.GetAllAccounts()
                                   .Where(account => account.OwnerId == userId && account is not LoanAccount)
                                   .Sum(account => account.Balance);
            if (totalBalance <= 0)
            {
               throw new InvalidOperationException("User has no active deposit accounts to support a loan request.");
            }

            decimal existingDebt = _loans.Where(loan => loan.UserId == userId && loan.IsActive).Sum(loan => loan.OutstandingPrincipal);

            decimal maxAllowed = Math.Round(totalBalance * 5, 2);

            if ((requestedAmount +existingDebt)> maxAllowed)
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
                TransactionType = TransactionType.Withdrawal,
                Status = TransferStatus.Completed,

            };


            return transaction;
        }


    }
}
