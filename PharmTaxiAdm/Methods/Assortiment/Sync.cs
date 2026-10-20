using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using NPOI.SS.Formula.Functions;
using PTConstants;
using System.Collections;

namespace PharmTaxiAdm.Methods.Assortiment
{
    public static class Sync
    {
        public static UMKBUtils.BigIdCryptography Crypto = new UMKBUtils.BigIdCryptography(PharmTaxiAdm.Models.Crypto.Password);
        public static async Task<AUpload.Global.Models.Response> SyncAssort(DataSet DtSet, List<uint> AvIds, bool ForceLoad = false)
        {
            #region Now we are initializing MongoTools
            UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Mongo.MongoDB, Mongo.Assortiment, Mongo.PHTFORSTFR);
            #endregion Done!
            List<uint> Phms = new List<uint>();
            Task<Exception> rrr = new Task<Exception>(()=>(null));
            foreach (DataTable i in DtSet.Tables)
            {
                int ColN = i.Columns["PharmId"].Ordinal;
                DataRow[] DtRaw = i.Select().Distinct().ToArray();
                Phms.AddRange(DtRaw.Select(x => uint.TryParse(x.ItemArray[ColN].ToString(), out uint ass)?ass:0).Distinct().ToList());
            }
            foreach(var i in Phms)
            {
                if (i != 0)
                {
                    #region Clearing this pharmas' assortiment in MongoDB  
                    AUpload.Global.Models.AssortmentItemData ToDel = new AUpload.Global.Models.AssortmentItemData() { PharmId = i, SellingPriceType = null };
                    AUpload.Global.Models.AssFD ToDelFD = new AUpload.Global.Models.AssFD() { PharmId = i };
                    UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Mongo.MongoDB, Mongo.Assortiment, Mongo.PHTFORSTFR);
                    rrr = Task<Exception>.Run(() => UsefullStuff.Common.Methods.MongoTools.Methods.Remove<AUpload.Global.Models.AssortmentItemData>.FilterAndRemove(ToDel));
                    var xxxx = UsefullStuff.Common.Methods.MongoTools.Methods.Remove<AUpload.Global.Models.AssortmentItemData>.FilterAndRemove(ToDel);
                    UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Mongo.MongoDB, Mongo.AssortimentSmall, Mongo.PHTFORSTFR);
                    rrr = Task<Exception>.Run(() => UsefullStuff.Common.Methods.MongoTools.Methods.Remove<AUpload.Global.Models.AssortmentItemData>.FilterAndRemove(ToDel));
                    var xxxx_small = UsefullStuff.Common.Methods.MongoTools.Methods.Remove<AUpload.Global.Models.AssFD>.FilterAndRemove(ToDelFD);
                    #endregion
                    PharmTaxiAdm.Models.MongoDS.Pharms pms = new Models.MongoDS.Pharms();
                    pms.PharmId = i;
                    Task.Run(() => PharmTaxiAdm.Methods.MongoDS.Pharms_Get.GetAndActivate(pms));
                }
            }
            //var t = rrr.Result;
            // return 
            AUpload.Config.MongoBase = Mongo.PHTFORSTFR;
            var t= AUpload.Global.Methods.LoadAssortiment.Load(DtSet, AvIds, ForceLoad).Result;
            return t;
        }
        
    }

}
