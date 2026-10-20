using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PharmTaxiApp.Models
{
	public class IdNamePair
	{
		public uint id { get; private set; }
		public string name { get; private set; }

		public IdNamePair(uint id, string name)
		{
			this.id = id;
			this.name = name;
		}
	}
}