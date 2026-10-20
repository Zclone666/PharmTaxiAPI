using PTConstants;

namespace PharmTaxiApp.Controllers.Drugs
{
    public partial class DrugsController
    {

        public string Dosages(PharmTaxiApp.Models.Request args)
        {
            if (args.authkey == "" || !(args.authkey is null))
            {
                args.authkey = ECP.Key;
            }
           return UMKBRequests.WebUtils.Post(URLs.Dosages, Newtonsoft.Json.JsonConvert.SerializeObject(args), UMKBRequests.WebUtils.WebContentType.JSON).Answer;
         //   return (IActionResult)UMKBRequests.WebUtils.Post("service.socmedica.com:9004", Newtonsoft.Json.JsonConvert.SerializeObject(args), UMKBRequests.WebUtils.WebContentType.JSON);
        }
    }
}
