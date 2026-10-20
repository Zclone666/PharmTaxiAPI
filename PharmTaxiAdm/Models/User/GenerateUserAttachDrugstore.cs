using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAdm.Models
{
    public class GenerateUserAttachDrugstore
    {
        /// <summary>
        /// authkey пользователя
        /// </summary>
        public string authkey { get; set; }
        /// <summary>
        /// key пользователя
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// IDшник аптеки
        /// </summary>
        public uint? drugstoreId { get; set; }
    }

    public class AttacheDrugstore:GenerateUserAttachDrugstore
    {
        public string lic_code { get; set; }
    }
}
