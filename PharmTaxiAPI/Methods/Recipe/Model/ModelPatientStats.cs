using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Methods.Recipe.Model
{
    public class ModelPatientStats
    {
        /// <summary>
        /// ulongId пациента
        /// </summary>
        [BsonId]
        public string id { get; set; }
        /// <summary>
        /// Список рецептов (ключ - код рецепта, тело - дата окончания действия рецепта)
        /// </summary>
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<string, long> recipes { get; set; } = new Dictionary<string, long>();
    }
}
