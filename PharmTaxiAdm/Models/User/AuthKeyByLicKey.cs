using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiAdm.Models.User
{
    public class AuthKeyByLicKey
    {
        public string authkey { get; set; }
        public string key { get; set; }
        public string email { get; set; }
        public string master_service { get; set; } = "7";
        public string slave_service { get; set; } = "6";
        public string drugstore_id { get; set; }
        public string master_authkey { get; set; }
        public string slave_authkey { get; set; }
    }
}
