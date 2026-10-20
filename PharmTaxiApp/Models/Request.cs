using ECPLib.Common.Drugs.Models;
using System.Text.Json.Serialization;

namespace PharmTaxiApp.Models
{
    public class Request
    {
        /// <summary>
        /// Лицензионный ключ клиента
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// Ключ авторизации (генерируется биллингом)
        /// </summary>
        public string authkey { get; set; }
        /// <summary>
        ///  индекс приложения/сервиса к которому обращается клиент
        /// </summary>
        public string service { get; set; }
        /// <summary>
        /// Концепт ДВ/ТН по которому нужно взять дозировки, метод применения, первичную упаковку
        /// </summary>
        public Concept drug { get; set; }
        /// <summary>
        /// Локаль клиента
        /// </summary>
        public string active_language { get; set; } = "ru";
    }

    /// <summary>
    /// Запрос клиента к серверу для GetDosage/GetDescriptions и т.д.
    /// </summary>
    public class RequestDrug
    {
        /// <summary>
        /// Лицензионный ключ клиента
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// Ключ авторизации (генерируется биллингом)
        /// </summary>
        public string authkey { get; set; }
        /// <summary>
        ///  индекс приложения/сервиса к которому обращается клиент
        /// </summary>
        public string service { get; set; }
        /// <summary>
        /// Концепт ДВ/ТН по которому нужно взять дозировки, метод применения, первичную упаковку
        /// </summary>
        /// 
        public Drug drug { get; set; }
        /// <summary>
        /// Локаль клиента
        /// </summary>
        public string active_language { get; set; } = "ru";
    }

    /// <summary>
    /// Запрос по списку ЖНВЛП
    /// </summary>
    public class RequestMustHave
    {
        /// <summary>
        /// FP_ID препарата на проверку
        /// </summary>
       
        public string fp_id { get; set; }
        /// <summary>
        /// TN_ID препарата на проверку
        /// </summary>
        public string tn_id { get; set; }
        /// <summary>
        /// DV_ID препарата на проверку
        /// </summary>
        public string dv_id { get; set; }
        /// <summary>
        /// Имя препаратов 
        /// </summary>
        [JsonIgnore]
        public string name { get; set; }

        /// <summary>
        /// Имя DV 
        /// </summary>
        [JsonIgnore]
        public string dv_name { get; set; }
        /// <summary>
        /// Цена
        /// </summary>
        [JsonIgnore]
        public decimal price { get; set; }
    }

    /// <summary>
    /// Ответ по списку ЖНВЛП
    /// </summary>
    public class ResponseMustHave:RequestMustHave
    {
        /// <summary>
        /// Если находится в списке то true, если нет то false
        /// </summary>
        public bool must_have { get; set; }

    }
}
