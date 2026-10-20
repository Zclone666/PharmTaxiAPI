

namespace Constants
{
    class Keys
    {
        /// <summary>
        /// API для поиска концептов в системе Сфинкс, нигде в решении не используется, возможно уже не нужен/не работает сервис 
        /// (c6cbdeb36a9d402d, 36600cde621cc253(ключ из ЭКФ) старый)
        /// </summary>
        public const string Sphinx = "97e48fd156cebb41";
        /// <summary>
        /// Очень не понятный ключ и хз как это будет работать, подставил 64d71f67087580b4 вместо 36e0687383ef02e3, в комментарии было написано что 5821b27bad65bea5 старый
        /// Универсальный метод анализа таблицы - 64d71f67087580b4 (36e0687383ef02e3 старый)
        /// Универсальный метод анализа таблицы(на входе файл) - 9201dfd925f13f86(36e0687383ef02e3 старый)
        /// Универсальный метод анализа и загрузки таблицы - ced3e4a7fc67b982(36e0687383ef02e3 старый)
        /// Универсальный метод анализа и загрузки таблицы(на входе файл) - f365e4d7a6910971(36e0687383ef02e3 старый)
        /// </summary>
        public const string AbdParse = "64d71f67087580b4";
        /// <summary>
        /// Загрузка формализованных данных (2c10185f18aac0a6 старый)
        /// </summary>
        public const string AbdWr = "4cbe7a73a6c491d1";
        /// <summary>
        /// Ключ для ЭКФ Web (технический API) (f37d7a0f2bdc4464 старый)
        /// </summary>
        public const string ECP = "379776b3570a3c22";
        /// <summary>
        /// API для получения смысловых графов из списка текстовых строк или концептов (c353686107266b69 старый)
        /// </summary>
        public const string GraphList = "90f0a8a1065691cf";
        /// <summary>
        /// API для поиска концептов в текстовой строке (03741c4866069ae2 старый)
        /// </summary>
        public const string Concept = "dc2411cf0af22e07";
        /// <summary>
        /// API для получения параметров концепта в формате value (табличные-параметрические данные) (bcec3f6965690cf1 старый)
        /// </summary>
        public const string TableGet = "cbb2a69af9ff974d";
        /// <summary>
        /// API для получения смысловых графов (4c849ec05fa683de старый)
        /// </summary>
        public const string Graph = "9902650c1d43506f";
        /// <summary>
        /// API Регистрация и авторизация пользователя (7db829babab985f7 старый)
        /// </summary>
        public const string Authorization = "1591f0189d8062fb";
        /// <summary>
        /// API для получения записи названий концепта из списка, обращения к БД (bdf3207f4dbf5a0f старый)
        /// </summary>
        public const string GetNamesDB = "3f1e3b5ba314cb6c";
        /// <summary>
        /// API для получения названий концептов, с дополнительными полями (3221b197bf5533b4 старый)
        /// </summary>
        public const string GetNamesAndFields = "3408898f75bac241";
        /// <summary>
        /// API для моделирования (добавления, изменения и удаления) данных в UMKB (05de468c4361f192 старый)
        /// </summary>
        public const string Modelling = "665f96918b1046c9";
        /// <summary>
        /// API для получения параметров концепта в формате varchar по фильтру (57c1edaa5c3be9a4 старый)
        /// </summary>
        public const string GetVarcharWhere = "871ab5ec27dbcd45";
        /// <summary>
        /// API для генерации пользователя по authkey (другим пользователем) (a6786df3cbc491a8 старый)
        /// </summary>
        public const string GenerateUserByAuthkey = "33efb9bab487e6af";
        /// <summary>
        /// API Запрос ссылки на сброс пароля (2df2fad66d5b359c старый)
        /// </summary>
        public const string PassReset = "70f1a529fafb5e60";
        /// <summary>
        /// API для поиска концептов в списке из текстовых строк (7b52c441e15d79cf старый)
        /// </summary>
        public const string ConceptList = "7953a80f46d792cc";
        /// <summary>
        /// API для приглашения пользователя (37551e9be0c9611e старый)
        /// </summary>
        public const string Invite = "5f8d6114c42de253";
        /// <summary>
        /// API для подтверждения приглашений существующим пользователем через его Authkey (0a09c738f73b6893 старый)
        /// </summary>
        public const string InviteAccept = "7222e668416891d8";
        /// <summary>
        /// API для регистрации пользователя пользователем (81d2c3d9af3d34ab старый)
        /// </summary>
        public const string RegisterByUser = "18a2fb2ff87dab00";
        /// <summary>
        /// API для получения информации о пользователе по его почте (93d04d69317f0dd4, 340f725c9ccb9779(из какого то другого проекта) старый)
        /// </summary>
        public const string GetUserInfoEmail = "1a7154198f39fc39";
        /// <summary>
        /// API для добавления сервиса пользователю по его Authkey (3d241e689f97a617 старый)
        /// </summary>
        public const string AddServiceByAuth = "0d33b972f7c285bc";
        /// <summary>
        /// API Парсер универсальный для извлечения смысловых объектов из текста -  (93da6407073da759 старый)
        /// Старое описание: Key for recognize
        /// </summary>
        public const string Recognize = "2ff933dab6562647";
        /// <summary>
        /// API для получения названия концептов, нигде в решении не используется (ca913542004c48c9 старый)
        /// </summary>
        public const string ConceptName = "1f20901fd46693a5";
        /// <summary>
        /// API Парсер для извлечения концептов из текста, нигде в решении не используется (99a0dd0efb8cab42 старый)
        /// </summary>
        public const string ConceptByText = "e1a45da8736101b1";
        /// <summary>
        /// API для получения id концептов по кодам из списка, нигде в решении не используется (304fabb9cd983d3b старый)
        /// </summary>
        public const string IdByCodeList = "b930bdfec8d313e2";
        /// <summary>
        /// API для создания гостя (a73096f04a1d22b6 старый)
        /// </summary>
        public const string CreateGuestUser = "97fc12081b258e5d";
        /// <summary>
        /// API для регистрации пользователя из гостя (cfaabb92bbf2f44e старый)
        /// </summary>
        public const string CreateUserFromGuest = "a7a76bc6731f5149";
        /// <summary>
        /// API для получения названий концептов из БД (5dbd34b00aa8068c(ключ из ЭКФ) старый)
        /// </summary>
        public const string GetNamesListDB = "e563519680941264";
    }
}
