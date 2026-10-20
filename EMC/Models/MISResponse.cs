using EMC.ECPIntegration.Models;
using System;
using System.Collections.Generic;
using System.Text;
using UMKBCoreAPI.Models.Api.Responses;

namespace EMC.Models
{
    public class MISResponse : MISData
    {
        public Alert? Alert { get; set; }
    }
}
