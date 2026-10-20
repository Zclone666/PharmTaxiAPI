using ECPLib.Common.Recipes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using ECPLib.Common.Patient.Models;
using UMKBCoreAPI.Models.Api.Responses;

namespace PharmTaxiAPI.Methods.Recipe.Model
{
    public class SearchRecipesResponse
    {
        /// <summary>
        /// стандартный алерт
        /// </summary>
        public Alert Alert { get; set; } = new Alert { Code = "200", Level = "success", };
        /// <summary>
        /// Список найденых рецептов
        /// </summary>
        public List<ModelRecipeLS> Recipes { get; set; } = new List<ModelRecipeLS>();
    }

    public class ModelRecipeLS
    {
        /// <summary>
        /// Стандартный алерт
        /// </summary>
        public Alert alert { get; set; } = new Alert() { Code = "200", Level = "success" };
        /// <summary>
        /// Уникальный код рецепта
        /// </summary>
        public string recipe_code { get; set; }
        /// <summary>
        /// Номер рецепта
        /// </summary>
        public string recipeNumber { get; set; }
        /// <summary>
        /// Серия рецепта
        /// </summary>
        public string recipeSeries { get; set; }
        /// <summary>
        /// Список препаратов в рецепте
        /// </summary>
        public List<LS> liquids { get; set; } = new List<LS>();
        /// <summary>
        /// Имена к айдишникам (если их нет в кэше) в фармакотерапии в нужной локали
        /// </summary>
        public Dictionary<string, string> names { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// Пациент
        /// </summary>
        public Patient patient { get; set; } = new Patient();
        /// <summary>
        /// Id врача выписавшего рецепт
        /// </summary>
        public string idDoc { get; set; }
        /// <summary>
        /// Id юзера, выписавшего рецепт
        /// </summary>
        public string idUser { get; set; }
        /// <summary>
        /// Id отделения, где выписали рецепт
        /// </summary>
        public string idDep { get; set; }
        /// <summary>
        /// Id клиники, где выписали рецепт
        /// </summary>        
        public string idClinic { get; set; }
        /// <summary>
        /// Дата выписки рецепта (UnixTime мс.)
        /// </summary>
        public long dateStart { get; set; }
        /// <summary>
        /// Дата окончания действия рецепта (UnixTime мс.)
        /// </summary>
        public long? dateEnd { get; set; }
        /// <summary>
        /// Табличное значение продолжительности действия рецепта
        /// </summary>
        public ECPLib.Common.TableValueItem dateEndItem { get; set; } = new ECPLib.Common.TableValueItem();
        /// <summary>
        /// ID Рецептурной формы
        /// </summary>
        public string idForm { get; set; }
        /// <summary>
        /// ID Рецептурной формы
        /// </summary>
        public string NameForm { get; set; }
        /// <summary>
        /// ID статуса рецепта
        /// </summary>
        public uint IdStatus { get; set; }
        /// <summary>
        /// Диагноз по МКБ10
        /// </summary>
        public string dCode { get; set; }
        /// <summary>
        /// Дата рождения пациента (на момент выписки)
        /// </summary>
        public string pBirthday { get; set; }
        /// <summary>
        /// СНИЛС пациента (на момент выписки)
        /// </summary>
        public string pSnils { get; set; }
        /// <summary>
        /// Коментарий врача
        /// </summary>
        public string dTextComment { get; set; }
        /// <summary>
        /// Рецепт для взр
        /// </summary>
        public bool? IsAdult { get; set; }
        ///// <summary>
        ///// Список тегов шаблона
        ///// </summary>
        //public Dictionary<string, List<string>> dText { get; set; } = new Dictionary<string, List<string>>();
    }

    public class LS
    {
        public ModelDrug liquid { get; set; }
        public List<ModelDrug> drugs { get; set; } = new List<ModelDrug>();
    }
}
