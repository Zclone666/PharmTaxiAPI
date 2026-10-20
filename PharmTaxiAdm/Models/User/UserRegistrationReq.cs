using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiAdm.Models.User
{
    public class BaseUserRegReq : UserRegistrationReq
    {
       
    }
    public class UserRegistrationReq : UserRegUMKB, IUserRegistrationReq
    {
        /// <summary>
        /// ключ
        /// </summary>
        public string authkey { get; set; }

       // public new string roleCode { get { return String.IsNullOrEmpty(lic_key)?roleCode:lic_key; } set { roleCode = value; } }

        /// <summary>
        /// заготовк лицензионного ключа
        /// </summary>
        public string lic_key { get; set; }

    }

    public class UserRegUMKB : IUserRegUMKB
    {

        /// <summary>
        /// ключ
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// id сервиса
        /// </summary>
        public string service { get; set; } = "7";
        public string email { get; set; }
        public string password { get; set; }
        /// <summary>
        /// Фамилия
        /// </summary>
        public string lastName { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        public string firstName { get; set; }
        /// <summary>
        /// Отчество
        /// </summary>
        public string middleName { get; set; }
        /// <summary>
        /// Имя ЛПУ
        /// </summary>
        public string lpuName { get; set; }
        /// <summary>
        /// отделение ЛПУ
        /// </summary>
        public string department { get; set; }
        /// <summary>
        /// id Главного пользователя
        /// </summary>
        public string parent_id { get; set; }
        /// <summary>
        /// Заготовка лицензионного кода аптеки
        /// </summary>
        public string roleCode { get; set; }
    }
}
