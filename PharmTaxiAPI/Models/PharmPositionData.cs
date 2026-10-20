using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Runtime.Serialization;

namespace Core.Data
{
	/// <summary>
	/// Упаковка медикамента
	/// </summary>
	[DataContract]
	public class PharmPositionData : ItemData
	{
		/// <summary>
		/// Штрихкод упаковки медикамента
		/// </summary>
		[DataMember]
		public string Barcode { get; set; }

		/// <summary>
		/// Полное название препарата или внутриаптечное наименование упаковки медикамента
		/// </summary>
		[BsonElement]
		public string Name { get; set; }

		/// <summary>
		/// Торговое наименование
		/// </summary>
		[DataMember]
		public string TradeName { get; set; }

		/// <summary>
		/// Международное непатентованное наименование (не для сервиса!!!)
		/// </summary>
		// [DataMember]
		public string MNN { get; set; }

		/// <summary>
		/// Форма выпуска (ЛФ+Дозировк+УП+Numero)
		/// </summary>
		[DataMember]
		public string Composition { get; set; }

		/// <summary>
		/// Лекарственная форма и дозировка
		/// </summary>
		[DataMember]
		public string FormDosage { get; set; }

		/// <summary>
		/// Лекарственная форма
		/// </summary>
		[DataMember]
		public string Form { get; set; }

		/// <summary>
		/// Дозировка с единицей измерения
		/// </summary>
		[DataMember]
		public string Dosage { get; set; }

		/// <summary>
		/// Упаковка (УП) или фасовка (первичная и вторичная)
		/// </summary>
		[DataMember]
		public string Pack { get; set; }

		/// <summary>
		/// Производитель + страна
		/// </summary>
		[DataMember]
		public string ManufacturerCountry { get; set; }

		/// <summary>
		/// Производитель медикамента
		/// </summary>
		[DataMember]
		public string Manufacturer { get; set; }

		/// <summary>
		/// Страна производителя
		/// </summary>
		[DataMember]
		public string Country { get; set; }

		public override bool IsRequiredDataExists()
		{
			if (!base.IsRequiredDataExists())
				return (false);

			if(PackId == null)
			{
				if (
						(
							String.IsNullOrEmpty(TradeName)
							||
							(
								String.IsNullOrEmpty(Composition)
								&& (String.IsNullOrEmpty(FormDosage) || String.IsNullOrEmpty(Pack))
								&& (String.IsNullOrEmpty(Form) || String.IsNullOrEmpty(Dosage) || String.IsNullOrEmpty(Pack))
							)
						)
						&& String.IsNullOrEmpty(Name))
					return (false);

				if (String.IsNullOrEmpty(ManufacturerCountry)
						&& (String.IsNullOrEmpty(Manufacturer) || String.IsNullOrEmpty(Country)))
					return (false);
			}

			return (true);
		}
	}
}