using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PharmTaxiApp.Methods
{
    public class Utils
    {
        /// <summary>
        /// очистка строки от всякой мишуры
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string ClearingNames(string e) { return Regex.Replace(SMCommonUtils.StringServices.BetweenTagRemove(e.Replace("&quot;", string.Empty)), "([^a-zA-Zа-яА-ЯёЁ+0-9-\\.\\s])", string.Empty); }
    }
}
