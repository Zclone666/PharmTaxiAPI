using SMReceipt.Global.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Methods.Recipe.Model
{
    public class DrugStatus
    {
        /// <summary>
        /// ключ драга
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// статус драга в рецепте(0 - активный, 1 - частично обслужен, 2 - обслужен, 3 - просрочен без обслуживания, 4 - просрочен и частично обслужен, 5 - просрочен и обслужен)
        /// </summary>
        public int idStatus { get; set; }
        /// <summary>
        /// Список препоратов который был выдан
        /// </summary>
        public List<Meds> medicaments { get; set; } = new List<Meds>();
    }
}
