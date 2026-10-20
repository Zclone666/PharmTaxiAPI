using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiApp.Models
{
    /// <summary>
    /// Аргументы для получения рецепта
    /// </summary>
    public class RecipeArgs
    {
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
        /// Номер сервиса (4 - экф, 108 технический экф2)
        /// </summary>
        public string service { get; set; } = "4";
        /// <summary>
        /// Код(номер) рецепта
        /// </summary>
        public string recipe { get; set; }
        /// <summary>
        /// E-mail, коротый используется при отправке рецепта
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// id пациента, по которому необходимо получить список рецептов
        /// </summary>
        public string patient_id { get; set; }
        /// <summary>
        /// СНИЛС пациента
        /// </summary>
        public string SNILS { get; set; }
        /// <summary>
        /// Локаль пользователя
        /// </summary>
        public string active_language { get; set; } = "ru";
    }

    public class PatFIOForRecps
    {
        public string FIO { get; set; }
        /// <summary>
        /// Дата рождения пациента
        /// </summary>
        public string birthdate { get; set; }
        /// <summary>
        /// СНИЛС пациента
        /// </summary>
        public string SNILS { get; set; }
        /// <summary>
        /// Регион работы аптеки
        /// </summary>
        public string Region { get; set; } = "Moscow";
        /// <summary>
        /// Дата рецепта
        /// </summary>
        public string DateOfReceipt { get; set; }
    }
}
