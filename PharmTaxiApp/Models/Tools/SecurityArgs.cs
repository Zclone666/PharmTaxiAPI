using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiApp.Models.Tools
{
    public class SecurityArgs
    {
#pragma warning disable IDE1006 // Стили именования        
        public string key { get; set; }
        public string authkey { get; set; }
        public string service { get; set; }
        public string active_language { get; set; } = "ru";
#pragma warning restore IDE1006 // Стили именования
    }
}
