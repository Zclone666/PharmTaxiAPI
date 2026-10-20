using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PharmTaxiAPI.Models;
using PharmTaxiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Controllers.ECP
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors(Cors.AllowAll.Name)]
    public class UPrimary:ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("/tools/UnitsPrimary")]
        public IActionResult UnitsPrimary(PharmTaxiApp.Models.Tools.UnitsPharm args)//(PharmTaxiApp.Models.Tools.UnitsPharm args)
        {
            try
            {
                //   AnswerUnits au = Newtonsoft.Json.JsonConvert.DeserializeObject<AnswerUnits>(PharmTaxiApp.Methods.ECP.UnitsPrimary());
                return Content(PharmTaxiApp.Methods.ECP.UnitsPrimary());// (args));
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("/tools/UnitsPharm")]
        public IActionResult UnitsPharm(PharmTaxiApp.Models.Tools.UnitsPharm args)
        {
            return Content(PharmTaxiApp.Methods.ECP.UnitsPharm(args));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("/info/Schedule")]
        public IActionResult Schedule(PharmTaxiApp.Models.Tools.SecurityArgs args)
        {
            return Content(PharmTaxiApp.Methods.ECP.Schedule(args));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("/info/UIText")]
        public IActionResult UIText(PharmTaxiApp.Models.Tools.SecurityArgs args)
        {
            return Content(PharmTaxiApp.Methods.ECP.UIText(args));
            //string locale;
            //if (args == null || string.IsNullOrEmpty(args.active_language)) { locale = "ru"; } else { locale = args.active_language; }
            //try { return Content(JsonConvert.SerializeObject(GetUMKBInfo.Services.UIText.Get(Crypto.Password, locale)), "application/json; charset=utf-8"); }
            //catch (Exception ex) { return Content(JsonConvert.SerializeObject(new UMKBRequests.Alert { level = "error", message = ex.Message, title = "Ошибка сервера" }), "application/json; charset=utf-8"); }

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("/info/buttonTexts")]
        public IActionResult buttonTexts(PharmTaxiApp.Models.Tools.SecurityArgs args)
        {
            return Content(PharmTaxiApp.Methods.ECP.buttonTexts(args));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("/info/elements")]
        public IActionResult elements(PharmTaxiApp.Models.Tools.SecurityArgs args)
        {
            return Content(PharmTaxiApp.Methods.ECP.elements(args));
        }
    }
}
