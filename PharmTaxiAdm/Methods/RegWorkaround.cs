using PharmTaxiAdm.Methods.Drugstore;
using PharmTaxiAdm.Models.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiAdm.Methods
{
    public static class RegWorkaround
    {
        public static bool AutoConfirmEmailPharmNet(AuthKeyByLicKey data)
        {
            UMKBRequests.Alert ret = new UMKBRequests.Alert();
            uint CheckUsId = DrugstoreMethods.GetUserIdByEmail(data.email);
            //UMKBNeedStuff.Responses.ResponseAlertData<RDGenerateUserByAuthkey> UId = Newtonsoft.Json.JsonConvert.DeserializeObject<UMKBNeedStuff.Responses.ResponseAlertData<RDGenerateUserByAuthkey>>(Methods.User.UserMethods.GenerateUser(data.authkey).Answer);
            string Code = DrugstoreMethods.GetVerCode(CheckUsId);
            //string req = "https://service.socmedica.com:9030/confirm/" + Code + '/' + data.email + '/' + '5';
            //var xz = UMKBRequests.WebUtils.Get(req);
            string posttest = "{\"email\":\"" + data.email + "\",\"code\":\"" + Code + "\",\"service\":\"7\",\"key\":\"7e649ecb733d2af1\"}";
            var xz = UMKBRequests.WebUtils.Post("https://cs.socmedica.com/api/registration/confirmemail", posttest, "application/json");
            if (xz.Answer.Contains("200")) return true;
            else return false;
        }
    }
}
