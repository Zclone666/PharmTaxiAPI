using Core.Pharm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PharmTaxiAPI.Models
{
	public class PriceResponse
	{
		public uint pages { get; set; }
		public uint itemsCount { get; set; }
		public PriceResponseItem[] data { get; set; }
		public string ErrorMessage { get; set; }
	}

	public class PriceResponseItem
	{
		public uint price_id { get; set; }
		public string drug_name { get; set; }
		public uint drugstore_id { get; set; }
		public string drugstore_name { get; set; }
		public decimal? value { get; set; }
		public uint? quantity { get; set; }
	}

}