using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiApp.Models.eRp.Response
{

    public class PrescrShort
    {
        public string requestUid { get; set; }
        public List<Error> errors { get; set; }
        public List<DatumBase> data { get; set; }
    }

}
