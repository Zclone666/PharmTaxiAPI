using PharmTaxiApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace PharmTaxiAPI.Models
{
    /// <summary>
    /// Параметры гостевого пользователя
    /// </summary>
    public class GuestUser
    {
        public string key { get; set; }
        public string application { get; set; } = "5";
    }

    /// <summary>
    /// Ответ по созданию гостя
    /// </summary>
    public class CreateGuestResponse
    {
        /// <summary>
        /// Выданный гостю ауткей
        /// </summary>
        public string authkey;

        /// <summary>
        /// Коллекция параметров оповещения
        /// </summary>
        public UMKBRequests.Alert alert;
    }

    /// <summary>
    /// Параметры для пользователя, который создается из гостя
    /// </summary>
    public class UserFromGuest
    {
        /// <summary>
        /// Лицензионный ключ клиента
        /// </summary>
        [JsonIgnore]
        public string key { get; set; }

        /// <summary>
        /// Аутки гостя
        /// </summary>
        [Required]
        public string authkey { get; set; }

        /// <summary>
        /// Email гостя
        /// </summary>
        [Required]
        public string email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [Required]
        public string password { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [Required]
        public string firstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [Required]
        public string lastName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string middleName { get; set; }

        /// <summary>
        /// Индекс сервиса/приложения
        /// </summary>
        [JsonIgnore]
        public string application { get; set; }
    }

    /// <summary>
    /// Ответ по созданию пользователя из гостя
    /// </summary>
    public class CreateUserFromGuestResponse
    {
        public string authkey { get; set; }
        public uint user_id { get; set; }
        public  PharmTaxiAdm.Models.User.Alert alert { get; set; }
    }
}
