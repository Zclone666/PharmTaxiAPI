using ECPLib.Common.Recipes.Models;
using PharmTaxiAPI.Methods.Recipe.Model;
using PTConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UMKBCoreAPI.Models.Api.Responses;

namespace PharmTaxiAPI.Methods.Recipe
{
    public static partial class Metods
    {
        public static RecipeAndHistory GetRecipeAndHistory(RecipeAndHistoryArgs args)
        {
            if (args is null || string.IsNullOrWhiteSpace(args.recipeId) || args.recipeId.Length < 3)
            {
                return new RecipeAndHistory { alert = new Alert { Code = "400", Level = "error", Message = "Bad recipe number" } };
            }

            try
            {
                RecipeAndHistory history = new RecipeAndHistory();

                args.recipeId = args.recipeId.ToUpper();
                history.recipe = MongoRecipe.GetRecipe(args.recipeId);

                if (history.recipe is null) { return new RecipeAndHistory { alert = new Alert { Code = "200", Level = "success", Message = "No Recipe Found" } }; }

                bool recipeEdit = false;
                if (history.recipe.DateEnd < SMCommonUtils.Converter.GetNowInt64())
                {//статус драга в рецепте(0 - активный, 1 - частично обслужен, 2 - обслужен, 3 - просрочен без обслуживания, 4 - просрочен и частично обслужен, 5 - просрочен и обслужен)
                    if (history.recipe.IdStatus == "0") { history.recipe.IdStatus = "3"; }
                    if (history.recipe.IdStatus == "1") { history.recipe.IdStatus = "4"; }
                    if (history.recipe.IdStatus == "2") { history.recipe.IdStatus = "5"; }

                    recipeEdit = true;
                }

                if (recipeEdit)
                {
                    MongoRecipe.UpdateRecipe(history.recipe);
                }

                RecipeEncrypt(history.recipe);

                if (args.getHistory)
                {
                    RecipeLogFilter filter = new RecipeLogFilter() { RecipeId = args.recipeId };
                    history.history = MongoRecipeLog.Select(filter);

                    if (history.history != null)
                    {
                        foreach (var hist in history.history)
                        {
                            if(hist.Drugs is null || hist.Drugs.Count < 0) { continue; }
                            
                            foreach (var drug in hist.Drugs)
                            {
                                if (drug.medicaments is null || drug.medicaments.Count < 0) { continue; }

                                foreach (var med in drug.medicaments)
                                {
                                    if (!string.IsNullOrWhiteSpace(med.id)) { med.id = Crypto.Сryptographer.Encrypt(med.id).ToString(); }
                                    if (!string.IsNullOrWhiteSpace(med.tn)) { med.tn = Crypto.Сryptographer.Encrypt(med.tn).ToString(); }
                                }
                            }
                        }
                    }
                }

                return history;
            }
            catch (Exception ex)
            {
                return new RecipeAndHistory { alert = new Alert { Code = "500", Level = "error", Title = "Server Error", Message = ex.Message, Sticky = true } };
            }
        }
    }
}
