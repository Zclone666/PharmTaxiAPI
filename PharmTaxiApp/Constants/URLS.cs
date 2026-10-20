//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace PharmTaxiApp.Constants
//{
//    /// <summary>
//    /// URLs
//    /// </summary>
//    public static class URLS
//    {
//        public static string ecp = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production") ? "https://service.socmedica.dev:9004" : "https://service.socmedica.com:9004";
//        /// <summary>
//        /// elements
//        /// </summary>
//        public static string elements = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production") ? $"{ecp}/info/elements" : $"{ecp}/info/elements";
//        /// <summary>
//        /// UIText
//        /// </summary>
//        public static string UIText = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production") ? $"{ecp}/info/UIText" : $"{ecp}/info/UIText";
//        /// <summary>
//        /// Dosages
//        /// </summary>
//        public static string Dosages = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production") ? $"{ecp}/drugs/Dosages" : $"{ecp}/drugs/Dosages";
//        /// <summary>
//        /// FPDosages
//        /// </summary>
//        public static string FPDosages = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production") ? $"{ecp}/drugs/FPDosage" : $"{ecp}/drugs/FPDosage";
//        /// <summary>
//        /// CreateDosage
//        /// </summary>
//        public static string CreateDosage = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production") ? $"{ecp}/drugs/CreateDosage" : $"{ecp}/drugs/CreateDosage";
//        /// <summary>
//        /// Deseases
//        /// </summary>
//        public static string Deseases = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production") ? $"{ecp}/Search/Diseases" : $"{ecp}/Search/Diseases";
//        /// <summary>
//        /// Allergy
//        /// </summary>
//        public static string Allergy = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production") ? $"{ecp}/Search/Allergy" : $"{ecp}/Search/Allergy";
//        /// <summary>
//        /// Info
//        /// </summary>
//        public static string Info = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production") ? $"{ecp}/drugs/Info" : $"{ecp}/drugs/Info";
//        /// <summary>
//        /// buttonTexts
//        /// </summary>
//        public static string buttonTexts = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production") ? $"{ecp}/info/buttonTexts" : $"{ecp}/info/buttonTexts";
//        /// <summary>
//        /// Schedule
//        /// </summary>
//        public static string Schedule = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production") ? $"{ecp}/info/Schedule" : $"{ecp}/info/Schedule";
//        /// <summary>
//        /// UnitsPharn
//        /// </summary>
//        public static string UnitsPharm = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production") ? $"{ecp}/tools/UnitsPharm" : $"{ecp}/tools/UnitsPharm";
//        /// <summary>
//        /// UnitsPrimary
//        /// </summary>
//        public static string UnitsPrimary = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production") ? $"{ecp}/tools/UnitsPrimary" : $"{ecp}/tools/UnitsPrimary";
//        /// <summary>
//        /// Actual
//        /// </summary>
//        public static string Actual = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production") ? $"{ecp}/Prescript/Actual" : "https://service.socmedica.com:9004/Prescript/Actual";
//        /// <summary>
//        /// Recipes
//        /// </summary>
//        public static string Recipes = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production") ? $"{ecp}/Prescript/Recipes" : "https://service.socmedica.com:9004/Prescript/Recipes";
//        /// <summary>
//        /// RecipesCore
//        /// </summary>
//        public static string RecipesCore = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production") ? $"{ecp}/Prescript/RecipeCore" : "https://service.socmedica.com:9004/Prescript/RecipeCore";
//        /// <summary>
//        /// Warnings
//        /// </summary>
//        public static string Warnings = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production") ? $"{ecp}/Check/Warnings" : $"{ecp}/Check/Warnings";
//        public static string PrescrInfoShort = $"{eRpPrescript}/api/prescription/short/list";
//        public static string PrescrInfoFull = $"{eRpPrescript}/api/prescription/list";
//        public static string PrescRelease = $"{eRpPrescript}/api/release";
//        public const string eRpPrescript = "https://e-rp.ru/PrescriptionDev";
//    }
//}
