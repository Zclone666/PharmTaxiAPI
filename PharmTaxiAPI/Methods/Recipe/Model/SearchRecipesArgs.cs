using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Methods.Recipe.Model
{
    public class SearchRecipesArgs
    {
        public string key { get; set; }
        public string authkey { get; set; }
        /// <summary>
        /// Номер рецепта
        /// </summary>
        public string recipeId { get; set; }
        /// <summary>
        /// Номер СНИЛСа для поиска
        /// </summary>
        public string snils { get; set; }
        /// <summary>
        /// Email пациента
        /// </summary>
        public string email { get; set; }
    }
}
