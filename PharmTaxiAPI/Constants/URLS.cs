//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace PharmTaxiAPI.Constants
//{
//    public static class URLS
//    {
//        public static string ecp = "https://service.socmedica.com:9004";
//        public static string elements = $"{ecp}/info/elements";
//        public static string UIText = $"{ecp}/info/UIText";
//        public static string Dosages = $"{ecp}/drugs/Dosages";
//        public static string buttonTexts = $"{ecp}/info/buttonTexts";
//        public static string Schedule = $"{ecp}/info/Schedule";
//        public static string UnitsPharm = $"{ecp}/tools/UnitsPharm";
//        public static string UnitsPrimary = $"{ecp}/tools/UnitsPrimary";

//        /// <summary>
//        /// URL для создания гостевого пользователя
//        /// </summary>
//        public static string CreateGuestUserURL = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production") ? "https://cs.socmedica.com/api/guest/create" : "https://cs.socmedica.com/api/guest/create";
//        /// <summary>
//        /// URL для создания пользователя из гостя
//        /// </summary>
//        public static string CreateUserFromGuestURL = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production") ? "https://cs.socmedica.com/api/guest/RegisterUserFromGuest" : "https://cs.socmedica.com/api/guest/RegisterUserFromGuest";
//        /// <summary>
//        /// URL для подтверждения приглашения
//        /// </summary>
//        public static string ConfirmInvite = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production") ? "https://cs.socmedica.com/api/invitations/confirmInviteAuthkey" : "https://cs.socmedica.com/api/invitations/confirmInviteAuthkey";
//        /// <summary>
//        /// URL для приглашения
//        /// </summary>
//        public static string CreateInvite = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production") ? "https://cs.socmedica.com/api/invitations/invite" : "https://cs.socmedica.com/api/invitations/invite";

//        public static string getUserInfoByEmail = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production") ? "https://cs.socmedica.com/api/user/getUserInfoByEmail" : "https://cs.socmedica.com/api/user/getUserInfoByEmail";
//    }
//}
