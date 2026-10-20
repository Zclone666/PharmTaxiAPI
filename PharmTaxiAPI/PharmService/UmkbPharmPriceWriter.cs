using Core.Data;
using Core.Models;
using OL.Conveyors;

namespace PharmService
{
	internal class UmkbPharmPriceData
	{
		public ItemData Data { get; private set; }
		public ConnectionMode Mode { get; private set; }

		public UmkbPharmPriceData(ItemData data, ConnectionMode mode)
		{
			Data = data;
			Mode = mode;
		}
	}

	/// <summary>
	/// Асинхронный (конвейерный) сохраняллер цен и аптечного ассортимента в базу UMKB
	/// </summary>
	public static class UmkbPharmPriceWriter
	{
		private static Conveyor<UmkbPharmPriceData, object> _conveyor = new Conveyor<UmkbPharmPriceData, object>(ProcessData, null,
			bufferType: Conveyor.BufferType.Queue,
			output: null,
			bufferLength: 512);

		/// <summary>
		/// Поставить данные в очередь на сохранение
		/// </summary>
		/// <param name="data">Данные</param>
		/// <param name="mode">Режим подключения к базе</param>
		public static void Write(ItemData data, ConnectionMode mode)
		{
			_conveyor.Put(new UmkbPharmPriceData(data, mode));
		}

		private static object ProcessData(UmkbPharmPriceData data)
		{
			//связь аптека-ФП: инструмент 1722
			//остаток = value_a
			//цена = value_b

			if (data.Data.PackId == null)
				return(null);

			decimal? price = null;
			long? quantity = null;

			PriceItemData pd = data.Data as PriceItemData;
			AssortmentItemData ad = data.Data as AssortmentItemData;
			ModifyPriceItemData md = data.Data as ModifyPriceItemData;
			RemovePriceItemData rd = data.Data as RemovePriceItemData;

			if (pd != null)
				price = pd.SellingPrice;
			else if (ad != null)
				quantity = ad.Quantity;
			else if (md != null)
			{
				price = md.SellingPrice;
				quantity = md.Quantity;
			}
			else if (rd != null)
			{
				Concept link = GetLink(data.Data.PharmId.Value, data.Data.PackId.Value, data.Mode);

				if (link != null)
				{
					link.Delete();
					UmkbDataSource.GetAdapters(data.Mode).ConceptAdapter.Save(link);
				}

				return (null);
			}

			if (price != null || quantity != null)
			{
				Concept link = GetLink(data.Data.PharmId.Value, data.Data.PackId.Value, data.Mode);

				if (link == null)
					link = UmkbDataSource.GetFp2DrugstoreInstrument(data.Mode).CreateLink(new Pair(67, data.Data.PackId.Value), new Pair(95, data.Data.PharmId.Value), null,
						(quantity == null) ? (double?)null : (double)quantity.Value,
						(price == null) ? (double?)null : (double)price.Value,
						null,
						null);
				else
				{
					link.BeginUpdate();
					try
					{
						if (quantity != null)
							link.value_a = quantity.Value;

						if (price != null)
							link.value_b = (double)price.Value;
					}
					finally
					{
						link.CommitUpdate();
					}
				}

				UmkbDataSource.GetAdapters(data.Mode).ConceptAdapter.Save(link);
			}

			return (null);
		}

		private static Concept GetLink(uint pharmId, uint packId, ConnectionMode mode)
		{
			return (UmkbDataSource.GetAdapters(mode).ConceptAdapter.Load(UmkbDataSource.GetFp2DrugstoreInstrument(mode), new Pair(95, pharmId), new Pair(67, packId)));
		}
	}
}