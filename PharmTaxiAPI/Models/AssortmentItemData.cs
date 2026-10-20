//using Core.PharmService;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace Core.Data
{
	public class AssItemData : AssortmentItemData
    {
		[BsonId]
		public ObjectId _id { get; set; }

		/// <summary>
		/// ФП ID
		/// </summary>
		public uint? PackID { get {return PackId; } set { PackId = value; } }

		public AssItemData() { }

		public AssItemData(AssortmentItemData data)
        {
			var fields = typeof(AssortmentItemData).GetFields();
			for (int i = 1; i < fields.Length; i++)
			{ }
			var fields2 = typeof(AssItemData).GetFields();
			foreach (var i in fields2)
            {
				PropertyInfo info2 = this.GetType().GetProperty(i.Name);
				foreach(var j in fields)
                {
					PropertyInfo info = this.GetType().GetProperty(j.Name);
					var u = info.GetValue(this);
					info2.SetValue(this, u);
				}
			}
        }
	}

	public class GetAss : AssortmentItemData
    {
		/// <summary>
		/// Это для паджинации - сколько пропустить
		/// </summary>
		public int SetSkip { get; set; }
		
		/// <summary>
		///тоже для паджинации - сколько за раз вывести 
		/// </summary>
		public int SetLimit { get; set; }


	}
	/// <summary>
	/// Позиция ассоритмента
	/// </summary>
	[DataContract]
	public class AssortmentItemData : PharmPositionData
	{

		/// <summary>
		/// Номер регистрационного удостоверения
		/// </summary>
		[DataMember]
		public string RegistrationCertificate { get; set; }

		/// <summary>
		/// Дата регистрации препарата
		/// </summary>
		[DataMember]
		public DateTime? RegistrationDate { get; set; }

		/// <summary>
		/// Дата окончания действия регистрационного удостоверения
		/// </summary>
		[DataMember]
		public DateTime? RegistrationEndDate { get; set; }

		/// <summary>
		/// Дата аннулирования регистрационного удостоверения
		/// </summary>
		[DataMember]
		public DateTime? RegistrationCancellationDate { get; set; }

		/// <summary>
		/// Юридическое лицо, на имя которого выдано регистрационное удостоверение
		/// </summary>
		[DataMember]
		public string RegistrationOwner { get; set; }

		/// <summary>
		/// Действующее вещество. Международное непатентованное или химическое наименование
		/// </summary>
		[DataMember]
		public string ActiveSubstance { get; set; }

		/// <summary>
		/// Сведения о стадиях производства
		/// </summary>
		[DataMember]
		public string ProductionStages { get; set; }

		/// <summary>
		/// Фармако-терапевтическая группа
		/// </summary>
		[DataMember]
		public string PharmacoTherapeuticGroup { get; set; }

		/// <summary>
		/// Номер партии упаковки медикамента
		/// </summary>
		[DataMember]
		public string ConsignmentNumber { get; set; }

		/// <summary>
		/// Срок годности медикамента
		/// </summary>
		[DataMember]
		public DateTime? BestBefore { get; set; }

		/// <summary>
		/// Дата поставки медикамента
		/// </summary>
		[DataMember]
		public DateTime? DeliveryDate { get; set; }

		/// <summary>
		/// Количество упаковок медикамента в наличии
		/// </summary>
		[DataMember]
		public long Quantity { get; set; }

		/// <summary>
		/// Цена оптовая закупочная
		/// </summary>
		public decimal? PurchasePrice { get; set; }

		/// <summary>
		/// Цена продажи
		/// </summary>
		public decimal SellingPrice { get; set; }

		/// <summary>
		/// Скидка
		/// </summary>
		[BsonElement]
		public float? Discount { get; set; }

		/// <summary>
		/// Тип цены (по умолчанию = 1)
		/// </summary>
		[BsonElement]
		public string SellingPriceType { get; set; } = "1";
		/// <summary>
		/// Комментарии к позиции ассортимента
		/// </summary>
		[DataMember]
		public string Comment { get; set; }

		/// <summary>
		/// Локайшн - это местоположение в рамках складов СКОЛа
		/// </summary>
		[BsonElement]
		public uint Location { get; set; }

		/// <summary>
		/// История перемещений
		/// </summary>
		[BsonElement]
		public uint LocationHistory { get; set; }

		/// <summary>
		/// Тип
		/// </summary>
		[BsonElement]
        public string Type { get; set; }

		/// <summary>
		/// Серийный номер
		/// </summary>
		public string Serial { get; set; }

		/// <summary>
		/// ГТИН
		/// </summary>
		public string GTIN { get; set; }

		/// <summary>
		/// Проверенность
		/// </summary>
		public bool Validated { get;set; }

		/// <summary>
		/// Подлинность
		/// </summary>
		public bool Authinticity { get; set; }

		/// <summary>
		/// С какой вероятностью товар у продавца
		/// </summary>
		public bool Probability_seller { get; set; }

		/// <summary>
		/// Товар у покупателя
		/// </summary>
		public bool Probability_aquire { get; set; }

		/// <summary>
		/// Количество сканирований
		/// </summary>
		public int ScansCount { get; set; }

		/// <summary>
		/// IDшник покупателя
		/// </summary>
		public uint IdBuyer { get; set; }

		/// <summary>
		/// Полное количество - количество в упаковке на дозу
		/// </summary>
		public long FullQuantity { get; set; }

		/// <summary>
		/// Статус перемещения - для СКОЛа
		/// </summary>
		public int RelocationStatus { get; set; }
	}
}