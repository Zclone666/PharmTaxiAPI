using Core.Data;
using Core.Models;
using Core.Statistic.Price;
using OL.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmService
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "StatisticApi" in code, svc and config file together.
	// NOTE: In order to launch WCF Test Client for testing this service, please select StatisticApi.svc or StatisticApi.svc.cs at the Solution Explorer and start debugging.
	public class StatisticApi : ApiBase, IStatisticApi
	{
		private const double MinSimilarity = 0.8;

		public StatisticApi()
		{
		}

		public IEnumerable<AvgResponseItem> CurrentAvgByPoint(GeoPoint point, double radix, IEnumerable<MedicamentData> medicaments, ConnectionMode mode, bool onlyVitalDrugs, bool withAnalogs)
		{
			Task logTask = Log(String.Format("StatisticApi.CurrentAvgByPoint([{0}, {1}], {2}, {3}, {4}, {5})",
					point.Latitude,
					point.Longitude,
					radix,
					String.Join(";", medicaments.Select(m => m.name)),
					mode,
					onlyVitalDrugs));
	
			try
			{
				try
				{
					Task<AvgResponseItem[]> tsk = PriceStatistic.GetAvgPrice(point, radix, medicaments, mode, onlyVitalDrugs, withAnalogs);
					tsk.Wait();
					return (tsk.Result);
				}
				finally
				{
					logTask.Wait();
				}
			}
			catch (Exception err)
			{
				Log(err);
				throw;
			}
		}
	}
}
