using ECPLib.Common.Recipes.Models;
using PTConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Methods.Recipe
{
    public static partial class Metods
    {
        public static ModelRecipeBase RecipeEncrypt(ModelRecipeBase recipe)
        {
            if (!string.IsNullOrWhiteSpace(recipe.IdUser)) recipe.IdUser = Crypto.Сryptographer.Encrypt(recipe.IdUser);
            if (!string.IsNullOrWhiteSpace(recipe.IdPatient)) recipe.IdPatient = Crypto.Сryptographer.Encrypt(recipe.IdPatient);
            if (!string.IsNullOrWhiteSpace(recipe.IdDoc)) recipe.IdDoc = Crypto.Сryptographer.Encrypt(recipe.IdDoc);
            if (!string.IsNullOrWhiteSpace(recipe.IdClinic)) recipe.IdClinic = Crypto.Сryptographer.Encrypt(recipe.IdClinic);
            if (!string.IsNullOrWhiteSpace(recipe.IdDep)) recipe.IdDep = Crypto.Сryptographer.Encrypt(recipe.IdDep);
            if (!string.IsNullOrWhiteSpace(recipe.IdForm)) recipe.IdForm = Crypto.Сryptographer.Encrypt(recipe.IdForm);
            if (!string.IsNullOrWhiteSpace(recipe.IdKey)) recipe.IdKey = Crypto.Сryptographer.Encrypt(recipe.IdKey);
            if (recipe.DateEndItem != null && !string.IsNullOrWhiteSpace(recipe.DateEndItem.id)) recipe.DateEndItem.id = Crypto.Сryptographer.Encrypt(recipe.DateEndItem.id);

            List<ModelDrugRecipe> ls = new List<ModelDrugRecipe>();
            if (recipe.Drugs != null && recipe.Drugs.Count > 0) { ls.AddRange(recipe.Drugs); }
            if (recipe.Liquids != null && recipe.Liquids.Count > 0) { ls.AddRange(recipe.Liquids); }

            foreach (var drug in ls)
            {
                if (!string.IsNullOrWhiteSpace(drug.Drug.id)) drug.Drug.id = Crypto.Сryptographer.Encrypt(drug.Drug.id);
                if (!string.IsNullOrWhiteSpace(drug.Drug.fpid)) drug.Drug.fpid = Crypto.Сryptographer.Encrypt(drug.Drug.fpid);
                if (!string.IsNullOrWhiteSpace(drug.Drug.mnnid)) drug.Drug.mnnid = Crypto.Сryptographer.Encrypt(drug.Drug.mnnid);
                if (!string.IsNullOrWhiteSpace(drug.Drug.recipenameid)) drug.Drug.recipenameid = Crypto.Сryptographer.Encrypt(drug.Drug.recipenameid);
                if (!string.IsNullOrWhiteSpace(drug.Drug.duration_id)) drug.Drug.duration_id = Crypto.Сryptographer.EncryptUInt64(drug.Drug.duration_id);
                if (!string.IsNullOrWhiteSpace(drug.Drug.freq_id)) drug.Drug.freq_id = Crypto.Сryptographer.Encrypt(drug.Drug.freq_id);
                if (!string.IsNullOrWhiteSpace(drug.Drug.repealdoc)) drug.Drug.repealdoc = Crypto.Сryptographer.Encrypt(drug.Drug.repealdoc);

                if (drug.Drug.dosage != null)
                {
                    if (!string.IsNullOrWhiteSpace(drug.Drug.dosage.form)) drug.Drug.dosage.form = Crypto.Сryptographer.Encrypt(drug.Drug.dosage.form);
                    if (!string.IsNullOrWhiteSpace(drug.Drug.dosage.method)) drug.Drug.dosage.method = Crypto.Сryptographer.Encrypt(drug.Drug.dosage.method);

                    if (drug.Drug.dosage.dosage != null)
                    {
                        foreach (var dose in drug.Drug.dosage.dosage)
                        {
                            if (!string.IsNullOrWhiteSpace(dose.id)) dose.id = Crypto.Сryptographer.Encrypt(dose.id);
                        }
                    }

                    if (drug.Drug.dosage.pack != null && !string.IsNullOrWhiteSpace(drug.Drug.dosage.pack.id)) drug.Drug.dosage.pack.id = Crypto.Сryptographer.Encrypt(drug.Drug.dosage.pack.id);
                }

                if (drug.Drug.schedule != null)
                {
                    if (!string.IsNullOrWhiteSpace(drug.Drug.schedule.finishAlternate)) drug.Drug.schedule.finishAlternate = Crypto.Сryptographer.Encrypt(drug.Drug.schedule.finishAlternate);
                }
            }

            return recipe;
        }
    }
}
