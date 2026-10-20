using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiAdm.Models
{
	public class AvgResponseItem
	{
		public string send { get; set; }

		public double? value { get; set; }

		public double? min { get; set; }

		public double? max { get; set; }

		public uint? id { get; set; }

		public ushort? table { get; set; }

		public string name { get; set; }
	}
}
