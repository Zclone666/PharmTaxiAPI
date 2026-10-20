using MongoDB.Bson.Serialization.Attributes;
using SMReceipt.Global.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Methods.Recipe.Model
{
    public class RecipeLog
    {
        /// <summary>
        /// ид записи из истории
        /// </summary>
        [BsonId]
        public string Id { get; set; }
        /// <summary>
        /// Номер рецепта
        /// </summary>
        [BsonElement]
        public string RecipeId { get; set; }
        /// <summary>
        /// Дата изменения рецепта (UnixTime в мс)
        /// </summary>
        [BsonElement]
        public Int64 Date { get; set; } = SMCommonUtils.Converter.GetNowInt64();
        /// <summary>
        /// Id фармаколога (пользователя)
        /// </summary>
        [BsonElement]
        public ulong UserId { get; set; }
        /// <summary>
        /// Статус рецепта после изменения
        /// </summary>
        [BsonIgnoreIfNull]
        public int IdStatus { get; set; }
        /// <summary>
        /// Список препоратов у которых был изменен статус
        /// </summary>
        [BsonIgnoreIfNull]
        public List<StatusLS> Drugs { get; set; }
        /// <summary>
        /// Список растворов у которых был изменен статус
        /// </summary>
        [BsonIgnoreIfNull]
        public List<StatusLS> Liquids { get; set; }

        #region Клиника или индивидульный предприниматель
        /// <summary>
        /// Id клиники, где изменили рецепт или индивидульного предпринимателя
        /// </summary>
        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public ulong IdClinic { get; set; }
        /// <summary>
        /// Имя клиники, где изменили рецепт
        /// </summary>
        [BsonIgnoreIfNull]
        public string CName { get; set; }
        /// <summary>
        /// Код клиники, где изменили рецепт или индивидульного предпринимателя
        /// </summary>
        [BsonIgnoreIfNull]
        public string CCode { get; set; }
        /// <summary>
        /// код субъекта РФ по ОКАТО или индивидульного предпринимателя+код района, который присваивает налоговая
        /// </summary>
        [BsonIgnoreIfNull]
        public string COkato { get; set; }
        /// <summary>
        /// Id отделения, где изменили рецепт
        /// </summary>
        [BsonIgnoreIfNull]
        public string IdDep { get; set; }
        /// <summary>
        /// Имя отделения, где изменили рецепт
        /// </summary>
        [BsonIgnoreIfNull]
        public string CDName { get; set; }
        /// <summary>
        /// Адрес клиники или индивидульного предпринимателя, где изменили рецепт
        /// </summary>
        [BsonIgnoreIfNull]
        public string CAdress { get; set; }
        /// <summary>
        /// Телефон клиники, где изменили рецепт
        /// </summary>
        [BsonIgnoreIfNull]
        public string CPhoneNumber { get; set; }
        /// <summary>
        /// ОРГН клиники, где изменили рецепт или ОГРНИП индивидуального предпринимателя
        /// </summary>
        [BsonIgnoreIfNull]
        public string COgrn { get; set; }
        #endregion

        #region Аптека
        /// <summary>
        /// Id аптеки
        /// </summary>
        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public ulong PHId { get; set; }
        /// <summary>
        /// Название аптеки
        /// </summary>
        [BsonIgnoreIfNull]
        public string PHName { get; set; }
        /// <summary>
        /// фамилия и инициалы провизора
        /// </summary>
        [BsonIgnoreIfNull]
        public string PHDispName { get; set; }
        /// <summary>
        /// Имя провизора
        /// </summary>
        [BsonIgnoreIfNull]
        public string PHDispFirstName { get; set; }
        /// <summary>
        /// Фамилия провизора
        /// </summary>
        [BsonIgnoreIfNull]
        public string PHDispLastName { get; set; }
        /// <summary>
        /// Отчество провизора
        /// </summary>
        [BsonIgnoreIfNull]
        public string PHDispPatr { get; set; }
        #endregion
    }

    public class StatusLS
    {
        /// <summary>
        /// Id препората в рецепте
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// Статус рецепта после изменения
        /// </summary>
        public int IdStatus { get; set; }
        /// <summary>
        /// ФП которые выписали по этому ЛС
        /// </summary>
        public List<Meds> medicaments { get; set; } = new List<Meds>();
    }
}
