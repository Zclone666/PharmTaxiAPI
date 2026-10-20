using System;
using System.Collections.Generic;
using PTConstants;
using System.Threading.Tasks;

namespace PharmTaxiApp.Methods.eRp
{
    public static class PrescFull
    {
        public static Models.eRp.Response.PrescrLong Get(Models.eRp.Request.PrescrList data)
        {
            Newtonsoft.Json.JsonSerializerSettings JS = new Newtonsoft.Json.JsonSerializerSettings();
            JS.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            string Sdata = Newtonsoft.Json.JsonConvert.SerializeObject(data, JS);
            string WebResp = UMKBRequests.WebUtils.Post(URLs.PrescrInfoFull, Sdata, "application/json").Answer;
            Models.eRp.Response.PrescrLong Resp = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.eRp.Response.PrescrLong>(WebResp);
            Task.Run(()=>SaveOrig(Resp));
            return Resp;
        }

        static void SaveOrig(Models.eRp.Response.PrescrLong data)
        {
            UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Mongo.MongoDB, Constants.eRpFull, Mongo.PHTFORSTFR);
            UsefullStuff.Common.Methods.MongoTools.Methods.Write<Models.eRp.Response.PrescrLong> WrT = new UsefullStuff.Common.Methods.MongoTools.Methods.Write<Models.eRp.Response.PrescrLong>();
            Models.eRp.Response.PrescrLong Filtr = new Models.eRp.Response.PrescrLong();
            Filtr.requestUid = data.requestUid;
            //WrT.UpSert_Compare(Filtr, data);
            WrT.InSert(data);
        }

    }
}
