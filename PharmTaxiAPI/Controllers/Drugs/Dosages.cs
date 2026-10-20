using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiAPI.Controllers.Drugs
{

    [ApiController]
    [Route("[controller]")]
    [EnableCors(Cors.AllowAll.Name)]
    public class DrugsController:ControllerBase
    {
        /// <summary>
        /// Получение дозировок, ЛФ, первичных упаковок
        /// </summary>
        /// <param name="args">GetDosage.Models.Request</param>
        /// <returns>GetDosage.Models.Answer</returns>
        [HttpPost("/Drugs/Dosages")]
        
        public IActionResult Dosages(PharmTaxiApp.Models.Request args)
        {
            return Content(PharmTaxiApp.Methods.Drugs.Dosages(args));
        }

        /// <summary>
        /// Получение дозировок, ЛФ, первичных упаковок
        /// </summary>
        /// <param name="args">GetDosage.Models.Request</param>
        /// <returns>GetDosage.Models.Answer</returns>
        [HttpPost("/Drugs/FPDosages")]

        public IActionResult FPDosages(PharmTaxiApp.Models.Request args)
        {
            return Content(PharmTaxiApp.Methods.Drugs.FPDosages(args));
        }

        /// <summary>
        /// Create Dosages
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        /// 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("/Drugs/CreateDosage")]
        public IActionResult CreateDosage(PharmTaxiApp.Models.Request args)
        {
            return Content(PharmTaxiApp.Methods.Drugs.CreateDosage(args));
        }
    }
}
