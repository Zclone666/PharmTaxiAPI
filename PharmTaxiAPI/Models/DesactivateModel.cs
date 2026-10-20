using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Models
{
    public class DesactivateModel
    {
        public string authkey { get; set; }
        public string key { get; set; }
        public uint user_id { get; set; }
        public bool full_desactivate { get; set; }
        public List<uint> pharms_to_exclude { get; set; }
    }
}
