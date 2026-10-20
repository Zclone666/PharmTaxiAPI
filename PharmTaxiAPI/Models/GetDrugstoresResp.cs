using PharmTaxiAdm.Models.Drugstore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Models
{
    public class GetDrugstoresResp
    {
        public UMKBRequests.Alert alert { get; set; } = new UMKBRequests.Alert();
        public string role { get; set; }
        public HashSet<DrugstoreModel> drugstores { get; set; } = new HashSet<DrugstoreModel>();
    }
}
