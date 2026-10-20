using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiAPI.Models
{

    public class Request
    {
        /// <summary>
        /// Концепт ДВ/ТН по которому нужно взять дозировки, метод применения, первичную упаковку
        /// </summary>
        public Concept drug { get; set; }

        /// <summary>
        /// Локаль клиента
        /// </summary>
        public string active_language { get; set; } = "ru";

        /// <summary>
        /// Ключ авторизации (генерируется биллингом)
        /// </summary>
        public string authkey { get; set; }

        /// <summary>
        /// Лицензионный ключ клиента
        /// </summary>
        public string key { get; set; }
        /// <summary>
        ///  индекс приложения/сервиса к которому обращается клиент
        /// </summary>
        public string service { get; set; }

    }
}
