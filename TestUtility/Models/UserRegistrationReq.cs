using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{

    public class GenerateUserAttachDrugstore
    {
        /// <summary>
        /// authkey пользователя
        /// </summary>
        public string authkey { get; set; }
        /// <summary>
        /// IDшник аптеки
        /// </summary>
        public uint? drugstoreId { get; set; }
    }

    public class AttacheDrug : GenerateUserAttachDrugstore
    {
        public string lic_code { get; set; }
    }

    public class Authorize
    {
        public string email { get; set; }
        public string password { get; set; }

        public string service { get; set; }
    }

    public class UserRegistrationReq
    {
        /// <summary>
        /// ключ
        /// </summary>
        public string key { get; set; }
        public string authkey { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public string fio { get; set; }
        /// <summary>
        /// Фамилия
        /// </summary>
        public string familia { get; set; }
        /// <summary>
        /// имя
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// отчество
        /// </summary>
        public string fathername { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        /// <summary>
        /// id сервиса (7 по умолчанию - это для сети; 6 - для аптеки)
        /// </summary>
        public string service { get; set; } = "7";
        /// <summary>
        /// роль пользователя
        /// </summary>
        public string role { get; set; }
        /// <summary>
        /// заготовк лицензионного ключа
        /// </summary>
        public string lic_key { get; set; }
    }

    public class UserRegUMKB
    {
        /// <summary>
        /// ключ
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// id сервиса
        /// </summary>
        public string service { get; set; }
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
        public string relationCode { get; set; }
    }
}
