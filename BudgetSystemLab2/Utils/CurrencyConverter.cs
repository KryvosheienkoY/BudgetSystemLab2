using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetSystemLab2
{
    public static class CurrencyConverter
    {
        private static Dictionary<Enums.Currency, decimal> CoefMap = new Dictionary<Enums.Currency, decimal>
        {
            [Enums.Currency.EUR] = 0.84m,
            [Enums.Currency.UAH] = 27.75m,
            [Enums.Currency.USD] = 1m,
        };
        public static decimal Convert(decimal amount, Enums.Currency currencyFrom, Enums.Currency currencyTo)
        {
            return (CoefMap.ContainsKey(currencyTo) && CoefMap.ContainsKey(currencyFrom)) ? amount * CoefMap[currencyTo] / CoefMap[currencyFrom] : decimal.MinValue;
        } 
        public static decimal Convert(decimal amount, string currencyFrom, string currencyTo)
        {
            Enums.Currency from = (Enums.Currency)Enum.Parse(typeof(Enums.Currency), currencyFrom, true);
            Enums.Currency to = (Enums.Currency)Enum.Parse(typeof(Enums.Currency), currencyTo, true);
            return (CoefMap.ContainsKey(to) && CoefMap.ContainsKey(from)) ? amount * CoefMap[to] / CoefMap[from] : decimal.MinValue;
        }
    }
}
