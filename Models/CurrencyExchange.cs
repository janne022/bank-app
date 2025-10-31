using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace bank_app.Models
{
    public class CurrencyExchange
    {
        //Exchange rates relative to our base currency (SEK)
        private static Dictionary<Currency, decimal> _ratesToSEK = new Dictionary<Currency, decimal>
        {
            {Currency.SEK, 1.00m },
            {Currency.USD, 0.11m },
            {Currency.EUR, 0.09m },
            {Currency.GBP, 0.08m },
            {Currency.JPY, 16.26m },
            {Currency.AUD, 0.16m },
            {Currency.CAD, 0.15m },
            {Currency.CHF, 0.09m },
            {Currency.CNY, 0.76m },
            {Currency.NZD, 0.18m },
            {Currency.SLC, 0.000004m }
        };
        public static decimal ExchangeToSek(decimal amount, Currency currencyFrom, Currency sek = Currency.SEK)
        {
            decimal exchangeRate = _ratesToSEK[currencyFrom];
            return amount / exchangeRate;
        }
        public static decimal ExchangeFromSek(decimal amount, Currency currencyTo, Currency sek = Currency.SEK)
        {
            decimal exchangeRate = _ratesToSEK[currencyTo];
            return amount * exchangeRate;
        }
    }
}