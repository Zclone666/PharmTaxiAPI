using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAdm.Models.User
{
    public class AuthParameter:GetUserInfo
    {
        public string service { get; set; }
        public string password { get; set; }
    }

    public class GetUserInfo
    {
        public string key { get; set; }
        public string email { get; set; }
    }
}
