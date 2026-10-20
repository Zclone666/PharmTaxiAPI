using Core.Data;
using Core.Finders.Custom;
using Core.Models;
using Core.Pharm;
using OL.Conveyors;

namespace PharmService
{
	internal class PharmLogData
	{
		public PharmDataAdapters Adapters { get; private set; }
		public string From { get; private set; }
		public string Operation { get; private set; }
		public ItemData Data { get; private set; }
		public Concept Pharm { get; private set; }
		public FpFinderResult Findres { get; private set; }
		public Response Responce { get; private set; }
		public ConnectionMode Mode { get; private set; }

		public PharmLogData(PharmDataAdapters adapters, string from, string operation, ItemData data, Concept pharm, FpFinderResult findres, Response responce, ConnectionMode mode)
		{
			Adapters = adapters;
			From = from;
			Operation = operation;
			Data = data;
			Pharm = pharm;
			Findres = findres;
			Responce = responce;
			Mode = mode;
		}
	}

	/// <summary>
	/// Асинхронный (конвейерный) сохраняллер в лог загрузок цен и ассортимерта
	/// </summary>
	public class PharmLogWriter
	{
		private static Conveyor<PharmLogData, object> _conveyor = new Conveyor<PharmLogData, object>(ProcessData, null,
			bufferType: Conveyor.BufferType.Queue,
			output: null,
			bufferLength: 512);

		/// <summary>
		/// Поставить информацию в очередь на сохранение в лог
		/// </summary>
		/// <param name="adapters">Адаптеры данных</param>
		/// <param name="from">IP-адрес и порт источника запроса</param>
		/// <param name="operation">Операция</param>
		/// <param name="data">Данные запроса</param>
		/// <param name="pharm">Аптека, по которой загружается информация</param>
		/// <param name="findres">Результат идентификации ФП</param>
		/// <param name="responce">Ответ, данный на этот запрос</param>
		/// <param name="mode">Режим подключения к базе</param>
		public static void Write(PharmDataAdapters adapters, string from, string operation, ItemData data, Concept pharm, FpFinderResult findres, Response responce, ConnectionMode mode)
		{
			_conveyor.Put(new PharmLogData(adapters, from, operation, data, pharm, findres, responce, mode));
		}

		private static object ProcessData(PharmLogData data)
		{
			data.Adapters.Log(data.From, data.Operation, data.Data, data.Data.ToXml(5000), data.Pharm, data.Findres, data.Responce, data.Mode);
			return (null);
		}
	}
}