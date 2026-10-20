using System.Collections.Generic;
using System.ComponentModel;

namespace Core.Operations
{
	public enum OperationType
	{
		[Description("Тест конвейера (файл не обрабатывается)")]
		Test = -1,
		[Description("Прайс-лист аптеки")]
		LoadPrice = 0,
		[Description("Ассортимент аптеки")]
		LoadAssortment = 1,
		[Description("Изменения по ценам/остаткам в аптеке")]
		ChangePrice = 2,
		[Description("Удаление позиций из аптечного прайс-листа/ассортимента")]
		RemoveFromPrice = 3,
		[Description("Реестр изъятых из обращения препаратов")]
		LoadWithdrawns = 4,
		[Description("ГРЛС (Россия)")]
		LoadGrls = 5
	}

	public static class OperationTypes
	{
		public static IEnumerable<OperationType> All
		{
			get
			{
				yield return (OperationType.Test);
				yield return (OperationType.LoadPrice);
				yield return (OperationType.LoadAssortment);
				yield return (OperationType.ChangePrice);
				yield return (OperationType.RemoveFromPrice);
				yield return (OperationType.LoadWithdrawns);
				yield return (OperationType.LoadGrls);
			}
		}
	}
}
