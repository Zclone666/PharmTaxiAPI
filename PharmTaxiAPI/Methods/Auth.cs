using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Methods
{
    public class Auth
    {
        public static bool Check(string key, string authkey, string service)
        {
            UMKBRequests.Auth auth = new UMKBRequests.Auth();

            //var ttt = Newtonsoft.Json.JsonConvert.SerializeObject(new UMKBRequests.Models.API.Semantic.RequestAuth()
            //{
            //    key = key,
            //    authkey = authkey,
            //    gateway = null,
            //    login = null,
            //    application = service
            //});

            UMKBRequests.Models.API.Semantic.Alert userauth = auth.Get(new UMKBRequests.Models.API.Semantic.RequestAuth()
            {
                key = key,
                authkey = authkey,
                gateway = null,
                login = null,
                application = service
            });

            if (userauth != null && userauth.level != null && userauth.level.Equals("success", StringComparison.InvariantCultureIgnoreCase)) { return true; }
            return false;
        }
    }
}
