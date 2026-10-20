using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PharmTaxiAPI.Methods;
using PharmTaxiApp.Models;
using PharmTaxiApp.Models.Barcodes;
using SMBarcode;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using PTConstants;

namespace PharmTaxiAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[EnableCors(Cors.AllowAll.Name)]
	[SwaggerTag(description: "Проверка препаратов и общие вспомогательные методы")]
	public class ProcessesController : ControllerBase
	{
		Logs lg = new Logs();
		private const string CachePreffix = "BuisnessProcess_";
		private BarcodeProcessor _barcodeProcessor = new BarcodeProcessor();

		readonly static DateTime _environmentStart = DateTime.Now.AddMilliseconds(-Environment.TickCount64);
		readonly static DateTime _currentProcessStart = Process.GetCurrentProcess().StartTime;
		public static bool IsStarted = false;

		/// <summary>
		/// как бы домашняя страница
		/// </summary>
		/// <returns></returns>
		[HttpGet("/process")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public IActionResult Get()
		{
			if (IsStarted)
			{
				var dateNow = DateTime.Now;
				var dateSinceProcessStart = dateNow.Subtract(_currentProcessStart);
				var dateSinceEnvironmentStart = dateNow.Subtract(_environmentStart);
				string timeRows = $"Current Server time is {dateNow:F} <br>"
					+ $"Process started (time passed): {_currentProcessStart:F} ({dateSinceProcessStart:d\\ \\d\\a\\y\\s\\ hh\\:mm\\:ss})<br>"
					+ $"Environment Started (time passed): {_environmentStart:F} ({dateSinceEnvironmentStart:d\\ \\d\\a\\y\\s\\ hh\\:mm\\:ss})<br>";

				return Content($"Socmedica PharmTaxi API: {GetType().Assembly.GetName().Version} <br> Server: {Startup.GetServer()}<br>{timeRows}<br>{Startup.Message_StatusInit}", "text/html; charset=utf-8");
			}
			else
			{
				return Content($"Socmedica PharmTaxi API: Bad start", "text/html; charset=utf-8");
			}
		}

		/// <summary>
		/// Тест
		/// </summary>
		/// <returns></returns>
		[HttpPost("/Test")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public IActionResult Test()
		{
			return Content("OK!");
		}

		/// <summary>
		/// получение id процесса
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		private static string GetProcessKey(Guid id)
		{
			return (String.Concat(CachePreffix, id));
		}

		/* private static IBuisnessProcess GetProcess(Guid id)
		{
			return (MemoryCache.Default[GetProcessKey(id)] as IBuisnessProcess);
		} */

		private static void DropProcess(Guid id)
		{
			// Отключено с целью мониторинга результатов работы процессов
			//MemoryCache.Default.Remove(GetProcessKey(id));
		}

		/*
		private BuisnessProcessResult RunProcess(IBuisnessProcess process)
		{
			MemoryCache.Default.Add(GetProcessKey(process.ID), process, DateTimeOffset.UtcNow.AddHours(2));

			process.Aborted += OnBuisnessProcessAborted;
			process.Finished += OnBuisnessProcessFinished;

#if LOG_ALL_STAGES
			process.StageRunned += OnBuisnessProcessStageRunned;
			process.StageFinished += OnBuisnessProcessStageFinished;

			lg.Log("==> BuisnessProcess", String.Format("RUNNED PROCESS {0}", process.ID));
#endif

			return (process.Do());
		}

		private IHttpActionResult ResultByProcess(IBuisnessProcess process, ConnectionMode mode, BuisnessProcessResult result)
		{
			if (process.DataType == typeof(BarcodeInfo))
				return (Json(new ProcessesBarcodeCheckResponse((BuisnessProcess<BarcodeInfo>)process, mode, result)));

			return (null);
		}*/

		/// <summary>
		/// Проверка подлинности препарата.
		/// Принимает дата матрикс, штрих код формата EAN13 или UPC, либо серийный номер препарата.
		/// Полная информация о подлинности препарата приходит только по дата матриксу!
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		/// <response code="200">Информация была получена целиком, или частично</response>
		/// <response code="400">Информация не была найдена либо произошла ошибка</response>
		[HttpPost("/Check")]
		[SwaggerResponse(200, "123", typeof(Drug))]
		[SwaggerResponse(400, "456", typeof(SMBarcode.Alert))]
		public async Task<IActionResult> Check(PharmTaxiApp.Models.CheckReq data) //JsonResult<ProcessesBarcodeCheckResponse>
		{

#if LOG_ALL_REQUESTS
	lg.Log("Check()", "RUNNED");
#endif

			//var authCheckReturn = CheckAuth(data.auth_key);
			//if (authCheckReturn.code is "400")
			//	return Content(JsonConvert.SerializeObject(authCheckReturn));

			try
			{
				return Content(Newtonsoft.Json.JsonConvert.SerializeObject(PharmTaxiApp.Methods.Scanner.Check(data)));
			}
			catch
            {
				return Content(Newtonsoft.Json.JsonConvert.SerializeObject(new SMBarcode.Alert() { code = 400, title = "Ошибка", message = "Неверные данные"}));
            }
		}

		/// <summary>
		/// какая-то фигня от старого ФармТакси
		/// </summary>
		/// <returns></returns>
		[HttpPost("/Detect")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public async Task<IActionResult> Detect()
		{

#if LOG_ALL_REQUESTS
	lg.Log("Detect()", "RUNNED");
#endif

			return Content(PharmTaxiApp.Methods.Scanner.Detect());
		}

		/// <summary>
		/// Получение актуальной фармакотерапии
		/// </summary>
		/// <param name="args"></param>
		/// <remarks>
		/// Пример запроса:
		///
		///		POST /Actual
		///		{
		///			"authkey": "fs43ASFDAfsdas34i",
		///			"email": "1st_man@mail.ru"
		///		}
		/// </remarks> 
		[SwaggerResponse(200, "", typeof(SMReceipt.MongoConnect.ECP.Models.GetInfoPreparations))]
		[HttpPost("/Actual")]
		public async Task<IActionResult> Actual(PharmTaxiApp.Models.GP2 args)
		{

#if LOG_ALL_REQUESTS
	lg.Log("Actual()", "RUNNED");
#endif

			//var authCheckReturn = Global.CheckAuth(args.authkey);
			//if (authCheckReturn.code is "400")
			//	return Content(JsonConvert.SerializeObject(authCheckReturn));

			return Content(PharmTaxiApp.Methods.ECP.Actual(args));
		}

		/// <summary>
		/// Проверка препарата по списку ЖНВЛП
		/// </summary>
		/// <param name="rqst"></param>
		/// <remarks>
		/// Пример запроса 1:
		///		POST /CheckMustHave
		///		{
		///			"fp_id": "",
		///			"tn_id": "",
		///			"dv_id": "pDKBBXF0ryDeaL3sGeYxBQ=="
		///		}
		/// </remarks>
		[HttpPost("/CheckMustHave")]
		[SwaggerResponse(200, "", typeof(ResponseMustHave))]
		public async Task<IActionResult> CheckMustHave(RequestMustHave rqst)
        {
			try
			{
				ResponseMustHave rmh = new ResponseMustHave();
				if (rmh.dv_id != "string" && !String.IsNullOrEmpty(rmh.dv_id)) rmh.dv_id = rqst.dv_id;
				else rmh.dv_id = null;
				if (rmh.fp_id != "string" && !String.IsNullOrEmpty(rmh.fp_id)) rmh.fp_id = rqst.fp_id;
				else rmh.fp_id = null;
				if (rmh.tn_id != "string" && !String.IsNullOrEmpty(rmh.tn_id)) rmh.tn_id = rqst.tn_id;
				else rmh.tn_id = null;
				if (!String.IsNullOrEmpty(rqst.dv_id)) rqst.dv_id = Crypto.Сryptographer.Decrypt(rqst.dv_id).ToString();
				if (!String.IsNullOrEmpty(rqst.fp_id)) rqst.fp_id = Crypto.Сryptographer.Decrypt(rqst.fp_id).ToString();
				if (!String.IsNullOrEmpty(rqst.tn_id)) rqst.tn_id = Crypto.Сryptographer.Decrypt(rqst.tn_id).ToString();
				rmh.must_have = Task.Run(() => PharmTaxiApp.Methods.Drugs.MustHaveCheck(ref rqst)).Result;
				rmh.price = rqst.price;
				rmh.name = rqst.name;
				if (!String.IsNullOrEmpty(rqst.dv_id)) rqst.dv_id = Crypto.Сryptographer.Encrypt(rqst.dv_id).ToString();
				if (!String.IsNullOrEmpty(rqst.fp_id)) rqst.fp_id = Crypto.Сryptographer.Encrypt(rqst.fp_id).ToString();
				if (!String.IsNullOrEmpty(rqst.tn_id)) rqst.tn_id = Crypto.Сryptographer.Encrypt(rqst.tn_id).ToString();
				rmh.tn_id = rqst.tn_id;
				rmh.fp_id = rqst.fp_id;
				rmh.dv_id = rqst.dv_id;
				rmh.dv_name = rqst.dv_name;
				return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rmh));
			}catch(Exception e)
            {
				return Content(Newtonsoft.Json.JsonConvert.SerializeObject(e.Message));
			}
        }
	}
}

#region
//	[HttpPost]
/*		public IHttpActionResult Advanced(ProcessesAdvancedRequest data)
		{
#if LOG_ALL_REQUESTS
			lg.Log("Advanced()", String.Format("RUNNED {0}", String.Join(", ",
				data.values.Select(p => String.Format("{0}:{1} ({2})", p.Key, p.Value, (p.Value == null) ? String.Empty : p.Value.GetType().Name)))));
#endif

			if (String.IsNullOrEmpty(data.lang))
				data.lang = "ru";

			try
			{
				/*	IBuisnessProcess process = GetProcess(data.id);

					if (process == null)
						throw Errors.Create(ErrorMessages.ProcessNotFoundsById, data.lang);
					else
					{
						IHttpActionResult ret;

						switch (process.Status)
						{
							case BuisnessProcessStatus.Finished:
							case BuisnessProcessStatus.Aborted:
								ret = ResultByProcess(process, ConnectionMode.Work, process.GetStatus());
								break;
							default:
								ret = ResultByProcess(process, ConnectionMode.Work, process.Do(data.values
									.ToDictionary(p => p.Key, p =>
									{
										AwaytableValue av = process.AwaytableValues.FirstOrDefault(v => v.Name == p.Key);

										if ((av == null) || (p.Value == null))
											return ((object)p.Value);
										else if (av.Type == typeof(bool))
										{
											bool? tmp = p.Value as bool?;

											if (tmp != null)
												return ((object)tmp.Value);

											bool tmpx;

											if (bool.TryParse(p.Value.ToString(), out tmpx))
												return ((object)tmpx);
										}
										else if (av.Type == typeof(Enum))
										{
											uint? tmp = p.Value as uint?;

											if (tmp != null)
												return ((object)tmp.Value);

											uint tmpx;

											if (uint.TryParse(p.Value.ToString(), out tmpx))
												return ((object)tmpx);
										}
										else if (av.Type == typeof(string))
											return ((object)p.Value.ToString());

										return ((object)null);
									})));
								break;
						}

	#if LOG_ALL_REQUESTS
						lg.Log("Advanced()", "RETURNED");
	#endif

						return (ret);
					}
				return ("");
			}
			catch (Exception e)
			{
				lg.Log("Advanced()", e.Message);
				return (Json(new ProcessesBarcodeCheckResponse(e)));
			}
		}

[HttpGet]
public IHttpActionResult Abort(Guid id, string lang)
{
#if LOG_ALL_REQUESTS
	lg.Log("Abort()", "RUNNED");
#endif

	if (String.IsNullOrEmpty(lang))
		lang = "ru";

	try
	{
		IBuisnessProcess process = GetProcess(id);

		if (process == null)
			throw Errors.Create(ErrorMessages.ProcessNotFoundsById, lang);

		IHttpActionResult ret = ResultByProcess(process, ConnectionMode.Work, process.Abort());

#if LOG_ALL_REQUESTS
		lg.Log("Abort()", "RETURNED");
#endif

		return (ret);
	}
	catch (Exception e)
	{
		lg.Log("Abort()", e.Message);
		return (Json(new ProcessesBarcodeCheckResponse(e)));
	}
}

[HttpGet]
public JsonResult<IEnumerable<ProcessGetAllResponse>> GetAll()
{
	return (Json(MemoryCache.Default
		.Where(i => i.Key.StartsWith(CachePreffix))
		.Select(i => i.Value as IBuisnessProcess)
		.Where(p => p != null)
		.Select(p => new ProcessGetAllResponse(p))));
}

#region Event handlers

private void OnBuisnessProcessAborted(object sender, BuisnessProcessAbortedEventArgs e)
{
#if LOG_ALL_STAGES
	lg.Log("==> BuisnessProcess", (e.Error != null)
		? String.Format("ABORTED PROCESS {0} with error {1} ({2})", ((IBuisnessProcess)sender).ID, e.Error.GetType(), e.Error.Message)
		: String.Format("ABORTED PROCESS {0}", ((IBuisnessProcess)sender).ID));
#endif

	DropProcess(((IBuisnessProcess)sender).ID);

	if (e.Error != null)
		throw new OperationCanceledException(e.Error.Message, e.Error);
}

private void OnBuisnessProcessFinished(object sender, EventArgs e)
{
#if LOG_ALL_STAGES
	lg.Log("==> BuisnessProcess", String.Format("FINISHED PROCESS {0}", ((IBuisnessProcess)sender).ID));
#endif

	DropProcess(((IBuisnessProcess)sender).ID);
}

private void OnBuisnessProcessStageRunned(object sender, BuisnessProcessStageEventArgs e)
{
#if LOG_ALL_STAGES
	lg.Log("==> BuisnessProcess", String.Format("RUNNED STAGE {0} on process {1}", e.Stage.Name, e.Process.ID));
#endif
}

private void OnBuisnessProcessStageFinished(object sender, BuisnessProcessStageFinishedEventArgs e)
{
#if LOG_ALL_STAGES
	lg.Log("==> BuisnessProcess", String.Format("FINISHED STAGE {0} on process {1} with result {2} ({3})", e.Stage.Name, e.Process.ID, e.Result.Type, e.Result.Message));
#endif
}

#endregion
}
}*/
#endregion