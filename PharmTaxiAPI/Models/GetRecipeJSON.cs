using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Models
{
    public class GetRecipeJSON
    {
        /// <summary>
        /// id Аптеки
        /// </summary>
            public string PharmId { get; set; }
        /// <summary>
        /// Код рецепта
        /// </summary>
            public string ReceiptCode { get; set; }
        /// <summary>
        /// ФИО пациента
        /// </summary>
            public string FIO { get; set; }
        /// <summary>
        /// Дата рождения пациента
        /// </summary>
            public string birthdate { get; set; }
        /// <summary>
        /// СНИЛС пациента
        /// </summary>
            public string SNILS { get; set; }
        /// <summary>
        /// Регион работы аптеки
        /// </summary>
            public string Region { get; set; }
        /// <summary>
        /// Дата рецепта
        /// </summary>
            public string DateOfReceipt { get; set; }
        /// <summary>
        /// Id пациента
        /// </summary>
            public uint PatientId { get; set; }
        /// <summary>
        /// Массив кодов рецептов
        /// </summary>
            public List<string> ReceiptsCodes { get; set; }
    }
}
