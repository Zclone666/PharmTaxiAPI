using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Models
{
    public class OldOrderResponse
    {
        public string code { get; set; }
        public string patient_id { get; set; }
        public string device_id { get; set; }
        public string recipe_id { get; set; }
        public string drugstore_id { get; set; }
        public string drugstore_yid { get; set; }
        public string order_date { get; set; }
        public string end_date { get; set; }
        public string status { get; set; }

        public Ord order { get; set; }

    }

    public class Ord
    {
        public List<SMReceipt.Global.Models.Meds> medicaments { get; set; } = new List<SMReceipt.Global.Models.Meds>() { new SMReceipt.Global.Models.Meds()};
    }

}