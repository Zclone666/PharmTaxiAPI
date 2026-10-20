using System;

namespace PharmTaxiApp.Models.Barcodes
{
	public static class EAN
	{
		public static bool CheckControlSum(string value)
		{
			if (value == null)
				throw new ArgumentNullException("value");

			uint sum = 0;

			for (int indx = 1; indx <= value.Length; indx++)
				sum += uint.Parse(value[value.Length - indx].ToString()) * ((indx % 2 == 0) ? (uint)3 : (uint)1);

			return ((sum % (uint)10) == (uint)0);
		}

		public static byte CalculateControlSum(string value, int eanLength)
		{
			if (value == null)
				throw new ArgumentNullException("value");
			if ((value.Length + 1) < eanLength)
				throw new ArgumentOutOfRangeException("value", String.Format("Длина строки недостаточна для вычисления контрольной суммы по EAN-{0}", eanLength));
			if (eanLength <= 0)
				throw new ArgumentException("Значение должно быть больше нуля", "eanLength");

			value = String.Concat(value.Substring(0, eanLength - 1), "0");
			uint sum = 0;

			for(int indx = 1; indx <= value.Length; indx++)
				sum += uint.Parse(value[value.Length - indx].ToString()) * ((indx % 2 == 0) ? (uint)3 : (uint)1);

			return ((byte)((uint)10 - sum % (uint)10));
		}
	}
}
