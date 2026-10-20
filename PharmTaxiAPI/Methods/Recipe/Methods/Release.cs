using System;
using System.Collections.Generic;
using System.Linq;
using ECPLib.Common.Recipes.Models;
using MongoDB.Bson.Serialization;
using PTConstants;
using UMKBCoreAPI.Models.Api.Responses;

namespace PharmTaxiAPI.Methods.Recipe
{
    public static partial class Metods
    {
        public static Alert Release(Model.RecipeReleaseArg args, PharmTaxiAdm.Models.User.AlertUserID userInfo)
        {
            if (args is null || string.IsNullOrWhiteSpace(args.recipeId) || args.recipeId.Length < 3)
            {
                return new Alert { Code = "400", Level = "error", Message = "Не верный номер рецепта" };
            }
            if (args.pharmId == 0) { return new Alert { Code = "400", Level = "error", Message = "Не указана аптека" }; }
            if (args.recipeFullRelease && (args.fullReleaseStatus is null || args.fullReleaseStatus == 0)) { return new Alert { Code = "400", Level = "error", Message = "Статус рецепта не заполнен" }; }
            if (args.recipeFullRelease &&  args.fullReleaseStatus < 1) { return new Alert { Code = "400", Level = "error", Message = "Указан неправильный статус рецепта" }; }
            if (!args.recipeFullRelease && (args.drugStatuses is null || args.drugStatuses.Count < 1)) { return new Alert { Code = "400", Level = "error", Message = "Статусы ЛС не заполнены" }; }
            
            //хз надо оно или нет, даня говорит не надо
            //if (string.IsNullOrWhiteSpace(args.pharmName) || args.pharmName.Length < 3) { return new Alert { Code = "400", Level = "error", Message = "Bad pharmName" }; }

            try
            {
                //статус драга в рецепте(0 - активный, 1 - частично обслужен, 2 - обслужен, 3 - просрочен без обслуживания, 4 - просрочен и частично обслужен, 5 - просрочен и обслужен)
                args.recipeId = args.recipeId.ToUpper();
                var recipe = MongoRecipe.GetRecipe(args.recipeId);

                if (recipe is null) { return new Alert { Code = "204", Level = "success", Message = "Рецепт не найден" }; }

                bool updateRecipe = UpdateStatusRecipe(recipe);
                if (updateRecipe)
                {
                    MongoRecipe.UpdateRecipe(recipe);
                }

                if (recipe.IdStatus != "0" && recipe.IdStatus != "1") { return new Alert { Code = "400", Level = "error", Message = "Данный рецепт нельзя изменять" }; }
                if (recipe.DateStart > SMCommonUtils.Converter.GetNowInt64()) { return new Alert { Code = "400", Level = "error", Message = "Время действия рецепта не началось" }; }

                if (recipe.DateEnd < SMCommonUtils.Converter.GetNowInt64())
                {
                    return new Alert { Code = "400", Level = "error", Message = "Время действия рецепта закончилось" };
                }

                if ((recipe.Drugs is null || recipe.Drugs.Count < 1) && (recipe.Liquids is null || recipe.Liquids.Count < 1)) 
                {
                    return new Alert() { Code = "400", Level = "error", Title = "Структура рецепта не соответствует необходимой" }; 
                }

                Model.RecipeLog log = new Model.RecipeLog();
                log.UserId = userInfo.userid;
                log.RecipeId = args.recipeId;
                log.PHName = args.pharmName;

                PharmTaxiAdm.Models.WriteOffRequest release = new PharmTaxiAdm.Models.WriteOffRequest()
                {
                    authkey = args.authkey,
                    key = args.key,
                    drugstore_id = args.pharmId.ToString(),
                    status = "2",
                };

                bool releaseMed = false;

                if (args.recipeFullRelease)
                {
                    if ((args.fullReleaseStatus == 1 || args.fullReleaseStatus == 2) && recipe.IdStatus != args.fullReleaseStatus.ToString())
                    {
                        recipe.IdStatus = args.fullReleaseStatus.ToString();
                        log.IdStatus = (int)args.fullReleaseStatus;
                        updateRecipe = UpdateStatusDrugs(recipe, (int)args.fullReleaseStatus);
                    }
                    else
                    {
                        return new Alert { Code = "400", Level = "error", Message = "Данному рецепту нельзя изменить статус на указанный" };
                    }
                }
                else
                {
                    List<ModelDrugRecipe> listLS = new List<ModelDrugRecipe>();
                    if (recipe.Drugs != null && recipe.Drugs.Count > 0) { listLS.AddRange(recipe.Drugs); }
                    if (recipe.Liquids != null && recipe.Liquids.Count > 0) { listLS.AddRange(recipe.Liquids); }

                    if (listLS.Count > 0)
                    {
                        if (args.drugStatuses.Count > 0)
                        {
                            foreach (var drugStatus in args.drugStatuses)
                            {
                                foreach (var ls in recipe.Drugs)
                                {
                                    if (string.IsNullOrWhiteSpace(ls.Drug.key) || string.IsNullOrWhiteSpace(drugStatus.key) || drugStatus.idStatus == 0) { continue; }

                                    if (ls.Drug.key != drugStatus.key && ls.IdStatus != 0 && ls.IdStatus != 1) { continue; }

                                    if (drugStatus.idStatus == 0 || drugStatus.idStatus > 2) { continue; }

                                    ls.IdStatus = drugStatus.idStatus;
                                    ls.DateСhange = UMKBUtils.Extensions.DateTimeExtensions.ToInt64(DateTime.Now);

                                    if (log.Drugs is null) { log.Drugs = new List<Model.StatusLS>(); }
                                    Model.StatusLS statusLS = new Model.StatusLS() { IdStatus = drugStatus.idStatus, key = ls.Drug.key };

                                    if (drugStatus.medicaments != null && drugStatus.medicaments.Count > 0)
                                    {
                                        foreach (var fp in drugStatus.medicaments)
                                        {
                                            if (!string.IsNullOrWhiteSpace(fp.id)) { fp.id = Crypto.Сryptographer.Decrypt(fp.id).ToString(); }
                                            if (!string.IsNullOrWhiteSpace(fp.tn)) { fp.tn = Crypto.Сryptographer.Decrypt(fp.tn).ToString(); }

                                            release.medicaments.Add(fp);
                                            statusLS.medicaments.Add(fp);
                                        }

                                        releaseMed = true;
                                    }

                                    log.Drugs.Add(statusLS);
                                    updateRecipe = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (!updateRecipe) { return new Alert() { Code = "400", Level = "success", Title = "Нет изменений" }; }

                if (recipe.IdStatus == "0") { recipe.IdStatus = "1"; }

                Exception rezLog = MongoRecipeLog.Insert(log);
                Exception rez = MongoRecipe.UpdateRecipe(recipe);

                if (rez != null || rezLog != null)
                {
                    string exMessange = string.Empty;

                    if (!(rez is null)) { exMessange = rez.Message; }
                    if (!(rezLog is null)) { exMessange = " " + rezLog.Message; }

                    return new Alert { Code = "500", Level = "error", Title = "Server Error", Message = exMessange, Sticky = true };
                }

                if (args.releaseMedicaments && releaseMed)
                {
                    foreach (var med in release.medicaments)
                    {
                        if (!string.IsNullOrWhiteSpace(med.id)) { med.id = Crypto.Сryptographer.Encrypt(med.id).ToString(); }
                        if (!string.IsNullOrWhiteSpace(med.tn)) { med.tn = Crypto.Сryptographer.Encrypt(med.tn).ToString(); }
                    }

                    string tmpAK = release.authkey;

                    PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.WriteOff(release);

                    if (release.authkey != tmpAK)
                    {
                        return new Alert() { Code = "400", Level = "error", Title = "Error", Message = release.auth_key };
                    }
                }

                return new Alert() { Code = "200", Level = "success" };
            }
            catch (Exception ex)
            {
                return new Alert { Code = "500", Level = "error", Title = "Server Error", Message = ex.Message, Sticky = true };
            }
        }

        public static bool UpdateStatusRecipe(ModelRecipeBase recipe)
        {
            bool updateRecipe = false;
            try
            {//статус драга в рецепте(0 - активный, 1 - частично обслужен, 2 - обслужен, 3 - просрочен без обслуживания, 4 - просрочен и частично обслужен, 5 - просрочен и обслужен)
                if (string.IsNullOrWhiteSpace(recipe.IdStatus)) { recipe.IdStatus = "0"; updateRecipe = true; }

                if (recipe.DateEnd < SMCommonUtils.Converter.GetNowInt64())
                {
                    if (recipe.IdStatus == "0") { recipe.IdStatus = "3"; updateRecipe = true; }
                    if (recipe.IdStatus == "1") { recipe.IdStatus = "4"; updateRecipe = true; }
                    if (recipe.IdStatus == "2") { recipe.IdStatus = "5"; updateRecipe = true; }
                }

                if (recipe.IdStatus == "0")
                {
                    List<ModelDrugRecipe> list = new List<ModelDrugRecipe>();

                    if (recipe.Drugs != null && recipe.Drugs.Count > 0) { list.AddRange(recipe.Drugs); }
                    if (recipe.Liquids != null && recipe.Liquids.Count > 0) { list.AddRange(recipe.Liquids); }

                    foreach (var drug in list)
                    {
                        if (drug.IdStatus != 0)
                        {
                            recipe.IdStatus = "0";
                            recipe.DateСhange = drug.DateСhange;
                            updateRecipe = true;
                            break;
                        }
                    }
                }

                if (recipe.IdStatus == "1")
                {
                    List<ModelDrugRecipe> list = new List<ModelDrugRecipe>();

                    if (recipe.Drugs != null && recipe.Drugs.Count > 0) { list.AddRange(recipe.Drugs); }
                    if (recipe.Liquids != null && recipe.Liquids.Count > 0) { list.AddRange(recipe.Liquids); }

                    foreach (var drug in list)
                    {
                        if (drug.IdStatus > 2)
                        {
                            drug.IdStatus = 2;
                            drug.DateСhange = SMCommonUtils.Converter.GetNowInt64();
                            updateRecipe = true;
                        }
                    }
                }

                if (recipe.IdStatus == "2")
                {
                    List<ModelDrugRecipe> list = new List<ModelDrugRecipe>();

                    if (recipe.Drugs != null && recipe.Drugs.Count > 0) { list.AddRange(recipe.Drugs); }
                    if (recipe.Liquids != null && recipe.Liquids.Count > 0) { list.AddRange(recipe.Liquids); }

                    foreach (var drug in list)
                    {
                        if (drug.IdStatus != 2)
                        {
                            drug.IdStatus = 2;
                            drug.DateСhange = SMCommonUtils.Converter.GetNowInt64();
                            updateRecipe = true;
                        }
                    }
                }


            }
            catch { }

            return updateRecipe;
        }

        public static bool UpdateStatusDrugs(ModelRecipeBase recipe, int status)
        {
            bool updateRecipe = false;
            try
            {
                if (status == 1 || status == 2)
                {
                    List<ModelDrugRecipe> list = new List<ModelDrugRecipe>();
                    if (recipe.Drugs != null && recipe.Drugs.Count > 0) { list.AddRange(recipe.Drugs); }
                    if (recipe.Liquids != null && recipe.Liquids.Count > 0) { list.AddRange(recipe.Liquids); }

                    foreach (var drug in list)
                    {
                        drug.IdStatus = status;
                        drug.DateСhange = SMCommonUtils.Converter.GetNowInt64();
                        updateRecipe = true;
                    }
                }
            }
            catch { }

            return updateRecipe;
        }
    }
}
