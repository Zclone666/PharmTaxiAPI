using Newtonsoft.Json;
using PharmTaxiAdm.Models.Drugstore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiAdm.Models.User
{
    public partial class DrugUserCrR : BaseUserRegReq, IUserRegistrationReq
    {
        public DrugUserCrR() : base()
        {

        }
        public BaseDrugNet drugNet { get; set; }
    }

}
