using ECPLib.Common.Drugs.Models;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiAdm.Models
{
    /// <summary>
    /// Класс рецепта Святозара
    /// </summary>
    public class ModelRecipeBase
    {
        /// <summary>
        /// Стандартный алерт
        /// </summary>
        [BsonIgnore]
        public UMKBRequests.Alert Alert { get; set; }// = new UMKBRequests.Alert() { code = "200", level = "success", sticky = false };

        #region
        /// <summary>
        /// Id юзера, из под которого выписывается рецепт
        /// </summary>
        [BsonElement]
        public string IdUser { get; set; }
        /// <summary>
        /// Id юзера, из под которого выписывается рецепт
        /// </summary>
        [BsonElement]
        public string IdKey { get; set; }
        #endregion

        /// <summary>
        /// Номер рецепта
        /// </summary>
        [BsonId]
        public string id { get; set; }
        /// <summary>
        /// Набор ЛС из протоколов
        /// первый ключ - дата(имя коллекции),
        /// второй ключ - id протокола
        /// третий ключ - key раствора ("N", в случае если ДВ не в растворе)
        /// тело - список ДВ
        /// </summary>
        [BsonElement]
        public Dictionary<string, Dictionary<string, Dictionary<string, HashSet<string>>>> LS { get; set; }

        #region Пациент
        /// <summary>
        /// Id пациента
        /// </summary>
        [BsonElement]
        public string IdPatient { get; set; }
        /// <summary>
        /// Дата рождения пациента (на момент выписки) UNIXTIME - до 1970 года - отрицательная
        /// </summary>
        [BsonIgnoreIfNull]
        public long? PBirthday { get; set; }
        /// <summary>
        /// Пол пациента
        /// </summary>
        [BsonIgnoreIfNull]
        public string PSex { get; set; }
        /// <summary>
        /// Флаг - взрослый или детский рецепт 
        /// </summary>
        [BsonIgnoreIfNull]
        public bool? PAdult { get; set; }
        /// <summary>
        /// id заболевания пациента (на момент выписки)
        /// </summary>
        [BsonIgnoreIfNull]
        public string PDid { get; set; }
        /// <summary>
        /// Код(коды) МКБ10 заболевания пациента (на момент выписки)
        /// </summary>
        [BsonIgnoreIfNull]
        public string PDCode { get; set; }
        /// <summary>
        /// Флаг, что заболевание хроническое
        /// </summary>
        [BsonIgnoreIfNull]
        public bool? PСhronic { get; set; }
        /// <summary>
        /// фамилия и инициалы пациента (на момент выписки)
        /// </summary>
        [BsonIgnoreIfNull]
        public string PName { get; set; }
        /// <summary>
        /// Имя пациента
        /// </summary>
        [BsonIgnoreIfNull]
        public string PFirstName { get; set; }
        /// <summary>
        /// Фамилия пациента
        /// </summary>
        [BsonIgnoreIfNull]
        public string PLastName { get; set; }
        /// <summary>
        /// Отчество пациента
        /// </summary>
        [BsonIgnoreIfNull]
        public string PPatronymic { get; set; }
        /// <summary>
        /// Снилс пациента
        /// </summary>
        [BsonIgnoreIfNull]
        public string PSnils { get; set; }
        /// <summary>
        ///   Серия мед. полиса пациента
        /// </summary>
        [BsonIgnoreIfNull]
        public string PPolicySN { get; set; }
        /// <summary>
        ///   Номер мед. полиса пациента
        /// </summary>
        [BsonIgnoreIfNull]
        public string PPolicyNum { get; set; }
        /// <summary>
        ///   Идентификатор организации в которой зарегистрирована мед.карта
        /// </summary>
        [BsonIgnoreIfNull]
        public string PMedCardOrg { get; set; }
        /// <summary>
        ///   Номер мед.карты
        /// </summary>
        [BsonIgnoreIfNull]
        public string PMedCardNum { get; set; }
        /// <summary>
        /// Место проживания пациента
        /// </summary>
        [BsonIgnoreIfNull]
        public string PAddress { get; set; }
        /// <summary>
        ///   Источник финансирования (варианты: федеральный бюджет, бюджет субъекта Российской Федерации, муниципальный бюджет)
        /// </summary>
        [BsonIgnoreIfNull]
        public string PCatLevel { get; set; }
        /// <summary>
        ///  Льготная категория гражданина(пациента)
        /// </summary>
        [BsonIgnoreIfNull]
        public string PCatCode { get; set; }
        /// <summary>
        /// % оплаты - следствие скидки по категории
        /// </summary>
        [BsonIgnoreIfNull]
        public string PDiscount { get; set; }
        #endregion

        #region Врач
        /// <summary>
        /// Id врача выписавшего рецепт
        /// </summary>
        [BsonElement]
        public string IdDoc { get; set; }
        /// <summary>
        /// фамилия и инициалы врача (на момент выписки)
        /// </summary>
        [BsonIgnoreIfNull]
        public string DName { get; set; }
        /// <summary>
        /// Имя врача, выписавшего рецепт
        /// </summary>
        [BsonIgnoreIfNull]
        public string DFirstName { get; set; }
        /// <summary>
        ///  Фамилия врача, выписавшего рецепт
        /// </summary>
        [BsonIgnoreIfNull]
        public string DLastName { get; set; }
        /// <summary>
        /// Отчество врача, выписавшего рецепт
        /// </summary>
        [BsonIgnoreIfNull]
        public string DPatronymic { get; set; }
        #endregion

        #region Клиника или индивидульный предприниматель
        /// <summary>
        /// Id клиники, где выписали рецепт или индивидульного предпринимателя
        /// </summary>
        [BsonElement]
        public string IdClinic { get; set; }
        /// <summary>
        /// Код клиники, где выписали рецепт или индивидульного предпринимателя
        /// </summary>
        [BsonIgnoreIfNull]
        public string CCode { get; set; }
        /// <summary>
        /// код субъекта РФ по ОКАТО или индивидульного предпринимателя+код района, который присваивает налоговая
        /// </summary>
        [BsonIgnoreIfNull]
        public string COkato { get; set; }
        /// <summary>
        /// Id отделения, где выписали рецепт
        /// </summary>
        [BsonIgnoreIfNull]
        public string IdDep { get; set; }
        /// <summary>
        /// Имя отделения, где выписали рецепт
        /// </summary>
        [BsonIgnoreIfNull]
        public string CDName { get; set; }
        /// <summary>
        /// Имя клиники, где выписали рецепт
        /// </summary>
        [BsonIgnoreIfNull]
        public string CName { get; set; }
        /// <summary>
        /// Адрес клиники или индивидульного предпринимателя, где выписали рецепт
        /// </summary>
        [BsonIgnoreIfNull]
        public string CAdress { get; set; }
        /// <summary>
        /// Телефон клиники, где выписали рецепт
        /// </summary>
        [BsonIgnoreIfNull]
        public string CPhoneNumber { get; set; }
        /// <summary>
        /// ОРГН клиники, где выписали рецепт или ОГРНИП индивидуального предпринимателя
        /// </summary>
        [BsonIgnoreIfNull]
        public string COgrn { get; set; }
        #endregion

        #region Аптека

        public string PHGovName { get; set; }
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
        /// <summary>
        /// Дата отоваривания рецепта
        /// </summary>
        [BsonIgnoreIfNull]
        public long? PHDate { get; set; }
        /// <summary>
        /// Список выданных препаратов
        /// </summary>
        [BsonIgnoreIfNull]
        public List<Drug> PHFP { get; set; }
        #endregion

        #region Сам рецепт
        /// <summary>
        /// Id рецептурной формы
        /// </summary>
        [BsonElement]
        public string IdForm { get; set; }
        /// <summary>
        /// Id статуса рецепта (выписан, отпущен, просрочен без отпуска)
        /// </summary>
        [BsonElement]
        public string IdStatus { get; set; }
        /// <summary>
        /// отметка “cito” - «срочно»
        /// </summary>
        [BsonIgnoreIfNull]
        public bool? Cito { get; set; }
        /// <summary>
        /// отметка “statim” - «немедленно» 
        /// </summary>
        [BsonIgnoreIfNull]
        public bool? Statim { get; set; }
        /// <summary>
        /// отметка отметка «По специальному назначению» (булево значение, только для бланка 107у, по умолчанию ставится true если срок действия рецепта больше 60 дней. Если срок действия рецепта меньше или равно 60 дней, то false ) 
        /// </summary>
        [BsonIgnoreIfNull]
        public bool? Special { get; set; }
        /// <summary>
        /// периодичность отпуска лекарственных препаратов (только если отметка «По специальному назначению» true) (поле типа input, куда врач должен написать «еженедельно» или «ежемесячно», или иное)
        /// </summary>
        [BsonIgnoreIfNull]
        public string SpecText { get; set; }
        /// <summary>
        /// Дата создания рецепта (UnixTime в мс)
        /// </summary>
        [BsonElement]
        public Int64? DateStart { get; set; }// = SMCommonUtils.Converter.GetNowInt64();
        /// <summary>
        /// Расчётная дата прекращения действия рецепта (UnixTime в мс)
        /// </summary>
        [BsonElement]
        public Int64? DateEnd { get; set; } //= SMCommonUtils.Converter.DateTimeToInt64(DateTime.Now.AddYears(1));
        /// <summary>
        /// Срок действия рецепта (табличные данные концепта)
        /// </summary>
        [BsonElement]
        public TableValueItem DateEndItem { get; set; }
        /// <summary>
        /// Фактическая дата прекращения действия рецепта (UnixTime в мс)
        /// Ставится, если отменили действие рецепта
        /// </summary>
        [BsonElement]
        public Int64? RepealDate { get; set; } //= 0;
        /// <summary>
        /// id врача, который отменил назначение
        /// </summary>
        [BsonElement]
        public string RepealDoc { get; set; }
        /// <summary>
        /// Комментарий к рецепту(ключ "C"), и возможность "варчаров" на всякий случай.
        /// </summary>
        [BsonElement]
        public Dictionary<string, List<string>> DText { get; set; }

        #endregion
    }
}
