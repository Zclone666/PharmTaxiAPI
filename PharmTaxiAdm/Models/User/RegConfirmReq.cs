using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiAdm.Models.User
{
    public class ReConfirmEmailArgs
    {
        public string email { get; set; }
    }
    public class RegConfirmReqUMKB
    {
        /// <summary>
        /// Лицензионный ключ клиента*
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// Аuthkey пользователя, который принимает приглашение*
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// Код подтверждения приглашения*
        /// </summary>      
        public string code { get; set; }
        /// <summary>
        /// id of service
        /// </summary>
        public string service { get; set; }
        /// <summary>
        /// id of parent user
        /// </summary>
        public string parentId { get; set; }
        /// <summary>
        /// New pass
        /// </summary>      
        public string newPassword { get; set; }
    }

    public class RegConfirmReqPHT:RegConfirmReqUMKB
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string firstname { get; set; }
        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string lastname { get; set; }
        /// <summary>
        /// Отчество пользователя
        /// </summary>
        [BsonElement("patronymic")]
        public string middlename { get; set; }
    }

    public class UserFIO
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string firstname { get; set; }
        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string lastname { get; set; }
        /// <summary>
        /// Отчество пользователя
        /// </summary>
        [BsonElement("patronymic")]
        public string middlename { get; set; }
    }

    public class UserFIOReq:UserFIO
    {
        public string authkey { get; set; }

        public string key { get; set; }
    }

}
