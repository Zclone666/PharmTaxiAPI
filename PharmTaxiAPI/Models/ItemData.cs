using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Core.Data
{
	/// <summary>
	/// Позиция
	/// </summary>
	[DataContract]
	public class ItemData
	{
		/// <summary>
		/// Персональный код aптеки
		/// </summary>
		[DataMember]
		public uint? PharmId { get; set; }

		/// <summary>
		/// Логин aптеки
		/// </summary>
		[DataMember]
		public string PharmLogin { get; set; }

		/// <summary>
		/// Уникальный номер упаковки медикамента в системе UMKB
		/// </summary>
		[DataMember]
		public uint? PackId { get; set; }

		/// <summary>
		/// Внутриаптечный ID упаковки медикамента или артикул
		/// </summary>
		[DataMember]
		public string PharmPackId { get; set; }

		public virtual bool IsRequiredDataExists()
		{
			if ((PharmId == null) && String.IsNullOrEmpty(PharmLogin))
				return (false);

			if((PackId == null) && String.IsNullOrEmpty(PharmPackId))
				return (false);

			return (true);
		}

		public virtual string ToXml(int maxLength)
		{
			string stringData = null;

			XmlSerializer formatter = new XmlSerializer(GetType());
			using (StringWriter wrtr = new StringWriter())
			{
				formatter.Serialize(wrtr, this);
				stringData = wrtr.ToString();

				if (stringData.Length > 5000)
					stringData = stringData.Substring(0, maxLength);
			}

			return (stringData);
		}
	}
}