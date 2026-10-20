using PharmTaxiApp.Models;
using System;

namespace PharmTaxiApp.Methods
{
    public class Auth
    {
        /// <summary>
        /// Проверка права доступа
        /// </summary>
        /// <param name="key"></param>
        /// <param name="authkey"></param>
        /// <param name="service"></param>
        /// <returns></returns>
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
