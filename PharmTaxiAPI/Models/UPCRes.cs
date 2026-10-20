using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PharmTaxiAPI.Models
{
    public class UPCRes
    {
        public string  code { get; set; }
        public int total { get; set; }
        public int offset { get; set; }

        public List<UPCItem> items { get; set; }

    }

    public class UPCItem
    {
        public string ean { get; set; }
        public string title { get; set; }
        public string description { get; set; }

    }
}