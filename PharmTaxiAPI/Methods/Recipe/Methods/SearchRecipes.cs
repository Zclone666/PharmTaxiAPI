using ECPLib.Common.Recipes.Models;
using MongoDB.Driver;
using PharmTaxiAPI.Methods.Recipe.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using UMKBCoreAPI.Models.Api.Responses;
using ECPLib.Common.Patient.Models;
using PTConstants;
using System.Threading.Tasks;
using EMC.ECPIntegration.Models;

namespace PharmTaxiAPI.Methods.Recipe
{
    public static partial class Metods
    {
        public static async Task<SearchRecipesResponse> SearchRecipes(SearchRecipesArgs args)
        {
            SearchRecipesResponse result = new SearchRecipesResponse();

            try
            {
                if (!string.IsNullOrWhiteSpace(args.recipeId) && args.recipeId.Length > 2)
                {
                    var recipe = MongoRecipe.GetRecipe(args.recipeId);

                    if (recipe is null)
                    {
                        result.Alert.Level = "info";
                        result.Alert.Title = "Recipe not found ";

                        return result;
                    }

                    var patient = await EMC.Singleton.GetPatient(recipe.IdPatient, "ru");
                    PatientEncrypt(patient.Patient);

                    result.Recipes.Add(ConvertRecipe(recipe, patient.Patient));
                }

                if (!string.IsNullOrWhiteSpace(args.snils) || !string.IsNullOrWhiteSpace(args.email))
                {
                    MongoClient clientRecipe = new MongoClient(Mongo.ReciepesConString);
                    IMongoDatabase dbRecipe = clientRecipe.GetDatabase(Mongo.ReciepesDBName);
                    IMongoCollection<ModelPatientStats> collectionRecipe = dbRecipe.GetCollection<ModelPatientStats>(Mongo.ReciepesPatientsCollectionName);

                    if (!string.IsNullOrWhiteSpace(args.snils) && args.snils.Length == 14 && args.snils.Contains("-") && args.snils.Contains(" "))
                    {
                        var patientResult = await EMC.Singleton.SearchPatient(new SearchPatientArgs() { SNILS = args.snils }, "ru");

                        if (patientResult.Alert is null)
                        {
                            ModelPatientStats recipes = null;
                            try
                            {
                                recipes = collectionRecipe.Find(Builders<ModelPatientStats>.Filter.Eq("id", patientResult.Patient.id)).First();
                            }
                            catch { }

                            PatientEncrypt(patientResult.Patient);

                            foreach (var recipeNumber in recipes.recipes.Keys)
                            {
                                var recipe = MongoRecipe.GetRecipe(recipeNumber);

                                result.Recipes.Add(ConvertRecipe(recipe, patientResult.Patient));
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(args.email) && args.email.Contains("@") && args.email.Contains("."))
                    {
                        var patientResult = await EMC.Singleton.SearchPatient(new SearchPatientArgs() { Email = args.email }, "ru");

                        if (patientResult.Alert is null)
                        {
                            ModelPatientStats recipes = null;
                            try
                            {
                                recipes = collectionRecipe.Find(Builders<ModelPatientStats>.Filter.Eq("id", patientResult.Patient.id)).First();
                            }
                            catch { }

                            PatientEncrypt(patientResult.Patient);

                            foreach (var recipeNumber in recipes.recipes.Keys)
                            {
                                var recipe = MongoRecipe.GetRecipe(recipeNumber);

                                result.Recipes.Add(ConvertRecipe(recipe, patientResult.Patient));
                            }
                        }
                    }
                }

                if (result.Recipes.Count < 1) { result.Alert.Level = "info"; result.Alert.Title = "Recipe not found "; }

                return result;
            }
            catch (Exception ex)
            {
                return new SearchRecipesResponse() { Alert = new Alert() { Code = "500", Level = "error", Message = ex.Message } };
            }
        }

        private static ModelRecipeLS ConvertRecipe(ModelRecipeBase recipe, Patient patient)
        {
            ModelRecipeLS recipeFront = new ModelRecipeLS();

            try
            {
                if (recipe == null && patient == null) { return recipeFront; }

                string idDoct = string.Empty;
                if (!string.IsNullOrWhiteSpace(recipe.IdDoc))
                {
                    idDoct = recipe.IdDoc;
                }

                string idClinica = string.Empty;
                if (!string.IsNullOrWhiteSpace(recipe.IdClinic))
                {
                    idClinica = recipe.IdClinic;
                }

                RecipeEncrypt(recipe);

                recipeFront.patient = patient;
                recipeFront.idClinic = recipe.IdClinic;
                recipeFront.recipe_code = recipe.id;
                recipeFront.idDoc = recipe.IdDoc;
                recipeFront.idUser = recipe.IdUser;
                recipeFront.idDep = recipe.IdDep;
                recipeFront.dateStart = recipe.DateStart;
                recipeFront.dateEnd = recipe.DateEnd;
                recipeFront.dateEndItem = recipe.DateEndItem;
                recipeFront.idForm = recipe.IdForm;
                recipeFront.dCode = recipe.PDCode;
                recipeFront.pSnils = recipe.PSnils;
                if (recipe.DText != null && recipe.DText.ContainsKey("C")) { recipeFront.dTextComment = recipe.DText["C"][0]; }
                if (recipe.PBirthday.HasValue) { recipeFront.pBirthday = recipe.PBirthday.ToString(); }
                if (recipe.PAdult != null) { recipeFront.IsAdult = recipe.PAdult; }
                //if (recipe.DText != null) { recipeFront.dText = recipe.DText; }

                if (!string.IsNullOrWhiteSpace(recipe.series)) { recipeFront.recipeSeries = recipe.series; }
                if (string.IsNullOrWhiteSpace(recipe.number)) 
                {
                    recipeFront.recipeNumber = recipe.id; 
                }
                else
                {
                    recipeFront.recipeNumber = recipe.number;
                }

                if (!string.IsNullOrWhiteSpace(recipe.IdStatus) && uint.TryParse(recipe.IdStatus, out uint idStatys)) { recipeFront.IdStatus = idStatys; }

                if (recipe.Liquids != null && recipe.Liquids.Count > 0)
                {
                    recipeFront.liquids = new List<LS>();
                    LS ls = new LS() { liquid = new ModelDrug(recipe.Liquids[0].Drug) { IdStatus = recipe.Liquids[0].IdStatus, DateСhange = recipe.Liquids[0].DateСhange } };

                    foreach (var liquid in recipe.Liquids)
                    {
                        ls.drugs.Add(new ModelDrug(liquid.Drug) { IdStatus = liquid.IdStatus, DateСhange = liquid.DateСhange });
                    }
                }

                if (recipe.Drugs != null && recipe.Drugs.Count > 0)
                {
                    LS newLS = new LS() { drugs = new List<ModelDrug>() };

                    foreach (var recipeDrug in recipe.Drugs)
                    {
                        newLS.drugs.Add(new ModelDrug(recipeDrug.Drug)
                        {
                            DateСhange = recipeDrug.DateСhange,
                            IdStatus = recipeDrug.IdStatus,
                        });

                        recipeFront.names.Add(recipeDrug.Drug.id, recipeDrug.Drug.name);
                    }

                    recipeFront.liquids.Add(newLS);
                }

                UMKBRequests.Models.API.Codes.CodesArgs names = new UMKBRequests.Models.API.Codes.CodesArgs()
                {
                    permit = new UMKBRequests.Models.API.Codes.Permit() { key = UMKB.GetNamesListDB },
                    longids = new List<string>() { idDoct, idClinica }
                };

                var resault = UMKBRequests.GetNameListDB.Get(names);

                if (resault.alert is null || resault.alert.code != "200" || resault.result is null || resault.result.Count < 0)
                {
                    return new ModelRecipeLS
                    {
                        alert = new Alert
                        {
                            Code = resault.alert.code,
                            Level = resault.alert.level,
                            Title = resault.alert.title,
                            Message = resault.alert.message,
                            Sticky = resault.alert.sticky,
                        }
                    };
                }

                if (uint.TryParse(idDoct, out uint idDoc))
                {
                    idDoc = idDoc / 10000;
                    recipeFront.names.Add(recipe.IdDoc, resault.result.GetValueOrDefault((1200000000 + idDoc).ToString()));
                }

                if (uint.TryParse(idClinica, out uint idClinic))
                {
                    idClinic = idClinic / 10000;
                    recipeFront.names.Add(recipe.IdClinic, resault.result.GetValueOrDefault((1210000000 + idClinic).ToString()));
                }


                return recipeFront;
            }
            catch (Exception ex)
            {
                recipeFront.alert = new Alert() { Code = "500", Level = "error", Message = ex.Message };
                return recipeFront;
            }
        }
    }
}
