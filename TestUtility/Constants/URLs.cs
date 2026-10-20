using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUtility.Constants
{
    public static class URLs
    {
        public const string PHTUrl_Local = "https://localhost:44362/";//"https://service.socmedica.com:9025/";
        public const string Check = "Check";
        public const string GetRecipeJson = "GetRecipeJson";
        public const string GetRecipeURL = "GetReceiptURL";
        public const string SaveOrder = "SaveOrder";
        public const string SavePrescript = "SavePrescript";
        public const string LoadAssortiment = "LoadAssortiment";
        public const string Authorize = "Authorize";
        public const string Register = "Register";
        public const string GenerateUser = "GenerateUser";
        public const string AttachDrugstoreToUser = "AttachDrugstoreToUser";
        public const string GetPharmOrders = "GetPharmOrders";
        public const string GetPharmOrdersOld = "getDrugstoreOrders";
        public const string CreateDrugstore ="CreateDrugstore";
        public const string CreateDrugstoreWL = "CreateDrugstoreWithLicKey";
        public const string CreateSlaveWithEmail = "CreateSlaveWithEmail";
        public const string CreateGuestUser = "CreateGuest";
        public const string CreateUserFromGuest = "CreateUserFromGuest";
        public const string CheckMustHave = "CheckMustHave";
        public const string GetDrugstoresByUserId = "GetDrugstoresByUserId";
    }
}
