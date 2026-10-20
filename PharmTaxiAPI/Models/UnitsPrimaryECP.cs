using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Models
{
    public class AnswerUnits
    {
        /// <summary>
        /// Стандартный класс алерта - информация о состоянии обработки запроса.
        /// </summary>
        public UMKBRequests.Alert alert { get; set; } = new UMKBRequests.Alert() { code = "200", message = "success", sticky = false, title = string.Empty, level = string.Empty };
        /// <summary>
        /// Ответ. Ключ - BigId из запроса, значание - Список [математическая операция, BigId]
        /// </summary>
        public Dictionary<string, List<complexUnit>> units { get; set; } = new Dictionary<string, List<complexUnit>>();
        public Dictionary<string, HashSet<string>> unitrefs { get; set; } = new Dictionary<string, HashSet<string>>();
        public HashSet<string> unitpharm { get; set; } = new HashSet<string>();
        public Dictionary<string, string> names { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> nameshort { get; set; } = new Dictionary<string, string>();
    }

    public class complexUnit
    {
        public string action { get; set; } = null;
        public double? value { get; set; } = 0;
        public string id { get; set; }
    }
}
