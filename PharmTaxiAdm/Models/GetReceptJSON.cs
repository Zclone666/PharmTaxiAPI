using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiAdm.Models
{
    public class GetReceptJSON
    {
        public string key { get; set; }
    }

    public class NewGetRecipeJSON
    {
        /// <summary>
        /// Постоянный ключ авторизации
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// Временный токен авторизации (https://cs.socmedica.com/api/user/authorization)
        /// </summary>
        public string authkey { get; set; }
        /// <summary>
        /// Идентификатор устройства
        /// </summary>
        public string device_id { get; set; }
        /// <summary>
        /// Номер сервиса (4 - экф, 108 технический экф2)
        /// </summary>
        public string service { get; set; } = "4";
        /// <summary>
        /// Код(номер) рецепта
        /// </summary>
        public string recipe { get; set; }
        /// <summary>
        /// E-mail, коротый используется при отправке рецепта
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// id пациента, по которому необходимо получить список рецептов
        /// </summary>
        public string patient_id { get; set; }
        /// <summary>
        /// Локаль пользователя
        /// </summary>
        public string active_language { get; set; } = "ru";

        //public string key { get; set; }
        //public string authkey { get; set; }
        //public string device_id { get; set; }
        //public string service { get; set; }
        //public string recipe { get; set; }
        //public string email { get; set; }
        //public string active_language { get; set; } = "ru";
        ///// <summary>
        ///// id пациента, по которому необходимо получить список рецептов
        ///// </summary>
        //public string patient_id { get; set; }

    }
}
