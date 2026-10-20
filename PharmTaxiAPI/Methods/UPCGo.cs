using API.Models;
using PharmTaxiAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PharmTaxiAPI.Methods
{
    public static class UPCGoMF
    {
        public static UPCRes Get(string upc) 
        {
            UPCReq req = new UPCReq(upc);
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(req);
            UMKBRequests.WebUtils.Result res=UMKBRequests.WebUtils.Post(Constants.Consts.UPCReqAddress, data, "application/json");
            UPCRes ret = Newtonsoft.Json.JsonConvert.DeserializeObject<UPCRes>(res.Answer);
            return ret;
        }
    }
}