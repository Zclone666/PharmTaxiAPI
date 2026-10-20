/*using Core.Models;
using Core.Processes;
using OL.BuisnessProcess;
using OL.Extensions;
using System;
using System.Linq;

namespace PharmTaxiApp.Models
{
	public class ProcessesBarcodeCheckResponse
	{
		public IdNamePair fp { get; private set; }
		public string gtin { get; private set; }
		public string seria { get; private set; }
		public DateTime? expiredate { get; private set; }
		public ProcessesBarcodeCheckProcessResponse process { get; private set; }

		public ProcessesBarcodeCheckResponse(BuisnessProcess<BarcodeInfo> buisnessProcess, BuisnessProcessResult result)
		{
			object o;
			Concept c = null;

			if (buisnessProcess.AdvancedValues.TryGetValue(CheckWhatIsIt.ConceptValueName, out o))
				c = o as Concept;

			fp = (c == null) ? null : new IdNamePair(c.id, c.ToString());
				// Здесь надо каким-то образом загрузить окружение ФП и закинуть в соответствующие поля
			gtin = buisnessProcess.Data.GTIN;
			seria = buisnessProcess.Data.Seria;
			expiredate = buisnessProcess.Data.UseBy;
			process = new ProcessesBarcodeCheckProcessResponse(buisnessProcess, result);
		}

		public ProcessesBarcodeCheckResponse(Exception err)
		{
			fp = null;
			gtin = null;
			seria = null;
			expiredate = null;
			process = new ProcessesBarcodeCheckProcessResponse(err);
		}
	}

	public class ProcessesBarcodeCheckProcessResponse
	{
		public Guid id { get; private set; }
		public uint status { get; private set; } //<Код статуса: 0 = проверка прошла, 1 = проверка не прошла, 2 = ожидается ввод значения>,
		public string message { get; private set; }
		public ProcessesBarcodeCheckAwaytableResponse[] awaytable_values { get; private set; }
		public ProcessesBarcodeCheckStageResponse[] stages { get; private set; }

		public ProcessesBarcodeCheckProcessResponse(IBuisnessProcess process, BuisnessProcessResult result)
		{
			id = process.ID;
			BuisnessProcess<BarcodeInfo> pbi = process as BuisnessProcess<BarcodeInfo>;

			switch(result.Status)
			{
				case BuisnessProcessStatus.Aborted:
					message = (pbi == null)
						? result.Status.GetDescription()
						: pbi.Results[pbi.Stage].Message;
					status = 1;
					break;
				case BuisnessProcessStatus.Finished:
					message = result.Status.GetDescription(); 
					status = 0;
					break;
				case BuisnessProcessStatus.Waiting:
					message = (pbi == null)
						? result.Status.GetDescription()
						: pbi.Results[pbi.Stage].Message;
					status = 2;
					break;
				default:
					message = result.Status.GetDescription(); 
					status = 99;
					break;
			}

			awaytable_values = (result.AwaytableValues == null)
				? null
				: result.AwaytableValues.Select(v => new ProcessesBarcodeCheckAwaytableResponse(v)).ToArray();

			if (pbi == null)
				stages = null;
			else
				stages = pbi.Results
					.Where(r => r.Value != null)
					.Select(r => new ProcessesBarcodeCheckStageResponse(r.Key, r.Value))
					.ToArray();
		}

		public ProcessesBarcodeCheckProcessResponse(Exception err)
		{
			id = Guid.Empty;
			status = 1;
			message = err.Message;
			awaytable_values = null;
			stages = null;
		}
	}

	public class ProcessesBarcodeCheckAwaytableResponse
	{
		public string key { get; private set; }
		public string type { get; private set; }
		public string prompt { get; private set; }

		public ProcessesBarcodeCheckAwaytableResponse(AwaytableValue val)
		{
			key = val.Name;
			prompt = val.Prompt;
			type = (val.Type == typeof(bool))
				? "boolean"
				: ((val.Type == typeof(string))
					? "text"
					: "unknown");
		}
	}

	public class ProcessesBarcodeCheckStageResponse
	{
		public string name { get; private set; }
		public uint status { get; private set; } // <результат этапа: 0 = выполнено, 1 = ошибка, 2 = не выполнялось>,
		public string message { get; private set; }

		public ProcessesBarcodeCheckStageResponse(BuisnessProcessStage<BarcodeInfo> stage, StageResult result)
		{
			name = stage.Name;

			switch(result.Type)
			{
				case StageResultType.Passed:
					status = 0;
					break;
				case StageResultType.Failed:
					status = 1;
					break;
				case StageResultType.NotExecuted:
				case StageResultType.Wait:
					status = 2;
					break;
			}

			message = result.Message;
		}
	}
}*/