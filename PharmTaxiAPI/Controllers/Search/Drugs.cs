using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using UMKBUtils;

namespace PharmTaxiAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors(Cors.AllowAll.Name)]
    public partial class SearchController:ControllerBase
    {
        /// <summary>
        /// Поиск концепта по строке sfinx
        /// </summary>
        /// <param name="args">GetDosage.Models.Request</param>
        /// <returns>GetDosage.Models.Answer</returns>
        [HttpPost("/Search/Drugs")]
   
        public IActionResult Drugs(UMKBRequests.Models.API.Semantic.RequestSearch args)
        {
           // return Content(PharmTaxiApp.Controllers.SearchController.Drugs(args));
            return PharmTaxiApp.Methods.Search.Drugs(args);
        }
    }
}
