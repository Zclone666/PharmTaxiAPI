using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Models
{
    public class GetBy
    {
        /// <summary>
        /// лиц ключ
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// ключ авторизации
        /// </summary>
        public string authkey { get; set; }
        /// <summary>
        /// id пользователя
        /// </summary>
        public string userid { get; set; }
    }

    public class GetByPharmId
    {
        /// <summary>
        /// лиц ключ
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// ключ авторизации
        /// </summary>
        public string authkey { get; set; }
        /// <summary>
        /// id пользователя
        /// </summary>
        public string pharmid { get; set; }
    }
}
