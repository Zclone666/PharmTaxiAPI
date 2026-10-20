using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
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
    }

    /// <summary>
    /// Ответ по списку ЖНВЛП
    /// </summary>
    public class ResponseMustHave
    {
        /// <summary>
        /// ID проверенного препарата
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// Если находится в списке то true, если нет то false
        /// </summary>
        public bool must_have { get; set; }
        /// <summary>
        /// Цена (если есть)
        /// </summary>
        public double price { get; set; }
    }
}
