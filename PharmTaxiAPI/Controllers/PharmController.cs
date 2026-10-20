//using Core.Data;
using Core.Pharm;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
//using PharmService;
using PharmTaxiAPI.Methods;
using PharmTaxiAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using PharmTaxiAdm.Models;
using PharmTaxiAdm.Models.Drugstore;
using Microsoft.AspNetCore.Cors;
using PharmTaxiAdm.Models.User;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.Extensions.Primitives;
using System.Data;
using ExcelDataReader;
using System.Text;
using PTConstants;

namespace PharmTaxiAPI.Controllers
{
    [ApiController]
	[Route("[controller]")]
	[EnableCors(Cors.AllowAll.Name)]
	public class PharmController : ControllerBase
    {
		Logs lg = new Logs();
		private PharmParser _parser = new PharmParser();

		public PharmController()
		{
		}

		/// <summary>
		/// Контроллер для создания сети аптек
		/// </summary>
		/// <param name="args"></param>
		/// <remarks>
		/// Пример запроса:
		///		
		///		POST /RegisterWNet
		///		{
		///				"authkey":"НАДО ПОЛУЧИТЬ ВАЛИДНЫЙ АУТКЕЙ",
		///				"parentId": "ID Родителя",
		///				"typeText": "Сеть Аптек",
		///				"address": "Москва, ул. Кикабидзе, 55",
		///				"license": "2ппу3231",
		///				"phone": "555-55-55",
		///				"title": "Сеть аптек Планета Чурок Доча"
		///			}
		/// </remarks>
		/// 
		[ApiExplorerSettings(IgnoreApi = false)]
		[HttpPost("/CreateDrugNet")]
		public async Task<IActionResult> CreateDrugNet(PharmTaxiAdm.Models.Drugstore.DrugNetModel args)
        {
			lg.Log("CreateDrugNet()", "RUNNING");
            try
            {
				AlertUserID auid = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(args.authkey, "7", args.key);
				if(auid.alert.code!="200" || auid.userid==0)return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "500", message = "Not Authorized!", level = "error", sticky = false, title = "failed" }));
				return Content(Newtonsoft.Json.JsonConvert.SerializeObject(PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.CreateDrugNet(args)));
            }
            catch(Exception e)
            {
				return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", message = e.Message, level = "error", sticky = false, title = "failed" }));
            }
        }

		#region
		/*	[HttpGet]
			public string Price(string auth_key, uint pageSize = 100, uint pageIndex = 1, uint? drugstore_id = null,
				string drug_name = null, string sortField = null, string sortOrder = null)
			{
	#if LOG_ALL_REQUESTS
				lg.Log("Price()", "RUNNED");
	#endif
				if (drugstore_id == (uint)0)
					drugstore_id = null;

				try
				{
					long count = 0;
					if (drugstore_id is null)
					{
						Dictionary<string, uint> dct = PharmApi.GetPharmMapping(auth_key, Core.Data.ConnectionMode.Work);
						foreach (var i in dct)
						{
							Core.Data.AssortmentItemData ass = new AssortmentItemData();
							ass.PharmId = i.Value;
							count += Core.MongoDB.Methods.Ass_get.getCount(ass);
						}
					}
					else
					{
						Dictionary<string, uint> dct = PharmApi.GetPharmMapping(auth_key, Core.Data.ConnectionMode.Work);
						Core.Data.AssortmentItemData ass = new AssortmentItemData();
						ass.PharmId = uint.TryParse(PricePager.Get(drugstore_id.ToString() + "0902"), out uint o) ? o : 0;
						if (ass.PharmId == 0)// ass.PharmId = dct[drugstore_id.ToString()];
						{
							bool x = dct.ContainsKey(drugstore_id.ToString());
							uint? zz = PharmApi.NewGetUserId(auth_key);
							string tst = zz.ToString() + "_" + drugstore_id.ToString();
							uint yyy = dct[tst];
							ass.PharmId = yyy;
						}
						count += Core.MongoDB.Methods.Ass_get.getCount(ass);
					}
					uint cnt_old = PricePager.GetCountOf(auth_key, drugstore_id, Core.Data.ConnectionMode.Work);
					uint cnt = (uint)count;
					return (new System.Web.Http.Results.JsonResult<PriceResponse>(new PriceResponse()
					{
						pages = cnt / pageSize + (((cnt % pageSize) > 0) ? (uint)1 : (uint)0),
						itemsCount = cnt,
						data = PricePager.GetPrices(auth_key, drugstore_id, drug_name, sortField, sortOrder, pageSize, pageIndex, Core.Data.ConnectionMode.Work)
								.ToArray(),
						ErrorMessage = null
					}, new Newtonsoft.Json.JsonSerializerSettings(), System.Text.Encoding.UTF8, new HttpRequestMessage()));
				}
				catch (Exception e)
				{
					lg.Log("Price()", e.Message);
					return (new System.Web.Http.Results.JsonResult<PriceResponse>(new PriceResponse()
					{
						pages = 0,
						itemsCount = 0,
						data = new PriceResponseItem[] { },
						ErrorMessage = e.Message
					}, new Newtonsoft.Json.JsonSerializerSettings(), System.Text.Encoding.UTF8, new HttpRequestMessage()));
				}
			}*/


		//[HttpPost("/Search/Drugs")]
		//public string Drugs(UMKBRequests.Models.API.Semantic.RequestSearch args)
		//{
		//	args.authkey = null;
		//	if (args.authkey == "" || args.authkey is null)
		//	{
		//		args.key = "";
		//	}
		//	//    UMKBRequests.WebUtils.Post("cs.socmedica.com/Search/Drugs", Newtonsoft.Json.JsonConvert.SerializeObject(args), UMKBRequests.WebUtils.WebContentType.JSON);
		//	UMKBRequests.WebUtils.Result res = UMKBRequests.WebUtils.Post("https://cs.socmedica.com/drugs/Dosages", Newtonsoft.Json.JsonConvert.SerializeObject(args), "application/json");
		//	return res.Answer;
		//}
		#endregion

		/// <summary>
		/// Создание аптеки
		/// </summary>
		/// <param name="dataO"></param>
		/// <returns></returns>
		/// <remarks>
		/// Пример запроса для создания аптеки:
		/// 
		///		POST /CreateDrugstore
		///		{
		///			"authkey": "АУТКЕЙ НЕОБХОДИМО ПОЛУЧИТЬ ПРИ ПОМОЩИ КОНТРОЛЛЕРА Authorize",
		///			"ParentId": "ID сети аптек" 
		///			"TypeText": "Аптека",
		///			"Address": "Москва, ул. Нобеля 1",
		///			"License": "1111111йв",
		///			"Phone": "+7 926 000 00 11",
		///			"Title": "TEST",
		///			"latitude": 75.14,
		///			"longitude": 55.14,
		///			"mon" : {
		///				"StartTime": 800,
		///				"EndTime": 2100,
		///				"BreakStart": 1200,
		///				"BreakEnd": 1300
		///			},
		///			"tue" : {
		///				"StartTime": 800,
		///				"EndTime": 2100,
		///				"BreakStart": 1200,
		///				"BreakEnd": 1300
		///			},
		///			"wed" : {
		///				"StartTime": 800,
		///				"EndTime": 2100,
		///				"BreakStart": 1200,
		///				"BreakEnd": 1300
		///			},
		///			"thu" : {
		///				"StartTime": 800,
		///				"EndTime": 2100,
		///				"BreakStart": 1200,
		///				"BreakEnd": 1300
		///			},
		///			"fri" : {
		///				"StartTime": 800,
		///				"EndTime": 2100,
		///				"BreakStart": 1200,
		///				"BreakEnd": 1300
		///			},
		///			"sat" : {
		///				"StartTime": 800,
		///				"EndTime": 2100,
		///				"BreakStart": 1200,
		///				"BreakEnd": 1300
		///			},
		///			"sun" : {
		///				"StartTime": 800,
		///				"EndTime": 2100,
		///				"BreakStart": 1200,
		///				"BreakEnd": 1300
		///			}
		///		}
		/// </remarks>
		[HttpPost("/CreateDrugstore")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public async Task<IActionResult> CreateDrugstore(DrugstoreModel dataO) //JsonResult<Core.Pharm.Response>
		{
			//DrugstoreModel data = new DrugstoreModel();
			//data = JsonConvert.DeserializeObject<DrugstoreModel>(dataO.ToString());
			lg.Log("CreateDrugstore()", "RUNNED");
			PharmTaxiAdm.Models.User.AlertUserID uID = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(dataO.authkey, "7", dataO.key);
			if (uID.alert.level != "success")
				return Content(JsonConvert.SerializeObject(uID.alert));

			try
			{
				PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.CreateDrugstore(dataO);
				return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "200", message = "success", title = "success", sticky = false }));
			}
			catch (Exception e)
			{
				return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", message = e.Message, title = "failed", sticky = false }));
			}
		}

		/// <summary>
		/// Создать список аптеку и вернуть заготовок лицензионного ключа для них 
		/// </summary>
		/// <param name="data"></param>
		/// <remarks>
		/// Пример запроса для множественного создания аптек:
		/// 
		///		POST /MassiveCreateDrugstore
		///		{
		///			"pharms": [
		///				{
		///					"authkey": "АУТКЕЙ НЕОБХОДИМО ПОЛУЧИТЬ ПРИ ПОМОЩИ КОНТРОЛЛЕРА Authorize",
		///					"ParentId": "ID сети аптек",
		///					"TypeText": "Аптека",
		///					"Address": "Москва, ул. Нобеля 1",
		///					"License": "1111111йв",
		///					"Phone": "+7 926 000 00 11",
		///					"Title": "TEST",
		///					"latitude": 75.14,
		///					"longitude": 55.14,
		///					"mon" : {
		///						"StartTime": 800,
		///						"EndTime": 2100,
		///						"BreakStart": 1200,
		///						"BreakEnd": 1300
		///					},
		///					"tue" : {
		///						"StartTime": 800,
		///						"EndTime": 2100,
		///						"BreakStart": 1200,
		///						"BreakEnd": 1300
		///					},
		///					"wed" : {
		///						"StartTime": 800,
		///						"EndTime": 2100,
		///						"BreakStart": 1200,
		///						"BreakEnd": 1300
		///					},
		///					"thu" : {
		///						"StartTime": 800,
		///						"EndTime": 2100,
		///						"BreakStart": 1200,
		///						"BreakEnd": 1300
		///					},
		///					"fri" : {
		///						"StartTime": 800,
		///						"EndTime": 2100,
		///						"BreakStart": 1200,
		///						"BreakEnd": 1300
		///					},
		///					"sat" : {
		///						"StartTime": 800,
		///						"EndTime": 2100,
		///						"BreakStart": 1200,
		///						"BreakEnd": 1300
		///					},
		///					"sun" : {
		///						"StartTime": 800,
		///						"EndTime": 2100,
		///						"BreakStart": 1200,
		///						"BreakEnd": 1300
		///					}
		///				},
		///				{
		///					"authkey": "АУТКЕЙ, ранее полученный (от Authorize)",
		///					"ParentId": "ID сети аптек",
		///					"TypeText": "Аптека",
		///					"Address": "Москва, ул. Нобеля 3",
		///					"License": "11542361йв",
		///					"Phone": "+7 926 000 20 11",
		///					"Title": "Нобель",
		///					"latitude": 75.14,
		///					"longitude": 55.14,
		///					"mon" : {
		///						"StartTime": 800,
		///						"EndTime": 2100,
		///						"BreakStart": 1200,
		///						"BreakEnd": 1300
		///					},
		///					"tue" : {
		///						"StartTime": 800,
		///						"EndTime": 2100,
		///						"BreakStart": 1200,
		///						"BreakEnd": 1300
		///					},
		///					"wed" : {
		///						"StartTime": 800,
		///						"EndTime": 2100,
		///						"BreakStart": 1200,
		///						"BreakEnd": 1300
		///					},
		///					"thu" : {
		///						"StartTime": 800,
		///						"EndTime": 2100,
		///						"BreakStart": 1200,
		///						"BreakEnd": 1300
		///					},
		///					"fri" : {
		///						"StartTime": 800,
		///						"EndTime": 2100,
		///						"BreakStart": 1200,
		///						"BreakEnd": 1300
		///					},
		///					"sat" : {
		///						"StartTime": 800,
		///						"EndTime": 2100,
		///						"BreakStart": 1200,
		///						"BreakEnd": 1300
		///					},
		///					"sun" : {
		///						"StartTime": 800,
		///						"EndTime": 2100,
		///						"BreakStart": 1200,
		///						"BreakEnd": 1300
		///					}
		///				}
		///			]
		///			
		///		}
		///		
		/// </remarks>
		[HttpPost("/MassiveCreateDrugstore")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public async Task<IActionResult> MassiveCreateDrugstore(DrugstoreModelList data) //JsonResult<Core.Pharm.Response>
		{
			lg.Log("MassiveCreateDrugstore()", "RUNNED");
			if (data.pharms.Count > 700) return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", message = "Too Much Items. 700 Items Max", title = "failed", sticky = false }));
			PharmTaxiAdm.Models.User.AlertUserID uID = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(data.pharms[0].authkey, "7", data.pharms[0].key);
			if (uID.alert.level != "success") return Content(JsonConvert.SerializeObject(uID.alert));
			try
			{
				List<CreatedDrugStore> crList = new List<CreatedDrugStore>();
				List<Task<CreatedDrugStore>> tsks = new List<Task<CreatedDrugStore>>();
				foreach (var i in data.pharms)
				{
					tsks.Add(Task.Run(() => PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.CreateDrugstoreWCode(i).Result));
				}
				foreach (var i in tsks)
				{
					if (i.IsCompleted) crList.Add(i.Result);
					else
					{
						await i;
						crList.Add(i.Result);
					}
				}
				return Content(JsonConvert.SerializeObject(crList));
			}
			catch (Exception e)
			{
				return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", message = e.Message, title = "failed", sticky = false }));
			}
		}

		/// <summary>
		/// Обновить информацию аптеки
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		[HttpPost("/UpdateDrugstore")]
		public async Task<IActionResult> UpdateDrugstore(DrugstoreModel data) //JsonResult<Core.Pharm.Response>
		{

			lg.Log("UpdateDrugstore()", "RUNNED");
			PharmTaxiAdm.Models.User.AlertUserID uID = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(data.authkey);
			if (uID.alert.level != "success") return Content(JsonConvert.SerializeObject(uID.alert));
			try
			{
				PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.UpdateDrugstore(data);
				return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "200", message = "success", title = "success", sticky = false }));
			}
			catch (Exception e)
			{
				return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", message = e.Message, title = "failed", sticky = false }));
			}
		}

		/// <summary>
		/// Получение всех актвных заказов для аптеки
		/// </summary>
		/// <remarks>
		/// Пример запроса:
		/// 
		///		POST /GetPharmOrders
		///		{
		///			"authkey": "АУТКЕЙ НЕОБХОДИМО ПОЛУЧИТЬ ПРИ ПОМОЩИ КОНТРОЛЛЕРА Authorize"
		///		}
		///		
		/// </remarks>
		[HttpPost("/GetPharmOrders")]
		[SwaggerResponse(200, "", typeof(SMReceipt.Global.Models.SaveOrder))]
		public async Task<IActionResult> GetPharmOrders(AuthKeyByLicKey data)
		{
			try
			{
				SMReceipt.Global.Models.SaveOrder ret = new SMReceipt.Global.Models.SaveOrder();
				//if (String.IsNullOrEmpty(data.authkey)) return Content("No authkey!");
				AlertUserID UsA = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(data.authkey, data.master_service, data.key);
				HashSet<DrugstoreModel> DrM = PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.GetDrugstoresByUserId(UsA.userid.ToString());
				List<Task<SMReceipt.Global.Models.SaveOrder>> PhmsTsks = new List<Task<SMReceipt.Global.Models.SaveOrder>>();
				foreach (var i in DrM)
				{
					PhmsTsks.Add(Task.Factory.StartNew(() => PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.GetPharmOrders(i.PharmId)));
				}
				foreach (var i in PhmsTsks)
				{
					if (i.IsCompleted) ret.order.AddRange(i.Result.order);
					else
					{
						await i;
						ret.order.AddRange(i.Result.order);
					}
				}
				return Content(JsonConvert.SerializeObject(ret));
			}
			catch (Exception e)
			{
				return Content(e.Message);
			}
		}

		/// <summary>
		/// Контроллер для изменения ассортимента
		/// </summary>
		/// <param name="data"></param>
		/// <remarks>
		/// Пример запроса:
		/// 
		///		POST /ChangeAssortiment
		///		{
		///			"authkey": "АУТКЕЙ НЕОБХОДИМО ПОЛУЧИТЬ ПРИ ПОМОЩИ КОНТРОЛЛЕРА Authorize",
		///			"lass": [
		///				{
		///					"pharmId": 392215,
		///					"pharm_pack_id": "55326",
		///					"fp_id": "i3lKz4MbKnAeXRA24YqEng==",
		///					"qty": 10,
		///					"qty_free": 2,
		///					"price": 1001.5
		///				}
		///			]
		///		}
		///		
		/// </remarks>
		[HttpPost("/ChangeAssortiment")]
		public async Task<IActionResult> ChangeAssortiment(APIChangeAss data)
		{

			//	Dictionary<string, uint> pharms = PharmApi.GetPharmMapping(data.key, ConnectionMode.Work);
			Dictionary<uint?, bool> apts = new Dictionary<uint?, bool>();
			/*foreach (var i in data.Lass)
			{
				if (!apts.ContainsKey(i.PharmId))
					apts.Add(i.PharmId, false);
				foreach (var j in pharms)
				{
					if (i.PharmId == j.Value)
					{
						apts[i.PharmId] = true;
					}
				}
			}*/
			lg.Log("ChangeAssortiment()", "RUNNED");
			if (!apts.ContainsValue(false))
			{
				try
				{
					List<AUpload.Global.Models.AssortmentItemData> LassC = new List<AUpload.Global.Models.AssortmentItemData>();
					foreach(var i in data.lass)
                    {
						AUpload.Global.Models.AssortmentItemData Ass = new AUpload.Global.Models.AssortmentItemData();
						LassC.Add(UsefullStuff.Common.Methods.Converter<AUpload.Global.Models.AssortmentItemData, Models.ChangeAss>.Convert(Ass, i));       //JsonConvert.DeserializeObject<AUpload.Global.Models.AssortmentItemData>(JsonConvert.SerializeObject(i)));
                    }
					return Content(JsonConvert.SerializeObject(PharmTaxiAdm.Methods.Assortiment.Change.Assortiment(LassC, data.authkey))); //PharmParser.APILoadAss(data.Lass, data.authkey)));
				}
				catch (Exception e)
				{
					lg.Log("ChangeAssortiment()", e.Message);
					return Content(JsonConvert.SerializeObject(new Core.Pharm.Response(false, String.Format("{0}: {1}", e.GetType().Name, e.Message))));
				}
			}
			else
			{
				return Content(JsonConvert.SerializeObject(new Core.Pharm.Response(false, String.Format("Нет прав для изменения ассортимента"))));
			}
		}

		/// <summary>
		/// Контроллер для получения ассортимента.
		/// </summary>
		/// <param name="data"></param>
		/// <remarks>
		/// Пример запроса:
		/// 
		///		POST /GetAssortiment
		///		
		///		{
		///			"authkey": "АУТКЕЙ НЕОБХОДИМО ПОЛУЧИТЬ ПРИ ПОМОЩИ КОНТРОЛЛЕРА Authorize"
		///		}
		///		
		/// </remarks>
		[HttpPost("/GetAssortiment")]
		public async Task<IActionResult> GetAssortiment(APIRequestAss data)
		{
			//	Dictionary<string, uint> pharms = PharmApi.GetPharmMapping(data.key, ConnectionMode.Work);
			Dictionary<uint?, bool> apts = new Dictionary<uint?, bool>();
			/*foreach (var i in data.Lass)
			{
				if (!apts.ContainsKey(i.PharmId))
					apts.Add(i.PharmId, false);
				foreach (var j in pharms)
				{
					if (i.PharmId == j.Value)
					{
						apts[i.PharmId] = true;
					}
				}
			}*/
			lg.Log("GetAssortiment()", "RUNNED");
			if (!apts.ContainsValue(false))
			{
				try
				{
					return Content(JsonConvert.SerializeObject(PharmTaxiAdm.Methods.Assortiment.Get.Assortiment(data.Lass, data.authkey, data.key))); //PharmParser.APILoadAss(data.Lass, data.authkey)));
				}
				catch (Exception e)
				{
					lg.Log("GetAssortiment()", e.Message);
					return Content(JsonConvert.SerializeObject(new Core.Pharm.Response(false, String.Format("{0}: {1}", e.GetType().Name, e.Message))));
				}
			}
			else
			{
				return Content(JsonConvert.SerializeObject(new Core.Pharm.Response(false, String.Format("Нет прав для получения ассортимента"))));
			}
		}

		/// <summary>
		/// Контроллер для загрузки ассортимента. Первоначально создавался для интеграции вместо WCF
		/// </summary>
		/// <param name="dataO"></param>
		/// <returns></returns>
		/// <remarks>
		/// Пример запроса:
		/// 
		///		POST /LoadAssortiment
		///		{
		///			"authkey": "АУТКЕЙ НЕОБХОДИМО ПОЛУЧИТЬ ПРИ ПОМОЩИ КОНТРОЛЛЕРА Authorize",
		///			"Lass": [
		///				{
		///					"PharmId": 392215,
		///					"PharmLogin": "392215",
		///					"PharmPackId": "5004951",
		///					"Barcode": "4030685259512",
		///					"Name": "Амарил, тбл 4мг №30",
		///					"TradeName": "Амарил, тбл 4мг №30",
		///					"Dosage": "4мг",
		///					"form": "таблетки",
		///					"RegistrationCertificate": "УП/321023",
		///					"Type": "внутрь",
		///					"Pack": "12",
		///					"Manufacturer": "Aventis Pharma Deutschland GmbH",
		///					"Country": "Германия",
		///					"PharmacoTherapeuticGroup": "Лекарственные средства",
		///					"Quantity": 15,
		///					"QuantityFree": 8,
		///					"SellingPrice": 1300.5,
		///					"SellingPriceType": "1"
		///				}
		///			]
		///		}
		///		
		/// </remarks>
		[HttpPost("/LoadAssortiment")]
		public async Task<IActionResult> LoadAssortiment(APIRequestAss data)//APIRequestAss data)
		{
			lg.Log("LoadAssortiment()", "RUNNED");

			try
			{
				return Content(JsonConvert.SerializeObject(PharmParser.APILoadAss(data.Lass, data.authkey, data.key)));
			}
			catch (Exception e)
			{
				lg.Log("LoadAssortiment()", e.Message);

				return Content(JsonConvert.SerializeObject(new Core.Pharm.Response(false, String.Format("{0}: {1}", e.GetType().Name, e.Message))));
			}
		}

		/// <summary>
		/// Контроллер для удаления ассортимента.
		/// </summary>
		/// <param name="data"></param>
		/// <remarks>
		/// Пример запроса:
		/// 
		///		POST /RemoveAssortiment
		///		{
		///			"authkey": "АУТКЕЙ НЕОБХОДИМО ПОЛУЧИТЬ ПРИ ПОМОЩИ КОНТРОЛЛЕРА Authorize",
		///			"Lass": [
		///				{
		///					"PharmId": 392215,
		///					"PharmLogin": "392215",
		///					"PharmPackId": "5004951",
		///					"Barcode": "4030685259512",
		///					"Name": "Амарил, тбл 4мг №30",
		///					"TradeName": "Амарил, тбл 4мг №30",
		///					"Dosage": "4мг",
		///					"form": "таблетки",
		///					"RegistrationCertificate": "УП/321023",
		///					"Type": "внутрь",
		///					"Pack": "12",
		///					"Manufacturer": "Aventis Pharma Deutschland GmbH",
		///					"Country": "Германия",
		///					"PharmacoTherapeuticGroup": "Лекарственные средства",
		///					"Quantity": 15,
		///					"QuantityFree": 8,
		///					"SellingPrice": 1300.5,
		///					"SellingPriceType": "1"
		///				}
		///			]
		///		}
		///		
		/// </remarks>
		[HttpPost("/RemoveAssortiment")]
		public async Task<IActionResult> RemoveAssortiment(APIRequestAss data)
		{

			//	Dictionary<string, uint> pharms = PharmApi.GetPharmMapping(data.key, ConnectionMode.Work);
			Dictionary<uint?, bool> apts = new Dictionary<uint?, bool>();
			/*foreach (var i in data.Lass)
			{
				if (!apts.ContainsKey(i.PharmId))
					apts.Add(i.PharmId, false);
				foreach (var j in pharms)
				{
					if (i.PharmId == j.Value)
					{
						apts[i.PharmId] = true;
					}
				}
			}*/
			lg.Log("RemoveAssortiment()", "RUNNED");
			if (!apts.ContainsValue(false))
			{
				try
				{
					return Content(JsonConvert.SerializeObject(PharmTaxiAdm.Methods.Assortiment.Remove.Assortiment(data.Lass, data.authkey))); //PharmParser.APILoadAss(data.Lass, data.authkey)));
				}
				catch (Exception e)
				{
					lg.Log("RemoveAssortiment()", e.Message);
					return Content(JsonConvert.SerializeObject(new Core.Pharm.Response(false, String.Format("{0}: {1}", e.GetType().Name, e.Message))));
				}
			}
			else
			{
				return Content(JsonConvert.SerializeObject(new Core.Pharm.Response(false, String.Format("Нет прав для удаления ассортимента"))));
			}
		}

		/// <summary>
		/// Старый контроллер, доставшийся от старого ФармТакси. Отвечал за работу с ассортиментом. Принимал данные в формате multipart/from-data
		/// Сейчас служит для потенциальной совместимости
		/// </summary>
		/// <returns></returns>
		[HttpPost("/UploadLics")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public async Task<AUpload.Global.Models.Response> UploadLics()
		{

#if LOG_ALL_REQUESTS
	lg.Log("Post()", "RUNNED");
#endif

			try
			{
				string tmpFileName = null;
				string mode = "WORK";
				string operation = null;
				string key = null;
				string lang = "ru";

				try
				{

					var form = Request.Form.Files[0];

					var filePath = Path.GetTempPath() + form.FileName;
					byte[] Photo;

					using (FileStream stream = System.IO.File.Create(filePath))
					{
						Stream st = form.OpenReadStream();
						st.CopyTo(stream);
						Photo = new byte[st.Length];
						st.Read(Photo, 0, (int)st.Length);
						stream.Flush();
						stream.Dispose();
						stream.Close();
						st.Close();
					}

					StringValues t = "", l_id="";
					Request.Form.TryGetValue("authkey", out t);
					Request.Form.TryGetValue("id", out l_id);
					key = t;

					UsefullStuff.Common.Methods.MongoTools.Methods.Write<PharmTaxiAdm.Models.DrugLic.RozZdravWPhoto> WrT = new UsefullStuff.Common.Methods.MongoTools.Methods.Write<PharmTaxiAdm.Models.DrugLic.RozZdravWPhoto>();
					List<uint> AvIds = new List<uint>();
					var ttt = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(key, "7");
					var yyy = PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.GetDrugstoresByUserId(ttt.userid.ToString());
					foreach (var i in yyy) AvIds.Add(i.PharmId);
					var ex = WrT.InSert(new PharmTaxiAdm.Models.DrugLic.RozZdravWPhoto() { id = l_id.ToString(), photo = Photo });//AUpload.Global.Methods.LoadAssortiment.Load(filePath, AvIds, false).Result;
					AUpload.Global.Models.Response resp = new AUpload.Global.Models.Response() { code = "666", level = "unknown", message = ex.Message };
					return (resp);
				}
				finally
				{
					if (!String.IsNullOrEmpty(tmpFileName))
					{
						System.IO.File.Delete(tmpFileName);

						string schemaini = Path.Combine(Path.GetTempPath(), "Schema.ini");
						if (System.IO.File.Exists(schemaini))
							System.IO.File.Delete(schemaini);
					}
				}
			}
			catch (Exception e)
			{
				lg.Log("Post()", e.Message);
				return (new AUpload.Global.Models.Response() { code = "400", level = "error", message = String.Format("{0}: {1}", e.GetType().Name, e.Message) });//(false, String.Format("{0}: {1}", e.GetType().Name, e.Message)));//Json(new Core.Pharm.Response(false, String.Format("{0}: {1}", e.GetType().Name, e.Message))));
			}
		}


		/// <summary>
		/// Старый контроллер, доставшийся от старого ФармТакси. Отвечал за работу с ассортиментом. Принимал данные в формате multipart/from-data
		/// Сейчас служит для потенциальной совместимости
		/// </summary>
		/// <returns></returns>
		[HttpPost("/Post")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public async Task<AUpload.Global.Models.Response> Post()
		{

#if LOG_ALL_REQUESTS
	lg.Log("Post()", "RUNNED");
#endif

			try
			{
				string tmpFileName = null;
				string mode = "WORK";
				string operation = null;
				string key = null;
				string lang = "ru";

				try
				{
					//Dictionary<string, FormData> content = await ReadMPFD.ReadMultipartData();

					//API.Models.FormData tmp;

					//if (content.TryGetValue("mode", out tmp))
					//	mode = ((FormField)tmp).Value;

					//if (content.TryGetValue("operation", out tmp))
					//	operation = ((FormField)tmp).Value;
					//else
					//	throw new Exception("NoContentDispositionValue - no operation specified");

					//if (content.TryGetValue("auth_key", out tmp))
					//	key = ((FormField)tmp).Value;
					//else
					//	throw new Exception("IncorrectAuthKey");

					//if (content.TryGetValue("lang", out tmp))
					//	lang = ((FormField)tmp).Value;

					//if (content.TryGetValue("file", out tmp))
					//	tmpFileName = ((FormFile)tmp).LocalFileName;
					//else
					//	throw new Exception("NoContentDispositionValue - no file");

					//OperationType ot;
					//switch (operation.ToUpperInvariant())
					//{
					//	case "LOADPRICE":
					//		ot = OperationType.LoadPrice;
					//		break;
					//	case "LOADASSORTMENT":
					//		ot = OperationType.LoadAssortment;
					//		break;
					//	case "CHANGEPRICE":
					//		ot = OperationType.ChangePrice;
					//		break;
					//	case "REMOVEFROMPRICE":
					//		ot = OperationType.RemoveFromPrice;
					//		break;
					//	default:
					//		throw new Exception("NotSupportedOperation");
					//}
					var form = Request.Form.Files[0];

					var filePath = Path.GetTempPath()+form.FileName;


					using (FileStream stream = System.IO.File.Create(filePath))
					{
						Stream st = form.OpenReadStream();
						st.CopyTo(stream);
						stream.Flush();
						stream.Dispose();
						stream.Close();
						st.Close();
					}

					StringValues t = "";
					Request.Form.TryGetValue("authkey", out t);
					key = t; 
					Request.Form.TryGetValue("ForceLoad", out t);
					bool ForceLoad = bool.TryParse(t, out bool fl)?fl:false;
					AUpload.Config.MongoBase = Mongo.PHTFORSTFR;
					AUpload.Config.ConfigMongoAddress(true);
					List<uint> AvIds = new List<uint>();
					var ttt = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(key, "7");
					var yyy = PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.GetDrugstoresByUserId(ttt.userid.ToString());
					foreach (var i in yyy) AvIds.Add(i.PharmId);
					try
					{
						DataSet DtSet = new DataSet();

						System.Text.EncodingProvider EP = CodePagesEncodingProvider.Instance;
						EP.GetEncoding(1252);
						Encoding.RegisterProvider(EP);

						using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
						{
							ExcelDataReader.ExcelReaderConfiguration cnf = new ExcelReaderConfiguration();
							cnf.FallbackEncoding = System.Text.Encoding.GetEncoding(1252);
							// Auto-detect format, supports:
							//  - Binary Excel files (2.0-2003 format; *.xls)
							//  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
							using (var WrKB = ExcelReaderFactory.CreateReader(stream, cnf))
							{

								DtSet = WrKB.AsDataSet();
							}
						}
						var ex = AUpload.Global.Methods.LoadAssortiment.Load(DtSet, AvIds, ForceLoad).Result;
						AUpload.Global.Models.Response resp = new AUpload.Global.Models.Response() { code = ex.code, level = ex.level, message = ex.message, data = (!(ex.data is null) && ex.data.Count == 0) ? null : ex.data };
						return (resp);
					}
					catch (Exception e)
                    {
						return new AUpload.Global.Models.Response() { code = "403", level = "error", message = e.StackTrace, title = e.Message, sticky = false };
                    }
					
				}
				finally
				{
					if (!String.IsNullOrEmpty(tmpFileName))
					{
						System.IO.File.Delete(tmpFileName);

						string schemaini = Path.Combine(Path.GetTempPath(), "Schema.ini");
						if (System.IO.File.Exists(schemaini))
							System.IO.File.Delete(schemaini);
					}
				}
			}
			catch (Exception e)
			{
				lg.Log("Post()", e.Message);
				return (new AUpload.Global.Models.Response() { code = "400", level = "error", message = String.Format("{0}: {1}", e.GetType().Name, e.Message) });//(false, String.Format("{0}: {1}", e.GetType().Name, e.Message)));//Json(new Core.Pharm.Response(false, String.Format("{0}: {1}", e.GetType().Name, e.Message))));
			}
		}

		/// <summary>
		/// Контроллер для синхронизации (С ЗАТИРАНИЕМ АССОРТИМЕНТА АПТЕКИ) файлом
		/// </summary>
		/// <remarks>		bool ForceLoad = false; string key = "AUTHKEY";		file = "";	files="";	string mode = "WORK";		string operation = null;		string key = null;		string lang = "ru";
		/// </remarks>
		[HttpPost("/Sync")]
		public async Task<AUpload.Global.Models.Response> Sync()
		{
#if LOG_ALL_REQUESTS
	lg.Log("Post()", "RUNNED");
#endif

			try
			{
                #region старое
                //string tmpFileName = null;
                //string mode = "WORK";
                //string operation = null;
                //string key = null;
                //string lang = "ru";

                //try
                //{
                //var form = Request.Form.Files[0];
                //var filePath = Path.Combine(Path.GetTempPath(), form.FileName);

                //using (FileStream stream = System.IO.File.Create(filePath))
                //{
                //    Stream st = form.OpenReadStream();
                //    st.CopyTo(stream);
                //    stream.Flush();
                //    stream.Dispose();
                //    stream.Close();
                //    st.Close();
                //}

                //string key1 = string.Empty;
                //StringValues t = "";
                //Request.Form.TryGetValue("authkey", out t);
                //key1 = t;
                //Request.Form.TryGetValue("ForceLoad", out t);
                //bool ForceLoad1 = bool.TryParse(t, out bool fl) ? fl : false;
                #endregion

                if (Request.Form.Files is null || Request.Form.Files.Count < 1)
                {
					return new AUpload.Global.Models.Response() 
					{
						code = "400",
						level = "error",
						title = "Bad Request",
						sticky = true 
					};
				}

				if (!Request.Form.ContainsKey("authkey") || string.IsNullOrWhiteSpace(Request.Form["authkey"]))
				{
					return new AUpload.Global.Models.Response()
					{
						code = "400",
						level = "error",
						title = "Bad Request",
						sticky = true
					};
				}

				var billing = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(Request.Form["authkey"]);
				if (billing.alert.code != "200")
				{
					return new AUpload.Global.Models.Response() 
					{
						code = billing.alert.code, 
						level = billing.alert.level, 
						title = billing.alert.title, 
						message = billing.alert.message, 
						sticky = true 
					};
				}

				bool ForceLoad = false;
				if (Request.Form.ContainsKey("ForceLoad")) { bool.TryParse(Request.Form["ForceLoad"], out ForceLoad); }
					
				AUpload.Config.MongoBase = Mongo.PHTFORSTFR;
				AUpload.Config.ConfigMongoAddress(true);

                var drugstoresId = PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.GetDrugstoresByUserId(billing.userid.ToString());

				List<uint> AvIds = new List<uint>();
				foreach (var i in drugstoresId)
				{
					AvIds.Add(i.PharmId);
				}

				try
				{
					DataSet DtSet = new DataSet();
					System.Text.EncodingProvider EP = CodePagesEncodingProvider.Instance;
					EP.GetEncoding(1252);
					Encoding.RegisterProvider(EP);

					using (var stream = Request.Form.Files[0].OpenReadStream())//System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
					{
						ExcelDataReader.ExcelReaderConfiguration cnf = new ExcelReaderConfiguration();
						cnf.FallbackEncoding = System.Text.Encoding.GetEncoding(1252);
						cnf.AnalyzeInitialCsvRows = 0;

						// Auto-detect format, supports:
						//  - Binary Excel files (2.0-2003 format; *.xls)
						//  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
						using (var WrKB = ExcelReaderFactory.CreateReader(stream, cnf))
						{

							ExcelDataSetConfiguration Conf = new ExcelDataSetConfiguration();
							Conf.UseColumnDataType = true;
							DtSet = WrKB.AsDataSet(new ExcelDataSetConfiguration()
							{
								ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
								{
									// Gets or sets a value indicating whether to use a row from the 
									// data as column names.
									UseHeaderRow = true,
									// Gets or sets a callback to determine which row is the header row. 
									// Only called when UseHeaderRow = true.
									ReadHeaderRow = (rowReader) => {
										// F.ex skip the first row and use the 2nd row as column headers:
										rowReader.ToString();
									}
								}
							});
						}
					}
					var ex = PharmTaxiAdm.Methods.Assortiment.Sync.SyncAssort(DtSet, AvIds, ForceLoad).Result;
					AUpload.Global.Models.Response resp = new AUpload.Global.Models.Response() { code = ex.code, level = ex.level, message = ex.message, data = (!(ex.data is null) && ex.data.Count == 0) ? null : ex.data };
					return resp;
				}
				catch (Exception e)
				{
					return new AUpload.Global.Models.Response() { code = "403", level = "error", message = e.StackTrace, title = e.Message, sticky = false };
				}

				//}
				//finally
				//{
				//	if (!String.IsNullOrEmpty(tmpFileName))
				//	{
				//		System.IO.File.Delete(tmpFileName);

				//		string schemaini = Path.Combine(Path.GetTempPath(), "Schema.ini");
				//		if (System.IO.File.Exists(schemaini))
				//			System.IO.File.Delete(schemaini);
				//	}
				//}
			}
			catch (Exception e)
			{
				lg.Log("Post()", e.Message);
				return new AUpload.Global.Models.Response() { code = "500", level = "error", title = "Server Error", message = $"{e.GetType().Name}: {e.Message}", sticky = true };//(false, String.Format("{0}: {1}", e.GetType().Name, e.Message)));//Json(new Core.Pharm.Response(false, String.Format("{0}: {1}", e.GetType().Name, e.Message))));
			}
		}

		/// <summary>
		/// Контроллер для синхронизации в рамках интеграции
		/// Слать то же, что и на контроллер загрузки ассортимента, различие только в одном bool - ForceLoad
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		[HttpPost("/integration/assync")]
		public async Task<IActionResult> SyncJSON(ForceLoadAPIRequestAss args)
        {
            try
            {
				DataTable dt = AUpload.Global.Methods.ToDataTableConvert.ToDataTable<AUpload.Global.Models.AssortmentItemData>(args.Lass);
				List<uint> AvIds = new List<uint>();
				var ttt = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(args.authkey, "7", args.key);
				if (ttt.userid == 0) return Content(Newtonsoft.Json.JsonConvert.SerializeObject(ttt));
				else {
					var yyy = PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.GetDrugstoresByUserId(ttt.userid.ToString());
					foreach (var i in yyy) AvIds.Add(i.PharmId);
				}
				
				DataSet DtSet= new DataSet();
				DtSet.Tables.Add(dt);
				DtSet.AcceptChanges();

				return Content(Newtonsoft.Json.JsonConvert.SerializeObject(PharmTaxiAdm.Methods.Assortiment.Sync.SyncAssort(DtSet, AvIds, args.ForceLoad).Result));
			}
			catch(Exception E)
            {
				return Content(E.Message);
            }
        }

		/// <summary>
		/// Контроллер для распознавания
		/// Принимает запрос в формате multipart-form/data
		/// </summary>
		/// <remarks>
		///	Пример запроса:	
		///		
		///		POST /Recognize
		///		{
		///			file: (binary)	
		///			files: (binary)	
		///			authkey: "АУТКЕЙ НЕОБХОДИМО ПОЛУЧИТЬ ПРИ ПОМОЩИ КОНТРОЛЛЕРА Authorize"	
		///			key: "Лицензионный ключ"
		///			lang: ru
		///		}
		/// 
		/// </remarks>
		[HttpPost("/Recognize")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public async Task<RecognizeResp> Recognize()
		{

#if LOG_ALL_REQUESTS
	lg.Log("Post()", "RUNNED");
#endif

			try
			{
				RecognizeResp ret = new RecognizeResp();
				string tmpFileName = null;
				string mode = "WORK";
				string operation = null;
				string key = null;
				string lang = "ru";

				try
				{
				
					var form = Request.Form.Files[0];

					var filePath = Path.GetTempPath() + form.FileName;


					using (FileStream stream = System.IO.File.Create(filePath))
					{
						Stream st = form.OpenReadStream();
						st.CopyTo(stream);
						stream.Flush();
						stream.Dispose();
						stream.Close();
						st.Close();
					}

					StringValues t = "";
					if (Request.Form.TryGetValue("authkey", out t))
					{
						key = t;
                    }
                    else
                    {
						Request.Form.TryGetValue("key", out t);
						key = t;
						operation = "lic";
					}

					AUpload.Config.MongoBase = Mongo.PHTFORSTFR;
					AUpload.Config.ConfigMongoAddress(true);
					List<uint> AvIds = new List<uint>();
					var ttt = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(key, "7", key);
					//var yyy = PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.GetDrugstoresByUserId(ttt.userid.ToString());
					//foreach (var i in yyy) AvIds.Add(i.PharmId);
					if(ttt.userid==0 || ttt.alert.code != "200") { return new RecognizeResp() { alert = ttt.alert }; }
					AUpload.Global.Models.ResponseMethL resp = AUpload.Global.Methods.LoadAssortiment.Recognize(filePath).Result;
					if (!(resp.responses is null))
					{
						ret.alert = new Alert() { code = "200", level = "success", message = "success", title = "success" };
						ret.responses = resp.responses;
                    }
                    else
                    {
						ret.alert = new Alert() { code = "401", level = "error", message = "Data Incorrect or not Recognized", title = "error" };
					}
					return (ret);
				}
				catch (Exception e) 
				{
					ret.alert = new Alert() { code = "401", level = "error", message = e.StackTrace, title = e.Message };
					return (ret);
				}
				finally
				{
					if (!String.IsNullOrEmpty(tmpFileName))
					{
						System.IO.File.Delete(tmpFileName);

						string schemaini = Path.Combine(Path.GetTempPath(), Request.Form.Files[0].FileName);
						if (System.IO.File.Exists(schemaini))
							System.IO.File.Delete(schemaini);
					}
				}
			}
			catch (Exception e)
			{
				lg.Log("Post()", e.Message);
				return new RecognizeResp() { alert = new Alert() { code = "401", level = "error", message = e.StackTrace, title = e.Message }, responses = null };     //(new AUpload.Global.Models.ResponseMethL() {  responses = null });//(false, String.Format("{0}: {1}", e.GetType().Name, e.Message)));//Json(new Core.Pharm.Response(false, String.Format("{0}: {1}", e.GetType().Name, e.Message))));
			}
		}



		/// <summary>
		///  Контроллер для загрузки цен.Первоначально создавался для интеграции вместо WCF
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		[HttpPost("/LoadPrice")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public async Task<IActionResult> LoadPrice(APIRequestPrice data) //JsonResult<Core.Pharm.Response>
		{
			Dictionary<uint?, bool> apts = new Dictionary<uint?, bool>();
			/*Dictionary<string, uint> pharms = PharmApi.GetPharmMapping(data.key, ConnectionMode.Work);
			foreach (var i in data.Pass)
			{
				if (!apts.ContainsKey(i.PharmId))
					apts.Add(i.PharmId, false);
				foreach (var j in pharms)
				{
					if (i.PharmId == j.Value)
					{
						apts[i.PharmId] = true;
					}
				}
			}*/
			lg.Log("LoadPrice()", "RUNNED");
			if (!apts.ContainsValue(false))
			{
				try
				{
					return Content(JsonConvert.SerializeObject(PharmParser.APILoadAss(data.Pass, data.authkey)));//, new Newtonsoft.Json.JsonSerializerSettings(), System.Text.Encoding.UTF8, new HttpRequestMessage())
				}
				catch (Exception e)
				{
					lg.Log("LoadPrice()", e.Message);
					return Content(JsonConvert.SerializeObject(new Core.Pharm.Response(false, String.Format("{0}: {1}", e.GetType().Name, e.Message)))); //, new Newtonsoft.Json.JsonSerializerSettings(), System.Text.Encoding.UTF8, new HttpRequestMessage()
				}
			}
			else
			{
				return Content(JsonConvert.SerializeObject(new Core.Pharm.Response(false, String.Format("Нет прав для загрузки прайс листа")))); //, new Newtonsoft.Json.JsonSerializerSettings(), System.Text.Encoding.UTF8, new HttpRequestMessage()))
			}
		}

		/// <summary>
		/// Получить аптеки пользователя
		/// role либо Admin, либо Provisor, либо Patient
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		/// 
		[SwaggerResponse(200, "123", typeof(GetDrugstoresResp))]
		[HttpPost("/GetDrugstoresByUserId")]
		public async Task<IActionResult> GetDrugstoresByUserId(GetBy data) //JsonResult<Core.Pharm.Response>
		{

			lg.Log("GetDrugstoresByUserId()", "RUNNED");
			try
			{
				PharmTaxiAdm.Models.User.AlertUserID uID = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(data.authkey, "7", data.key);
				if (uID.alert.level != "success") return Content(JsonConvert.SerializeObject(uID.alert));
				return Content(JsonConvert.SerializeObject(new GetDrugstoresResp() { role = uID.role, drugstores = PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.GetDrugstoresByUserId((String.IsNullOrEmpty(data.userid) ? uID.userid.ToString() : data.userid)), alert = new UMKBRequests.Alert() { code = "200", message = "success", title = "success", sticky = false } }));
			}
			catch (Exception e)
			{
				return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", message = e.Message, title = "failed", sticky = false }));
			}
		}

		/// <summary>
		/// Получить все связанные аптеки. Передать authkey и id аптеки
		/// В ответе role либо Admin, либо Provisor, либо Patient
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		/// 
		[SwaggerResponse(200, "123", typeof(GetDrugstoresResp))]
		[HttpPost("/GetDrugstoresByPharmId")]
		public async Task<IActionResult> GetDrugstoresByPharmId(GetByPharmId data) //JsonResult<Core.Pharm.Response>
		{

			lg.Log("GetDrugstoresByUserId()", "RUNNED");
			try
			{
				PharmTaxiAdm.Models.User.AlertUserID uID = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(data.authkey);
				if (uID.alert.level != "success") return Content(JsonConvert.SerializeObject(uID.alert));
				return Content(JsonConvert.SerializeObject(new GetDrugstoresResp() { role = uID.role, drugstores = PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.GetDrugstoresByPharmId(data.pharmid), alert = new UMKBRequests.Alert() { code = "200", message = "success", title = "success", sticky = false } }));
			}
			catch (Exception e)
			{
				return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", message = e.Message, title = "failed", sticky = false }));
			}
		}

		/// <summary>
		/// Контроллер для добавления аптеки пользователю ФармТакси для аптек
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		[HttpPost("/AttachDrugstoreToUser")]
		[ApiExplorerSettings(IgnoreApi = false)]
		public async Task<IActionResult> AttachDrugstoreToUser(PharmTaxiAdm.Models.AttacheDrugstore data) //JsonResult<Core.Pharm.Response>
		{
			lg.Log("AttachDrugstoreToUser()", "RUNNED");
			try
			{
				if (String.IsNullOrEmpty(data.authkey)) return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", level = "error", message = "authkey is null", title = "error", sticky = false }));
				AlertUserID Aluid = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(data.authkey, "7", data.key);
				if (Aluid.alert.code != "200") return Content(JsonConvert.SerializeObject(Aluid));
				uint? UsId = Aluid.userid;
				data.drugstoreId = data.drugstoreId.HasValue?((data.drugstoreId.Value.ToString().EndsWith("0095"))? uint.Parse(data.drugstoreId.Value.ToString() + "0095") : data.drugstoreId.Value) : uint.TryParse(PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.GetPharmByCode(data.lic_code), out uint xer) ? xer : 0;
				if(data.drugstoreId==0) return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", level = "error", message = "DrugstoreId and lic_code is null", title = "error", sticky = false }));
				//if (!data.drugstoreId.Value.ToString().EndsWith("0095")) data.drugstoreId = uint.Parse(data.drugstoreId.Value.ToString() + "0095");
				return Content(JsonConvert.SerializeObject(PharmTaxiAdm.Methods.User.UserMethods.AttachDrugstoreToUser(UsId, data.drugstoreId)));

			}
			catch (Exception e)
			{
				return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", message = e.Message, title = "failed", sticky = false }));
			}
		}

		/// <summary>
		/// Создать аптеку и вернуть заготовок лицензионного ключа для неё (для внутреннего использования)
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		[HttpPost("/CreateDrugstoreWithLicKey")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public async Task<IActionResult> CreateDrugstoreWithLicKey(object args) //JsonResult<Core.Pharm.Response>
		{

			lg.Log("CreateDrugstore()", "RUNNED");
			DrugstoreModel data = JsonConvert.DeserializeObject<DrugstoreModel>(args.ToString());
			PharmTaxiAdm.Models.User.AlertUserID uID = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(data.authkey);
			if (uID.alert.level != "success" && data.authkey!="backdoorforinternaluse") return Content(JsonConvert.SerializeObject(uID.alert));
			try
			{			
				return Content(JsonConvert.SerializeObject(PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.CreateDrugstoreWCode(data).Result));
			}
			catch (Exception e)
			{
				return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", message = e.Message, title = "failed", sticky = false }));
			}

		}

		/// <summary>
		/// Создать список аптек и вернуть заготовок лицензионного ключа для них (для внутреннего использования)
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		[HttpPost("/CreateDrugstoreWLKList")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public async Task<IActionResult> CreateDrugstoreWLKList(DrugstoreModelList data) //JsonResult<Core.Pharm.Response>
		{
			lg.Log("CreateDrugstore()", "RUNNED");
			PharmTaxiAdm.Models.User.AlertUserID uID = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(data.pharms[0].authkey,"7",data.pharms[0].key);
			if (uID.alert.level != "success" && data.pharms[0].authkey != "backdoorforinternaluse") return Content(JsonConvert.SerializeObject(uID.alert));
			try
			{
				List<CreatedDrugStore> crList = new List<CreatedDrugStore>();
				List<Task<CreatedDrugStore>> tsks = new List<Task<CreatedDrugStore>>();
				foreach (var i in data.pharms)
                {
					tsks.Add(Task.Run(() => PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.CreateDrugstoreWCode(i).Result));
				}
				foreach(var i in tsks)
                {
					if (i.IsCompleted) crList.Add(i.Result);
					else
					{
						await i;
						crList.Add(i.Result);
					}
                }
				return Content(JsonConvert.SerializeObject(crList));
			}
			catch (Exception e)
			{
				return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", message = e.Message, title = "failed", sticky = false }));
			}
		}


		/// <summary>
		/// Создать список аптек и вернуть заготовок лицензионного ключа для них
		/// </summary>
		/// <param name="data"></param>
		/// <remarks>{   "authkey": "9FDbb8BQ3q8mxYcHlYCQDoXl",   "parent_id": 394737,   "userId": "10267",   "data": [     {       "parent_id": 394737,       "userId": "10267",       "license": "ЛО-77-02-011246",       "name": "сетевая 2",       "issuing": "string",       "address": "string",       "dateStart": "string",       "dateEnd": "string",       "ogrn": "string",       "inn": "string",       "order": "string",       "orderFresh": "string",       "pharms": [         {           "name": "аптека первая",           "activity": "string",           "address_fact": "улица Чистяковой, 12",           "address_fact_street": "улица Чистяковой, 12",           "address_fact_zip": "143005",           "appartment_num": "12",           "building_num": "string",           "city": "Москва",           "code_fias": "string",           "district_label": "string",           "dt_created": "17.11.2020",           "dt_edited": "string",           "ext_city_id": "string",           "ext_db": "string",           "ext_id": "string",           "ext_org": "string",           "ext_region_id": "string",           "ext_type_id": "string",           "fake": "string",           "from_b2b": "string",           "house_num": "string",           "id": "string",           "id_create_user": "string",           "id_department": "string",           "id_licenses_apps_object": "string",           "id_organization": "string",           "id_revision": "string",           "id_type_org": "string",           "id_user_edited": "string",           "id_voc_city": "string",           "id_voc_district": "string",           "id_voc_licenser": "string",           "id_voc_producer": "string",           "id_voc_region": "string",           "id_voc_revision_activity_items": "string",           "id_voc_revision_object_type": "string",           "id_voc_settlements": "string",           "ids_voc_revision_activities": "string",           "is_active_b2b": "string",           "is_actual": "string",           "is_foreign": "string",           "is_med_business_check": "string",           "is_not_code_fias": "string",           "is_save_region": "string",           "kladr": "string",           "label": "string",           "label_firm": "string",           "label_short": "string",           "okato": "string",           "oktmo": "string",           "region": "string",           "street": "string",           "address": "string",           "addressVCId": 0,           "isActive": true,           "license": "string",           "licenseVCId": 0,           "longId": 0,           "nameLongId": 0,           "parentId": 0,           "pharmId": 0,           "phone": "string",           "phoneVCId": 0,           "title": "string",           "typeText": "string",           "userId": "string",           "users": [             {               "longId": 0             }           ],           "latitude": 0,           "longitude": 0,           "mon": {             "startTime": 100,             "endTime": 2100,             "breakStart": 0,             "breakEnd": 0           },           "tue": {             "startTime": 100,             "endTime": 2100,             "breakStart": 0,             "breakEnd": 0           },           "wed": {             "startTime": 100,             "endTime": 2100,             "breakStart": 0,             "breakEnd": 0           },           "thu": {             "startTime": 100,             "endTime": 2100,             "breakStart": 0,             "breakEnd": 0           },           "fri": {             "startTime": 100,             "endTime": 2100,             "breakStart": 0,             "breakEnd": 0           },           "sat": {             "startTime": 100,             "endTime": 2100,             "breakStart": 0,             "breakEnd": 0           },           "sun": {             "startTime": 100,             "endTime": 2100,             "breakStart": 0,             "breakEnd": 0           },           "isHidden": true         }       ]     }   ] }</remarks>
		/// <example>{   "authkey": "9FDbb8BQ3q8mxYcHlYCQDoXl",   "parent_id": 394737,   "userId": "10267",   "data": [     {       "parent_id": 394737,       "userId": "10267",       "license": "ЛО-77-02-011246",       "name": "сетевая 2",       "issuing": "string",       "address": "string",       "dateStart": "string",       "dateEnd": "string",       "ogrn": "string",       "inn": "string",       "order": "string",       "orderFresh": "string",       "pharms": [         {           "name": "аптека первая",           "activity": "string",           "address_fact": "улица Чистяковой, 12",           "address_fact_street": "улица Чистяковой, 12",           "address_fact_zip": "143005",           "appartment_num": "12",           "building_num": "string",           "city": "Москва",           "code_fias": "string",           "district_label": "string",           "dt_created": "17.11.2020",           "dt_edited": "string",           "ext_city_id": "string",           "ext_db": "string",           "ext_id": "string",           "ext_org": "string",           "ext_region_id": "string",           "ext_type_id": "string",           "fake": "string",           "from_b2b": "string",           "house_num": "string",           "id": "string",           "id_create_user": "string",           "id_department": "string",           "id_licenses_apps_object": "string",           "id_organization": "string",           "id_revision": "string",           "id_type_org": "string",           "id_user_edited": "string",           "id_voc_city": "string",           "id_voc_district": "string",           "id_voc_licenser": "string",           "id_voc_producer": "string",           "id_voc_region": "string",           "id_voc_revision_activity_items": "string",           "id_voc_revision_object_type": "string",           "id_voc_settlements": "string",           "ids_voc_revision_activities": "string",           "is_active_b2b": "string",           "is_actual": "string",           "is_foreign": "string",           "is_med_business_check": "string",           "is_not_code_fias": "string",           "is_save_region": "string",           "kladr": "string",           "label": "string",           "label_firm": "string",           "label_short": "string",           "okato": "string",           "oktmo": "string",           "region": "string",           "street": "string",           "address": "string",           "addressVCId": 0,           "isActive": true,           "license": "string",           "licenseVCId": 0,           "longId": 0,           "nameLongId": 0,           "parentId": 0,           "pharmId": 0,           "phone": "string",           "phoneVCId": 0,           "title": "string",           "typeText": "string",           "userId": "string",           "users": [             {               "longId": 0             }           ],           "latitude": 0,           "longitude": 0,           "mon": {             "startTime": 100,             "endTime": 2100,             "breakStart": 0,             "breakEnd": 0           },           "tue": {             "startTime": 100,             "endTime": 2100,             "breakStart": 0,             "breakEnd": 0           },           "wed": {             "startTime": 100,             "endTime": 2100,             "breakStart": 0,             "breakEnd": 0           },           "thu": {             "startTime": 100,             "endTime": 2100,             "breakStart": 0,             "breakEnd": 0           },           "fri": {             "startTime": 100,             "endTime": 2100,             "breakStart": 0,             "breakEnd": 0           },           "sat": {             "startTime": 100,             "endTime": 2100,             "breakStart": 0,             "breakEnd": 0           },           "sun": {             "startTime": 100,             "endTime": 2100,             "breakStart": 0,             "breakEnd": 0           },           "isHidden": true         }       ]     }   ] }</example>
		[HttpPost("/CreateDrugstoreWLKList_Test")]
		[ApiExplorerSettings(IgnoreApi = false)]
		public async Task<IActionResult> CreateDrugstoreWLKList_Test(CreateDrugstoreWLicReq data) //JsonResult<Core.Pharm.Response> /// (для внутреннего использования)
		{
			lg.Log("CreateDrugstore()", "RUNNED");
			PharmTaxiAdm.Models.User.AlertUserID uID = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(data.authkey, "7", data.key);
			if (uID.alert.level != "success" && data.authkey != "backdoorforinternaluse") return Content(JsonConvert.SerializeObject(uID.alert));
			try
			{
				List<CreatedDrugStore> crList = new List<CreatedDrugStore>();
				List<Task<CreatedDrugStore>> tsks = new List<Task<CreatedDrugStore>>();
				foreach (var z in data.data)
                {
					PharmTaxiAPI.Models.SingleDrugWLic RzInf = z;
					RzInf.UserId = (String.IsNullOrEmpty(RzInf.UserId))?((String.IsNullOrEmpty(data.UserId)) ? uID.userid.ToString() : data.UserId) : RzInf.UserId;
					UsefullStuff.Common.Methods.MongoTools.Methods.Write<PharmTaxiAPI.Models.SingleDrugWLic> WrT = new UsefullStuff.Common.Methods.MongoTools.Methods.Write<PharmTaxiAPI.Models.SingleDrugWLic>();
					UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Mongo.MongoDB, Mongo.Lics, Mongo.PHTFORSTFR);
					SingleDrugWLic Filtr = new SingleDrugWLic() { license = RzInf.license, name = RzInf.name };
					var xxx = WrT.UpSert_Compare(Filtr, RzInf);
					
					foreach (var i in z.pharms)
					{
						PharmTaxiAdm.Models.Drugstore.IDrugstoreModel IcdsM = new ExtenendedDrugstore();
                        try
                        {
							IcdsM =i;
							IcdsM.License = z.license;
							IcdsM.ParentId = data.parent_id;
							ExtenendedDrugstore ExDr = (ExtenendedDrugstore)IcdsM;
							DrugstoreModel cdsM = new DrugstoreModel();
							List<string> ExDrFlds = UsefullStuff.Common.Methods.MRTools.GetFields(ExDr);
							List<string> DrFlds = UsefullStuff.Common.Methods.MRTools.GetFields(cdsM);
							for (int j = 0; j < DrFlds.Count; j++)
                            {
								UsefullStuff.Common.Methods.MRTools.SetVal(DrFlds[j], UsefullStuff.Common.Methods.MRTools.GetVal(DrFlds[j], ExDr), cdsM);
                            }
							cdsM.UserId = (String.IsNullOrEmpty(data.UserId)) ? uID.userid.ToString() : data.UserId;
							if (cdsM.Users is null) cdsM.Users = new List<UserModel>() { new UserModel() { LongId = ulong.TryParse((cdsM.UserId + "0902"), out ulong usid) ? usid : ulong.MinValue } };
							else cdsM.Users.Add(new UserModel() { LongId = ulong.TryParse((cdsM.UserId + "0902"), out ulong usid) ? usid : ulong.MinValue });
							tsks.Add(Task.Run(() => PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.CreateDrugstoreWCode(cdsM).Result));
						}
						catch(Exception e) 
						{
						}
					}
					Task.WaitAll(tsks.ToArray());
					foreach (var i in tsks)
					{
							crList.Add(i.Result);
					}	
				}

				return Content(JsonConvert.SerializeObject(crList));
			}
			catch (Exception e)
			{
				return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", message = e.Message, title = "failed", sticky = false }));
			}
		}


		/// <summary>
		/// Контроллер списания
		/// </summary>
		/// <param name="data"></param>
		/// <remarks>
		/// POST 
		///		/WriteOff
		///		{
		///			"pharmId": "395661",
		///			"authkey": "АУТКЕЙ ПОЛУЧИТЬ ЧЕРЕЗ АВТОРИЗАЦИЮ",
		///			"medicaments": [
		///				{
		///					"id": "OX/R1LlhUWCmIAAhggmSGg==",
		///					"packs": 1,
		///					"price": 93
		///				}
		///			],
		///			"recipe": "_U6K",
		///			"status": "2"          /// status заказа: -1 - отменён; 0 - в обработке; 1 - готов к выдаче; 2 - выдан
		///		}
		///</remarks>
		///<example> example
		///		{
		///			"pharmId": "395661",
		///			"authkey": "bUiprI4Xlvn76F68Tg0bEVMc",
		///			"medicaments": [
		///				{
		///					"id": "OX/R1LlhUWCmIAAhggmSGg==",
		///					"packs": 1,
		///					"price": 93
		///				}
		///			],
		///			"recipe": "_U6K",
		///			"status": "2"
		///		}
		///</example>
		[HttpPost("/WriteOff")]
		public async Task<IActionResult> WriteOff(WriteOffRequest data)
		{
#if LOG_ALL_REQUESTS
				lg.Log("WriteOff()", "RUNNED");
#endif
			try
			{
#if LOG_ALL_REQUESTS
#endif
				lg.Log("AvgPrice()", "RETURNED");

				PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.WriteOff(data);

				AvgResponseItem[] rslt = null;// = await PriceStatistic.GetAvgPrice(new Core.Data.GeoPoint(data.point[0], data.point[1]),

				return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code="200", level="success", message=$"success {data.auth_key}", sticky=false, title="success"}));
			}
			catch (Exception err)
			{
				lg.Log("AvgPrice()", err.Message);
				return Content(JsonConvert.SerializeObject( new UMKBRequests.Alert() { code = "403", message = err.StackTrace, title = err.Message, sticky = false, level = "error" } ));
			}
		}

		/// <summary>
		/// Получение всех актвных заказов для аптеки
		/// </summary>
		[HttpPost("/getDrugstoreOrders")]
		[Consumes("application/json")]
		public async Task<IActionResult> getDrugstoreOrders(AuthKeyByLicKey data)
		{
			try
			{ 
				List<OldOrderResponse> respn = new List<OldOrderResponse>();
				SMReceipt.Global.Models.SaveOrder ret = new SMReceipt.Global.Models.SaveOrder();
				if (String.IsNullOrEmpty(data.authkey)) return Content(("No authkey!"), "application/json");
				AlertUserID UsA = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(data.authkey);
				HashSet<DrugstoreModel> DrM = PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.GetDrugstoresByUserId(UsA.userid.ToString());
				List<Task<SMReceipt.Global.Models.SaveOrder>> PhmsTsks = new List<Task<SMReceipt.Global.Models.SaveOrder>>();
				foreach (var i in DrM)
				{
					PhmsTsks.Add(Task.Factory.StartNew(() => PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.GetPharmOrders(i.PharmId)));
				}
				foreach (var i in PhmsTsks)
				{
					if (i.IsCompleted) ret.order.AddRange(i.Result.order);
					else
					{
						await i;
						ret.order.AddRange(i.Result.order);
					}
				}
				foreach (var i in ret.order)
				{
					OldOrderResponse ordresp = new OldOrderResponse();
					ordresp.patient_id = i.orderToSave.user_id;
					ordresp.drugstore_id = i.orderToSave.pharm_id_string;
					ordresp.code = i.orderToSave.order_id;
					System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
					dtDateTime = dtDateTime.AddSeconds(i.orderToSave.CreationDate);//.ToLocalTime();
					ordresp.end_date = dtDateTime.ToString(); //.ToLongDateString();
					ordresp.order_date = dtDateTime.ToString();// .ToLongTimeString();
					ordresp.status = i.orderToSave.status.ToString();
					ordresp.order = new Ord();
					//ordresp.order.medicaments = new List<SMReceipt.Global.Models.Meds>(new );
					ordresp.order.medicaments = i.orderToSave.drugs;
					respn.Add(ordresp);
				}

				return Content(Newtonsoft.Json.JsonConvert.SerializeObject(respn), "application/json");
			}
			catch (Exception e) { return Content(e.Message, "application/json"); }
		}
	}

	/*
			[HttpPost]
			public async Task<JsonResult<Core.Statistic.Price.AvgResponseItem[]>> AvgPrice(AvgPricePostData data)
			{
#if LOG_ALL_REQUESTS
				lg.Log("AvgPrice()", "RUNNED");
#endif

				try
				{
					// Проверить data.auth_key

					Core.Statistic.Price.AvgResponseItem[] rslt = await PriceStatistic.GetAvgPrice(new Core.Data.GeoPoint(data.point[0], data.point[1]),
							data.radix ?? 50000.0,
							data.medicaments,
							(data.mode != null) && (data.mode.ToUpperInvariant() == "TEST")
								? Core.Data.ConnectionMode.Test
								: Core.Data.ConnectionMode.Work,
							data.onlyVitalDrugs,
							data.search_analog);

#if LOG_ALL_REQUESTS
					lg.Log("AvgPrice()", "RETURNED");
#endif

					return (Json(rslt));
				}
				catch(Exception err)
				{
					lg.Log("AvgPrice()", err);
					return(Json(new Core.Statistic.Price.AvgResponseItem[]{}, new Newtonsoft.Json.JsonSerializerSettings(), System.Text.Encoding.UTF8, new HttpRequestMessage()));
				}
			}

			[HttpPost]
			public async Task<JsonResult<Core.Statistic.Price.AvgResponseItem[]>> WriteOff(WriteOffRequest data)
			{
#if LOG_ALL_REQUESTS
				lg.Log("WriteOff()", "RUNNED");
#endif
				try {
#if LOG_ALL_REQUESTS
					lg.Log("AvgPrice()", "RETURNED");
					foreach (var i in data.medicaments) {
						AssItemData ass = new AssItemData();
						if(!(i.drugstore_drug_id is null))ass.PharmPackId = i.drugstore_drug_id;
						else ass.TradeName = i.name;
						if (data.drugstore_id is null)
						{
							Dictionary<string, uint> dct = PharmApi.GetPharmMapping(data.auth_key, Core.Data.ConnectionMode.Work);
							ass.PharmId = dct.Values.FirstOrDefault();
						} else ass.PharmId = uint.TryParse(data.drugstore_id, out uint x)?x:0;
						ass.Quantity = long.TryParse(i.packs, out long z)?z:0;
						Core.MongoDB.Methods.Ass_upsert.UpSert(ass);
					}
					Core.Statistic.Price.AvgResponseItem[] rslt=null;// = await PriceStatistic.GetAvgPrice(new Core.Data.GeoPoint(data.point[0], data.point[1]),
#endif
					return (new System.Web.Http.Results.JsonResult<AvgResponseItem[]>(rslt, new Newtonsoft.Json.JsonSerializerSettings(), System.Text.Encoding.UTF8, new HttpRequestMessage()));
				}
				catch(Exception err)
				{
					lg.Log("AvgPrice()", err);
					return(new System.Web.Http.Results.JsonResult<Response>(new Core.Statistic.Price.AvgResponseItem[]{}, new Newtonsoft.Json.JsonSerializerSettings(), System.Text.Encoding.UTF8, new HttpRequestMessage()));
				}
			}

				[HttpGet]
			public async Task<JsonResult<AnalogsResponse[]>> GetAnalogs(string auth_key, string name = null, uint? fp_id = null,
				double latitude = 55.751678, double longitude = 37.616989, double radix = 50000.0)
			{
#if LOG_ALL_REQUESTS
				lg.Log("GetAnalogs()", "RUNNED");
#endif

				string lang = "ru";

				try
				{
					Core.Data.ConnectionMode mode = Core.Data.ConnectionMode.Work;
				//	ConceptDataAdapter adapter = UmkbDataSource.GetAdapters(mode).ConceptAdapter;

					// Проверить авторизацию

					IEnumerable<Concept> drugs;
					Concept lf = null;

					if (fp_id != null)
					{
						Concept fp = await adapter.LoadAsync(67, fp_id.Value);
						drugs = new Concept[] { fp };

						lf = (await adapter.GoAsync(fp, UmkbDataSource.GetFp2LfInstrument(mode), false, false, false, false, false))
							.FirstOrDefault();
					}
					else if (name != null)
						drugs = PharmFinder.DrugByName(name, mode);
					else
						throw new ArgumentNullException("name");

					HashSet<ConceptPair> tmp = new HashSet<ConceptPair>();

					Dictionary<Concept, Concept[]> analogs = (await adapter.GetAnalogs(drugs, withNames: true))
						.GroupBy(p => p.Left)
						.ToDictionary(g => g.Key, g => g
							.Select(p => p.Right)
							.Distinct()
							.ToArray());

					Dictionary<uint, MinMaxAvgItem> prices = PharmDataSource.GetAdapters(mode).AvgCurrentByPoint(new Core.Data.GeoPoint(latitude, longitude),
							radix,
							analogs.Values
								.SelectMany(v => v)
								.Select(v => v.id)
								.Distinct(),
							Core.Data.ConnectionMode.Work,
							false)
						.ToDictionary(p => p.PackId); // ------------ 1 секунда

					AnalogsResponse[] resp = analogs
						.Select(d => new AnalogsResponse(d.Key.ToString(lang, shortName: false), d.Value
							.Select(a =>
								{
									MinMaxAvgItem prit;

									if (!prices.TryGetValue(a.id, out prit))
										prit = null;

									return (new AnalogResponse(a.ToString(lang, shortName: true), a.id,
										new AnalogPrice((prit == null) ? (double?)null : (double?)Math.Round(prit.Min, 2),
											(prit == null) ? (double?)null : (double?)Math.Round(prit.Max, 2),
											(prit == null) ? (double?)null : (double?)Math.Round(prit.Avg, 2))));
								})
							.OrderBy(a => (a.price == null) ? Double.MaxValue : (a.price.average ?? Double.MaxValue))))
						.OrderBy(d => (d.price == null) ? Double.MaxValue : (d.price.average ?? Double.MaxValue))
						.ToArray();

#if LOG_ALL_REQUESTS
					lg.Log("GetAnalogs()", "RETURNED");
#endif

					return (new System.Web.Http.Results.JsonResult<AnalogsResponse[]>(resp, new Newtonsoft.Json.JsonSerializerSettings(), System.Text.Encoding.UTF8, new HttpRequestMessage()));
				}
				catch(Exception err)
				{
					lg.Log("GetAnalogs()", err.Message);
					return (new System.Web.Http.Results.JsonResult<AnalogsResponse[]>(new AnalogsResponse[] {}, new Newtonsoft.Json.JsonSerializerSettings(), System.Text.Encoding.UTF8, new HttpRequestMessage()));
				}
			}/*

			protected void Dispose(bool disposing)
			{
				_parser.Dispose();

				//base.Dispose(disposing);
			}
		}*/
}
