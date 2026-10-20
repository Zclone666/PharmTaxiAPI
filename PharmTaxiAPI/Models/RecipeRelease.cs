using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Models
{
    public class RecipeReleaseResp
    {
        List<PharmTaxiAdm.Models.ModelRecipeBase> recipes { get; set; }// = new List<PharmTaxiAdm.Models.ModelRecipeBase>();
    }

    public class RecipeReleaseReq: ECPLib.Common.Recipes.Models.ModelRecipeBase
    {
        /// <summary>
        /// Сессионный ключ
        /// </summary>
        public string authkey { get; set; }
        /// <summary>
        /// Лицензионный ключ
        /// </summary>
        public string key { get; set; }
    }
}
