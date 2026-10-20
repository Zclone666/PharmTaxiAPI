using PTConstants;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiApp.Controllers
{
    public static class SearchController
    {

        public static string Drugs(UMKBRequests.Models.API.Semantic.RequestSearch args)
        {
          //  if (args.authkey == "" || !(args.authkey is null))
           // {
                args.authkey = ECP.Key;
          //  }
            return UMKBRequests.WebUtils.Post($"{URLs.DomainECP}/Search/Drugs", Newtonsoft.Json.JsonConvert.SerializeObject(args), UMKBRequests.WebUtils.WebContentType.JSON).Answer;
            //return (IActionResult)UMKBRequests.WebUtils.Post("service.socmedica.com:9004", Newtonsoft.Json.JsonConvert.SerializeObject(args), UMKBRequests.WebUtils.WebContentType.JSON);
        }
    }
}
