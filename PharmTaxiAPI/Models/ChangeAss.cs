using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Models
{
    public class ChangeAss
    {
		[JsonRequired]
        public long? qty { get { return Quantity; } set { Quantity = value; } }

        [JsonIgnore]
        public long? Quantity { get; set; }

		[JsonRequired]
        public float? price { get { return SellingPrice; } set { SellingPrice = value; } }

        [JsonIgnore]
        public float? SellingPrice { get; set; }

		[JsonRequired]
		public uint? pharm_id { get { return PharmId; } set { PharmId = value; } }

		[JsonIgnore]
		public uint? PharmId { get; set; }

		[JsonRequired]
		public string pharm_pack_id { get { return PharmPackId; } set { PharmPackId = value; } }

		[JsonIgnore]
		public string PharmPackId { get; set; }


		/// <summary>
		/// Уникальный номер упаковки медикамента в системе UMKB
		/// </summary>
		/// 
		[JsonRequired]
		public string fp_id { get { return PackId; } set { PackId = value; } }

		[JsonIgnore]
		public string PackId { get; set; }

		[JsonRequired]
		public long? qty_free { get { return QuantityFree; } set { QuantityFree = value; } }

		[JsonIgnore]
		public long? QuantityFree { get; set; }

	}
}
