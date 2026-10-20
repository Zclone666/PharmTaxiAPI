using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Methods.Recipe.Model
{
    public class RecipeLogFilter
    {
        /// <summary>
        /// Номер рецепта
        /// </summary>
        public string RecipeId { get; set; }
        /// <summary>
        /// Дата изменения рецепта (UnixTime в мс)
        /// </summary>
        public Int64 Date { get; set; } = SMCommonUtils.Converter.GetNowInt64();
        /// <summary>
        /// Id пользователя
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Id клиники, где изменили рецепт или индивидульного предпринимателя
        /// </summary>
        public string IdClinic { get; set; }
        /// <summary>
        /// Id отделения, где изменили рецепт
        /// </summary>
        public string IdDep { get; set; }
        /// <summary>
        /// Id фармаколога
        /// </summary>
        public string PHId { get; set; }
    }
}
