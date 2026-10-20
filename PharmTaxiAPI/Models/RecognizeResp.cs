using AUpload.Global.Models;
using PharmTaxiAdm.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Models
{
    public class RecognizeResp : ResponseMethL
    {
        public Alert alert { get; set; } = new Alert();
    }
}
