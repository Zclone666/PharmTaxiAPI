//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace PharmTaxiAPI.Models
//{
//    public class WriteOffRequest
//    {
//        public string PharmId { get; set; }
//        public string authkey { get; set; }
//        public string drugstore_id { get { return PharmId; }  set {PharmId=value; } }
//        public string auth_key { get { return authkey; } set { authkey = value; } }
//        public List<Meds> medicaments { get; set; } = new List<Meds>();
//        /// <summary>
//        /// Номер заказа
//        /// </summary>
//        public string recipe { get; set; } 
//    }

//    public class Meds
//    {
//        public string id { get; set; }
//        public string table { get; set; }
//        public string name { get; set; }
//        public string value { get; set; }
//        public string packs { get; set; }
//        public string send { get; set; }
//        public string removed { get; set; }
//        public string dosage { get; set; }
//        public string tn { get; set; }
//        public string drugstore_drug_id { get; set; }
//    }

//}