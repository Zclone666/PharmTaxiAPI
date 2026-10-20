using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Methods.Recipe.Model
{
    public class RecipeAndHistoryArgs
    {
        /// <summary>
        /// Сессионный ключ
        /// </summary>
        public string authkey { get; set; }
        /// <summary>
        /// Лицензионный ключ
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// Номер рецепта
        /// </summary>
        public string recipeId { get; set; }
        /// <summary>
        /// Флаг получить историю рецепта
        /// </summary>
        public bool getHistory { get; set; } = false;
    }
}
