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

            if (existingDebt <= 0)
            {
                return false;
            }

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
                TransactionType = TransactionType.Withdrawal,
                Status = TransferStatus.Completed,

            };


            return transaction;
        }


    }
}
