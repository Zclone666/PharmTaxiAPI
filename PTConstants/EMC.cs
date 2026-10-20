using System;
using System.Collections.Generic;
using System.Text;

namespace PTConstants
{
    public class EMC
    {
        /// <summary>
        /// Строка подключения к Tarantool EMC
        /// </summary>
        public static string TarantoolConnectionString;
        /// <summary>
        /// Строка подключения к MongoDB EMC
        /// </summary>
        public static string MongoDbConnectionString;
        /// <summary>
        /// Флаг сбора метрик, заполнять true или false
        /// </summary>
        public static bool UseMetrics;
    }
}
