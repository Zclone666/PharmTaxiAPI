using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiAdm.Models.User
{
    public class AlertUserID
    {
        public uint userid; // id пользователя
        public string role;
        [JsonIgnore]
        public List<string> applications = new List<string>();
        public Alert alert; // Коллекция параметров оповещения
    }
    public class Alert // Коллекция параметров оповещения
    {
        public string code; // Код ошибки
        public string title; // Заголовок
        public string message; // Описание ошибки
        public string level; // Тип сообщения, влияет на внешний вид сообщения, по умолчанию error. Возможные варианты: error-ошибка, success-успех, info-информация, danger-опасность
        public bool sticky; // Должно ли сообщение само пропасть, через несколько секунд. true - зафиксированное сообщение. По умолчанию false.
    }

    [BsonIgnoreExtraElements]
    public class AuthK
    {
        public string authkey { get; set; }
        public string role { get; set; }
        public Alert alert { get; set; }
    }

    public class AuthKUserId : AuthK
    {
        public uint userid;
    }

    public class AuthUserIdPharmId : AuthKUserId
    {
        public uint pharm_net_id { get; set; }
    }
}
