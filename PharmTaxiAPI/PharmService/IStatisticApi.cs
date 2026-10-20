using Core.Data;
using Core.Statistic.Price;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PharmService
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IStatisticApi" in both code and config file together.
	[ServiceContract]
	//("Сервис статистики по аптекам")]
	public interface IStatisticApi
	{
		[OperationContract]
		//("Средняя/минимальная/максимальная цена по списку препаратов на текущий момент")]
		IEnumerable<AvgResponseItem> CurrentAvgByPoint(GeoPoint point, double radix, IEnumerable<MedicamentData> medicaments, ConnectionMode mode, bool onlyVitalDrugs, bool withAnalogs);
	}
}
