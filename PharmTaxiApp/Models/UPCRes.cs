using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PharmTaxiApp.Models
{
    /// <summary>
    /// Класс ответа для получения инфы по америкосовским штрихкодам. Вероятно, надо бы дополнить в соответствии с данной документацией https://www.upcitemdb.com/wp/docs/main/development/responses/
    /// </summary>
    public class UPCRes
    {
        public string  code { get; set; }
        public int total { get; set; }
        public int offset { get; set; }

        public List<UPCItem> items { get; set; }

    }

    public class UPCItem
    {
        public string ean { get; set; }
        public string title { get; set; }
        public string description { get; set; }

    }
}