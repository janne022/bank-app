using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Models
{
    public enum Currency
    {
        SEK,  //Swedish krona (BASE CURRENCY)
        USD,  //Dollar
        EUR,  //Euro
        GBP,  //Pound Sterling
        JPY,  //Japanese Yen
        AUD,  //Australian Dollar
        CAD,  //Canadian Dollar
        CHF,  //Swiss Franc
        CNY,  //Chinese Yuan
        NZD,  //New Zealand Dollar
        SLC   // Slava Coin
    }


    public enum Status
    {
        Active,

        Closed,

    }


    public enum TransactionType
    {
        Deposit,
        Withdrawal,
    }
    public enum AccountType
    {
        CheckingAcc,
        SavingsAcc,
        LoanAcc,

    }


}
