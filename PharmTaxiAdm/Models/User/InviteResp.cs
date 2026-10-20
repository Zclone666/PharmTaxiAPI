using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace PharmTaxiAdm.Models.User
{
    public class InviteResp:UMKBRequests.Alert
    {
       // public List<Alert> alerts { get; set; } = new List<Alert>();
    }

    public class InviteViaEmail
    {
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
}
