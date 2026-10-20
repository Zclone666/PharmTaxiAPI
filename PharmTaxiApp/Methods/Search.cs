using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using UMKBUtils;
using static PharmTaxiApp.Models.WebServer;
using PTConstants;

namespace PharmTaxiApp.Methods
{
    public class Search : ControllerBase 
    {
        /// <summary>
        /// чтобы можно было юзать унаследованные от абстрактно класса методы внутри статических фунций
        /// </summary>
     
        static Search sr = new Search();

        /// <summary>
        /// Побуквенный поиск ЛС (т.н. Сфинкс), работает с ключами от ЭКФ (и номером сервиса тож)
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IActionResult Drugs(UMKBRequests.Models.API.Semantic.RequestSearch args)
        {
            // args.authkey = null;
            //  if (args.authkey == "" || args.authkey is null)
            //{
                args.key = PTConstants.ECP.Key;
            //}

            //    UMKBRequests.WebUtils.Post("cs.socmedica.com/Search/Drugs", Newtonsoft.Json.JsonConvert.SerializeObject(args), UMKBRequests.WebUtils.WebContentType.JSON);
            if (args == null)// || args.key == null || !args.key.Equals("4vvqd9zcewdc2a", StringComparison.InvariantCulture))
            { return sr.Content(JsonConvert.SerializeObject(new PharmTaxiApp.Models.Answer() { alert = new UMKBRequests.Alert() { code = "401", level = "error", title = "Ошибка авторизации" } }), "application/json; charset=utf-8"); }
            try
            {
                if (!Methods.Auth.Check(args.key, args.authkey, PTConstants.ECP.Service)) { return sr.Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "401", level = "Unauthorized", title = "Ошибка авторизации" }), "application/json; charset=utf-8"); }
                UMKBRequests.GetSearch getSearch = new UMKBRequests.GetSearch();
                var items = getSearch.Get(new UMKBRequests.Models.API.Semantic.RequestSearch()
                {
                    key = UMKB.Sphinx,
                    security_key = UMKB.Sphinx,
                    authkey = UMKB.Sphinx,
                    text = args.text,
                    lib = "62,52",
                    limit = args.limit
                });
                if (items.alert.code != 200)
                {
                    return sr.Content(JsonConvert.SerializeObject(new PharmTaxiApp.Models.Answer()
                    {
                        alert = new UMKBRequests.Alert()
                        {
                            code = items.alert.code.ToString(),
                            level = items.alert.level,
                            message = $"UMKB: {items.alert.message}",
                            sticky = items.alert.sticky,
                            title = items.alert.title
                        }
                    }), "application/json; charset=utf-8");
                }
                PharmTaxiApp.Models.Answer answer = new PharmTaxiApp.Models.Answer();
                int count = items.concepts.Count;
                if (count > 30) { count = 30; }
                uint bigId = 0;
                using (BigIdCryptography service = new BigIdCryptography("DESKTOP-0JFBQ9R"))
                {
                    for (int i = 0; i < count; i++)
                    {
                        bigId = (uint)items.concepts[i].lib * 10000000 + items.concepts[i].id;
                        PharmTaxiApp.Models.Concept concept = new PharmTaxiApp.Models.Concept()
                        {
                            id = service.Encrypt(bigId),
                            level = items.concepts[i].idlevel,
                            name = Methods.Utils.ClearingNames(items.concepts[i].name)//SMCommonUtils.StringServices.ClearingNames(items.concepts[i].name)
                        };
                        switch (items.concepts[i].lib)
                        {
                            case 62: concept.type = Libs.LibsString.TN; break;
                            case 52: concept.type = Libs.LibsString.DV; break;
                        }
                        answer.concepts.Add(concept);
                    }
                }
                //conceptsAnswer.concepts.OrderBy(x=>x.name);
                return sr.Content(JsonConvert.SerializeObject(answer), "application/json; charset=utf-8");
            }
            catch (Exception ex) { return sr.Content(JsonConvert.SerializeObject(new PharmTaxiApp.Models.Answer() { alert = new UMKBRequests.Alert() { level = "error", message = ex.Message, title = "Ошибка сервера" } }), "application/json; charset=utf-8"); }
        }

        public static string Diseases(UMKBRequests.Models.API.Semantic.RequestSearch args)
        {

            //      args = Newtonsoft.Json.JsonConvert.DeserializeObject<PharmTaxiApp.Models.Request>(data);
            // if (args.authkey == "" || args.authkey is null)
            //  {
            args.key = PTConstants.ECP.Key;
            args.authkey = "";
            
            // }
            UMKBRequests.WebUtils.Result res = UMKBRequests.WebUtils.Post(URLs.Diseases, Newtonsoft.Json.JsonConvert.SerializeObject(args), "application/json");

            return res.Answer;
        }

        public static string Allergy(UMKBRequests.Models.API.Semantic.RequestSearch args)
        {

            //      args = Newtonsoft.Json.JsonConvert.DeserializeObject<PharmTaxiApp.Models.Request>(data);
            // if (args.authkey == "" || args.authkey is null)
            //  {
            args.key = PTConstants.ECP.Key;
            args.authkey = "";

            // }
            UMKBRequests.WebUtils.Result res = UMKBRequests.WebUtils.Post(URLs.Allergy, Newtonsoft.Json.JsonConvert.SerializeObject(args), "application/json");

            return res.Answer;
        }

        
    }
}
