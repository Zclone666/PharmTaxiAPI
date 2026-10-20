using System;
using System.Collections.Generic;
using System.Linq;
using PTConstants;
using System.Threading.Tasks;

namespace PharmTaxiAdm.Methods.Assortiment
{
    public static class Change
    {
        public static UMKBUtils.BigIdCryptography Crypto = new UMKBUtils.BigIdCryptography(PharmTaxiAdm.Models.Crypto.Password);
        public static AUpload.Global.Models.Response Assortiment(List<AUpload.Global.Models.AssortmentItemData> Lass, string authkey, string key=null)
        {
            Models.User.AlertUserID GetId;// = Methods.User.UserMethods.GetUserIdByAuthkey(key);
            if (String.IsNullOrEmpty(key)) GetId = Methods.User.UserMethods.GetUserIdByAuthkey(authkey);
            else GetId = Methods.User.UserMethods.GetUserIdByAuthkey(authkey, "7", key);
            if (GetId.alert.level == "success")
            {
                HashSet<Models.Drugstore.DrugstoreModel> drugstrs = Methods.Drugstore.DrugstoreMethods.GetDrugstoresByUserId(GetId.userid.ToString());
              //  AUpload.Config.Setup(Constants.Consts.SQLConnectionString, Constants.Keys.AbdParseKey, Constants.Keys.TableGetKey, Constants.Keys.GraphKey, Constants.Keys.ConceptKey, Crypto, Constants.Keys.GraphListKey, Constants.Keys.ConceptListKey);
                List<uint> AvailIds = drugstrs.Distinct().Select(o => ((uint)o.PharmId)).ToList();
                //  AUpload.Config.MongoDb = Constants.Consts.MongoDb;
                // AUpload.Config.ConfigMongoAddress(true);
               /// AUpload.Config.MongoBase = Constants.Consts.PHT;
                //var t = AUpload.Global.Methods.ChangeAssortiment.Change(Lass, AvailIds).Result;
                AUpload.Config.MongoBase = Mongo.PHTFORSTFR;
                UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Mongo.MongoDB, Mongo.Assortiment, Mongo.PHTFORSTFR);
                UsefullStuff.Common.Methods.MongoTools.Methods.Write<AUpload.Global.Models.AssortmentItemData> Wrt = new UsefullStuff.Common.Methods.MongoTools.Methods.Write<AUpload.Global.Models.AssortmentItemData>();
                UsefullStuff.Common.Methods.MongoTools.Methods.Write<AUpload.Global.Models.AssFD> WrtSmall = new UsefullStuff.Common.Methods.MongoTools.Methods.Write<AUpload.Global.Models.AssFD>();
                var t=AUpload.Global.Methods.ChangeAssortiment.Change(Lass, AvailIds).Result;
                //AUpload.Global.Models.Response t = new AUpload.Global.Models.Response();
                //t.level = "success";
                if (t.level == "success")
                {
                    PharmTaxiAdm.Models.MongoDS.Pharms pms = new Models.MongoDS.Pharms();
                    foreach (var i in Lass)
                    {
                        AUpload.Global.Models.AssFD assFD = new AUpload.Global.Models.AssFD();
                        AUpload.Global.Models.AssFD TmpassFD = new AUpload.Global.Models.AssFD();
                        assFD = UsefullStuff.Common.Methods.Converter<AUpload.Global.Models.AssFD, AUpload.Global.Models.AssortmentItemData>.Convert(assFD, i);
                        AUpload.Global.Models.AssortmentItemData tmp = new AUpload.Global.Models.AssortmentItemData();
                        tmp = UsefullStuff.Common.Methods.Converter<AUpload.Global.Models.AssortmentItemData, AUpload.Global.Models.AssortmentItemData>.Convert(tmp, i);
                        tmp.SellingPrice = null;
                        tmp.Quantity = null;
                        Exception e = Wrt.UpSert_Compare(tmp, i);
                        t.message = (e is null)?"succecc":e.StackTrace;
                        TmpassFD = assFD;
                        TmpassFD.Quantity = 0;
                        TmpassFD.SellingPrice = 0;
                        e = WrtSmall.UpSert_Compare(assFD, TmpassFD);
                        t.title = (e is null)?"success":e.Message;
                        if (t.title == t.message) t.code = "200";
                        pms.PharmId = (uint)i.PharmId;
                        Task.Run(() => PharmTaxiAdm.Methods.MongoDS.Pharms_Get.GetAndActivate(pms));
                    }
                }
                return t;
            }
            else
            {
                return new AUpload.Global.Models.Response() { code = GetId.alert.code, level = GetId.alert.level, message = GetId.alert.message, title = GetId.alert.title, sticky = GetId.alert.sticky, data = null };
            }
        }
    }
}
