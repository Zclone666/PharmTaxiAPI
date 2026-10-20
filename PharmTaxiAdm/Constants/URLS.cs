//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace PharmTaxiAdm.Constants
//{
//    public static class URLS
//    {
//        public static string ecp = "https://service.socmedica.com:9004";
//        public static string eRpPrescript = "https://e-rp.ru/PrescriptionDev";
//        public static string elements = $"{ecp}/info/elements";
//        public static string UIText = $"{ecp}/info/UIText";
//        public static string Dosages = $"{ecp}/drugs/Dosages";
//        public static string buttonTexts = $"{ecp}/info/buttonTexts";
//        public static string Schedule = $"{ecp}/info/Schedule";
//        public static string UnitsPharm = $"{ecp}/tools/UnitsPharm";
//        public static string UnitsPrimary = $"{ecp}/tools/UnitsPrimary";
//        public static string GetReceiptJson = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production") ? $"{ecp}/Prescript/RecipeCore" : "https://service.socmedica.com:9004/Prescript/RecipeCore";
//        public static string GetReceiptURL = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production") ? $"{ecp}/prescript/recipe?code=" : "https://service.socmedica.com:9004/prescript/recipe?code=";
//        //public static string Dev = $"{ecp}";
//        //public static string Prod = "https://cs.socmedica.com";
//        public static readonly string Server = GetServer();
//        private static string GetServer()
//        {

//            //if(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
//            //  if (Configuration.serverType == Configuration.ServerType.Dev) 
//            //  return $"{ecp}";//(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production") ? "https://cs.socmedica.com": $"{ecp}";
//            return "https://cs.socmedica.com";
//        }
//        public static readonly string Authorization = $"{Server}/api/user/authorization";
//        public static readonly string CheckRighstUserId = $"{Server}/api/umkb/CheckRightsUserid";
//        public static readonly string GenerateUserByAuthkey = $"{Server}/api/user/GenerateUserByAuthkey";
//        public static readonly string Registration = $"{Server}/api/registration/send";
//        //public static string UriCheckRighstUserIdPROD = "https://cs.socmedica.com/api/umkb/CheckRightsUserid";
//        //public static string UriGetDescriptionsDBDEV = $"{ecp}/API/UMKB/GetDescriptionsDB";
//        public static readonly string GetNamesDB = $"{Server}/api/umkb/GetNamesDB";
//        public static readonly string Modelling = $"{Server}/api/umkb/modeling";
//        public static readonly string GetVarcharWhere = $"{Server}/API/UMKB/GetVarcharWhere";
//        public static string GetUserInfoByEmail = $"{Server}/api/user/getUserInfoByEmail";
//        public static string PassRecover = $"{Server}/api/user/recovery";
//        public static string ConfirmEmail = $"{Server}/api/registration/confirmemail";
//        public static string Invite = $"{Server}/api/invitations/invite";
//        public static string ConfirmInvite = $"{Server}/api/invitations/confirmInviteAuthkey";
//        //public static string UriGetVarcharListDEV = $"{ecp}/api/umkb/getVarcharList";

//        public static string RegisterUserByUser = $"{Server}/api/registration/registeruserbyuser";

//        public static string CreateSrvByUsersAuth = $"{Server}/api/user/addservicebyauthkey";

//        public static string PrescrInfoShort = $"{eRpPrescript}/api/prescription/short/list";
//        public static string PrescrInfoFull = $"{eRpPrescript}/api/prescription/list";
//        public static string PrescRelease = $"{eRpPrescript}/api/release";
//    }
//}
