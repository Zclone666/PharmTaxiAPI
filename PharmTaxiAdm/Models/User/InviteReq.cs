using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiAdm.Models.User
{
    public class InviteReq
    {
        public string key { get; set; }
        public string authkey { get; set; }
        public string invitedEmail { get; set; }
        public List<string> invitedRoleCodes { get; set; } = new List<string>();
        public string application { get; set; }
    }
}
