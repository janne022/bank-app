using bank_app.Utility;

namespace bank_app.Models
{
    public class CurrencyExchange
    {
        private static Dictionary<Currency, decimal> _ratesToSEK = new Dictionary<Currency, decimal>
        {
            {Currency.SEK, 1.00m }, //SEK is base currency. All rates are realative to SEK. 
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

        /// <summary>
        /// Exchanges an amount of a selected currency to SEK based on current exchange rates. 
        /// </summary>
        public static decimal ExchangeToSek(decimal amount, Currency currencyFrom, Currency sek = Currency.SEK)
        {
            decimal exchangeRate = _ratesToSEK[currencyFrom];
            return amount / exchangeRate;
        }

        /// <summary>
        /// Exchanges an amount of SEK into a selected currency based on current exchange rates. 
        /// </summary>
        public static decimal ExchangeFromSek(decimal amount, Currency currencyTo, Currency sek = Currency.SEK)
        {
            decimal exchangeRate = _ratesToSEK[currencyTo];
            return amount * exchangeRate;
        }

        /// <summary>
        /// Updates the exchange rate of a chosen currency in the currency list based on the base currency SEK.
        /// This method is meant to be used by Admin user to update current exchange rates. 
        /// </summary>
        public static bool UpdateRate(Currency currency, decimal rateToSek)
        {
            if (currency == Currency.SEK)
            {
                return false;
                throw new ArgumentException("Cannot change the base rate (SEK)");
            }

            if (rateToSek <= 0)
            {
                return false;
                throw new ArgumentException("Exchange rate must be more than 0");
            }

            _ratesToSEK[currency] = rateToSek;
            return true;
        }

        /// <summary>
        /// Method to be used by UI to display all current rates based on the base rate.
        /// </summary>
        public static Dictionary<Currency, decimal> GetAllRates()
        {
            return new Dictionary<Currency, decimal>(_ratesToSEK);
        }

        /// <summary>
        /// Method to be used by UI to display current rate of a chosen currency based on the base rate. 
        /// </summary>
        public static decimal GetRate(Currency currency)
        {
            return _ratesToSEK[currency];
        }
    }
}