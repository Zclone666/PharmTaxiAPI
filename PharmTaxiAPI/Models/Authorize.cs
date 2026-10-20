using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Models
{
    public class Authorize
    {
        public string email { get; set; }
        public string password { get; set; }
        public string service { get; set; }
    }

    /// <summary>
    /// Параметры для создания приглашения и отправки на почту
    /// </summary>
    public class InviteViaEmail
    {
        /// <summary>
        /// Лицензионный ключ клиента
        /// </summary>
        [Required]
        public List<string> lic_key { get; set; }

        /// <summary>
        /// Authkey пользователя, который создает приглашение
        /// </summary>
        public string authkey { get; set; }
        /// <summary>
        /// Authkey пользователя, который создает приглашение
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// Электронная почта пользователя, которого нужно пригласить
        /// </summary>
        [Required]
        public string invitedEmail { get; set; }
        /// <summary>
        /// Коды добавляемых ролей (используется код белона)
        /// </summary>
        [Required]
        public List<string> invitedRoleCodes { get; set; }
        /// <summary>
        /// Индекс сервиса/приложения
        /// </summary>
        [JsonIgnore]
        public string application { get; set; } = "6";
    }

    /// <summary>
    /// Параметры для подтверждения приглашения
    /// </summary>
    public class ConfirmInvite
    {
        /// <summary>
        /// Лицензионный ключ клиента*
        /// </summary>
        [Required]
        public string key { get; set; }
        /// <summary>
        /// Аuthkey пользователя, который принимает приглашение*
        /// </summary>
        [Required]
        public string authkey { get; set; }
        /// <summary>
        /// Код подтверждения приглашения*
        /// </summary>
        [Required]
        public string code { get; set; }
        /// <summary>
        /// Индекс сервиса/приложения под которым создается аккаунт*
        /// </summary>
        [JsonIgnore]
        public string application { get; set; }
    }
}
