using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PharmTaxiAdm.Models;
using PharmTaxiAdm.Models.User;
using PharmTaxiAPI.Methods;
using PharmTaxiAPI.Models;
using PharmTaxiApp.Models.Barcodes;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using PTConstants;

namespace PharmTaxiAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors(Cors.AllowAll.Name)]
	[SwaggerTag(description: "Методы для всего что связано с авторизацией")]
    public class AuthControllers : ControllerBase
    {

		Logs lg = new Logs();
		private const string CachePreffix = "BuisnessProcess_";
		private BarcodeProcessor _barcodeProcessor = new BarcodeProcessor();

		/// <summary>
		/// Авторизация
		/// В ответе role либо Admin, либо Provisor, либо Patient
		/// </summary>
		/// <param name="data"></param>
		/// <remarks>
		/// Пример запроса:
		/// 
		///		POST /Authorize
		///		{
		///			"email": "testphtx@yopmail.com",
		///			"password": "test1234",
		///			"service": "7"
		///		}
		///		
		/// </remarks>
		[SwaggerResponse(200, "123", typeof(AuthKUserId))]
		[HttpPost("/Authorize")]
		public async Task<IActionResult> Authorize(Authorize data) //JsonResult<Core.Pharm.Response>
		{

			lg.Log("Authorize()", "RUNNED");
			try
			{
				return Content(PharmTaxiAdm.Methods.User.UserMethods.Authorize2(data.email, data.password, data.service));
			}
			catch (Exception e)
			{
				return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", message = e.Message, title = "failed", sticky = false }));
			}

		}


		/// <summary>
		/// Авторизация
		/// В ответе role либо Admin, либо Provisor, либо Patient
		/// </summary>
		/// <param name="data"></param>
		/// <remarks>
		/// Пример запроса:
		/// 
		///		POST /api/user/authorization2
		///		{
		///			"email": "testphtx@yopmail.com",
		///			"password": "test1234",
		///			"service": "7"
		///		}
		///		
		/// </remarks>
		/// 
		[ApiExplorerSettings(IgnoreApi = true)]
		[SwaggerResponse(200, "123", typeof(AuthKUserId))]
		[HttpPost("/api/user/authorization2")]
		public async Task<IActionResult> Authorize2(Authorize data) //JsonResult<Core.Pharm.Response>
		{

			lg.Log("Authorize()", "RUNNED");
			try
			{
				return Content(PharmTaxiAdm.Methods.User.UserMethods.Authorize2(data.email, data.password, data.service));
			}
			catch (Exception e)
			{
				return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", message = e.Message, title = "failed", sticky = false }));
			}

		}


		/// <summary>
		/// Выход 
		/// </summary>
		/// <param name="data"></param>
		/// <remarks>
		/// Пример запроса:
		/// 
		///		GET /api/user/authexit?authkey=
		///		
		/// </remarks>
		/// 
		[ApiExplorerSettings(IgnoreApi = true)]
		[SwaggerResponse(200, "123", typeof(AuthKUserId))]
		[HttpGet("/api/user/authexit")]
		public async Task<IActionResult> Authexit(string authkey) //JsonResult<Core.Pharm.Response>
		{

			lg.Log("Authorize()", "RUNNED");
			try
			{
				return Content(UMKBRequests.WebUtils.Get($"https://cs.socmedica.com/api/user/authexit?authkey={authkey}").Answer);
			}
			catch (Exception e)
			{
				return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", message = e.Message, title = "failed", sticky = false }));
			}

		}

		/// <summary>
		/// Получить info пользователя по email
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		[HttpPost("/api/user/getUserInfoByEmail")]

		public async Task<IActionResult> GetUserInfoByEmail(GetUserInfo data) //JsonResult<Core.Pharm.Response>
		{
			try
			{
				return Content(JsonConvert.SerializeObject(PharmTaxiAdm.Methods.User.UserMethods.GetUserInfoByEmail(data)));
			}
			catch (Exception e)
			{
				return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", message = e.Message, title = "failed", sticky = false }));
			}

		}


		/// <summary>
		/// Создание гостевого пользователя. Обращение без параметров.
		/// </summary>
		/// <returns></returns>
		/// 

		[SwaggerResponse(200, "Пользователь создан, возвращается Authkey", typeof(AuthKUserId))]
		[HttpPost("/CreateGuest")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public async Task<IActionResult> CreateGuest()
		{

#if LOG_ALL_REQUESTS
	lg.Log("CreateGuest()", "RUNNED");
#endif

			GuestUser gu = new GuestUser()
			{
				key = UMKB.CreateGuestUser,
				application = "5"
			};

			CreateGuestResponse cgs = JsonConvert.DeserializeObject<CreateGuestResponse>(
				UMKBRequests.WebUtils.Post(URLs.CreateGuestUserURL, JsonConvert.SerializeObject(gu), "application/json").Answer);
			AlertUserID AuId = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(cgs.authkey, "5");
			AuthKUserId AuthAndId = new AuthKUserId();
			AuthAndId.alert = AuId.alert;
			AuthAndId.authkey = cgs.authkey;
			AuthAndId.userid = AuId.userid;
			AuthAndId.role = AuId.role;
			return Content(JsonConvert.SerializeObject(AuthAndId));//(new PharmTaxiAdm.Models.User.AuthKeyByLicKey() { authkey = cgs.authkey }));
		}

		/// <summary>
		/// Создание приглашения и отправка его на почту
		/// </summary>
		/// <param name="args">Параметры для создания приглашения и отправки на почту</param>
		/// <remarks>
		/// Пример запроса:
		///
		///		POST /CreateInvite
		///		{
		///			"authkey": "fs43ASFDAfsdas34i",
		///			"invitedEmail": "qwerty@example.com",
		///			"invitedRoleCodes":["КОД АПТЕКИ, ПОЛУЧЕННЫЙ ОТ КОНТРОЛЛЕРА MassiveCreateDrugstore","КОД АПТЕКИ, ПОЛУЧЕННЫЙ ОТ КОНТРОЛЛЕРА MassiveCreateDrugstore"]
		///		}
		/// </remarks> 
		/// 
		[ApiExplorerSettings(IgnoreApi = false)]
		[SwaggerResponse(200, "", typeof(UMKBRequests.Alert))]
		[HttpPost("/CreateInvite")]
		public async Task<IActionResult> CreateInvite(PharmTaxiAdm.Models.User.InviteViaEmail args)
        {
			try
			{
				InviteResp ret = new InviteResp();
				AlertUserID uID = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(args.authkey, args.application, args.key);
				if (uID.alert.level != "success")
					return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", level = "error", message = uID.alert.message, title = "error" }));
				ret = PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.SlaveCreation3(args).Result;
				return Content(JsonConvert.SerializeObject(ret));
			}
			catch (Exception e)
			{
				return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", level = "error", message = e.Message, title = "error" }));
			}
		}

		/// <summary>
		/// Подтверждения приглашения через Authkey
		/// </summary>
		/// <param name="args">Параметры для подтверждения приглашения</param>
		/// <remarks>
		/// Пример запроса:
		///
		///		POST /ConfirmInviteByAuthkey
		///		{
		///			"authkey": "АУТКЕЙ НЕОБХОДИМО ПОЛУЧИТЬ ПРИ ПОМОЩИ КОНТРОЛЛЕРА Authorize",
		///			"code": "123456"
		///		}
		/// </remarks> 
		/// 
		[ApiExplorerSettings(IgnoreApi = false)]
		[SwaggerResponse(200, "", typeof(UMKBRequests.Alert))]
		[HttpPost("/ConfirmInviteByAuthkey")]
		public async Task<IActionResult> ConfirmInviteByAuthkey(PharmTaxiAdm.Models.User.ConfirmInvite args)
        {
			try
			{
				return Content(JsonConvert.SerializeObject(PharmTaxiAdm.Methods.User.UserMethods.ConfirmInvite(args).Answer));
			}catch (Exception e)
            {
				return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", message = e.Message + ' ' + e.StackTrace, level = "error", title = "error", sticky = false }));
            }
		}

		/// <summary>
		/// Вам нужен раб - платите three hundred bucks и fucking slave - ваш. 
		/// Главное - прислать email будущего раба. 
		/// Можно ещё прислать id fisting (но мы можем сами догадаться)
		/// </summary>
		/// 
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpPost("/CreateSlaveWithEmail")]
		public async Task<IActionResult> CreateSlaveWithEmail(AuthKeyByLicKey data)
		{
			try
			{
				InviteResp ret = new InviteResp();
				AlertUserID uID = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(data.master_authkey);
				if (uID.alert.level != "success")
					return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", level = "error", message = uID.alert.message, title = "error" }));
				ret = PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.SlaveCreation2(data).Result;
				return Content(JsonConvert.SerializeObject(ret));
			}
			catch (Exception e)
			{
				return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", level = "error", message = e.Message, title = "error" }));
			}
		}

		/// <summary>
		/// Создание пользователя из гостя 
		/// </summary>
		/// <param name="args">Параметры для пользователя, который создается из гостя</param>
		/// <remarks>
		/// Пример запроса:
		///
		///		POST /CreateUserFromGuest
		///		{
		///			"authkey": "fs43ASFDAfsdas34i"
		///			"email": "123@example.com",
		///			"password": "1234abc",
		///			"firstName": "Иван",
		///			"lastName": "Иванов",
		///			"middleName": "Иванович"
		///		}
		/// </remarks>
		/// 
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpPost("/CreateUserFromGuest")]
		[SwaggerResponse(200, "", typeof(CreateUserFromGuestResponse))]
		public async Task<IActionResult> CreateUserFromGuest(UserFromGuest args)
		{

#if LOG_ALL_REQUESTS
	lg.Log("CreateUserFromGuest()", "RUNNED");
#endif
			return Content(Newtonsoft.Json.JsonConvert.SerializeObject(UserFromG.CreateUserFromGuest(args)));

		}

		/// <summary>
		/// Контроллер для генерации подчинённого пользователя ФармТакси для аптек
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		/// 
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpPost("/GenerateUser")]
		public async Task<IActionResult> GenerateUser(AuthKeyByLicKey data) //JsonResult<Core.Pharm.Response>
		{

			lg.Log("GenerateUser()", "RUNNED");
			try
			{
				if (String.IsNullOrEmpty(data.authkey))
					return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", level = "error", message = "authkey is null", title = "error", sticky = false }));

				return Content(JsonConvert.SerializeObject(PharmTaxiAdm.Methods.User.UserMethods.GenerateUser(data.authkey)));
			}
			catch (Exception e)
			{
				return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", message = e.Message, title = "failed", sticky = false }));
			}

		}

		/// <summary>
		/// Создать пользователя и прикрепить к нему аптеку
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		/// 
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpPost("/GenerateUserAttachDrugstore")]
		public async Task<IActionResult> GenerateUserAttachDrugstore(GenerateUserAttachDrugstore data) //JsonResult<Core.Pharm.Response>
		{

			lg.Log("GenerateUserAttachDrugstore()", "RUNNED");
			PharmTaxiAdm.Models.User.AlertUserID uID = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(data.authkey);
			if (uID.alert.level != "success") return Content(JsonConvert.SerializeObject(uID.alert));
			try
			{
				return Content(JsonConvert.SerializeObject(PharmTaxiAdm.Methods.User.UserMethods.GenerateUserAttachDrugstore(data.authkey, data.drugstoreId)));
			}
			catch (Exception e)
			{
				return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", message = e.Message, title = "failed", sticky = false }));
			}

		}

		/// <summary>
		/// Получить id пользователя по ауткею
		/// в ответе role либо Admin, либо Provisor, либо Patient
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		/// 
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpPost("/GetUserIdByAuthkey")]
		public async Task<IActionResult> GetUserIdByAuthkey(GetBy data) //JsonResult<Core.Pharm.Response>
		{

			lg.Log("GetUserIdByAuthkey()", "RUNNED");
			PharmTaxiAdm.Models.User.AlertUserID uID = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(data.authkey);
			if (uID.alert.level != "success") return Content(JsonConvert.SerializeObject(uID.alert));
			try
			{
				return Content(JsonConvert.SerializeObject(PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(data.authkey)));
			}
			catch (Exception e)
			{
				return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", message = e.Message, title = "failed", sticky = false }));
			}

		}

		/// <summary>
		/// Контроллер для регистрации пользователя ФармТакси для аптек
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		/// 
		[ApiExplorerSettings(IgnoreApi = false)]
		[HttpPost("/Register")]
		public async Task<IActionResult> Register(PharmTaxiAdm.Models.User.UserRegistrationReq data) //JsonResult<Core.Pharm.Response>
		{

			lg.Log("Register()", "RUNNED");
			try
			{
				data.lic_key = data.roleCode ?? data.lic_key;
				PharmTaxiAdm.Models.User.IUserRegistrationReq dt = data;
				return Content(JsonConvert.SerializeObject(PharmTaxiAdm.Methods.User.UserMethods.Register(dt)));
			}
			catch (Exception e)
			{
				return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", message = e.Message, title = "failed", sticky = false }));
			}

		}

		/// <summary>
		/// Контроллер для ресгистрации с созданием сети
		/// </summary>
		/// <param name="data"></param>
		/// <remarks>
		/// Пример запроса:
		///		POST /RegisterWNet
		///		{
		///			 "lastname": "Tesxt",
		///			 "firstname":"Tesxt",
		///			 "middlename": "Tesxt",
		///			 "email": "test_xxxx@yopmail.com",
		///			 "password": "test1234",
		///			 "service": "7",
		///			 "drugNet": {
		///				"typeText": "Сеть Аптек",
		///				"address": "Москва, ул. Кикабидзе, 50",
		///				"license": "2ппу323109",
		///				"phone": "555-55-55",
		///				"title": "Сеть аптек Планета Чурок X"
		///			}
		///		}
		///	</remarks>
		///	
		[ApiExplorerSettings(IgnoreApi = false)]
		[HttpPost("/RegisterWNet")]
		public async Task<IActionResult> RegisterWNet(PharmTaxiAdm.Models.User.DrugUserCrR data)
        {
			lg.Log("RegWNet()", "RUNNED");
			try
			{
				return Content(Newtonsoft.Json.JsonConvert.SerializeObject(PharmTaxiAdm.Methods.User.DrugUserCrR.RegUserNetCr(data)));
            }
            catch (Exception e)
            {
				return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", message = e.Message, title="failed", sticky=false }));
            }
        }

		/// <summary>
		/// Самая Новая Версия ПРИГЛАШАЛКИ! Использовать её!
		/// </summary>
		/// <remarks>{  
		///	"authkey": "СЮДА ВСТАВЛЯЕМ АУТКЕЙ ИЗ КОНТРОЛЛЕРА AUTHORIZE ДЛЯ АПТЕКИ",   
		///	"email": "inv_test@yopmail.com",   
		///	"firstname": "Test",   
		///	"lastname": "Test",   
		///	"middlename": "Test",   
		///	"roleCode": "_237YFS5KXKFPM" 
		///	}</remarks>
		/// <example>{  
		///	"authkey": "СЮДА_ВСТАВЛЯЕМ_АУТКЕЙ_ИЗ_КОНТРОЛЛЕРА_AUTHORIZE_ДЛЯ_АПТЕЧНОЙ_СЕТИ",   
		///	"email": "inv_test@yopmail.com",   
		///	"firstname": "Test",   
		///	"lastname": "Test",   
		///	"middlename": "Test",   
		///	"roleCode": "_237YFS5KXKFPM" 
		///	}</example>
		///	
		/// <param name="data"></param>
		/// <returns></returns>
		/// 
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpPost("/Invite")]
		public async Task<IActionResult> Invite(PharmTaxiAdm.Models.User.RegUserByUserReq data)
        {
            try
            {
				return Content(Methods.Inviter.Get(data));
            }
			catch(Exception e)
            {
				return Content(Newtonsoft.Json.JsonConvert.SerializeObject(new UMKBRequests.Alert() { code="403", level="error", message=e.StackTrace, title=e.Message, sticky=false}));
            }
		}

		/// <summary>
		/// Подтверждение регистрации
		/// </summary>
		/// <param name="data"></param>
		/// <remarks>{  "email": "inv_test@yopmail.com",  "code": "КОД ИЗ ПИСЬМА",  "service": "6",  "middlename": "NotTest", "newPassword": "test1234", "firstname": "NotTest", "lastname": "NotTest",  "middlename": "NotTest"}</remarks>
		[HttpPost("/RegistrationConfirm")]
		[ApiExplorerSettings(IgnoreApi = false)]
		public async Task<IActionResult> RegistrationConfirm(RegConfirmReqPHT data)
        {
			try
			{
				return Content(Methods.Inviter.Confirm(data));
			}
			catch (Exception e)
			{
				return Content(Newtonsoft.Json.JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "403", level = "error", message = e.StackTrace, title = e.Message, sticky = false }));
			}
		}

		/// <summary>
		/// Запрос на повторную отправку кода подтверждения регистрации
		/// </summary>
		/// <remarks>
		/// POST /ReConfirmEmail
		/// 
		///		Пример:
		///			{
		///				"email": "yikajiw692@veb27.com"
		///			}	
		/// </remarks>
		[HttpPost("/ReConfirmEmail")]
		[ApiExplorerSettings(IgnoreApi = false)]
		public async Task<IActionResult> ConfirmEmail(ReConfirmEmailArgs args)
		{
			try
			{
				return Content(Methods.Inviter.ReConfirm(args));
			}
			catch (Exception e)
			{
				return Content(Newtonsoft.Json.JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "403", level = "error", message = e.StackTrace, title = e.Message, sticky = false }));
			}
		}

		/// <summary>
		/// Изменение ФИО пользователя самим пользователем.
		/// Отправляются новые ФИО + authkey или key
		/// </summary>
		/// <param name="data"></param>
		/// <example>{
		/// "authkey": "AUTHKEY ПОЛУЧАЕМ В КОНТРОЛЛЕРЕ AUTHORIZE",
		/// "firstname": "Новое Имя",
		/// "middlename": "Новое Отчество",
		/// "lastname": "Новая Фамилия"
		/// }</example>
		/// <remarks>
		/// {
		/// "authkey": "AUTHKEY ПОЛУЧАЕМ В КОНТРОЛЛЕРЕ AUTHORIZE",
		/// "firstname": "Новое Имя",
		/// "middlename": "Новое Отчество",
		/// "lastname": "Новая Фамилия"
		/// }
		/// </remarks>
		[HttpPost("/ChangeUserFIO")]
		[ApiExplorerSettings(IgnoreApi = false)]
		public async Task<IActionResult> ChangeUserFIO(UserFIOReq data)
        {
            try
            {
				return Content(Methods.UserChangeRemove.UserMod(data));
            }
			catch(Exception e)
            {
				return Content(Newtonsoft.Json.JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "403", level = "error", message = e.StackTrace, title = e.Message, sticky = false }));
			}
        }

		[HttpPost("/DesactivateUser")]
		[ApiExplorerSettings(IgnoreApi = false)]
		public async Task<IActionResult> DesactivateUser(DesactivateModel data)
        {
			try
			{
				return Content(Methods.UserChangeRemove.UserDel(data));
			}
			catch (Exception e)
			{
				return Content($"YOU SHALL NOT PASS! Exception: {e.Message};\nStack: {e.StackTrace}");
			}
        }


		/// <summary>
		/// Получение пациента по id пользователя
		/// </summary>
		/// <param name="data"></param>
		/// <remarks>]
		/// 
		///	POST /GetPatByUserId
		///	
		/// {
		/// 
		///			"authkey": "АУТКЕЙ НАДО ПОЛУЧИТЬ ИЗ AUTHORIZE",
		///			
		///			"userid": "10169"
		///	
		///	}
		///		
		/// </remarks>
		[HttpPost("/GetPatByUserId")]
		public async Task<IActionResult> GetPatByUserId(GetBy data)
		{
			try
			{
				AlertUserID AutUId = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(data.authkey, "5", data.key);
				uint UserId = 62;
				if (AutUId.alert.code != "200" && AutUId.userid == 0) return Content(Newtonsoft.Json.JsonConvert.SerializeObject(AutUId));
                else
                {
					if (!String.IsNullOrEmpty(data.userid)) UserId = uint.TryParse(data.userid, out uint rr) ? rr : (AutUId.userid == 0) ? UserId : AutUId.userid;
					else if (String.IsNullOrEmpty(data.userid) && AutUId.userid != 0) UserId = AutUId.userid;
					else if (String.IsNullOrEmpty(data.userid) && AutUId.userid == 0) UserId = UserId;									
                }
				return Content(Newtonsoft.Json.JsonConvert.SerializeObject(EMC.Singleton.GetPatient(PharmTaxiApp.Methods.EMCLocal.PatByUserId.GetPatId(UserId) * 10000 + 199, "ru")));
			}
			catch (Exception e)
			{
				return Content($"YOU SHALL NOT PASS! Exception: {e.Message};\nStack: {e.StackTrace}");
			}
		}

	}
}
