using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PharmTaxiApp.Models.eRp.Request
{
    public class PrescrList
    {
        public string uid { get; set; }
        public string prescribedMedicationGuid { get; set; }
        public string snils { get; set; }
        public string formTypeCode { get; set; }
        public string phone { get; set; }
        public bool? isSendFailed { get; set; }
        public DateTime? dateBegin { get; set; }
        public DateTime? dateEnd { get; set; }
        public int? skip { get; set; }
        public string take { get; set; }
    }
}
