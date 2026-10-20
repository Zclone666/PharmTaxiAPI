using PharmTaxiApp.Models;
using SMBarcode.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using PTConstants;

namespace PharmTaxiApp.Methods
{
    public static class Drugs
    {
        /// <summary>
        /// Вытаскивание дозировок для препарата. Прокси к Святозару
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Dosages(PharmTaxiApp.Models.Request args)
        {

            //      args = Newtonsoft.Json.JsonConvert.DeserializeObject<PharmTaxiApp.Models.Request>(data);
            //  if (args.authkey == "" || args.authkey is null)
            //  {
            args.key = PTConstants.ECP.Key;
                args.authkey = "";
           // }
            UMKBRequests.WebUtils.Result res = UMKBRequests.WebUtils.Post(URLs.Dosages, Newtonsoft.Json.JsonConvert.SerializeObject(args), "application/json");

            return res.Answer;
        }

        /// <summary>
        /// Вытаскивание дозировок для препарата. Прокси к Святозару
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FPDosages(PharmTaxiApp.Models.Request args)
        {

            //      args = Newtonsoft.Json.JsonConvert.DeserializeObject<PharmTaxiApp.Models.Request>(data);
            //  if (args.authkey == "" || args.authkey is null)
            // {
            args.key = PTConstants.ECP.Key;
                args.authkey = "";
           // }
            UMKBRequests.WebUtils.Result res = UMKBRequests.WebUtils.Post(URLs.FPDosages, Newtonsoft.Json.JsonConvert.SerializeObject(args), "application/json");

            return res.Answer;
        }

        /// <summary>
        /// Вытаскивание дозировок для препарата. Прокси к Святозару
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string CreateDosage(PharmTaxiApp.Models.Request args)
        {

            //      args = Newtonsoft.Json.JsonConvert.DeserializeObject<PharmTaxiApp.Models.Request>(data);
           // if (args.authkey == "" || args.authkey is null)
          //  {
                args.key = PTConstants.ECP.Key;
            args.authkey = "";
                args.service = null;
           // }
            UMKBRequests.WebUtils.Result res = UMKBRequests.WebUtils.Post(URLs.CreateDosage, Newtonsoft.Json.JsonConvert.SerializeObject(args), "application/json");

            return res.Answer;
        }

        /// <summary>
        /// Get drug info
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Info(PharmTaxiApp.Models.RequestDrug args)
        {

            //      args = Newtonsoft.Json.JsonConvert.DeserializeObject<PharmTaxiApp.Models.Request>(data);
           // if (args.authkey == "" || args.authkey is null)
            //{
                args.key = PTConstants.ECP.Key;
            args.authkey = "";
            // }
            UMKBRequests.WebUtils.Result res = UMKBRequests.WebUtils.Post(URLs.Info, Newtonsoft.Json.JsonConvert.SerializeObject(args), "application/json");

            return res.Answer;
        }

        /// <summary>
        /// Проверка лс в списке ЖНВЛП
        /// </summary>
        /// <returns></returns>
        public static bool MustHaveCheck(ref RequestMustHave rmh)
        {
            bool isZNVLP = false;
            UMKBRequests.Models.API.Semantic.RequestSemantic rs = new UMKBRequests.Models.API.Semantic.RequestSemantic();
            UMKBRequests.GetSemantic gs = new UMKBRequests.GetSemantic();

            rs.key = UMKB.Graph;
            rs.lib = new List<ushort>() { 84, 67, 62, 52 };
            rs.level = new List<ushort>() { 1101, 1600, 1608, 241, 240, 244 };
            rs.route = 1;
            rs.deep = 3;
            string id = "";
            id = rmh.dv_id??rmh.fp_id??rmh.tn_id;
            List<string> ids = new List<string>();
            ids.Add(id);
            rs.libid = ids;
            var answer = gs.Get(rs);
            try
            {
                if (answer.names.ContainsKey(840000012)) isZNVLP = true;
            }
            catch (Exception e) { }
            if (!String.IsNullOrEmpty(rmh.dv_id))
            {
                try
                {
                    rmh.name = answer.names[uint.Parse(rmh.dv_id)];                
                    uint fp = answer.graph.Where(x => x.idb.ToString().StartsWith("67") && SMBarcode.FPCache.FPPriceCache.ContainsKey(x.idb - 670000000)).Select(x => x.idb).FirstOrDefault()- 670000000;
                    rmh.price = SMBarcode.FPCache.FPPriceCache[fp];
                    fp += 670000000;
                    uint dv = uint.Parse(rmh.dv_id);
                    uint tn = answer.graph.Where(x => x.idb.ToString().StartsWith("62")).Select(x => x.idb).FirstOrDefault();
                    rmh.dv_name = answer.names.Where(x => x.Key == dv).Select(x => x.Value).FirstOrDefault();
                    if (String.IsNullOrEmpty(rmh.tn_id)) rmh.tn_id = tn.ToString();
                    if (String.IsNullOrEmpty(rmh.fp_id)) rmh.fp_id = fp.ToString();
                }
                catch(Exception e) { }
            }
            else if (!String.IsNullOrEmpty(rmh.tn_id))
            {
                try
                {
                    rmh.name = answer.names[uint.Parse(rmh.tn_id)];    
                    uint fp = answer.graph.Where(x => x.idb.ToString().StartsWith("67") && SMBarcode.FPCache.FPPriceCache.ContainsKey(x.idb - 670000000)).Select(x => x.idb).FirstOrDefault()- 670000000;
                    rmh.price = SMBarcode.FPCache.FPPriceCache[fp];
                    fp += 670000000;
                    uint dv = answer.graph.Where(x => x.idb.ToString().StartsWith("52")).Select(x => x.idb).FirstOrDefault();
                    rmh.dv_name = answer.names.Where(x => x.Key == dv).Select(x => x.Value).FirstOrDefault();
                    if (String.IsNullOrEmpty(rmh.dv_id)) rmh.dv_id = dv.ToString();
                    if (String.IsNullOrEmpty(rmh.fp_id)) rmh.fp_id = fp.ToString();
                }
                catch (Exception e) { }
            }
            else if (!String.IsNullOrEmpty(rmh.fp_id))
            {
                try
                {
                    uint fp = uint.Parse(rmh.fp_id) - 670000000;
                    rmh.name = SMBarcode.FPCache.FullFPNamesCache[fp];
                    fp = answer.graph.Where(x => x.idb.ToString().StartsWith("67") && SMBarcode.FPCache.FPPriceCache.ContainsKey(x.idb - 670000000)).Select(x => x.idb).FirstOrDefault() - 670000000;
                    rmh.price = SMBarcode.FPCache.FPPriceCache[fp];
                    fp += 670000000;
                    uint dv = answer.graph.Where(x => x.idb.ToString().StartsWith("52")).Select(x => x.idb).FirstOrDefault();
                    rmh.dv_name = answer.names.Where(x => x.Key == dv).Select(x => x.Value).FirstOrDefault();
                    uint tn = answer.graph.Where(x => x.idb.ToString().StartsWith("62")).Select(x => x.idb).FirstOrDefault();
                    if (String.IsNullOrEmpty(rmh.dv_id)) rmh.dv_id = dv.ToString();
                    if (String.IsNullOrEmpty(rmh.tn_id)) rmh.tn_id = tn.ToString();
                }
                catch (Exception e) { }
            }
            return isZNVLP;  
        }
    }
}
