using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Methods.Recipe.Model
{
    public class RecipeReleaseArg
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
        /// Ид аптеки
        /// </summary>
        public ulong? pharmId { get; set; }
        /// <summary>
        /// Название аптеки
        /// </summary>
        public string pharmName { get; set; }
        /// <summary>
        /// Номер рецепта
        /// </summary>
        public string recipeId { get; set; }
        /// <summary>
        /// Флаг статуса для всего рецепта
        /// </summary>
        public bool recipeFullRelease { get; set; }
        /// <summary>
        /// Ид статуса для всего рецепта (0 - активный, 1 - частично обслужен, 2 - обслужен, 3 - просрочен без обслуживания, 4 - просрочен и частично обслужен, 5 - просрочен и обслужен)
        /// </summary>
        public int? fullReleaseStatus { get; set; }
        /// <summary>
        /// Флаг списать ли выданые препораты
        /// </summary>
        public bool releaseMedicaments { get; set; } = false;
        /// <summary>
        /// Список драгов со статусом
        /// </summary>
        public List<DrugStatus> drugStatuses { get; set; }
    }
}
