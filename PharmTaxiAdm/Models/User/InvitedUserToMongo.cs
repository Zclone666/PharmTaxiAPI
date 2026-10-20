using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiAdm.Models.User
{
    [BsonIgnoreExtraElements]
    public class InvitedUserToMongo
    {
        /// <summary>
        /// ID документа Mongo
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public string? _id { get; set; } = BsonObjectId.GenerateNewId().ToString();

        public uint? inviter_id { get; set; }
        public uint? user_id { get; set; }
        /// <summary>
        /// Гость или нет
        /// </summary>
        public bool? isGuest { get; set; } = false;
        /// <summary>
        /// Что то для приложения
        /// </summary>
        public short? distance { get; set; } = 10;
        /// <summary>
        /// Аналоги
        /// </summary>
        public bool? analogs { get; set; } = false;
        /// <summary>
        /// Что то для приложения
        /// </summary>
        public bool? fullList { get; set; } = false;
        /// <summary>
        /// Что то для приложения
        /// </summary>
        public bool? partners { get; set; } = false;
        /// <summary>
        /// Роль
        /// </summary>
        public string? role { get; set; } = "Provisor";
        /// <summary>
        /// Дата создания пользователя
        /// </summary>
        public DateTime? createdAt { get; set; } = DateTime.Now;
        /// <summary>
        /// Список аптек
        /// </summary>
        public List<uint> pharms { get; set; } = new List<uint>();
        /// <summary>
        /// Email пользователя
        /// </summary>
        public string email { get; set; }
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
        /// <summary>
        /// Email подтвержден? (UserCore.status==0)
        /// </summary>
        public bool? isEmailConfirmed { get; set; }
        /// <summary>
        /// Список id сервисов пользователя
        /// </summary>
        public List<uint> permissions { get; set; }
        
        [JsonIgnore]    
        public object services { get; set; }
    }
}
