using System;
using System.Collections.Generic;
using System.Text;

namespace EMC.Models
{
    public class PatientSearch
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public long? birthdate { get; set; }
    }
}
