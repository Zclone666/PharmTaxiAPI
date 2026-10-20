using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiApp.Models.eRp.Response
{
    public class ReleasePrescr
    {
        public string requestUid { get; set; }
        public List<Error> errors { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string localUid { get; set; }
        public string state { get; set; }
        public DateTime stateDate { get; set; }
        public List<RemdError> remdErrors { get; set; }
    }

}
