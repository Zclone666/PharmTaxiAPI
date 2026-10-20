using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiApp.Models
{
	public class BarcodeRequestData
	{
		/// <summary>
		/// штрих код
		/// </summary>
		public string text { get; set; }
		/// <summary>
		/// формат штрих кода
		/// </summary>
		public string format { get; set; }
	}
}
