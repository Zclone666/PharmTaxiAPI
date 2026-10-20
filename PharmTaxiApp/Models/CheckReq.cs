using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiApp.Models
{
    public class CheckReq
    {
        public BarcodeRequestData code { get; set; } = new BarcodeRequestData();
        /// <summary>
        /// штрих код
        /// </summary>
        public string text { get; set; }
        /// <summary>
		/// формат штрих кода
		/// </summary>
        public string format { get; set; }
        /// <summary>
        /// массив double с координатами
        /// </summary>
        public double?[] coords { get; set; }
        /// <summary>
        /// точность
        /// </summary>
        public uint? accuracy { get; set; }
        /// <summary>
        /// аутизм кей
        /// </summary>
        public string auth_key { get; set; }
        /// <summary>
        /// по умолчанию "ru"
        /// </summary>
        public string lang { get; set; } = "ru";
        /// <summary>
        /// режим (строгий) WORK
        /// </summary>
        public string mode { get; set; } = "WORK";
        /// <summary>
        /// Страна. Слать "sru"
        /// </summary>
        public string country { get; set; }
    }
}
