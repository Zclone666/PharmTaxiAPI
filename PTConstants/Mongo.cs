using System;
using System.Collections.Generic;
using System.Text;

namespace PTConstants
{
    /// <summary>
    /// Все что относится к MongoDB
    /// </summary>
    public class Mongo
    {
        /// <summary>
        /// Используется для задания всем строки подключения к MongoDB
        /// </summary>
        public static string MongoDB= "mongodb://pht:pht2021@mongodb.socmedica.com:27027,mongodb04arm.socmedica.com:27027/?authSource=admin&readPreference=secondaryPreferred&directConnection=false&ssl=false";

        #region Рецепты
        /// <summary>
        /// Строка подключения к MongoDB, типа для рецептов
        /// </summary>
        public static string ReciepesConString = MongoDB;
        /// <summary>
        /// Имя БД рецептов 
        /// </summary>
        public const string ReciepesDBName = "ECP_Recipes";
        /// <summary>
        /// Коллекция пациентов ECP
        /// </summary>
        public const string ReciepesPatientsCollectionName = "Patients";
        #endregion

        #region SKOL
        /// <summary>
        /// Строка подключения к MongoDB, типа для СКОЛа 
        /// </summary>
        public static string SKOLConString = MongoDB;
        /// <summary>
        /// Имя БД СКОЛа
        /// </summary>
        public const string SKOLDBName = "SKOL";
        #endregion

        #region PharmTaxi
        /// <summary>
        /// Имя БД чего то, там одна коллекция pharms, не факт что это актуальное
        /// </summary>
        public const string PHT = "PharmTaxi";
        /// <summary>
        /// Имя БД чего то, там десяток коллекций, скорее всего актуальное
        /// </summary>
        public const string PHTFORSTFR = "PHTForSLF";
        /// <summary>
        /// Имя коллекции, вроде там лежат актуальные аптеки
        /// </summary>
        public const string Pharms = "pharms";
        /// <summary>
        /// Имя коллекции, вроде там лежат укороченные актуальные аптеки
        /// </summary>
        public const string PharmsFD = "pharmsFD";
        /// <summary>
        /// Имя коллекции, вроде там сети аптек лежат
        /// </summary>
        public const string PharmNets = "pharmNets";
        /// <summary>
        /// Имя коллекции, вроде ассортимента аптеки
        /// </summary>
        public const string Assortiment = "assortiment";
        /// <summary>
        /// Имя коллекции, вроде для укороченного ассортимента аптеки
        /// </summary>
        public const string AssortimentSmall = "assortiment_small";
        /// <summary>
        /// Имя коллекции, вроде лицензий для сети аптек
        /// </summary>
        public const string Lics = "lics";
        /// <summary>
        /// Имя коллекции, вроде там пользователи ФТ
        /// </summary>
        public const string UsersCollection = "users";
        /// <summary>
        /// Имя коллекции, вроде заказы
        /// </summary>
        public const string Orders_DataBase = "orders";
        #endregion
    }
}
