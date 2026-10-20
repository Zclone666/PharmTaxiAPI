using PTConstants;
using SMReceipt.MongoConnect.ECP.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiApp.Methods
{
    public static class CheckWarnings
    {
        /// <summary>
        /// Warnings from ECP
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Warnings(object args)
        {
            GetInfoPreparations gip = Newtonsoft.Json.JsonConvert.DeserializeObject<GetInfoPreparations>(args.ToString());
            //      args = Newtonsoft.Json.JsonConvert.DeserializeObject<PharmTaxiApp.Models.Request>(data);
           // if (gip.authkey == "" || gip.authkey is null)
            //{
                gip.key = PTConstants.ECP.Key;

            //}
            UMKBRequests.WebUtils.Result res = UMKBRequests.WebUtils.Post(URLs.Warnings, Newtonsoft.Json.JsonConvert.SerializeObject(gip), "application/json");

            return res.Answer;
        }
    }
}
