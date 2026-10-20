using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PharmTaxiApp.Models
{
    /// <summary>
    /// Класс запроса америкосовских штрихкодов
    /// </summary>
    public class UPCReq
    {
        public string upc { get; set; }

        public UPCReq(string upc)
        {
            this.upc = upc;
        }
    }
}