using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UMKBNeedStuff;

namespace PharmTaxiApp.Models
{
    public class RecipesResponse
    {
        public Alert alert { get; set; } = new Alert() { code = "200", level = "success" };
        public string patient_id { get; set; }
        public Dictionary<string, long> recipes { get; set; }
    }
}
