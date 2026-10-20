using System.Runtime.Serialization;

namespace Core.Data
{
	/// <summary>
	/// Позиция прайс-листа
	/// </summary>
	[DataContract]
	public class PriceItemData : PharmPositionData
	{
		/// <summary>
		/// Цена розничная отпускная
		/// </summary>
		[DataMember]
		public decimal SellingPrice { get; set; }

		/// <summary>
		/// Тип цены розничной
		/// </summary>
		[DataMember]
		public string SellingPriceType { get; set; }

		/// <summary>
		/// Размер скидки позиции прайс-листа в процентах
		/// </summary>
		[DataMember]
		public float? Discount { get; set; }

		/// <summary>
		/// Комментарии к позиции прайс-листа
		/// </summary>
		[DataMember]
		public string Comment { get; set; }
	}
}