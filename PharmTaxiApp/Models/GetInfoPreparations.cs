using ECPLib.Common.Drugs.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiApp.Models
{

     public class GetInfoPreparations
    {
        public UMKBRequests.Alert alert { get; set; } = new UMKBRequests.Alert() { code = "200", level = "success", sticky = false };
#pragma warning disable IDE1006 // Стили именования
        /// <summary>
        /// Постоянный ключ авторизации
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// Временный токен авторизации (https://cs.socmedica.com/api/user/authorization)
        /// </summary>
        public string authkey { get; set; }
        /// <summary>
        /// Идентификатор устройства
        /// </summary>
        public string device_id { get; set; }
        /// <summary>
        /// Номер сервиса (4 - экф, 108 технический экф2, 2 - Экф премиум)
        /// </summary>
        public string service { get; set; } = "108";
        /// <summary>
        /// id МИСа
        /// </summary>
        public string misid { get; set; }
        /// <summary>
        /// МИС-овский id протокола
        /// Техническое поле, используется при интеграции с МИС-ами
        /// </summary>
        public string misprotocol { get; set; }

        #region Доктор
        /// <summary>
        /// id врача
        /// </summary>
        public string doctor_id { get; set; }
        /// <summary>
        ///  МИС-совский код
        ///  Техническое поле, используется при интеграции с МИС-ами
        /// </summary>
        public string doctor_code { get; set; }
        /// <summary>
        /// Имя врача
        /// </summary>
        public string doctor_name { get; set; }
        #endregion

        #region Клиника
        /// <summary>
        /// id клиники
        /// </summary>
        public string clinic_id { get; set; }
        /// <summary>
        /// МИС-совский код клиники
        /// Техническое поле, используется при интеграции с МИС-ами
        /// </summary>
        public string clinic_code { get; set; }
        /// <summary>
        /// Наименование клиники
        /// </summary>
        public string clinic_name { get; set; }
        #endregion

        #region Отделение
        /// <summary>
        /// id отделения
        /// </summary>
        public string dep_id { get; set; }
        /// <summary>
        /// МИС-совский код отделения 
        /// Техническое поле, используется при интеграции с МИС-ами
        /// </summary>
        public string dep_code { get; set; }
        /// <summary>
        /// Наименование отделения
        /// </summary>
        public string dep_name { get; set; }
        #endregion
        /// <summary>
        /// Главный класс слепка состояния пациента
        /// </summary>
        public Patient patient { get; set; }
        /// <summary>
        /// Локаль пользователя
        /// </summary>
        public string active_language { get; set; } = "ru";
        /// <summary>
        /// Список активных ЛС
        /// </summary>
        public List<Drug> drugs { get; set; } = new List<Drug>();
        /// <summary>
        /// Список активных растворов
        /// </summary>
        public List<Drug> liquids { get; set; } = new List<Drug>();
        /// <summary>
        /// Список деактивированных ЛС
        /// </summary>
        public List<Drug> deletedDrugs { get; set; } = new List<Drug>();
        /// <summary>
        /// Список деактивированных растворов
        /// </summary>
        public List<Drug> deletedLiquids { get; set; } = new List<Drug>();
        /// <summary>
        /// Флаг того, что необходимо вернуть формализованные поля активных ЛС/растворов и т.д.
        /// </summary>
        public bool return_subjects { get; set; }
        /// <summary>
        /// Ссылка на сервис на стороне МИС-ов, куда необходимо переслать результаты 
        /// </summary>
        public string misurl { get; set; }
        /// <summary>
        /// Номер протокола.
        /// Техническое поле, необходимо при интеграции с МИС-ами
        /// </summary>
        public string protocol { get; set; } = null;
        #region Рецептурный раздел
        /// <summary>
        /// Флаг от фронта, что необходимо сохранить простокол и создать из него рецепт. Устарело, уже не используется.
        /// </summary>
        [Obsolete]
        public bool save_recipe { get; set; } = false;
        /// <summary>
        /// Номер сгенерированного рецепта сформированного из этой структуры. Устарело, уже не используется.
        /// </summary>
        [Obsolete]
        public string recipe { get; set; } = null;
        /// <summary>
        /// Время окончания действия рецепта (используется при выписке рецепта) в UnixTime(мc)
        /// </summary>
        public long? recipe_endtime { get; set; } = SMCommonUtils.Converter.DateTimeToInt64(DateTime.Now.AddYears(1));
        /// <summary>
        /// ID концепта продолжительности действия рецепта
        /// </summary>
        public TableValueItem recipe_endtimeitem { get; set; }
        /// <summary>
        /// ID формы рецепта
        /// </summary>
        public string recipe_formid { get; set; }
        /// <summary>
        /// Комментарий к рецепту (используется при выписке рецепта)
        /// </summary>
        public string recipe_text { get; set; }
        /// <summary>
        /// Данные для выписки рецепта
        /// </summary>
        public RecipeData recipe_data { get; set; }
        #endregion
#pragma warning restore IDE1006 // Стили именования
    }

    public class RecipeData
    {
        /// <summary>
        /// ID формы рецепта
        /// </summary>
        public string formid { get; set; }
        /// <summary>
        /// Дата создания рецепта (UnixTime в мс)
        /// </summary>
        public Int64 DateStart { get; set; } = SMCommonUtils.Converter.GetNowInt64();
        /// <summary>
        /// Комментарий к рецепту (используется при выписке рецепта)
        /// </summary>
        public string recipe_text { get; set; }
        /// <summary>
        /// Время окончания действия рецепта (используется при выписке рецепта) в UnixTime(мc)
        /// </summary>
        public long? recipe_endtime { get; set; } = SMCommonUtils.Converter.DateTimeToInt64(DateTime.Now.AddDays(30));
        /// <summary>
        /// ID концепта продолжительности действия рецепта
        /// </summary>
        public TableValueItem recipe_endtimeitem { get; set; }
        /// <summary>
        /// id заболевания пациента (на момент выписки)
        /// </summary>
        public string pdid { get; set; }
        /// <summary>
        /// Код(коды) МКБ10 заболевания пациента (на момент выписки)
        /// </summary>
        public string pdcode { get; set; }
        /// <summary>
        /// Флаг, что заболевание хроническое
        /// </summary>
        public bool? pchronic { get; set; }
        /// <summary>
        ///   Источник финансирования (варианты: федеральный бюджет, бюджет субъекта Российской Федерации, муниципальный бюджет)
        /// </summary>
        public string pcatlevel { get; set; }
        /// <summary>
        ///  Льготная категория гражданина(пациента)
        /// </summary>
        public string pcatcode { get; set; }
        /// <summary>
        /// % оплаты - следствие скидки по категории
        /// </summary>
        public string pdiscount { get; set; }
        /// <summary>
        /// Id статуса рецепта (выписан, отпущен, просрочен без отпуска)
        /// </summary>
        public string idStatus { get; set; }
        /// <summary>
        /// отметка “cito” - «срочно»
        /// </summary>
        public bool? cito { get; set; }
        /// <summary>
        /// отметка “statim” - «немедленно» 
        /// </summary>
        public bool? statim { get; set; }
        /// <summary>
        /// отметка отметка «По специальному назначению» (булево значение, только для бланка 107у, по умолчанию ставится true если срок действия рецепта больше 60 дней. Если срок действия рецепта меньше или равно 60 дней, то false ) 
        /// </summary>
        public bool? special { get; set; }
        /// <summary>
        /// периодичность отпуска лекарственных препаратов (только если отметка «По специальному назначению» true) (поле типа input, куда врач должен написать «еженедельно» или «ежемесячно», или иное)
        /// </summary>
        public string spectext { get; set; }
        /// <summary>
        /// Электронная подпись врача
        /// </summary>
        public string docsign { get; set; }
        /// <summary>
        /// Электронный сертификат врача
        /// </summary>
        public string doccertify { get; set; }
        /// <summary>
        /// Электронная подпись врач. комиссии
        /// </summary>
        public string commsign { get; set; }
        /// <summary>
        /// Электронный сертификат врачебной комисии
        /// </summary>
        public string commcertify { get; set; }
    }

}
