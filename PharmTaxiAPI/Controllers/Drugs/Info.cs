using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Controllers.Drugs
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors(Cors.AllowAll.Name)]
    public class DrugInfo : ControllerBase
    {
        /// <summary>
        /// Get Info about drug
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost("/Drugs/Info")]
        public IActionResult Info(PharmTaxiApp.Models.RequestDrug args)
        {

            return Content(PharmTaxiApp.Methods.Drugs.Info(args));
        }

    }
}
