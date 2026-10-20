using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiAdm.Models.User
{
    public class ConfirmInvite
    {
        /// <summary>
        /// Лицензионный ключ клиента*
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// Аuthkey пользователя, который принимает приглашение*
        /// </summary>
        public string authkey { get; set; }
        /// <summary>
        /// Код подтверждения приглашения*
        /// </summary>      
        public string code { get; set; }
        /// <summary>
        /// Индекс сервиса/приложения под которым создается аккаунт*
        /// </summary>
        public string application { get; set; }
    }
}
