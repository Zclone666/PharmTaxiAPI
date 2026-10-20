using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UMKBUtils;
using PharmTaxiApp.Models;
using UMKBNeedStuff;
using PTConstants;
using EMC.ECPIntegration.Models;
using static ICSharpCode.SharpZipLib.Zip.ExtendedUnixData;

namespace PharmTaxiApp.Methods
{
    /// <summary>
    /// Methods for emulate ecp (proxy-methods
    /// </summary>
    public static class ECP
    {
        //public static UMKBUtils.BigIdCryptography Crypto = new UMKBUtils.BigIdCryptography(Constants.Consts.Password);
        public static string UnitsPrimary()//(PharmTaxiApp.Models.Tools.UnitsPharm args)
        {
            try
            {
                Models.Tools.UnitsPharm args = new Models.Tools.UnitsPharm();
                //  if (args.authkey == "" || args.authkey is null)
                //  {
                args.key = PTConstants.ECP.Key;
                args.authkey = null;
                // }
                UMKBRequests.WebUtils.Result res = UMKBRequests.WebUtils.Post(URLs.UnitsPrimary, Newtonsoft.Json.JsonConvert.SerializeObject(args), "application/json");

                return res.Answer;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public static string UnitsPharm(PharmTaxiApp.Models.Tools.UnitsPharm args)
        {
          //  if (args.authkey == "" || args.authkey is null)
           // {
                args.key = PTConstants.ECP.Key;
            args.authkey = null;
          //  }
            UMKBRequests.WebUtils.Result res = UMKBRequests.WebUtils.Post(URLs.UnitsPharm, Newtonsoft.Json.JsonConvert.SerializeObject(args), "application/json");

            return res.Answer;
        }

        public static string Schedule(PharmTaxiApp.Models.Tools.SecurityArgs args)
        {
            //if (args.authkey == "" || args.authkey is null)
            //{
                args.key = PTConstants.ECP.Key;
            args.authkey = null;
            //}
            UMKBRequests.WebUtils.Result res = UMKBRequests.WebUtils.Post(URLs.Schedule, Newtonsoft.Json.JsonConvert.SerializeObject(args), "application/json");
            Models.ECP.AnswerSchedule AnsOrig = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.ECP.AnswerSchedule>(res.Answer);
            Models.ECP.AnswerSchedule AnsOldFormat = new Models.ECP.AnswerSchedule();
            AnsOldFormat.alert = AnsOrig.alert;
            foreach (var i in AnsOrig.names)
            {
                AnsOldFormat.names.TryAdd(i.Value, i.Key);
            }
            foreach (var i in AnsOrig.freq)
            {
                AnsOldFormat.freq.TryAdd(AnsOrig.names[i.Key], i.Value);
            }
            foreach (var i in AnsOrig.duration)
            {
                AnsOldFormat.duration.TryAdd(AnsOrig.names[i.Key], i.Value);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(AnsOldFormat);
        }

        public static string UIText(PharmTaxiApp.Models.Tools.SecurityArgs args)
        {
            //if (args.authkey == "" || args.authkey is null)
            //{
                args.key = PTConstants.ECP.Key;
                args.authkey = null;
            //}
            
            string locale;
            if (args == null || string.IsNullOrEmpty(args.active_language)) { locale = "ru"; } else { locale = args.active_language; }
            try { var r= (JsonConvert.SerializeObject(Get(Crypto.Password, locale)), "application/json; charset=utf-8"); }
            catch (Exception ex)
            {
                var r = (JsonConvert.SerializeObject(new UMKBRequests.Alert { level = "error", message = ex.Message, title = "Ошибка сервера" }), "application/json; charset=utf-8");
            }
            UMKBRequests.WebUtils.Result res = UMKBRequests.WebUtils.Post(URLs.UIText, Newtonsoft.Json.JsonConvert.SerializeObject(args), "application/json");
            Models.ECP.AnswerUIText AnsOrig = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.ECP.AnswerUIText>(res.Answer);
            Models.ECP.AnswerUIText AnsOldFormat = new Models.ECP.AnswerUIText();
            AnsOldFormat.alert = AnsOrig.alert;
            AnsOldFormat.button = AnsOrig.button;
            AnsOldFormat.caution=AnsOrig.caution;
            AnsOldFormat.ecptext = AnsOrig.ecptext;
            AnsOldFormat.suptext = AnsOrig.suptext;
            foreach (var i in AnsOrig.duration)
            {
                AnsOldFormat.duration.TryAdd(i.Value, i.Key);
            }
            foreach (var i in AnsOrig.freq)
            {
                AnsOldFormat.freq.TryAdd(i.Value, i.Key);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(AnsOldFormat);

        }

        public static AnswerUIText Get(string password, string locale)
        {
            AnswerUIText answer = new AnswerUIText();
            try
            {
                using (BigIdCryptography service = new BigIdCryptography(password))
                {
                    UMKBRequests.GetSemantic getSemantic = new UMKBRequests.GetSemantic();
                    var result = getSemantic.Get(new UMKBRequests.Models.API.Semantic.RequestSemantic()
                    {
                        binarization = 0,
                        deep = 1,
                        key = UMKB.Graph,
                        level = new List<ushort>() { 1051 },
                        lib = new List<ushort>() { 26, 71 },
                        libid = new List<string>() { PTConstants.ECP.Buttons, PTConstants.ECP.SupportText, PTConstants.ECP.Freq, PTConstants.ECP.Duration, PTConstants.ECP.Caution },
                        route = 0
                    });
                    if (result.graph == null || result.alerts.code != 200) { answer.alert = UMKBUtils.Converter.ToUMKBAlert(result.alerts); return answer; }
                    foreach (var item in result.graph)
                    {
                        if (PTConstants.ECP.Exclusion.Contains(item.idb) || !result.names.ContainsKey(item.idb)) { continue; }
                        if (item.type == 0)
                        {
                            Dictionary<string, string> dic = null;
                            switch (item.ida.ToString())
                            {
                                case PTConstants.ECP.Buttons: dic = answer.button; break;
                                case PTConstants.ECP.SupportText: dic = answer.suptext; break;
                                case PTConstants.ECP.Freq: dic = answer.freq; break;
                                case PTConstants.ECP.Duration: dic = answer.duration; break;
                                case PTConstants.ECP.Caution: dic = answer.caution; break;
                                default: continue;
                            }
                            dic.Add(service.Encrypt(item.idb), result.names[item.idb]);
                        }
                        else
                        {
                            if (item.id == PTConstants.ECP.Cautionuint) { answer.caution.Add(service.Encrypt(item.id), result.names[item.id]); }
                        }
                    }
                    answer.alert = new UMKBRequests.Alert() { level = "success", code = "200" };
                }
            }
            catch (Exception ex) { answer.alert = new UMKBRequests.Alert() { level = "error", code = "500", message = ex.Message }; }

            return answer;
        }

        public static string buttonTexts(PharmTaxiApp.Models.Tools.SecurityArgs args)
        {
            args.authkey = null;
            if (args.authkey == "" || args.authkey is null)
            {
                args.key = PTConstants.ECP.Key;

            }
            UMKBRequests.WebUtils.Result res = UMKBRequests.WebUtils.Post(URLs.ButtonTexts, Newtonsoft.Json.JsonConvert.SerializeObject(args), "application/json");

            return res.Answer;
        }

        public static string elements(PharmTaxiApp.Models.Tools.SecurityArgs args)
        {
            args.authkey = null;
            if (args.authkey == "" || args.authkey is null)
            {
                args.key = PTConstants.ECP.Key;

            }
            UMKBRequests.WebUtils.Result res = UMKBRequests.WebUtils.Post(URLs.Elements, Newtonsoft.Json.JsonConvert.SerializeObject(args), "application/json");

            return res.Answer;
        }

        public static string Actual(PharmTaxiApp.Models.GP2 fargs)
        {
            GetPatient args = new GetPatient();
            args.authkey = null;
            bool multiply = false;
            uint cnt = 0;
            List<string> MultPatId = new List<string>();
            UMKBRequests.WebUtils.Result res;
            GetInfoPreparations rsp = new GetInfoPreparations(); ;
            if (args.authkey == "" || args.authkey is null)
            {
                args.key = PTConstants.ECP.Key;

            }
            if (String.IsNullOrEmpty(args.id))
            {
                var patientResult = EMC.Singleton.SearchPatient(new SearchPatientArgs() { Email= fargs.email },"ru").Result;
                
                if(!(patientResult.Alert is null)) 
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(rsp);
                }

                var t = patientResult.Patient;

                var y = EMCLib.EMC.PatReader.CheckPatientsEmail(fargs.email);
                if (t is null) return Newtonsoft.Json.JsonConvert.SerializeObject(new Alert() { code = "400", level = "error", message = "No such patient!", title = "Check email" });
                if (y.Count == 0) args.id = t.id;
                else if (y.Count == 1) args.id = y[0].id;
                else if (y.Count > 1)
                {
                    multiply = true;
                    foreach (var smth in y) MultPatId.Add(smth.id + "0119");
                    MultPatId.Add(t.id);
                }
            }
            if (multiply)
            {
                foreach (var vl in MultPatId)
                {
                    args.id = args.id = Crypto.Сryptographer.EncryptUInt64(vl);

                    res = UMKBRequests.WebUtils.Post(URLs.Actual, Newtonsoft.Json.JsonConvert.SerializeObject(args), "application/json");
                    GetInfoPreparations mlt_rsp = Newtonsoft.Json.JsonConvert.DeserializeObject<GetInfoPreparations>(res.Answer);
                    if (mlt_rsp.alert.code == "200" && cnt == 0)
                    {
                        rsp = mlt_rsp;
                        cnt++;
                    }
                    if (mlt_rsp.alert.code == "200" && !(mlt_rsp.drugs is null) && mlt_rsp.drugs.Count > 0 && cnt > 1)
                    {
                        foreach (var yo in mlt_rsp.drugs) rsp.drugs.Add(yo);
                        cnt++;
                    }
                }
                cnt = 0;
            }
            else
            {
                res = UMKBRequests.WebUtils.Post(URLs.Actual, Newtonsoft.Json.JsonConvert.SerializeObject(args), "application/json");
                rsp = Newtonsoft.Json.JsonConvert.DeserializeObject<GetInfoPreparations>(res.Answer);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(rsp);
        }
        public static string Recipes(PharmTaxiApp.Models.RecipeArgs args)
        {
            args.authkey = null;
            bool multiply = false;
            uint cnt = 0;
            List<string> MultPatId = new List<string>();
            UMKBRequests.WebUtils.Result res;
            RecipesResponse rsp = new RecipesResponse(); ;
            if (args.authkey == "" || args.authkey is null)
            {
                args.key = PTConstants.ECP.Key;

            }
            try
            {
                if (String.IsNullOrEmpty(args.recipe))
                {
                    if (String.IsNullOrEmpty(args.patient_id))
                    {
                        var patientResult = EMC.Singleton.SearchPatient(new SearchPatientArgs() { Email = args.email }, "ru").Result;

                        if (!(patientResult.Alert is null))
                        {
                            return Newtonsoft.Json.JsonConvert.SerializeObject(rsp);
                        }

                        var t = patientResult.Patient;

                        var y = EMCLib.EMC.PatReader.CheckPatientsEmail(args.email);
                        if (t is null) return Newtonsoft.Json.JsonConvert.SerializeObject(new Alert() { code = "400", level = "error", message = "No such patient!", title = "Check email" });
                        if (y.Count == 0) args.patient_id = t.id;
                        else if (y.Count == 1) args.patient_id = (y[0].id.Contains("0119"))?y[0].id:y[0].id + "0119";
                        else if (y.Count > 1)
                        {
                            multiply = true;
                            foreach (var smth in y) MultPatId.Add(((smth.id.Contains("0119")) ? smth.id : smth.id + "0119"));
                            MultPatId.Add(t.id);
                        }
                    }
                    if (multiply)
                    {
                        foreach (var vl in MultPatId)
                        {
                            args.patient_id = vl;
                            // string sr = Newtonsoft.Json.JsonConvert.SerializeObject(args);
                            res = UMKBRequests.WebUtils.Post(URLs.Recipes, Newtonsoft.Json.JsonConvert.SerializeObject(args), "application/json");
                            RecipesResponse mlt_rsp = Newtonsoft.Json.JsonConvert.DeserializeObject<RecipesResponse>(res.Answer);
                            if (mlt_rsp.alert.code == "200" && cnt == 0)
                            {
                                rsp = mlt_rsp;
                                cnt++;
                            }
                            if (mlt_rsp.alert.code == "200" && !(mlt_rsp.recipes is null) && mlt_rsp.recipes.Count > 0 && cnt > 1)
                            {
                                foreach (var yo in mlt_rsp.recipes) rsp.recipes.Add(yo.Key, yo.Value);
                                cnt++;
                            }
                        }
                        cnt = 0;
                    }
                    else
                    {
                        res = UMKBRequests.WebUtils.Post(URLs.Recipes, Newtonsoft.Json.JsonConvert.SerializeObject(args), "application/json");
                        rsp = Newtonsoft.Json.JsonConvert.DeserializeObject<RecipesResponse>(res.Answer);
                    }
                }
                else
                {
                    rsp.recipes = new Dictionary<string, long>();
                    rsp.recipes.Add(args.recipe, 0);
                }
                AllRecipes all = new AllRecipes();
                List<ModelRecipeLS> all_recipes = new List<ModelRecipeLS>();
                if (!(rsp.recipes is null) && rsp.recipes.Count > 0)
                {

                    foreach (var i in rsp.recipes)
                    {

                        args.recipe = i.Key;
                        res = UMKBRequests.WebUtils.Post(URLs.RecipesCore, Newtonsoft.Json.JsonConvert.SerializeObject(args), "application/json");
                        if (res.Answer.Contains("Ошибка получения рецепта"))
                        {
                            RecipesResponse RecResp = Newtonsoft.Json.JsonConvert.DeserializeObject<RecipesResponse>(res.Answer);
                            return Newtonsoft.Json.JsonConvert.SerializeObject(RecResp.alert);
                        }
                        ModelRecipeLS recpt = Newtonsoft.Json.JsonConvert.DeserializeObject<ModelRecipeLS>(res.Answer);
                        for (int Mf = 0; Mf < recpt.liquids.Count; Mf++)
                        {
                            for (int Hn = 0; Hn < recpt.liquids[Mf].drugs.Count; Hn++)
                            {
                                if (!recpt.liquids[Mf].drugs[Hn].recipes.Contains(i.Key))
                                {
                                    try
                                    {
                                        recpt.names.Remove(recpt.liquids[Mf].drugs[Hn].id);
                                    }
                                    catch (Exception e) { }
                                    recpt.liquids[Mf].drugs.RemoveAt(Hn);
                                    Hn--;
                                }
                            }
                        }
                        if (recpt.alert.code == "200")
                        {
                            recpt.recipe_code = i.Key;
                            recpt.idForm = Crypto.Сryptographer.Encrypt(recpt.idForm);
                            recpt.NameForm = (recpt.idForm == "7bp//3zBarihBTTB5IbdXg==") ? "107-1/у" : (recpt.idForm == "2ApW9Ve9l7XXlXy5/59EUg==") ? "148-1/у-04(л)" : "148-1/у-88";
                            for (int ii = 0; ii < recpt.liquids.Count; ii++)
                            {
                                for (int iii = 0; iii < recpt.liquids[ii].drugs.Count; iii++)
                                {
                                    if (!String.IsNullOrEmpty(recpt.liquids[ii].drugs[iii].fpid))
                                    {
                                        recpt.liquids[ii].drugs[iii].fpid = Crypto.Сryptographer.Encrypt(recpt.liquids[ii].drugs[iii].fpid);
                                    }

                                    if (!String.IsNullOrEmpty(recpt.liquids[ii].drugs[iii].mnnid))
                                    {
                                        recpt.liquids[ii].drugs[iii].mnnid = Crypto.Сryptographer.Encrypt(recpt.liquids[ii].drugs[iii].mnnid);
                                    }

                                    if (!String.IsNullOrEmpty(recpt.liquids[ii].drugs[iii].id))
                                    {
                                        recpt.liquids[ii].drugs[iii].id = Crypto.Сryptographer.Encrypt(recpt.liquids[ii].drugs[iii].id);
                                    }

                                    if (!String.IsNullOrEmpty(recpt.liquids[ii].drugs[iii].recipenameid))
                                    {
                                        recpt.liquids[ii].drugs[iii].recipenameid = Crypto.Сryptographer.Encrypt(recpt.liquids[ii].drugs[iii].recipenameid);
                                    }

                                    if (!(recpt.liquids[ii].drugs[iii].dosage is null) && !String.IsNullOrEmpty(recpt.liquids[ii].drugs[iii].dosage.form))
                                    {
                                        recpt.liquids[ii].drugs[iii].dosage.form = Crypto.Сryptographer.Encrypt(recpt.liquids[ii].drugs[iii].dosage.form);
                                    }

                                    if (!(recpt.liquids[ii].drugs[iii].dosage is null) && !String.IsNullOrEmpty(recpt.liquids[ii].drugs[iii].dosage.method))
                                    {
                                        recpt.liquids[ii].drugs[iii].dosage.method = Crypto.Сryptographer.Encrypt(recpt.liquids[ii].drugs[iii].dosage.method);
                                    }

                                    if (!(recpt.liquids[ii].drugs[iii].dosage.pack is null) && !(recpt.liquids[ii].drugs[iii].dosage.pack.dosage is null) 
                                                                        && !String.IsNullOrEmpty(recpt.liquids[ii].drugs[iii].dosage.pack.id))
                                    {
                                        recpt.liquids[ii].drugs[iii].dosage.pack.id = Crypto.Сryptographer.Encrypt(recpt.liquids[ii].drugs[iii].dosage.pack.id);
                                    }
                                }
                                if (!(recpt.liquids[ii].liquid is null)) recpt.liquids[ii].liquid.fpid = Crypto.Сryptographer.Encrypt(recpt.liquids[ii].liquid.fpid);
                                if (!(recpt.liquids[ii].liquid is null)) recpt.liquids[ii].liquid.mnnid = Crypto.Сryptographer.Encrypt(recpt.liquids[ii].liquid.mnnid);
                                UMKBRequests.Models.API.Codes.CodesArgs req = new UMKBRequests.Models.API.Codes.CodesArgs();
                                req.permit = new UMKBRequests.Models.API.Codes.Permit() { key = UMKB.GetNamesListDB };
                                req.longids = new List<string>() { recpt.idDoc, recpt.idClinic };
                                //req.libids = new List<UMKBRequests.Models.API.Codes.Libids>() { new UMKBRequests.Models.API.Codes.Libids() { lib = 120, id = 283 } };
                                string rq = Newtonsoft.Json.JsonConvert.SerializeObject(req);
                                var xx = UMKBRequests.GetNameListDB.Get(req);
                                string idDoc = recpt.idDoc.Substring(0, recpt.idDoc.Length - 4);
                                string idClinic = recpt.idClinic.Substring(0, recpt.idClinic.Length - 4);
                                recpt.idDoc = xx.result.GetValueOrDefault((1200000000 + uint.Parse(idDoc)).ToString());
                                recpt.idClinic = xx.result.GetValueOrDefault((1210000000 + uint.Parse(idClinic)).ToString());
                                if (!(recpt.dText is null) && recpt.dText.TryGetValue("AdultChild", out List<string> AoC) && AoC.Count > 0) recpt.IsAdult = (AoC[0].Contains("взрослый")) ? true : false;
                                else recpt.IsAdult = null;
                            }
                            all_recipes.Add(recpt);
                        }
                    }
                }
                all.recipes = all_recipes;
                all.alert = (all_recipes is null) ? new Alert() { code = "200", message = $"{MultPatId.Count.ToString()} {rsp}", level = "success", title = "success", sticky = false } : all_recipes[0].alert;
                return Newtonsoft.Json.JsonConvert.SerializeObject(all);
            }catch(Exception e)
            {
                return e.Message;
            }
        }

    }
}
