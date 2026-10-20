using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Models
{
    public class CheckReq
    {
        public BarcodeRequestData code { get; set; }
        public string text { get; set; }
        public string format { get; set; }
        public double?[] coords { get; set; }
        public uint? accuracy { get; set; }
        public string auth_key { get; set; }
        public string lang { get; set; } = "ru";
        public string mode { get; set; } = "WORK";
        public string country { get; set; }
    }
}
