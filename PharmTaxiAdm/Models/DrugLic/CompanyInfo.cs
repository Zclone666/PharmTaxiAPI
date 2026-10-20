using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiAdm.Models.DrugLic
{
    public class CompanyInfo
    {
        /// <summary>
        /// Код ОГРН/ОГРИП
        /// </summary>
        public uint ogrn_ogrip { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        public uint inn { get; set; }

        public string label { get; set; }
    }
}
