//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace PharmTaxiApp.Constants
//{
//    public static class Keys
//    {
//        /// <summary>
//        /// API для поиска концептов в системе Сфинкс, нигде в решении не используется, возможно уже не нужен/не работает сервис 
//        /// (36600cde621cc253(ключ из ЭКФ) старый)
//        /// </summary>
//        public const string Sphinx = "97e48fd156cebb41";
//        /// <summary>
//        /// Очень не понятный ключ и хз как это будет работать, подставил 64d71f67087580b4 вместо 36e0687383ef02e3, в комментарии было написано что 5821b27bad65bea5 старый
//        /// Универсальный метод анализа таблицы - 64d71f67087580b4 (36e0687383ef02e3 старый)
//        /// Универсальный метод анализа таблицы(на входе файл) - 9201dfd925f13f86(36e0687383ef02e3 старый)
//        /// Универсальный метод анализа и загрузки таблицы - ced3e4a7fc67b982(36e0687383ef02e3 старый)
//        /// Универсальный метод анализа и загрузки таблицы(на входе файл) - f365e4d7a6910971(36e0687383ef02e3 старый)
//        /// </summary>
//        public const string AbdParseKey = "36e0687383ef02e3";
//        /// <summary>
//        /// Загрузка формализованных данных (2c10185f18aac0a6 старый)
//        /// </summary>
//        public const string AbdWrKey = "4cbe7a73a6c491d1";
//        /// <summary>
//        /// API для поиска концептов в текстовой строке (03741c4866069ae2 старый)
//        /// </summary>
//        public const string ConceptKey = "dc2411cf0af22e07";
//        /// <summary>
//        /// API для получения параметров концепта в формате value (табличные-параметрические данные) (bcec3f6965690cf1 старый)
//        /// </summary>
//        public const string TableGetKey = "cbb2a69af9ff974d";
//        /// <summary>
//        /// API для получения смысловых графов (4c849ec05fa683de старый)
//        /// </summary>
//        public static readonly string GraphKey = "9902650c1d43506f";
//        /// <summary>
//        /// API Регистрация и авторизация пользователя (7db829babab985f7 старый)
//        /// </summary>
//        public static readonly string AuthorizationKey = "1591f0189d8062fb";
//        /// <summary>
//        /// API для получения названий концептов, с дополнительными полями (3221b197bf5533b4 старый)
//        /// </summary>
//        public static readonly string GetNamesDBKey = "3408898f75bac241";
//        /// <summary>
//        /// API для получения названий концептов из БД (5dbd34b00aa8068c старый)
//        /// </summary>
//        public static readonly string GetNamesListDB = "e563519680941264";
//        /// <summary>
//        /// API для получения смысловых графов из списка текстовых строк или концептов (c353686107266b69 старый)
//        /// </summary>
//        public const string GraphListKey = "90f0a8a1065691cf";
//        /// <summary>
//        /// API для моделирования (добавления, изменения и удаления) данных в UMKB (05de468c4361f192 старый)
//        /// </summary>
//        public static readonly string ModellingKey = "665f96918b1046c9";
//        /// <summary>
//        /// API для получения параметров концепта в формате varchar по фильтру (57c1edaa5c3be9a4 старый)
//        /// </summary>
//        public static readonly string GetVarcharWhereKey = "871ab5ec27dbcd45";
//        /// <summary>
//        /// API для генерации пользователя по authkey (другим пользователем) (a6786df3cbc491a8 старый)
//        /// </summary>
//        public static readonly string GenerateUserByAuthkeyKey = "33efb9bab487e6af";
//        /// <summary>
//        /// API для поиска концептов в списке из текстовых строк (7b52c441e15d79cf старый)
//        /// </summary>
//        public const string ConceptListKey = "7953a80f46d792cc";
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
//        internal class Crypto { internal const string Password = "DESKTOP-0JFBQ9R"; internal const string EcpService = "108"; internal const string OldEcpService = "4"; }
//    }
//}
