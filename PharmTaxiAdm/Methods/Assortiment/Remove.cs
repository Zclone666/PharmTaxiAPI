using System;
using System.Collections.Generic;
using System.Linq;
using PTConstants;
using System.Threading.Tasks;

namespace PharmTaxiAdm.Methods.Assortiment
{
    public static class Remove
    {
        public static UMKBUtils.BigIdCryptography Crypto = new UMKBUtils.BigIdCryptography(PharmTaxiAdm.Models.Crypto.Password);
        public static AUpload.Global.Models.Response Assortiment(List<AUpload.Global.Models.AssortmentItemData> Lass, string key)
        {
            Models.User.AlertUserID GetId = Methods.User.UserMethods.GetUserIdByAuthkey(key);
            if (GetId.alert.level == "success")
            {
                HashSet<Models.Drugstore.DrugstoreModel> drugstrs = Methods.Drugstore.DrugstoreMethods.GetDrugstoresByUserId(GetId.userid.ToString());
                //AUpload.Config.Setup(Constants.Consts.SQLConnectionString, Constants.Keys.AbdParseKey, Constants.Keys.TableGetKey, Constants.Keys.GraphKey, Constants.Keys.ConceptKey, Crypto, Constants.Keys.GraphListKey, Constants.Keys.ConceptListKey);
                List<uint> AvailIds = drugstrs.Distinct().Select(o => ((uint)o.PharmId)).ToList();
                //AUpload.Config.MongoDb = Constants.Consts.MongoDb;
                //AUpload.Config.ConfigMongoAddress(true);
                AUpload.Config.MongoBase = Mongo.PHT;
                var t = AUpload.Global.Methods.RemoveAssortiment.Remove(Lass, AvailIds).Result;
                AUpload.Config.MongoBase = Mongo.PHTFORSTFR;
                t = AUpload.Global.Methods.RemoveAssortiment.Remove(Lass, AvailIds).Result;
                if (t.level == "success")
                {
                    PharmTaxiAdm.Models.MongoDS.Pharms pms = new Models.MongoDS.Pharms();
                    foreach (var i in Lass)
                    {
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
