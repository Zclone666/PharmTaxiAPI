using PharmTaxiAdm.Models.Drugstore;
using PharmTaxiAdm.Models.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiAdm.Methods.User
{
    public partial class DrugUserCrR
    {
        public static AuthUserIdPharmId RegUserNetCr(PharmTaxiAdm.Models.User.DrugUserCrR args)
        {
            AuthUserIdPharmId res = new AuthUserIdPharmId();
            IUserRegistrationReq idata = args;
            UserRegistrationReq data = Newtonsoft.Json.JsonConvert.DeserializeObject<UserRegistrationReq>(Newtonsoft.Json.JsonConvert.SerializeObject(idata));
            var t = PharmTaxiAdm.Methods.User.UserMethods.Register(data);
            if (t.Answer.Contains("200"))
            {
                IBaseDrugNet ibdn = args.drugNet;
                DrugNetModel drgnet = Newtonsoft.Json.JsonConvert.DeserializeObject<DrugNetModel>(Newtonsoft.Json.JsonConvert.SerializeObject(ibdn));
                Models.MongoDS.PharmNets PhmId=PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.CreateDrugNet(drgnet);
                res.pharm_net_id = PhmId.PharmId;
                AuthKeyByLicKey Eml = new AuthKeyByLicKey() { email = args.email };
                if (PharmTaxiAdm.Methods.RegWorkaround.AutoConfirmEmailPharmNet(Eml))
                {
                    AuthKUserId uid=Newtonsoft.Json.JsonConvert.DeserializeObject<AuthKUserId>(PharmTaxiAdm.Methods.User.UserMethods.Authorize2(args.email, args.password, "7"));
                    res.alert = uid.alert;
                    res.role = uid.role;
                    res.userid = uid.userid;
                    Methods.User.UserMethods.AttachDrugstoreToUser(uid.userid, uint.Parse(PhmId.PharmId.ToString() + "0095"));
                }
                return res;
            }
            else
            {
                return new AuthUserIdPharmId() { alert = new Alert() { code = "413", message = t.Answer } };
            }
        }
    }
}
