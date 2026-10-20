using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiApp.Models.eRp.Response
{
    public class Common
    {
    }

    public class Error
    {
        public string key { get; set; }
        public string message { get; set; }
        public string code { get; set; }
    }

    public class RemdError
    {
        public string key { get; set; }
        public string message { get; set; }
        public string code { get; set; }
    }

    public class MedicationBase
    {
        public string prescriptionName { get; set; }
        public string signa { get; set; }
        public string precondition { get; set; }
        public int numberDose { get; set; }
    }

    public class DatumBase
    {
        public string uid { get; set; }
        public string series { get; set; }
        public string number { get; set; }
        public string formType { get; set; }
        public MedicationBase medication { get; set; }
        public DateTime dateCreate { get; set; }
        public DateTime dateEnd { get; set; }
        public string state { get; set; }
    }
}
