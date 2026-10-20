using System;
using System.Collections.Generic;
using System.Text;
using UMKBCoreAPI.Models.Api.Responses;

namespace EMC.Models
{
    public class PatientsSearchResponse
    {
        public Alert alert { get; set; } = new Alert() { Code = "200", Level = "success" };
        public List<PatientSearch> patients { get; set; } = new List<PatientSearch>();
    }
}
