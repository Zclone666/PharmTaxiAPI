using ECPLib.Common.Drugs.Models;
using ECPLib.Common.Patient.Models;
using ECPLib.Checkup.Models;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using SMCommonUtils.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiApp.Models
{
    public class SetInfoPreparations
    {
#pragma warning disable IDE1006 // Стили именования
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        [BsonId]
        public string id { get; set; } //= ObjectId.GenerateNewId().ToString();
        ///// <summary>
        ///// 
        ///// </summary>
        //[JsonIgnore]
        //[BsonElement]
        //public string misid { get; set; } = string.Empty;

        [BsonElement]
        public long date { get; set; } = SMCommonUtils.Converter.DateTimeToInt64(DateTime.Now);
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        [BsonElement]
        public string userid { get; set; } = string.Empty;
        [JsonIgnore]
        [BsonElement]
        public string patientid { get; set; } = string.Empty;
        /// <summary>
        /// Временной ненужное поле - сюда обычно суём счётчики времени выволнения запроса и т.д. Потом можно удалить
        /// </summary>
        [JsonIgnore]
        [BsonElement]
        public string statistic { get; set; }
        /// <summary>
        /// Стандартный Алерт - для передачи ошибки
        /// </summary>
        [BsonElement]
        public UMKBRequests.Alert alert { get; set; } = null;
        [BsonElement]
        public List<Drug> drugs { get; set; } = null;
        /// <summary>
        /// Список имён. Ключ - id в системе умкб, тело - имя, показываемое пользователю
        /// В случае работы с фронтом - все ключи зашифрованы
        /// </summary>
        [BsonElement]
        public ADictionary<string, string> names { get; set; } = new ADictionary<string, string>();
        /// <summary>
        /// Список конфликтов
        /// </summary>
        [BsonElement]
        public List<Warning> warnings { get; set; } = new List<Warning>();
        /// <summary>
        /// Описания (взаимодействий)
        /// </summary>
        [BsonElement]
        public ADictionary<string, List<string>> description { get; set; } = new ADictionary<string, List<string>>();
        /// <summary>
        /// Список литературы
        /// </summary>
        [BsonElement]
        public ADictionary<string, List<string>> literature { get; set; } = new ADictionary<string, List<string>>();
        /// <summary>
        /// Последствия/результат
        /// </summary>
        [BsonElement]
        public ADictionary<string, List<string>> outcome { get; set; } = new ADictionary<string, List<string>>();
        /// <summary>
        /// Ссылки на внешние источники
        /// </summary>
        [BsonElement]
        public ADictionary<string, string> urls { get; set; } = new ADictionary<string, string>();

        [BsonElement]
        public string cautiontitle { get; set; }
        [BsonElement]
        public HashSet<string> cautionitems { get; set; } = new HashSet<string>();

        /// <summary>
        /// Данные пациента
        /// </summary>
        [BsonElement]
        public Patient patient { get; set; }
        /// <summary>
        /// Тут заголовок подсказок для значков (Id типа варнинга->id текста в names)
        /// </summary>
        [BsonElement]
        public ADictionary<string, string> hinttype { get; set; } = new ADictionary<string, string>();
        [BsonElement]
        public string key { get; set; }
        [BsonElement]
        public string authkey { get; set; }
        [BsonElement]
        public string misurl { get; set; }
        [BsonElement]
        public string device_id { get; set; }
        [BsonElement]
        public string service { get; set; } = "108";
        [BsonElement]
        public string misid { get; set; }
        [BsonElement]
        public string misprotocol { get; set; }
        [BsonElement]
        public string clinic_id { get; set; }
        [BsonElement]
        public string clinic_code { get; set; }
        [BsonElement]
        public string clinic_name { get; set; }
        [BsonElement]
        public string dep_id { get; set; }
        [BsonElement]
        public string dep_code { get; set; }
        [BsonElement]
        public string dep_name { get; set; }
        [BsonElement]
        public string doctor_id { get; set; }
        [BsonElement]
        public string doctor_code { get; set; }
        [BsonElement]
        public string doctor_name { get; set; }
        //[BsonElement]
        //public KeyValue clinic { get; set; }
        //[BsonElement]
        //public KeyValue department { get; set; }
        //[BsonElement]
        //public KeyValue doctor { get; set; }
        [BsonElement]
        public string recipe { get; set; } = null;
        [BsonElement]
        public string protocol { get; set; } = null;
        public List<Drug> liquids { get; set; } = new List<Drug>();
        /// <summary>
        /// Флаг отмены
        /// </summary>
        [BsonElement] public bool repeal { get; set; } = false;
#pragma warning restore IDE1006 // Стили именования
    }
}
