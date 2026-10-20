using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PharmTaxiAdm.Models
{
    public class WriteOffRequest
    {
        /// <summary>
        /// лицц ключ
        /// </summary>
        public string key { get; set; }
        public string PharmId { get; set; }
        public string authkey { get; set; }
        public string drugstore_id { get { return PharmId; }  set {PharmId=value; } }
        public string auth_key { get { return authkey; } set { authkey = value; } }
        public List<SMReceipt.Global.Models.Meds> medicaments { get; set; } = new List<SMReceipt.Global.Models.Meds>();
        /// <summary>
        /// Номер заказа
        /// </summary>
        public string recipe { get; set; } 
        public string status { get; set; }
    }

    //public class Meds : SMReceipt.Global.Models.Meds
    //{
        
    //}

}