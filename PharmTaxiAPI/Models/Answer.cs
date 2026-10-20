using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UMKBRequests;

namespace PharmTaxiAPI.Models
{
    public class Answer
    {
#pragma warning disable IDE1006 // Стили именования
        /// <summary>
        /// Стандартный алерт для передачи состояния
        /// </summary>
        public Alert alert { get; set; }
        public List<Concept> concepts { get; set; } = new List<Concept>();
#pragma warning restore IDE1006 // Стили именования
    }

    public class Concept
    {
#pragma warning disable IDE1006 // Стили именования
        /// <summary>
        /// Уникальный индентификатор (нужен для GUI)
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// BigId (в случае работы с фронтом - зашифрованный)
        /// </summary>
        public string id { get; set; }
        public string fpid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// Левел 
        /// </summary>
        public uint level { get; set; }
        /// <summary>
        /// Наименование ЛС
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Строка по которой был осуществлён поиск
        /// </summary>
        public string namesearch { get; set; }
        /// <summary>
        /// Номер группы (для растворов)
        /// </summary>
        public int group { get; set; } = 0;
        /// <summary>
        /// Флаг - true - основное заболевание, false - сопутствующее заболевание
        /// </summary>
        public bool main { get; set; }
#pragma warning restore IDE1006 // Стили именования
    }

}
