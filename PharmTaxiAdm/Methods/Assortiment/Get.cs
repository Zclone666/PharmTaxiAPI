using System;
using System.Collections.Generic;
using System.Linq;
using PTConstants;
using System.Threading.Tasks;

namespace PharmTaxiAdm.Methods.Assortiment
{
    public static class Get
    {
        public static UMKBUtils.BigIdCryptography Crypto = new UMKBUtils.BigIdCryptography(PharmTaxiAdm.Models.Crypto.Password);
        public static AUpload.Global.Models.Response Assortiment(List<AUpload.Global.Models.AssortmentItemData> Lass, string authkey, string key=null)
        {
            Models.User.AlertUserID GetId;
            if (String.IsNullOrEmpty(key))GetId = Methods.User.UserMethods.GetUserIdByAuthkey(authkey);
            else GetId = Methods.User.UserMethods.GetUserIdByAuthkey(authkey, "7", key);
            if (GetId.alert.level == "success")
            {
                HashSet<Models.Drugstore.DrugstoreModel> drugstrs = Methods.Drugstore.DrugstoreMethods.GetDrugstoresByUserId(GetId.userid.ToString());
                List<uint> AvailIds = drugstrs.Distinct().Select(o => (uint.Parse(o.LongId.Value.ToString().Substring(0, o.LongId.Value.ToString().Length-4)))).ToList();
                AUpload.Config.MongoBase = Mongo.PHTFORSTFR;
                if (Lass is null)
                {
                    Lass = new List<AUpload.Global.Models.AssortmentItemData>();
                    foreach (var i in AvailIds) Lass.Add(new AUpload.Global.Models.AssortmentItemData() { PharmId = i });
                }
                var t = AUpload.Global.Methods.GetAssortiment.Get2(Lass, AvailIds).Result;
                //AUpload.Config.MongoBase = Constants.Consts.PHTFORSTFR;
                //t = AUpload.Global.Methods.GetAssortiment.Get2(Lass, AvailIds).Result;

                return t;
            }
            else
            {
                return new AUpload.Global.Models.Response() { code = GetId.alert.code, level = GetId.alert.level, message = GetId.alert.message, title = GetId.alert.title, sticky = GetId.alert.sticky, data = null };
            }
        }
    }
}
