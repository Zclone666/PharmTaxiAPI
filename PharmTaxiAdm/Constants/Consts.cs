//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace PharmTaxiAdm.Constants
//{
//    public static class Consts
//    {
//        //public const string SQLConnectionString = ""server=db.socmedica.com;port=33066;database=umkb;user id = umkb_o; password='HG;ls;kg8934=12f';allowuservariables=True;CharSet=utf8;Convert Zero Datetime=True;Allow Zero Datetime=True; Pooling = false; Connect Timeout = 60"; //"server=db.socmedica.dev;port=33066;Initial Catalog = umkb; User ID = dssc_user; Password=o5BqvY3Til14xdXY; Pooling = false; Connect Timeout = 60"";
//        public const string TableGetKey = "bcec3f6965690cf1";
//        public const string GraphKey = "4c849ec05fa683de";
//        public const string ConceptKey = "03741c4866069ae2";
//        public const string NamesKey = "3221b197bf5533b4";
//        public const string AbdParseKey = "5821b27bad65bea5";
//        public const string ConceptListKey = "7b52c441e15d79cf";
//        public const string GraphListKey = "c353686107266b69";
//        public const string UPCReqAddress = "https://api.upcitemdb.com/prod/trial/lookup";
//        public const string SQLConnectionString = "server=db.socmedica.com;port=33066;database=umkb;user id=umkb_o;password='HG;ls;kg8934=12f';allowuservariables=True;CharSet=utf8;Convert Zero Datetime=True;Allow Zero Datetime=True; Pooling = false; Connect Timeout=60"; //"server=umkb.socmedica.dev;port=33066;Initial Catalog=dccs;User ID=dssc_user;Password=o5BqvY3Til14xdXY";
//        public const string SQLDev = "server=db.socmedica.com;port=33066;database=umkb;user id=umkb_o;password='HG;ls;kg8934=12f';allowuservariables=True;CharSet=utf8;Convert Zero Datetime=True;Allow Zero Datetime=True; Pooling = false; Connect Timeout=60"; //"server=db.socmedica.dev;port=33066;Initial Catalog=umkb;User ID=dssc_user;Password=o5BqvY3Til14xdXY; Pooling = false; Connect Timeout=60";
//        public const string SQLConnectionString73 = "server=db.socmedica.com;port=33066;database=umkb;user id=umkb_o;password='HG;ls;kg8934=12f';allowuservariables=True;CharSet=utf8;Convert Zero Datetime=True;Allow Zero Datetime=True; Pooling = false; Connect Timeout=60";
//        //public const string TableGetKey = "bcec3f6965690cf1";
//        //public const string GraphKey = "34ff6a9a4b004fa1";
//        //public const string ConceptKey = "03741c4866069ae2";
//        //public const string NamesKey = "3221b197bf5533b4";
//        //public const string AbdParseKey = "5821b27bad65bea5";

//        /// <summary>
//        /// Key for recognize
//        /// </summary>
//        /// 
//        public const string RecognizeKey = "93da6407073da759";
//        public const int ScriptId = 5714;
//        public static string MongoDb = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production") ? "mongodb://pht:pht2021@mongodb.socmedica.com:27027" : "mongodb://pht:pht2021@mongodb.socmedica.com:27027";
//        public const string Reciepes_Connect = "mongodb://pht:pht2021@mongodb.socmedica.com:27027/";
//        public const string SKOL = "SKOL";
//        public const string PHT = "PharmTaxi";
//        public const string PHTFORSTFR = "PHTForSLF";
//        public const string Assortiment = "assortiment";
//        public const string AssortimentSmall = "assortiment_small";
//        public const string Pharms = "pharms";
//        public const string PharmsFD = "pharmsFD";
//        public const string PharmNets = "pharmNets";
//        public const string UsersCollection = "users";
//        public const string GS1_user = "4640027099999";
//        public const string GS1_pass = "socmedica123";
//        public const string ECPTechService = "108";
//        /// <summary>
//        /// MongoDB имя БД пациентов
//        /// </summary>
//        public static string Patients_DataBase = "ECP";
//        /// <summary>
//        /// MongoDB Коллекция пациентов
//        /// </summary>
//        public static string Patients_Collection = "Patients";

//        /// <summary>
//        /// MongoDB Имя БД рецептов 
//        /// </summary>
//        public static string Reciepes_DataBase = "ECP_Recipes";
//        public const string Orders_DataBase = "orders";
//    }
//}