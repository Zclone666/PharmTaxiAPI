using PTConstants;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PharmTaxiAdm.Models;
using PharmTaxiAPI.Methods;
using PharmTaxiAPI.Methods.Recipe;
using PharmTaxiAPI.Methods.Recipe.Model;
using PharmTaxiAPI.Models;
using PharmTaxiApp.Models.Barcodes;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UMKBCoreAPI.Models.Api.Responses;

namespace PharmTaxiAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors(Cors.AllowAll.Name)]

    public class RecipesController : ControllerBase
    {

        Logs lg = new Logs();
        private const string CachePreffix = "BuisnessProcess_";
        private BarcodeProcessor _barcodeProcessor = new BarcodeProcessor();

        /// <summary>
        /// Получение JSONа рецепта
        /// </summary>
        /// <param name="argd"></param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///		POST /GetRecipeJson
        ///		{
        ///			"receiptsCodes": [
        ///				"NCW"
        ///			],
        ///			"receiptCode": "NCW"
        ///		}
        /// </remarks>
        [HttpPost("/GetRecipeJson")]
        public async Task<IActionResult> GetRecipeJson(GetRecipeJSON argd)
        {

#if LOG_ALL_REQUESTS
	lg.Log("GetrecJson()", "RUNNED");
#endif
            try
            {
                SMReceipt.Global.Consts.URLs.Reciepes_Connect = Mongo.ReciepesConString;
                if (!String.IsNullOrEmpty(argd.ReceiptCode))
                {
                    argd.ReceiptCode = argd.ReceiptCode.ToUpper();
                    argd.ReceiptCode = SMBarcode.Methods.Common.Transliteration.Front(argd.ReceiptCode, SMBarcode.Methods.Common.TransliterationType.sim);
                }

                //var authCheckReturn = Global.CheckAuth(argd.authkey);
                //if (authCheckReturn.code is "400")
                //	return Content(JsonConvert.SerializeObject(authCheckReturn));
                SMReceipt.Global.Models.SearchReceipt gc = new SMReceipt.Global.Models.SearchReceipt() { birthdate = argd.birthdate, DateOfReceipt = argd.DateOfReceipt, FIO = argd.FIO, PatientId = argd.PatientId, PharmId = argd.PharmId, ReceiptCode = argd.ReceiptCode, ReceiptsCodes = argd.ReceiptsCodes, Region = argd.Region, SNILS = argd.SNILS };
                List<GetOrdRec> ret = PharmTaxiAdm.Methods.Receipt.GetNew(gc);
                SMReceipt.Global.Consts.URLs.Reciepes_Connect = Mongo.MongoDB;
                return Content(JsonConvert.SerializeObject(ret));
            }
            catch (Exception e)
            {
                return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "500", title = "Ошибка получения рецепта", message = (!String.IsNullOrEmpty(argd.ReceiptCode)) ? $"Рецепт с номером {argd.ReceiptCode} не найден" : e.Message, level = "success", sticky = true }));
            }
        }

        /// <summary>
        /// Контроллер для отпуска рецептов (интеграция)
        /// </summary>
        /// <param name="argd"></param>
        /// <returns></returns>
        [HttpPost("/RecipeRelease_old")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> RecipeRelease_old(RecipReleaseReq argd)
        {

#if LOG_ALL_REQUESTS
	lg.Log("GetrecJson()", "RUNNED");
#endif


            //var authCheckReturn = Global.CheckAuth(argd.authkey);
            //if (authCheckReturn.code is "400")
            //	return Content(JsonConvert.SerializeObject(authCheckReturn));
            //SMReceipt.Global.Models.SearchReceipt gc = new SMReceipt.Global.Models.SearchReceipt() { birthdate = argd.birthdate, DateOfReceipt = argd.DateOfReceipt, FIO = argd.FIO, PatientId = argd.PatientId, PharmId = argd.PharmId, ReceiptCode = argd.ReceiptCode, ReceiptsCodes = argd.ReceiptsCodes, Region = argd.Region, SNILS = argd.SNILS };
            return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "403", level = "error", message = "Доступ запрещён", title = "error" }));
        }

        /// <summary>
        /// Внутренний контроллер для отпуска рецепта
        /// Принимает на вход класс рецепта ЭКФа
        /// Можно заполнить СНИЛС(PSnils)/id Пациента(IdPatient), а можно простро указать поле id - там лежит номер рецепта
        /// </summary>
        /// <param name="argd"></param>
        /// <remarks>
        /// POST /RecipeReleaseInt
        /// 
        ///		Пример:
        ///		
        ///			{
        ///				"authkey":"СЮДА ВСТАВЛЯЕМ АУТКЕЙ ИЗ КОНТРОЛЛЕРА AUTHORIZE ДЛЯ АПТЕКИ",
        ///				"id": "YJQ"
        ///			}
        ///			
        /// </remarks>
        [HttpPost("/RecipeReleaseInt")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> RecipeReleaseInt(RecipeReleaseReq argd)
        {

#if LOG_ALL_REQUESTS
	lg.Log("GetrecJson()", "RUNNED");
#endif
            try
            {
                if (PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(argd.authkey, "7", argd.key).userid != 0 && !String.IsNullOrEmpty(argd.id))
                {
                    argd.authkey = null;
                    return Content(JsonConvert.SerializeObject(((!String.IsNullOrEmpty(argd.IdPatient) || !String.IsNullOrEmpty(argd.PSnils)) ? PharmTaxiAdm.Methods.Receipt.ReleasePrescript(argd) : PharmTaxiAdm.Methods.Receipt.ReleasePrescript(argd.id))));
                }
                //var authCheckReturn = Global.CheckAuth(argd.authkey);
                //if (authCheckReturn.code is "400")
                //	return Content(JsonConvert.SerializeObject(authCheckReturn));
                //SMReceipt.Global.Models.SearchReceipt gc = new SMReceipt.Global.Models.SearchReceipt() { birthdate = argd.birthdate, DateOfReceipt = argd.DateOfReceipt, FIO = argd.FIO, PatientId = argd.PatientId, PharmId = argd.PharmId, ReceiptCode = argd.ReceiptCode, ReceiptsCodes = argd.ReceiptsCodes, Region = argd.Region, SNILS = argd.SNILS };
                return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "403", level = "error", message = "Доступ запрещён", title = "error" }));
            }
            catch (Exception e)
            {
                return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "413", level = "error", message = e.StackTrace, title = e.Message }));
            }
        }

        /// <summary>
        /// Внутренний контроллер для отпуска рецепта
        /// </summary>
        /// <param name="argd"></param>
        /// <remarks>
        /// Если recipeFullRelease = true и fullReleaseStatus != 0, то статус самого рецепта и всех ЛС будет fullReleaseStatus, 
        /// Если нет то изменится статус у препоратов которые есть в drugStatuses
        /// Нужно заполнить authkey или key, fullReleaseStatus если recipeFullRelease = true, drugStatuses если recipeFullRelease = false, и recipeId.
        /// Расшифровка idStatus в рецепте 0 - активный, 1 - частично обслужен, 2 - обслужен, 3 - просрочен без обслуживания, 4 - просрочен и частично обслужен, 5 - просрочен и обслужен.
        /// 
        /// POST /RecipeRelease
        /// 
        ///		Пример:
        ///			{
        ///				"authkey": "",
        ///				"recipeId": "92Q",
        ///				"pharmId": 0,
        ///				"pharmName": "аптечка",
        ///				"recipeFullRelease": false,
        ///				"releaseMedicaments": true,
        ///				"drugStatuses": [
        ///					{
        ///					    "key": "Drug_0002",
        ///					    "idStatus": 3,
        ///					    "medicaments":[
        ///                         {
        ///                             "id": "OX/R1LlhUWCmIAAhggmSGg==",
        ///                             "packs": 1,
        ///                             "price": 105
        ///                         }
        ///                     ]
        ///					},
        ///					{
        ///					    "key": "Drug_0003",
        ///					    "idStatus": 1
        ///					}
        ///				]
        ///			}	
        /// </remarks>
        [HttpPost("/RecipeRelease")]
        public async Task<IActionResult> RecipeRelease(RecipeReleaseArg arg)
        {

#if LOG_ALL_REQUESTS
	lg.Log("GetrecJson()", "RUNNED");
#endif
            try
            {
                if (arg.pharmId is null || arg.pharmId == 0)
                {
                    var alert = new Alert();
                    alert.BadRequest("pharmId is bad", true);

                    return Content(JsonConvert.SerializeObject(alert));
                }

                PharmTaxiAdm.Models.User.AlertUserID userInfo = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(arg.authkey, "7", arg.key);

                if (userInfo.userid != 0 && !String.IsNullOrEmpty(arg.recipeId))
                {
                    //погос сказал пока убрать эту проверку
                    //var pharmsId = PharmTaxiAdm.Methods.User.UserMethods.GetDrugstoresIdByUserId(userInfo.userid.ToString());

                    //if (pharmsId is null || !pharmsId.Contains(arg.pharmId * 10000 + 95)) 
                    //{
                    //    return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", level = "error", message = "Пользователь не имеет доступа к указанной аптеке", title = "error" })); 
                    //}

                    return Content(JsonConvert.SerializeObject(Metods.Release(arg, userInfo), new JsonSerializerSettings 
                    { 
                        ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() },  
                        Formatting = Formatting.Indented 
                    }));
                }

                return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "403", level = "error", message = "Доступ запрещён", title = "error" }));
            }
            catch (Exception e)
            {
                return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "413", level = "error", message = e.StackTrace, title = e.Message }));
            }
        }

        /// <summary>
        /// Внутренний контроллер для получения рецепта и опционально истории по нему
        /// Нужно заполнить authkey или key, recipeId
        /// </summary>
        /// <param name="argd"></param>
        /// <remarks>
        /// Расшифровка idStatus в рецепте 0 - активный, 1 - частично обслужен, 2 - обслужен, 3 - просрочен без обслуживания, 4 - просрочен и частично обслужен, 5 - просрочен и обслужен.
        /// 
        /// POST /GetRecipeAndHistory
        /// 
        ///		Пример:
        ///			{
        ///				"authkey": "",
        ///				"recipeId": "92Q",
        ///				"key":"",
        ///				"getHistory": true
        ///			}	
        /// </remarks>
        [HttpPost("/GetRecipeAndHistory")]
        public async Task<IActionResult> GetRecipeAndHistory(RecipeAndHistoryArgs args)
        {

#if LOG_ALL_REQUESTS
	lg.Log("GetrecJson()", "RUNNED");
#endif
            try
            {
                PharmTaxiAdm.Models.User.AlertUserID userInfo = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(args.authkey, "7", args.key);

                if (userInfo.userid != 0 && !String.IsNullOrEmpty(args.recipeId))
                {
                    return Content(JsonConvert.SerializeObject(Metods.GetRecipeAndHistory(args), new JsonSerializerSettings
                    {
                        ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() },
                        Formatting = Formatting.Indented
                    }));
                }

                return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "403", level = "error", message = "Доступ запрещён", title = "error" }));
            }
            catch (Exception e)
            {
                return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "413", level = "error", message = e.StackTrace, title = e.Message }));
            }
        }

        /// <summary>
        /// Внутренний контроллер для получения списка рецептов
        /// </summary>
        /// <param name="argd"></param>
        /// <remarks>
        /// Поиск работает отдельно для каждого поля и они не влияют друг на друга, можно заполнить как одно поле, так и все, snils должен быть в формате 000-000-000(тут пробел)00
        /// Нужно заполнить recipeId или snils или email
        /// Расшифровка idStatus в рецепте 0 - активный, 1 - частично обслужен, 2 - обслужен, 3 - просрочен без обслуживания, 4 - просрочен и частично обслужен, 5 - просрочен и обслужен.
        /// 
        /// POST /SearchRecipes
        /// 
        ///		Пример:
        ///			{
        ///			    "authkey":"",
        ///				"recipeId": "92Q",
        ///				"snils": "345-353-464 56",
        ///				"email": "gfhf@fgh.com"
        ///			}	
        /// </remarks>
        [HttpPost("/SearchRecipes")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> SearchRecipes(SearchRecipesArgs args)
        {

#if LOG_ALL_REQUESTS
	lg.Log("GetrecJson()", "RUNNED");
#endif
            try
            {
                //PharmTaxiAdm.Models.User.AlertUserID userInfo = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(args.authkey, "7", args.key);

                //if (userInfo.userid != 0)
                //{
                    return Content(JsonConvert.SerializeObject(await Metods.SearchRecipes(args), new JsonSerializerSettings
                    {
                        ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() },
                        Formatting = Formatting.Indented
                    }));
             //   }

                return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "403", level = "error", message = "Доступ запрещён", title = "error" }));
            }
            catch (Exception e)
            {
                return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "413", level = "error", message = e.StackTrace, title = e.Message }));
            }
        }

        /// <summary>
        /// Внутренний контроллер для проверки статуса рецепта
        /// Принимает на вход класс рецепта ЭКФа
        /// достаточно указать id - номер рецепта и authkey
        /// </summary>
        /// <param name="argd"></param>
        /// <remarks>
        /// POST /CheckPrescript
        /// 
        ///		Пример:
        ///		
        ///			{
        ///				"authkey":"СЮДА ВСТАВЛЯЕМ АУТКЕЙ ИЗ КОНТРОЛЛЕРА AUTHORIZE ДЛЯ АПТЕКИ",
        ///				"id": "YJQ"
        ///			}
        ///			
        /// </remarks>
        [HttpPost("/CheckPrescript")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> CheckPrescript(RecipeReleaseReq argd)
        {

#if LOG_ALL_REQUESTS
	lg.Log("GetrecJson()", "RUNNED");
#endif
            try
            {
                if (PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(argd.authkey, "7", argd.key).userid != 0 && !String.IsNullOrEmpty(argd.id))
                {
                    argd.authkey = null;
                    return Content(JsonConvert.SerializeObject(PharmTaxiAdm.Methods.Receipt.CheckPrescript(argd)));
                }
                //var authCheckReturn = Global.CheckAuth(argd.authkey);
                //if (authCheckReturn.code is "400")
                //	return Content(JsonConvert.SerializeObject(authCheckReturn));
                //SMReceipt.Global.Models.SearchReceipt gc = new SMReceipt.Global.Models.SearchReceipt() { birthdate = argd.birthdate, DateOfReceipt = argd.DateOfReceipt, FIO = argd.FIO, PatientId = argd.PatientId, PharmId = argd.PharmId, ReceiptCode = argd.ReceiptCode, ReceiptsCodes = argd.ReceiptsCodes, Region = argd.Region, SNILS = argd.SNILS };
                return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "403", level = "error", message = "Доступ запрещён", title = "error" }));
            }
            catch (Exception e)
            {
                return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "413", level = "error", message = e.StackTrace, title = e.Message }));
            }
        }


        /// <summary>
        /// Получить URL рецепта, по которому располагается бланк рецепта
        /// </summary>
        /// <param name="argd"></param>
        /// <returns></returns>
        [HttpPost("/GetReceiptURL")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> GetRecipeURL(SMReceipt.Global.Models.SearchReceipt argd)
        {

#if LOG_ALL_REQUESTS
	lg.Log("GetRecUrl()", "RUNNED");
#endif
            try
            {
                if (!String.IsNullOrEmpty(argd.ReceiptCode))
                {
                    argd.ReceiptCode = argd.ReceiptCode.ToUpper();
                    argd.ReceiptCode = SMBarcode.Methods.Common.Transliteration.Front(argd.ReceiptCode, SMBarcode.Methods.Common.TransliterationType.sim);
                }

                //	var authCheckReturn = Global.CheckAuth(argd.authkey);
                //	if (authCheckReturn.code is "400")
                //		return Content(JsonConvert.SerializeObject(authCheckReturn));
                SMReceipt.Global.Consts.URLs.Reciepes_Connect = Mongo.ReciepesConString;
                string ret = PharmTaxiAdm.Methods.Receipt.GetURLNew(argd);
                SMReceipt.Global.Consts.URLs.Reciepes_Connect = Mongo.MongoDB;
                return Content(ret);
            }
            catch (Exception e)
            {
                return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "200", title = "Ошибка получения рецепта", message = (!String.IsNullOrEmpty(argd.ReceiptCode)) ? $"Рецепт с номером {argd.ReceiptCode} не найден" : e.Message, level = "success", sticky = true }));
            }
        }

        /// <summary>
        /// Получение короткого рецепта из eRp
        /// </summary>
        /// <param name="data"></param>
        /// <example>
        /// Пример запроса рецептов по человеку со СНИЛС «000-000-600 02»:
        /// {
        /// "snils": "00000060002",
        /// "take": "20"
        /// }
        /// </example>
        /// 
        [HttpPost("GetErpPrescrShort")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> GetErpPrescrShort(PharmTaxiApp.Models.eRp.Request.PrescrList data)
        {
            try
            {
                return Content(Newtonsoft.Json.JsonConvert.SerializeObject(PharmTaxiApp.Methods.eRp.PrescrShort.Get(data)));
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
        }

        /// <summary>
        /// получение полного рецепта из eRp
        /// </summary>
        /// <param name="data"></param>
        /// <example>
        /// Пример запроса рецептов по человеку со СНИЛС «000-000-600 02»:
        /// {
        /// "snils": "00000060002",
        /// "take": "20"
        /// }
        /// </example>
        /// 
        [HttpPost("GetErpPrescrFull")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> GetErpPrescrFull(PharmTaxiApp.Models.eRp.Request.PrescrList data)
        {
            try
            {
                return Content(Newtonsoft.Json.JsonConvert.SerializeObject(PharmTaxiApp.Methods.eRp.PrescFull.Get(data)));
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
        }

        [HttpPost("ErpRelease")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> ErpRelease(PharmTaxiApp.Models.eRp.Request.ReleasePrescr data)
        {
            try
            {
                return Content(Newtonsoft.Json.JsonConvert.SerializeObject(PharmTaxiApp.Methods.eRp.RelaesePresc.Get(data)));
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
        }

        /// <summary>
        /// Сохранение ЛН (аптечки)
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost("/SavePrescript")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> SavePrescript(object args)
        {

#if LOG_ALL_REQUESTS
	lg.Log("SaveOrder()", "RUNNED");
#endif

            try
            {
                SMReceipt.Global.Models.SavePrescript so = new SMReceipt.Global.Models.SavePrescript();
                so = JsonConvert.DeserializeObject<SMReceipt.Global.Models.SavePrescript>(args.ToString());

                var authCheckReturn = Global.CheckAuth(so.authkey);
                if (authCheckReturn.code is "400")
                    return Content(JsonConvert.SerializeObject(authCheckReturn));

                PharmTaxiApp.Methods.SaveOrder.Save(ref so);
                return Content(JsonConvert.SerializeObject(so));
            }
            catch
            {
                SMReceipt.Global.Models.SavePrescript so = new SMReceipt.Global.Models.SavePrescript();
                so = JsonConvert.DeserializeObject<SMReceipt.Global.Models.SavePrescript>(args.ToString());

                var authCheckReturn = Global.CheckAuth(so.authkey);
                if (authCheckReturn.code is "400")
                    return Content(JsonConvert.SerializeObject(authCheckReturn));

                PharmTaxiApp.Methods.SaveOrder.Save(ref so);
                return Content(JsonConvert.SerializeObject(so));
            }
            //so = Newtonsoft.Json.JsonConvert.DeserializeObject<SMReceipt.Global.Models.SavePrescript>(args.ToString());

            //var authCheckReturn = CheckAuth(so.authkey);
            //if (authCheckReturn.code is "400")
            //	return Content(JsonConvert.SerializeObject(authCheckReturn));

            //PharmTaxiApp.Methods.SaveOrder.Save(ref so);
            //return Content(Newtonsoft.Json.JsonConvert.SerializeObject(so));
        }

        /// <summary>
        /// Сохранение Заказов
        /// </summary>
        /// <param name="args"></param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///	POST /SaveOrder
        ///	{
        ///		"authkey": "HLtvDMmz3qZLlT4HboIPYx0S",  
        ///		"order": {    
        ///			"pharm_id": 395661,    
        ///			"user_id": "9464",            
        ///			"drugs": 
        ///				[                
        ///					{                       
        ///						"id": "NKTWBFHcxsRO3SG0Xww7EA==",                       
        ///						"packs": 1,                       
        ///						"price": 75				
        ///					}            
        ///				],    
        ///			"isActive": true,    
        ///			"status": 0,    
        ///			"delivery": false  
        ///		}
        ///	}
        ///	</remarks>

        [HttpPost("/SaveOrder")]
        public async Task<IActionResult> SaveOrder(SaveOrderD args)
        {

#if LOG_ALL_REQUESTS
	lg.Log("SaveOrder()", "RUNNED");
#endif

            try
            {
                SMReceipt.Global.Models.SaveOrder so = new SMReceipt.Global.Models.SaveOrder();
                try
                {
                    so = JsonConvert.DeserializeObject<SMReceipt.Global.Models.SaveOrder>(JsonConvert.SerializeObject(args));
                }
                catch
                {
                    so.authkey = args.authkey;
                    so.service = "5";
                    try
                    {
                        so.order = new List<SMReceipt.Global.Models.Order>() { new SMReceipt.Global.Models.Order() { orderToSave = JsonConvert.DeserializeObject<SMReceipt.Global.Models.OrderToSave>(JsonConvert.SerializeObject(args.order)) } };
                    }
                    catch
                    {
                        so.order = new List<SMReceipt.Global.Models.Order>();
                        SMReceipt.Global.Models.Order Lord = new SMReceipt.Global.Models.Order();
                        Lord.orderToSave = new SMReceipt.Global.Models.OrderToSave();
                        SMReceipt.Global.Models.OrderToSave orderToSave = new SMReceipt.Global.Models.OrderToSave();
                        orderToSave.CreationDate = args.order.CreationDate;
                        orderToSave.Delivery = (args.order.Delivery is null) ? false : true;
                        orderToSave.DeliveryAddress = args.order.DeliveryAddress;
                        orderToSave.pharm_id_string = args.order.pharm_id.ToString();
                        orderToSave.drugs = new List<SMReceipt.Global.Models.Meds>();
                        if (!(args.order.drugs is null))
                            foreach (var i in args.order.drugs)
                            {
                                SMReceipt.Global.Models.Meds drug = new SMReceipt.Global.Models.Meds();
                                drug.dosage = i.dosage;
                                drug.drugstore_drug_id = i.drugstore_drug_id;
                                drug.id = i.id;
                                drug.name = i.name;
                                drug.packs_string = i.packs.ToString();
                                drug.price = i.price;
                                drug.removed = i.removed;
                                drug.send = i.send;
                                drug.table = i.table;
                                drug.tn = i.tn;
                                drug.value = i.value;
                                orderToSave.drugs.Add(drug);
                            }
                        orderToSave.IsActive = true;
                        orderToSave.IssueDate = args.order.IssueDate;
                        orderToSave.user_id = args.order.user_id;
                        // Lord.PharmID=args.order.pharm_id.ToString()
                        Lord.orderToSave = orderToSave;
                        so.order.Add(Lord);
                    }
                }
                var authCheckReturn = Global.CheckAuth(so.authkey);
                if (String.IsNullOrEmpty(args.authkey))
                    return Content(JsonConvert.SerializeObject(authCheckReturn));

                var aut = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(so.authkey, "5");
                //if(String.IsNullOrEmpty(args.order[0].order_to_save.user_id) && )
                //		string tst = Newtonsoft.Json.JsonConvert.SerializeObject(so);
                if (aut.alert.code == "200" || aut.alert.code == "202" || (args.authkey.StartsWith("_") || String.IsNullOrEmpty(args.authkey)) && String.IsNullOrEmpty(args.order.user_id))
                {
                    so.order[0].orderToSave.user_id = (String.IsNullOrEmpty(args.order.user_id)) ? (aut.alert.code != "200") ? "1488" : aut.userid.ToString() : args.order.user_id;
                    PharmTaxiApp.Methods.SaveOrder.OrderSave(ref so);
                    SOAns ans = new SOAns() { alert = so.alert, order_id = so.order[0].orderToSave.order_id };
                    return Content(JsonConvert.SerializeObject(ans));
                }
                else
                {
                    return Content(JsonConvert.SerializeObject(aut.alert));
                }
            }
            catch (Exception e)
            { return Content("hz"); }
        }


        /// <summary>
        /// Сохранение Заказов Аптекой
        /// </summary>
        /// <param name="args"></param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///	POST /SaveForeignOrder
        ///	{
        ///		"authkey": "HLtvDMmz3qZLlT4HboIPYx0S",  
        ///		"order": {    
        ///			"pharm_id": 395661,         
        ///			"drugs": 
        ///				[                
        ///					{                       
        ///						"id": "NKTWBFHcxsRO3SG0Xww7EA==",                       
        ///						"packs": 1,                       
        ///						"price": 75				
        ///					}            
        ///				],    
        ///			"isActive": true,    
        ///			"status": 0,    
        ///			"delivery": false  
        ///		}
        ///	}
        ///	</remarks>

        [HttpPost("/SaveForeignOrder")]
        public async Task<IActionResult> SaveForeignOrder(SaveOrderD args)
        {

#if LOG_ALL_REQUESTS
	lg.Log("SaveOrder()", "RUNNED");
#endif

            try
            {
                SMReceipt.Global.Models.SaveOrder so = new SMReceipt.Global.Models.SaveOrder();
                try
                {
                    so = JsonConvert.DeserializeObject<SMReceipt.Global.Models.SaveOrder>(JsonConvert.SerializeObject(args));
                }
                catch
                {
                    so.authkey = args.authkey;
                    so.service = "5";
                    try
                    {
                        so.order = new List<SMReceipt.Global.Models.Order>() { new SMReceipt.Global.Models.Order() { orderToSave = JsonConvert.DeserializeObject<SMReceipt.Global.Models.OrderToSave>(JsonConvert.SerializeObject(args.order)) } };
                    }
                    catch
                    {
                        so.order = new List<SMReceipt.Global.Models.Order>();
                        SMReceipt.Global.Models.Order Lord = new SMReceipt.Global.Models.Order();
                        Lord.orderToSave = new SMReceipt.Global.Models.OrderToSave();
                        SMReceipt.Global.Models.OrderToSave orderToSave = new SMReceipt.Global.Models.OrderToSave();
                        orderToSave.CreationDate = args.order.CreationDate;
                        orderToSave.Delivery = (args.order.Delivery is null) ? false : true;
                        orderToSave.DeliveryAddress = args.order.DeliveryAddress;
                        orderToSave.pharm_id_string = args.order.pharm_id.ToString();
                        orderToSave.drugs = new List<SMReceipt.Global.Models.Meds>();
                        if (!(args.order.drugs is null))
                            foreach (var i in args.order.drugs)
                            {
                                SMReceipt.Global.Models.Meds drug = new SMReceipt.Global.Models.Meds();
                                drug.dosage = i.dosage;
                                drug.drugstore_drug_id = i.drugstore_drug_id;
                                drug.id = i.id;
                                drug.name = i.name;
                                drug.packs_string = i.packs.ToString();
                                drug.price = i.price;
                                drug.removed = i.removed;
                                drug.send = i.send;
                                drug.table = i.table;
                                drug.tn = i.tn;
                                drug.value = i.value;
                                orderToSave.drugs.Add(drug);
                            }
                        orderToSave.IsActive = true;
                        orderToSave.IssueDate = args.order.IssueDate;
                        orderToSave.user_id = args.order.user_id;
                        // Lord.PharmID=args.order.pharm_id.ToString()
                        Lord.orderToSave = orderToSave;
                        so.order.Add(Lord);
                    }
                }
                var authCheckReturn = Global.CheckAuth(so.authkey);
                if (String.IsNullOrEmpty(args.authkey))
                    return Content(JsonConvert.SerializeObject(authCheckReturn));

                var aut = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(so.authkey);
                //if(String.IsNullOrEmpty(args.order[0].order_to_save.user_id) && )
                //		string tst = Newtonsoft.Json.JsonConvert.SerializeObject(so);
                if (aut.alert.code == "200" || aut.alert.code == "202" || (args.authkey.StartsWith("_") || String.IsNullOrEmpty(args.authkey)) && String.IsNullOrEmpty(args.order.user_id))
                {
                    so.order[0].orderToSave.user_id = (String.IsNullOrEmpty(args.order.user_id)) ? "2" : args.order.user_id; //(String.IsNullOrEmpty(args.order.user_id)) ? (aut.alert.code != "200") ? "1488" : aut.userid.ToString() : args.order.user_id;
                    PharmTaxiApp.Methods.SaveOrder.OrderSave(ref so);
                    SOAns ans = new SOAns() { alert = so.alert, order_id = so.order[0].orderToSave.order_id };
                    return Content(JsonConvert.SerializeObject(ans));
                }
                else
                {
                    return Content(JsonConvert.SerializeObject(aut.alert));
                }
            }
            catch (Exception e)
            { return Content("hz"); }
        }


        /// <summary>
        /// Сохранение Заказов ЖНВЛП
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///		POST /SaveFakeOrder
        ///		{
        ///			"authkey": "1h8Bvn87h3a8G26sGASe",
        ///			"fakeOrders": [
        ///				{
        ///					"pharm_id": 1031,
        ///					"user_id": "1488",
        ///					"CreationDate": 1622637785,
        ///					"IssueDate": 1622637785,
        ///					"DeliveryAddress": "Москва",
        ///					"Delivery": false,
        ///					"drugs": [
        ///						{
        ///							
        ///						}
        ///					] 
        ///				}
        ///			]
        ///		}
        /// </remarks>
        [HttpPost("/SaveFakeOrder")]
        public async Task<IActionResult> SaveFakeOrder(SMReceipt.Global.Models.FakeOrder fakeOrder)
        {
            var authCheckReturn = Global.CheckAuth(fakeOrder.authkey);
            if (String.IsNullOrEmpty(fakeOrder.authkey))
                return Content(JsonConvert.SerializeObject(authCheckReturn));

            var aut = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(fakeOrder.authkey, "5");

            fakeOrder.user_id = aut.userid.ToString();
            fakeOrder.CreationDate = fakeOrder.IssueDate = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();

            if (aut.alert.code == "200" || (fakeOrder.authkey.StartsWith("_") || String.IsNullOrEmpty(fakeOrder.authkey)))
            {
                SOAns ans = new SOAns() { alert = SMReceipt.Global.Methods.Mongo.InsertFakeOrder(ref fakeOrder), order_id = fakeOrder.order_id };
                return Content(JsonConvert.SerializeObject(ans));
            }
            else
            {
                return Content(JsonConvert.SerializeObject(aut.alert));
            }
        }

        /// <summary>
        /// Получение рецептов пациента
        /// </summary>
        /// <param name="args"></param>
        /// <remarks>
        /// Пример запроса:
        ///
        ///		POST /Recipes
        ///		{
        ///			"authkey": "fs43ASFDAfsdas34i",
        ///			"email": "1st_man@mail.ru"
        ///		}
        /// </remarks> 
        [SwaggerResponse(200, "", typeof(PharmTaxiAdm.Models.ModelRecipeLS))]
        [HttpPost("/Recipes")]
        public async Task<IActionResult> Recipes(PharmTaxiApp.Models.RecipeArgs args)
        {

#if LOG_ALL_REQUESTS
	lg.Log("Recipes()", "RUNNED");
#endif
            try
            {
                //	var authCheckReturn = Global.CheckAuth(args.authkey);
                //	if (authCheckReturn.code is "400")
                //		return Content(JsonConvert.SerializeObject(authCheckReturn));
                string ret = "";
                PharmTaxiApp.Models.AllRecipes all = new PharmTaxiApp.Models.AllRecipes();
                SMReceipt.Global.Consts.URLs.Reciepes_Connect = Mongo.ReciepesConString;
                if (!string.IsNullOrEmpty(args.recipe))
                {
                    args.recipe = args.recipe.ToUpper();
                    ret = PharmTaxiApp.Methods.ECP.Recipes(args);
                    SMReceipt.Global.Consts.URLs.Reciepes_Connect = Mongo.MongoDB;
                }
                else if (!String.IsNullOrEmpty(args.SNILS) && string.IsNullOrEmpty(args.recipe))
                {
                    List<PharmTaxiApp.Models.RecipeArgs> RecArgs = new List<PharmTaxiApp.Models.RecipeArgs>();
                    RecArgs = PharmTaxiApp.Methods.RecipeCodesToRecipeArgs.RecArgsFromSNILS(args.SNILS);
                    List<PharmTaxiApp.Models.ModelRecipeLS> all_recipes = new List<PharmTaxiApp.Models.ModelRecipeLS>();
                    foreach (var i in RecArgs)
                    {
                        all_recipes.AddRange(Newtonsoft.Json.JsonConvert.DeserializeObject<PharmTaxiApp.Models.AllRecipes>(PharmTaxiApp.Methods.ECP.Recipes(i)).recipes);
                    }
                    all.recipes = all_recipes;
                    all.alert = new UMKBNeedStuff.Alert() { code = "200", title = "success", message = ret, level = "success", sticky = true };
                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(all);
                }
                else
                {
                    ret = PharmTaxiApp.Methods.ECP.Recipes(args);
                    SMReceipt.Global.Consts.URLs.Reciepes_Connect = Mongo.MongoDB;
                }
                if (ret.Contains("\"alert\":{\"code\":"))
                    return Content(ret);
                else
                {
                    return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "200", title = "Ошибка получения рецепта", message = ret, level = "success", sticky = true }));
                }
            }
            catch (Exception e)
            {
                return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "200", title = "Ошибка получения рецепта", message = (!String.IsNullOrEmpty(args.recipe)) ? $"Рецепт с номером {args.recipe} не найден" : e.Message, level = "success", sticky = true }));
            }
        }


    }
}
