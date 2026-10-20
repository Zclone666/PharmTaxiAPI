using ECPLib.Common.Recipes.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UMKBCoreAPI.Models.Api.Responses;

namespace PharmTaxiAPI.Methods.Recipe.Model
{
    public class RecipeAndHistory
    {
        /// <summary>
        /// Стандартный алерт
        /// </summary>
        public Alert alert { get; set; } = new Alert() { Code = "200", Level = "success" };
        /// <summary>
        /// Рецепт
        /// </summary>
        public ModelRecipeBase recipe { get; set; }
        /// <summary>
        /// История изменения рецепта
        /// </summary>
        public List<RecipeLog> history { get; set; }
    }
}
