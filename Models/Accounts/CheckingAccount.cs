using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Models.Accounts
{
    public class CheckingAccount : Account
    {

        public decimal MonthlyFee { get; private set; }
        public decimal OverdraftLimit { get; private set; }
        public CheckingAccount(Currency currency, decimal balance, decimal monthlyFee, decimal overdraftLimit)
            : base(currency, balance)
        {

            SetMonthlyFee(monthlyFee);
            SetOverdraftLimit(overdraftLimit);

        }
        public void UpdateOverdraftLimit(decimal limit)
        {
            SetOverdraftLimit(limit);
        }
        public void UpdateMonthlyFee(decimal fee)
        {
            SetMonthlyFee(fee);
        }

        public void SetMonthlyFee(decimal fee)
        {
            MonthlyFee = fee < 0 ? 0 : fee;

        }

        public void SetOverdraftLimit(decimal limit)
        {
            OverdraftLimit = limit < 0 ? 0 : limit;
        }

        public bool ApplyMonthlyFee()
        {
            if (CanWithdraw(MonthlyFee))
            {
                UpdateBalance(-MonthlyFee);
                return true;
            }
            return false;
        }


        public override void Withdraw(decimal amount)
        {
            if (amount<=0)
            {
                Console.WriteLine("Withdrawal amount must be positive.");
                return;

            }


            if (CanWithdraw(amount))
            {
                UpdateBalance(-amount);
            }
            else
            {
                Console.WriteLine("Withdrawal would exceed overdraft limit.");
            }
        }

        public override bool CanWithdraw(decimal amount)
        {
            return amount>0 &&  Balance - amount>= -OverdraftLimit;
        }

    }
}
