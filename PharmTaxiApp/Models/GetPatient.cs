using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiApp.Models
{
    /// <summary>
    /// Для получения актуальной фармакотерапии
    /// </summary>
    public class GetPatient
    {
        public string key { get; set; }
        public string authkey { get; set; }
        public string service { get; set; }
        public string active_language { get; set; } = "ru";
        public string id { get; set; }
    }

    public class GP2
    {
        public string authkey { get; set; }
        public string email { get; set; }

    }
}
