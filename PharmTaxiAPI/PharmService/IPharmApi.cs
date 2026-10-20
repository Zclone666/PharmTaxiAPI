using Core.Data;
using Core.Pharm;
using System.Collections.Generic;
using System.ServiceModel;

namespace PharmService
{

	/*
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPharmApi" in both code and config file together.
	[ServiceContract]
	[WsdlDocumentation("Сервис загрузки цен и аптечного ассортимента системы Pharmtaxi")]
	public interface IPharmApi
	{
	/*
		/// <summary>
		/// Загрузка прайс-листа
		/// </summary>
		[OperationContract]
		[WsdlDocumentation("Загрузка прайс-листа")]
		Response LoadPrice(IEnumerable<PriceItemData> data, ConnectionMode mode, string key);

		/// <summary>
		/// Изменение цен в прайс-листе и остатков
		/// </summary>
		[OperationContract]
		[WsdlDocumentation("Изменение цен в прайс-листе и остатков")]
		Response ChangePrice(IEnumerable<ModifyPriceItemData> data, ConnectionMode mode, string key);

		/// <summary>
		/// Удаление позиций из прайс-листа
		/// </summary>
		[OperationContract]
		[WsdlDocumentation("Удаление позиций из прайс-листа")]
		Response RemoveFromPrice(IEnumerable<RemovePriceItemData> data, ConnectionMode mode, string key);

		/// <summary>
		/// Загрузка ассортимента аптеки
		/// </summary>
		[OperationContract]
		[WsdlDocumentation("Загрузка ассортимента аптеки")]
		Response LoadAssortment(IEnumerable<AssortmentItemData> data, ConnectionMode mode, string key);

		/// <summary>
		/// Проверка связи
		/// </summary>
		[OperationContract]
		[WsdlDocumentation("Проверка связи")]
		string Ping();


		/// <summary>
		/// Проверка ключа авторизации (возвращает либо OWNER, либо логин аптеки, либо null)
		/// </summary>
		[OperationContract]
		[WsdlDocumentation("Проверка ключа авторизации (возвращает либо OWNER, либо логин аптеки, либо null)")]
		string CheckKey(ConnectionMode mode, string key);
	}*/
}
