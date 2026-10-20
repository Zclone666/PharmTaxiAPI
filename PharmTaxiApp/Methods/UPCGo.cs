using PharmTaxiApp.Models;
using PTConstants;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PharmTaxiApp.Methods
{
    /// <summary>
    /// Класс для работы с америкосовскими штрих кодами
    /// </summary>
    public static class UPCGoMF
    {
        /// <summary>
        /// метод для получения инфы по американским штришкодам
        /// </summary>
        /// <param name="upc"></param>
        /// <returns></returns>
        public static UPCRes Get(string upc) 
        {
            UPCReq req = new UPCReq(upc);
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(req);
            UMKBRequests.WebUtils.Result res=UMKBRequests.WebUtils.Post(URLs.UPCReqAddress, data, "application/json");
            UPCRes ret = Newtonsoft.Json.JsonConvert.DeserializeObject<UPCRes>(res.Answer);
            return ret;
        }
    }
}