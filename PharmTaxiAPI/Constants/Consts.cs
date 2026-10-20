//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace PharmTaxiAPI.Constants
//{
//    public static class Consts
//    {
//        public enum Server
//        {
//            Development,
//            Production
//        }

//        public static Server GetServer()
//        {
//            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
//            {
//                return Server.Production;
//            }
//            return Server.Development;
//        }

//        //public const string SQLConnectionString = ""server=db.socmedica.com;port=33066;database=umkb;user id=umkb_o;password='HG;ls;kg8934=12f';allowuservariables=True;CharSet=utf8;Convert Zero Datetime=True;Allow Zero Datetime=True; Pooling = false; Connect Timeout=60"; //"server=db.socmedica.dev;port=33066;Initial Catalog=umkb;User ID=dssc_user;Password=o5BqvY3Til14xdXY; Pooling = false; Connect Timeout=60"";
//        /// <summary>
//        /// API для получения параметров концепта в формате value (табличные-параметрические данные) (bcec3f6965690cf1 старый)
//        /// </summary>
//        public const string TableGetKey = "cbb2a69af9ff974d";
//        /// <summary>
//        /// API для получения смысловых графов (4c849ec05fa683de старый)
//        /// </summary>
//        public const string GraphKey = "9902650c1d43506f";
//        /// <summary>
//        /// API для поиска концептов в текстовой строке (03741c4866069ae2 старый)
//        /// </summary>
//        public const string ConceptKey = "dc2411cf0af22e07";
//        /// <summary>
//        /// API для получения названий концептов, с дополнительными полями (3221b197bf5533b4 старый)
//        /// </summary>
//        public const string NamesKey = "3408898f75bac241";
//        /// <summary>
//        /// Очень не понятный ключ и хз как это будет работать, подставил 64d71f67087580b4 вместо 36e0687383ef02e3, в комментарии было написано что 5821b27bad65bea5 старый
//        /// Универсальный метод анализа таблицы - 64d71f67087580b4 (36e0687383ef02e3 старый)
//        /// Универсальный метод анализа таблицы(на входе файл) - 9201dfd925f13f86(36e0687383ef02e3 старый)
//        /// Универсальный метод анализа и загрузки таблицы - ced3e4a7fc67b982(36e0687383ef02e3 старый)
//        /// Универсальный метод анализа и загрузки таблицы(на входе файл) - f365e4d7a6910971(36e0687383ef02e3 старый)
//        /// </summary>
//        public const string AbdParseKey = "64d71f67087580b4";
//        /// <summary>
//        /// Загрузка формализованных данных (2c10185f18aac0a6 старый)
//        /// </summary>
//        public const string AbdParseKeyN = "4cbe7a73a6c491d1";
//        /// <summary>
//        /// API для создания гостя (a73096f04a1d22b6 старый)
//        /// </summary>
//        public const string CreateGuestUser = "97fc12081b258e5d";
//        /// <summary>
//        /// API для регистрации пользователя из гостя (cfaabb92bbf2f44e старый)
//        /// </summary>
//        public const string CreateUserFromGuestKey = "a7a76bc6731f5149";
//        /// <summary>
//        /// API Парсер универсальный для извлечения смысловых объектов из текста (93da6407073da759 старый)
//        /// Старое описание: Key for recognize
//        /// </summary>
//        public const string RecognizeKey = "2ff933dab6562647";




//        public const string MongoBase = "PHTForSLF";
//        public const string Assortiment = "assortiment";
//        public const string Pharms = "pharms";
//        public const string Lics = "lics";
//        public const string GS1_user = "4640027099999";
//        public const string GS1_pass = "socmedica123";




//        public const string UPCReqAddress = "https://api.upcitemdb.com/prod/trial/lookup";
//        public const string SQLConnectionString = "server=db.socmedica.com;port=33066;database=umkb;user id=umkb_o;password='HG;ls;kg8934=12f';allowuservariables=True;CharSet=utf8;Convert Zero Datetime=True;Allow Zero Datetime=True; Pooling = false; Connect Timeout=60"; //""server=db.socmedica.com;port=33066;database=umkb;user id=umkb_o;password='HG;ls;kg8934=12f';allowuservariables=True;CharSet=utf8;Convert Zero Datetime=True;Allow Zero Datetime=True; Pooling = false; Connect Timeout=60"; //"server=db.socmedica.dev;port=33066;Initial Catalog=umkb;User ID=dssc_user;Password=o5BqvY3Til14xdXY; Pooling = false; Connect Timeout=60"";
//        public const string SQLConnectionString73 = "server=db.socmedica.com;port=33066;database=pharm;user id=umkb_o;password='HG;ls;kg8934=12f';allowuservariables=True;CharSet=utf8;Convert Zero Datetime=True;Allow Zero Datetime=True; Pooling = false; Connect Timeout=60";
//        public static string FPPrice_Connect = "server=db.socmedica.com;port=33066; Initial Catalog = pharm; user id=umkb_o;password='HG;ls;kg8934=12f';allowuservariables=True;CharSet=utf8;Convert Zero Datetime=True;Allow Zero Datetime=True; Pooling = false; Connect Timeout=60"; //user id = dssc_user; password = 'PoHOvu9DYYZLMLWt'; Pooling = false;";
//        //public const string TableGetKey = "bcec3f6965690cf1";
//        //public const string GraphKey = "34ff6a9a4b004fa1";
//        //public const string ConceptKey = "03741c4866069ae2";
//        //public const string NamesKey = "3221b197bf5533b4";
//        //public const string AbdParseKey = "5821b27bad65bea5";
//        public const int ScriptId = 5714;
//        public static string MongoDb = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production") ? "mongodb://pht:pht2021@mongodb.socmedica.com:27027" : "mongodb://pht:pht2021@mongodb.socmedica.com:27027";
//    }
//}