using ECPLib.Common.Patient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using UMKBCoreAPI.Models.Api.Responses;

namespace EMC.Models
{
    public class ResultPatient
    {
        public Alert? Alert { get; set; }
        public Patient? Patient { get; set; }
    }
}
