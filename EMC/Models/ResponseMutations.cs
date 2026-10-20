using System;
using System.Collections.Generic;
using System.Text;
using UMKBCoreAPI.Models.Api.Responses;

namespace EMC.Models
{
    public class ResponseMutations
    {
        public Alert? Alert { get; set; }
        public Dictionary<string, ECPLib.Common.Patient.Models.Mutation>? Mutations { get; set; }
    }
}
