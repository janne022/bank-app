using bank_app.Models;
using bank_app.Models.Accounts;
using bank_app.Utility;

namespace bank_app.Managers
{
    internal class LoanManager
    {
        private static readonly List<Loan> _loans = new List<Loan>();

        /// <summary>
        /// Creates a new loan connected to a user, identified by their userId. Runs methods
        /// to verify that the user is permitted to take a loan, and creates a new loan object. 
        /// </summary>
        public Loan DisburseLoan(string userId, decimal principal)
        {
            if (!ValidateLoanLimit(userId, principal))
            {
                throw new InvalidOperationException("Loan denied: exceeds allowed limit.");
            }

            var loanAccount = GetLoanAccount(userId);
            var checkingAccount = AccountManager.GetAllAccounts(userId).OfType<CheckingAccount>().FirstOrDefault();

            if (checkingAccount == null)
            {
                throw new InvalidOperationException("User must have a checking account to receive loan.");
            }

            var loan = new Loan(userId, principal, AccountDefaults.LoanInterestRate, DateTime.Now);
            _loans.Add(loan);

            var transactionLoan = CreateLoanTransaction(loanAccount, principal);
            loanAccount.ApplyTransaction(transactionLoan, principal);

            var transactionToChecking = CreateCheckingAccTransaction(checkingAccount, principal, TransactionType.Deposit);
            checkingAccount.ApplyTransaction(transactionToChecking, principal);
            return loan;
		}

        /// <summary>
        /// Checks if the input user has a loan account. Returns it if true, otherwise
        /// automatically creates a new one and adds it to the users account list. 
        /// </summary>
        public LoanAccount GetLoanAccount(string userId)
        {
            var loanAccount = AccountManager.GetAllAccounts(userId)
                .OfType<LoanAccount>()
                .FirstOrDefault();

            if (loanAccount==null)
			{
				throw new InvalidOperationException("User must have a checking account to receive loan.");
			}
           
            return loanAccount;
        }

        public static  decimal CalcculateMonthlyPayment(string userId, int months)
        {
            var loan = LoanManager.GetLatestLoan(userId);

            if (loan==null)
            {
                throw new InvalidOperationException("User must have a checking account to receive loan.");
            }

            decimal monthlyRate = loan.AnnualRate / 100m / months;
            if (monthlyRate == 0)
            {
                return loan.Principal / months;
            }

            decimal factor = (decimal)Math.Pow((double)(1 + monthlyRate), months);
            decimal monthlyPayment = loan.Principal * monthlyRate * factor / (factor - 1);
            return monthlyPayment;

        }


        public void AccrueInterestForAllLoans(DateTime currentDate)
        {
            foreach (var loan in _loans.Where(l => l.IsActive))
            {
                decimal before = loan.AccruedInterest;

               // loan.ApplyInterest(currentDate);

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

            if (loan == null)
                return false;

            loan?.ApplyPayment(paymentAmount);

            var loanAccount = AccountManager.GetAllAccounts(userID).OfType<LoanAccount>().FirstOrDefault();
            var checkingAccount = AccountManager.GetAllAccounts(userID).OfType<CheckingAccount>().FirstOrDefault();

            if (checkingAccount != null)
            {
                var transactionToChecking = CreateCheckingAccTransaction(checkingAccount, paymentAmount, TransactionType.Withdrawal);
                checkingAccount.ApplyTransaction(transactionToChecking, paymentAmount);
            }

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
            bool isUserHasCheckingAccount = AccountManager.GetAllAccounts(userId).Any(a => a is CheckingAccount);

            if (!isUserHasCheckingAccount)
            {
                return false;
            }

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

        private Transaction CreateCheckingAccTransaction(CheckingAccount checkingAccount, decimal principal, TransactionType transactionType)
        {
            if (principal <= 0)
                throw new ArgumentOutOfRangeException(nameof(principal), "Loan amount must be positive.");

            var transaction = new Transaction(
                checkingAccount.AccountID,
                checkingAccount.AccountID,
                principal)
            {
                TransactionType = transactionType,
                Status = TransferStatus.Completed,
            };
            return transaction;
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

        public static Loan? GetLatestLoan(string userId)
        {
            return _loans.Where(l => l.UserId == userId).OrderByDescending(l => l.LastAccrualDate).FirstOrDefault();
        }
    }
}
