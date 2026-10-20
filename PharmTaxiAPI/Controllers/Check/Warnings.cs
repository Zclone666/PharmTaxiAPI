using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PharmTaxiApp.Models;
using SMReceipt.MongoConnect.ECP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Controllers.Check
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors(Cors.AllowAll.Name)]
    public class CheckController:ControllerBase
    {
        /// <summary>
        /// Check Drug Warnings
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost("/Check/Warnings")]
        public IActionResult Warnings(object args)
        {
            return Content(PharmTaxiApp.Methods.CheckWarnings.Warnings(args));
        }
    }
}
