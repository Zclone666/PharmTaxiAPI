using System;
using System.Collections.Generic;
using PTConstants;

namespace PharmTaxiApp.Methods.eRp
{
    public static class RelaesePresc
    {
        
        public static Models.eRp.Response.ReleasePrescr Get(Models.eRp.Request.ReleasePrescr data)
        {
            Newtonsoft.Json.JsonSerializerSettings JS = new Newtonsoft.Json.JsonSerializerSettings();
            JS.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            string Sdata = Newtonsoft.Json.JsonConvert.SerializeObject(data, JS);
            string WebResp = UMKBRequests.WebUtils.Post(URLs.PrescRelease, Sdata, "application/json").Answer;
            Models.eRp.Response.ReleasePrescr Resp = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.eRp.Response.ReleasePrescr>(WebResp);
            ChangeOrigFull(data);
            return Resp;
        }

        static void ChangeOrigFull(Models.eRp.Request.ReleasePrescr data)
        {
            UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Mongo.MongoDB, Constants.eRpFull, Mongo.PHTFORSTFR);
            Models.eRp.Response.PrescrLong PrescrCh = new Models.eRp.Response.PrescrLong();
            PrescrCh=UsefullStuff.Common.Methods.Converter<Models.eRp.Response.PrescrLong, Models.eRp.Request.ReleasePrescr>.Convert(PrescrCh, data);
            UsefullStuff.Common.Methods.MongoTools.Methods.Write<Models.eRp.Response.PrescrLong> Wrt = new UsefullStuff.Common.Methods.MongoTools.Methods.Write<Models.eRp.Response.PrescrLong>();
            Models.eRp.Response.PrescrLong Filtr = new Models.eRp.Response.PrescrLong();
            Filtr.requestUid = data.prescriptionUid;
            //Wrt.UpSert_Compare(Filtr, PrescrCh);
            Wrt.InSert(PrescrCh);
        }

        //static void ChangeOrigShort(Models.eRp.Request.ReleasePrescr data)
        //{
        //    UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Constants.Consts.MongoDb, Constants.Consts.eRpShort, Constants.Consts.PHTFORSTFR);
        //    Models.eRp.Response.PrescrShort PrescrCh = new Models.eRp.Response.PrescrShort();
        //    PrescrCh = UsefullStuff.Common.Methods.Converter<Models.eRp.Response.PrescrShort, Models.eRp.Request.ReleasePrescr>.Convert(PrescrCh, data);
        //    UsefullStuff.Common.Methods.MongoTools.Methods.Write<Models.eRp.Response.PrescrShort> Wrt = new UsefullStuff.Common.Methods.MongoTools.Methods.Write<Models.eRp.Response.PrescrShort>();
        //    Wrt.UpSert(PrescrCh);
        //}
    }
}
