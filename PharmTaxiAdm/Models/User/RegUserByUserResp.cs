using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiAdm.Models.User
{
    public class RegUserByUserResp
    {
        public List<UMKBRequests.Alert> alerts { get; set; }
        public uint? userId { get; set; }
    }
}
