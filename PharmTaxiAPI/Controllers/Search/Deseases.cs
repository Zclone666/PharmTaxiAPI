using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Controllers
{ 
   
    public partial class SearchController:ControllerBase
    {
        [HttpPost("/Search/Diseases")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Deseases(UMKBRequests.Models.API.Semantic.RequestSearch args)
        {
            return Content(PharmTaxiApp.Methods.Search.Diseases(args));
        }
    }
}
