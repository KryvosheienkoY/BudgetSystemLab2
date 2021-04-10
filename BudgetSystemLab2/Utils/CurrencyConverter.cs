using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetSystemLab2
{
    public static class CurrencyConverter
    {
        private static Dictionary<Enums.Currency, decimal> _coefMap = new Dictionary<Enums.Currency, decimal>
        {
            [Enums.Currency.EUR] = 0.84m,
            [Enums.Currency.UAH] = 27.75m,
            [Enums.Currency.USD] = 1m,
        };
        public static decimal Convert(decimal amount, Enums.Currency currencyFrom, Enums.Currency currencyTo)
        {
            return (_coefMap.ContainsKey(currencyTo) && _coefMap.ContainsKey(currencyFrom)) ? amount * _coefMap[currencyTo] / _coefMap[currencyFrom] : decimal.MinValue;
        }
    }
}
