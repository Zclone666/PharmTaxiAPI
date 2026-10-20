using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiAdm.Models.User
{
    public class RegUserByUserReq
    {
        public string authkey { get { return parentAuthkey; } set { parentAuthkey = value; } }
        /// <summary>
        /// лицензионный ключ клиента
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// индекс сервиса/приложения под которым создается аккаунт  - для ФТ должно быть равно 6
        /// </summary>
        public string service { get; set; } = "6";
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string middleName { get; set; }

       /// <summary>
       /// ключ пользователя, который регистрирует. Используется, если не указан parentAuthkey
       /// </summary>
        public string parentKey { get; set; }
        public string parentAuthkey { get; set; }
        /// <summary>
        /// индекс сервиса/приложения родителя, который создает аккаунт
        /// </summary>
        public string parentService { get; set; } = "7";
        public string roleCode { get; set; }

    }

    public class BaseRegUserByUserReq
    {
        public string key { get; set; }
        /// <summary>
        /// индекс сервиса/приложения под которым создается аккаунт - для ФТ должно быть равно 6
        /// </summary>
        public string service { get; set; } = "6";
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string middleName { get; set; }

        /// <summary>
        /// ключ пользователя, который регистрирует. Используется, если не указан parentAuthkey
        /// </summary>
        public string parentKey { get; set; }
        public string parentAuthkey { get; set; }
        public string parentService { get; set; }
        public string roleCode { get; set; }

    }
}
