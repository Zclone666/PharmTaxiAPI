//using Core.Config;
using AUpload.Global.Models;
using Core.Data;
//using Core.Finders.Custom;
//using Core.Messages;
using Core.Operations;
//using Core.Operations.FileConvayor;
//using Core.PharmService;
//using OL.Connections;
//using OL.Extensions;
using PharmService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using PTConstants;

namespace Core.Pharm
{

	public class PharmParser : IDisposable
	{
		
		public Response ParseAndExecute(OperationType operation, string fileName, Core.Data.ConnectionMode mode, string key, string lang)
		{
					
			PharmApi phap = new PharmApi();
			string defaultLogin = null;// phap.CheckKey((Core.Data.ConnectionMode)mode, key);
			//string defaultLogin = _client.CheckKey((Core.PharmService.ConnectionMode)mode, key);

			if (defaultLogin == null)
				throw new Exception("IncorrectAuthKey");
			else if (defaultLogin == "OWNER")
				defaultLogin = null;
	
			using (DataSet ds = PharmTaxiAPI.Methods.ExcelRead.Parse(fileName))
			{
				return (ReadAndSend(ds.Tables[0], operation, mode, key, lang, defaultLogin));
			}
		}


		private Response ReadAndSend(DataTable tbl, OperationType operation, Core.Data.ConnectionMode mode, string key, string lang, string defaultLogin)
		{
			switch(operation)
			{
				case OperationType.LoadPrice:
					return (DoLoadPrice(tbl, mode, key, lang, defaultLogin));
				case OperationType.LoadAssortment:
					return (DoLoadAssortment(tbl, mode, key, lang, defaultLogin));
				case OperationType.ChangePrice:
					return (DoChangePrice(tbl, mode, key, lang, defaultLogin));
				case OperationType.RemoveFromPrice:
					return (DoRemoveFromPrice(tbl, mode, key, lang, defaultLogin));
				default:
					throw new Exception("NotSupportedOperation");
			}
		}

		private static Response JoinWithTitle(IEnumerable<Response> src, string title)
		{
			bool ok = false;
			bool wr = false;
			int rownum = 1;
			LinkedList<RowWarning> rowWarnings = new LinkedList<RowWarning>();

			foreach (Response r in src)
			{
				if (r.Successed)
					ok = true;

				if (!(r.RowWarnings is null))
				{
					foreach (RowWarning rw in r.RowWarnings.OrderBy(w => w.RowNumber))
					{
						wr = true;
						rowWarnings.AddLast(new RowWarning(rownum, rw.Message));
					}
				}

				rownum++;
			}

			return (new Response(ok, String.Format("{0}: {1}", title, ok ? (wr ? "Выполнено с ошибками" : "Выполнено") : "Не выполнено"), rowWarnings.ToArray()));
		}

		private Response DoLoadPrice(DataTable tbl, Core.Data.ConnectionMode mode, string key, string lang, string defaultLogin)
		{
			// Здесь надо как-то понять, кто залогинился: аптека или владелец. Если аптека - подсовываем её логин во все строки, где его нет
			/*if(defaultLogin == null)
				CheckRequiredFields(lang, tbl.Columns, PharmParserTableConvert.DrugstoreIdFieldName, PharmParserTableConvert.DrugstoreLoginFieldName);
			*/

			if((tbl.Rows is null))
				throw new Exception("FileIsEmpty");
            try
            {
				List<Core.Data.AssortmentItemData> Lass = PharmParserTableConvert.ToAssortmentItemData(tbl, defaultLogin, key).ToList();
				Dictionary<string, uint> pharms = null;// PharmApi.GetPharmMapping(key, Core.Data.ConnectionMode.Work);
				Dictionary<uint?, bool> apts = new Dictionary<uint?, bool>();
				foreach (var i in Lass)
				{
					if (!apts.ContainsKey(i.PharmId)) 
						apts.Add(i.PharmId, false);
					foreach (var j in pharms)
					{
						if (i.PharmId == j.Value)
						{
							apts[i.PharmId] = true;
						}
					}
				}
				if (!apts.ContainsValue(false))
				{
					Exception ex = Core.MongoDB.Methods.Ass_Insert.ButchInSert(Lass);
					if (!(ex is null))
					{
						foreach (var ass in Lass)
						{
							Core.MongoDB.Methods.Ass_Insert.InSertAsync(ass);
						}
					}
				//	PutItInUMKBPrice(tbl, mode, key, lang, defaultLogin);
					return new Response(true, "Загрузка ассортимента Выполнена");
			}
				else
			{
				return new Core.Pharm.Response(false, String.Format("Нет прав для загрузки ассортимента"));
			}
		}
			catch (Exception e) { return new Response(false, $"Загрузка ассортимента Не выполнена: {e.Message} "); }

			/*PharmApi phap = new PharmApi();
			return (JoinWithTitle(PharmParserTableConvert.ToPriceItemData(tbl, defaultLogin)
					.By(50)
					.Select(d => phap.LoadPrice(d.ToArray(), (Core.Data.ConnectionMode)mode, key))
					.Select(r => new Response(r.Successed, r.Message, (r.RowWarnings == null)
						? new RowWarning[] {}
						: r.RowWarnings
							.Select(rw => new RowWarning(rw.RowNumber, rw.Message))
							.ToArray())),
				"Загрузка прайс-листа"));*/
		}

		private Response DoLoadAssortment(DataTable tbl, Core.Data.ConnectionMode mode, string key, string lang, string defaultLogin)
		{
			/*if (defaultLogin == null)
				CheckRequiredFields(lang, tbl.Columns, PharmParserTableConvert.DrugstoreIdFieldName, PharmParserTableConvert.DrugstoreLoginFieldName);

			if (!tbl.Columns.Contains(PharmParserTableConvert.DrugIdFieldName))
			{
				CheckRequiredFields(lang, tbl.Columns, PharmParserTableConvert.DrugInternalIdFieldName);

				if (!tbl.Columns.Contains(PharmParserTableConvert.NameFieldName))
				{
					CheckRequiredFields(lang, tbl.Columns, PharmParserTableConvert.TradeNameFieldName);

					CheckRequiredFields(lang, tbl.Columns,
						new string[] { PharmParserTableConvert.CompositionFieldName },
						new string[] { PharmParserTableConvert.FormDosageFieldName, PharmParserTableConvert.PackFieldName },
						new string[] { PharmParserTableConvert.FormFieldName, PharmParserTableConvert.DosageFieldName, PharmParserTableConvert.PackFieldName });
				}

				CheckRequiredFields(lang, tbl.Columns,
					new string[] { PharmParserTableConvert.ManufacturerCountryFieldName },
					new string[] { PharmParserTableConvert.ManufacturerFieldName, PharmParserTableConvert.CountryFieldName });
			}*/

	//		CheckRequiredFields(lang, tbl.Columns, PharmParserTableConvert.QuantityFieldName);

			//if (tbl.Rows.IsNullOrEmpty())
			//	throw Errors.Create(ErrorMessages.FileIsEmpty, lang);
			PharmApi phap = new PharmApi();
			//pharmApi.LoadAssortment()
			UMKBRequests.GetUniversalModuleAnalyseScriptParse prs = new UMKBRequests.GetUniversalModuleAnalyseScriptParse();
			UMKBRequests.Models.API.Semantic.UMA.Request.RequestParseTable rpt = new UMKBRequests.Models.API.Semantic.UMA.Request.RequestParseTable();
			UMKBRequests.Models.API.Semantic.UMA.Result.ParseResult resl = new UMKBRequests.Models.API.Semantic.UMA.Result.ParseResult();
			UMKBRequests.Models.API.Semantic.UMA.Data.Table.Table tb = new UMKBRequests.Models.API.Semantic.UMA.Data.Table.Table();
			tb = UMKBRequests.Models.API.Semantic.UMA.Data.Table.Table.FromDataTable(tbl);
			rpt.Tables.Add(tb);
			rpt.Permit = new UMKBRequests.Models.API.Codes.Permit();
			rpt.Permit.authkey = UMKB.AbdParse;
			rpt.Permit.key = rpt.Permit.authkey;
			rpt.Id = Constants.ScriptId;
			resl=prs.Get(rpt);
			Dictionary<int,List<UMKBRequests.Models.API.Semantic.UMA.Result.ColumnData>> columns = new Dictionary<int,List<UMKBRequests.Models.API.Semantic.UMA.Result.ColumnData>>();
			columns = resl.ResultData.TablesParseResults[0].ColumnResults;
			List<string> drug_internal_id = new List<string>();
			List<UMKBRequests.Models.API.Semantic.UMA.Result.ColumnData>[] o = new List<UMKBRequests.Models.API.Semantic.UMA.Result.ColumnData>[columns.Count];
			int counter = 0;
			foreach (var i in columns)
            {
				o[counter] = i.Value;
				counter++;
            }
			foreach(var i in o)
            {
				if (i.Count>0) if(i[0].Code == "drug_internal_id")
					foreach (var j in i[0].FieldParseResult.OriginData) drug_internal_id.Add(j.Value);

			}
			try
			{
				List<Core.Data.AssortmentItemData> Lass = PharmParserTableConvert.ToAssortmentItemData(tbl, defaultLogin, key).ToList();
				Dictionary<string, uint> pharms = null;// PharmApi.GetPharmMapping(key, Core.Data.ConnectionMode.Work);
				Dictionary<uint?, bool> apts = new Dictionary<uint?, bool>();
				foreach (var i in Lass)
				{
					if (!apts.ContainsKey(i.PharmId))
						apts.Add(i.PharmId, false);
					foreach (var j in pharms)
					{
						if (i.PharmId == j.Value)
						{
							apts[i.PharmId] = true;
						}
					}
				}
				if (!apts.ContainsValue(false))
				{
					Exception ex = Core.MongoDB.Methods.Ass_Insert.ButchInSert(Lass);
					if (!(ex is null))
					{
						foreach (var ass in Lass)
						{
							Core.MongoDB.Methods.Ass_Insert.InSertAsync(ass);
						}
					}
					//PutItInUMKB(tbl, mode, key, lang, defaultLogin);
					PPIInAss(Lass);
					return new Response(true, "Загрузка ассортимента Выполнена");
				}
				else
				{
					return new Core.Pharm.Response(false, String.Format("Нет прав для загрузки ассортимента"));
				}
			}
            catch (Exception e) { return new Response(false,$"Загрузка ассортимента Не выполнена: {e.Message} "); }
			
		}
		
		public static AUpload.Global.Models.Response APILoadAss(List<AUpload.Global.Models.AssortmentItemData> Lass, string authkey, string key=null)
        {
			try
			{
			//	string xz = "{     \"key\": \"kX0wz4bG67PPxwsLcqyo72Gg\",     \"Lass\": [         {             \"PharmId\": 392215,             \"pharm_login\": \"392215\",                         \"pharm_pack_id\": \"348\",             \"barcode\": \"4030685259512\",             \"name\": \"Амарил, тбл 4мг №30\",             \"trade_name\": \"Амарил, тбл 4мг №30\",             \"dosage_value\": 3,             \"dosage_id\": 248,                                     \"pack_s\": \"12\",             \"manufacturer\": \"Aventis Pharma Deutschland GmbH\",             \"country\": \"Германия\",             \"pharmaco_therapeutic_group\": \"Лекарственные средства\",             \"qty\": 15,             \"qty_free\": 8,             \"price\": 1300.5,             \"selling_price_type\": \"1\",                      }     ] }";
			//	APIRequestAss xxx= Newtonsoft.Json.JsonConvert.DeserializeObject<APIRequestAss>(xz);
				AUpload.Global.Models.Response resp = PharmTaxiAdm.Methods.Assortiment.Load.Assortiment(Lass, authkey, key);
				//List<Core.Data.AssortmentItemData> Lass = PharmParserTableConvert.ToAssortmentItemData(tbl, defaultLogin, key).ToList();
				//Exception ex = Core.MongoDB.Methods.Ass_Insert.ButchInSert(Lass);
				//if (!(ex is null))
				//{
				//	foreach (var ass in Lass)
				//	{

				//		Core.MongoDB.Methods.Ass_Insert.InSertAsync(ass);
				//	}
				//}
				//APIPutItInUMKB(Lass, key);
				return resp;// new Response(true, "Загрузка ассортимента Выполнена");
			}
			catch (Exception e) { return new AUpload.Global.Models.Response() { level = "success", message = $"Загрузка ассортимента Не выполнена: {e.Message} ", code = "200" }; };
		}

		//public static Response APILoadPrice(List<Core.Data.PriceItemData> Pass, string key)
  //      {
		//	try
		//	{
		//		List<Core.Data.AssortmentItemData> Lass = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Core.Data.AssortmentItemData>>(Newtonsoft.Json.JsonConvert.SerializeObject(Pass));
		//		Exception ex = Core.MongoDB.Methods.Ass_Insert.ButchInSert(Lass);
		//		if (!(ex is null))
		//		{
		//			foreach (var ass in Lass)
		//			{
		//				Core.MongoDB.Methods.Ass_Insert.InSertAsync(ass);
		//			}
		//		}
		//		//APIPutItInUMKBPrice(Pass, key);
		//		return new Response(true, "Загрузка ассортимента Выполнена");
		//	}
		//	catch (Exception e) { return new Response(false, $"Загрузка ассортимента Не выполнена: {e.Message} "); }

		//}

		//static async Task<Response> PutItInUMKB(DataTable tbl, Core.Data.ConnectionMode mode, string key, string lang, string defaultLogin)
  //      {
		//	//PharmApi phap = new PharmApi();
		//	//return await Task.Run(() => (JoinWithTitle(PharmParserTableConvert.ToAssortmentItemData(tbl, defaultLogin)
		//	//		.By(50)
		//	//		.Select(d => phap.LoadAssortment(d, (Core.Data.ConnectionMode)mode, key))
		//	//		.Select(r => new Response(r.Successed, r.Message, r.RowWarnings
		//	//			.Select(rw => new RowWarning(rw.RowNumber, rw.Message))
		//	//			.ToArray())),
		//	//	"Загрузка ассортимента")));
  //      }

		static async Task<Response> PPIInAss(List<Core.Data.AssortmentItemData> Lass)
        {
			return await Task.Run(() => PutPackIDInAss(Lass));
		}

		static async Task<Response> PutPackIDInAss(List<Core.Data.AssortmentItemData> Lass)
        {
		//	SMBarcode.Configuration.Setup(PharmTaxiAPI.Constants.Consts.SQLConnectionString, PharmTaxiAPI.Constants.Consts.TableGetKey, PharmTaxiAPI.Constants.Consts.GraphKey, PharmTaxiAPI.Constants.Consts.ConceptKey, new SMBarcode.Configuration.SetupBase(), Crypto, PharmTaxiAdm.Constants.Keys.GraphListKey, PharmTaxiAdm.Constants.Keys.ConceptListKey);
			//SMBarcode.Configuration.getNames = PharmTaxiAPI.Constants.Consts.NamesKey;
			
			foreach (var dt in Lass)
			{
				SMBarcode.Drug drug = new SMBarcode.Drug();
				uint? findres = null;
				PharmApi phap = new PharmApi();
				//findres = phap.GetFinder(Data.ConnectionMode.Work).GetFpIdByPharmPositionData(dt, true);
				if (findres != null)
					dt.PackId = findres;
                else
                {
					if (dt.TradeName != "" && dt.Manufacturer != "")
					{
						drug.gtin = dt.Barcode;
						drug.torgName = dt.TradeName;
						drug.dosage = dt.Dosage;
						drug.qty = dt.Quantity.ToString();
						drug.manuf = dt.Manufacturer;
						drug.GetMasterIdLib67(ref drug);
						try
						{
							if (drug.fpIdShort is null || drug.fpIdShort == "0") drug.fpIdShort = SMBarcode.UMKBGetInfo.GetMasterIDByGTIN(drug.gtin);
						}
						catch 
						{
							try
							{
								if (drug.fpIdShort is null || drug.fpIdShort == "0") drug.fpIdShort = SMBarcode.UMKBGetInfo.GetMasterIDByGTIN73(drug.gtin);
							}
							catch { }
						}
					}
				}
				AssItemData ass = Newtonsoft.Json.JsonConvert.DeserializeObject<AssItemData>(Newtonsoft.Json.JsonConvert.SerializeObject(dt));
				Core.MongoDB.Methods.Ass_upsert.UpdatePharmID(ass);
			}

			return new Response(true,"Ok");
		}

		//static async Task<Response> APIPutItInUMKB(List<Core.Data.AssortmentItemData> Lass, string key, Core.Data.ConnectionMode mode=Data.ConnectionMode.Work, string lang="ru")
		//{
		//	PharmApi phap = new PharmApi();
		//	return await Task.Run(() => (JoinWithTitle(Lass
		//			.By(50)
		//			.Select(d => phap.LoadAssortment(d, (Core.Data.ConnectionMode)mode, key))
		//			.Select(r => new Response(r.Successed, r.Message, r.RowWarnings
		//				.Select(rw => new RowWarning(rw.RowNumber, rw.Message))
		//				.ToArray())),
		//		"Загрузка ассортимента")));
		//}

		//static async Task<Response> APIPutItInUMKBPrice(List<Core.Data.PriceItemData> Lass, string key, Core.Data.ConnectionMode mode=Data.ConnectionMode.Work, string lang="ru")
		//{
		//	PharmApi phap = new PharmApi();
		//	return await Task.Run(() => (JoinWithTitle(Lass
		//			.By(50)
		//			.Select(d => phap.LoadPrice(d.ToArray(), (Core.Data.ConnectionMode)mode, key))
		//			.Select(r => new Response(r.Successed, r.Message, (r.RowWarnings == null)
		//				? new RowWarning[] { }
		//				: r.RowWarnings
		//					.Select(rw => new RowWarning(rw.RowNumber, rw.Message))
		//					.ToArray())),
		//		"Загрузка прайс-листа")));
		//}



		//static async Task<Response> PutItInUMKBPrice(DataTable tbl, Core.Data.ConnectionMode mode, string key, string lang, string defaultLogin)
		//{
		//	PharmApi phap = new PharmApi();
		//	return await Task.Run(() => (JoinWithTitle(PharmParserTableConvert.ToPriceItemData(tbl, defaultLogin)
		//			.By(50)
		//			.Select(d => phap.LoadPrice(d.ToArray(), (Core.Data.ConnectionMode)mode, key))
		//			.Select(r => new Response(r.Successed, r.Message, (r.RowWarnings == null)
		//				? new RowWarning[] { }
		//				: r.RowWarnings
		//					.Select(rw => new RowWarning(rw.RowNumber, rw.Message))
		//					.ToArray())),
		//		"Загрузка прайс-листа")));
		//}


			private Response DoChangePrice(DataTable tbl, Core.Data.ConnectionMode mode, string key, string lang, string defaultLogin)
		{
			if (defaultLogin == null)
				CheckRequiredFields(lang, tbl.Columns, PharmParserTableConvert.DrugstoreIdFieldName, PharmParserTableConvert.DrugstoreLoginFieldName);

			CheckRequiredFields(lang, tbl.Columns, PharmParserTableConvert.DrugIdFieldName, PharmParserTableConvert.DrugInternalIdFieldName);
			CheckRequiredFields(lang, tbl.Columns, PharmParserTableConvert.SellingPriceFieldName);
			CheckRequiredFields(lang, tbl.Columns, PharmParserTableConvert.QuantityFieldName);

			//if (tbl.Rows.IsNullOrEmpty())
			//	throw Errors.Create(ErrorMessages.FileIsEmpty, lang);

			return (null);

			//return (JoinWithTitle(PharmParserTableConvert.ToModifyPriceItemData(tbl, defaultLogin)
			//		.By(100)
			//		.Select(d => _client.ChangePrice(d.ToArray(), (Core.PharmService.ConnectionMode)mode, key))
			//		.Select(r => new Response(r.Successed, r.Message, r.RowWarnings
			//			.Select(rw => new RowWarning(rw.RowNumber, rw.Message))
			//			.ToArray())),
			//	"Изменение цен и остатков"));
		}

		private Response DoRemoveFromPrice(DataTable tbl, Core.Data.ConnectionMode mode, string key, string lang, string defaultLogin)
		{
			//if (defaultLogin == null)
			//	CheckRequiredFields(lang, tbl.Columns, PharmParserTableConvert.DrugstoreIdFieldName, PharmParserTableConvert.DrugstoreLoginFieldName);

			//CheckRequiredFields(lang, tbl.Columns, PharmParserTableConvert.DrugIdFieldName, PharmParserTableConvert.DrugInternalIdFieldName);

			//if (tbl.Rows.IsNullOrEmpty())
			//	throw Errors.Create(ErrorMessages.FileIsEmpty, lang);

			return (null);//JoinWithTitle(PharmParserTableConvert.ToRemovePriceItemData(tbl, defaultLogin)
				//	.By(100)
				//	.Select(d => .RemoveFromPrice(d.ToArray(), (Core.PharmService.ConnectionMode)mode, key))
				//	.Select(r => new Response(r.Successed, r.Message, r.RowWarnings
				//		.Select(rw => new RowWarning(rw.RowNumber, rw.Message))
				//		.ToArray())),
				//"Удаление позиций из прайс-листа"));
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool clearAll)
		{
			//_client.Dispose();
		}

		private static void CheckRequiredFields(string lang, DataColumnCollection columns, string required)
		{
			//if (!columns.Contains(required))
			//	throw Errors.Create(ErrorMessages.MissingRequiredColumn_template_1, lang, required);
		}

		private static void CheckRequiredFields(string lang, DataColumnCollection columns, string required1, string required2)
		{
			//if (!(columns.Contains(required1) || columns.Contains(required2)))
			//	throw Errors.Create(ErrorMessages.MissingRequired2Columns_template_2, lang, required1, required2);
		}

		private static void CheckRequiredFields(string lang, DataColumnCollection columns, params string[][] required)
		{
			//if (!required.Any(g => g.Any(c => columns.Contains(c))))
			//	throw Errors.Create(ErrorMessages.MissingRequiredColumns_template_1, lang, String.Join(" ИЛИ ", required
			//		.Select(g => String.Join(", ", g
			//			.Select(f => String.Format("\"{0}\"", f))))));
		}
	}
}
