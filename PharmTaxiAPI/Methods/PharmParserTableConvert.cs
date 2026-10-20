//using Core.Finders.Custom;
//using Core.PharmService;
using OL.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using PharmTaxiAPI.Models;
using PTConstants;

namespace Core.Pharm
{
	public static class PharmParserTableConvert
	{
		public const string DrugstoreIdFieldName = "drugstore_id";
		public const string DrugstoreLoginFieldName = "drugstore_login";
		public const string DrugIdFieldName = "drug_id";
		public const string DrugInternalIdFieldName = "drug_internal_id";

		public const string BarcodeFieldName = "barcode";
		public const string NameFieldName = "name";
		public const string TradeNameFieldName = "trade_name";
		public const string CompositionFieldName = "composition";
		public const string FormDosageFieldName = "form_dosage";
		public const string FormFieldName = "form";
		public const string DosageFieldName = "dosage";
		public const string PackFieldName = "pack";
		public const string ManufacturerCountryFieldName = "manufacturer_country";
		public const string ManufacturerFieldName = "manufacturer";
		public const string CountryFieldName = "country";

		public const string SellingPriceFieldName = "selling_price";
		public const string SellingPriceTypeFieldName = "selling_price_type";
		public const string DiscountFieldName = "discount";

		public const string RegistrationCertificateFieldName = "registration_sertificate";
		public const string RegistrationDateFieldName = "registration_date";
		public const string RegistrationEndDateFieldName = "registration_end_date";
		public const string RegistrationCancellationDateFieldName = "registration_cancellation_date";
		public const string RegistrationOwnerFieldName = "registration_owner";
		public const string ActiveSubstanceFieldName = "active_substance";
		public const string ProductionStagesFieldName = "production_stages";
		public const string PharmacoTherapeuticGroupFieldName = "ptg";
		public const string ConsignmentNumberFieldName = "consignment_number";
		public const string BestBeforeFieldName = "best_before";
		public const string DeliveryDateFieldName = "delivery_date";
		public const string QuantityFieldName = "quantity";
		public const string PurchasePriceFieldName = "purchase_price";

		public const string CommentFieldName = "comment";


		public static IEnumerable<Core.Data.PriceItemData> ToPriceItemData(DataTable tbl, string defaultLogin)
		{
			DataTableColumnsIndex index = tbl.BuildColumnsIndex();
			try
			{
				return AbdulToPrice(tbl, defaultLogin);
			}
			catch
			{
				foreach (DataRow r in tbl.Rows)
				{
					uint? phid = (uint)index.TryGetValue(r, DrugstoreIdFieldName);
					string phlogin = (string)index.TryGetValue(r, DrugstoreLoginFieldName);
					decimal? vl = (decimal)index.TryGetValue(r, SellingPriceFieldName);

					if (String.IsNullOrEmpty(phlogin))
						phlogin = defaultLogin;

					// Пропускаем всякий мусор
					if (vl == null)
						continue;
					Core.Data.PriceItemData ret = (new Core.Data.PriceItemData()
					{
					//yield return (new PriceItemData()
					//{
						SellingPrice = vl.Value,
						SellingPriceType = (String)index.TryGetValue(r, SellingPriceTypeFieldName),
						Discount = (float)index.TryGetValue(r, DiscountFieldName),
						Comment = (string)index.TryGetValue(r, CommentFieldName),

						Barcode = (string)index.TryGetValue(r, BarcodeFieldName),
						Name = (string)index.TryGetValue(r, NameFieldName),
						TradeName = (string)index.TryGetValue(r, TradeNameFieldName),
						Composition = (string)index.TryGetValue(r, CompositionFieldName),
						FormDosage = (string)index.TryGetValue(r, FormDosageFieldName),
						Form = (string)index.TryGetValue(r, FormFieldName),
						Dosage = (string)index.TryGetValue(r, DosageFieldName),
						Pack = (string)index.TryGetValue(r, PackFieldName),
						ManufacturerCountry = (string)index.TryGetValue(r, ManufacturerCountryFieldName),
						Manufacturer = (string)index.TryGetValue(r, ManufacturerFieldName),
						Country = (string)index.TryGetValue(r, CountryFieldName),

						PharmId = phid,
						PharmLogin = phlogin,
						PackId = (uint)index.TryGetValue(r, DrugIdFieldName),
						PharmPackId = (string)index.TryGetValue(r, DrugInternalIdFieldName)
					});
					List<Core.Data.PriceItemData> retlist = new List<Core.Data.PriceItemData>();
					retlist.Add(ret);
					return retlist;
				}
			}
			return null;
		}

		public static IEnumerable<Core.Data.AssortmentItemData> AbdulToAss(DataTable tbl, string defaultLogin)
        {
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
			resl = prs.Get(rpt);

			Dictionary<int, List<UMKBRequests.Models.API.Semantic.UMA.Result.ColumnData>> columns = new Dictionary<int, List<UMKBRequests.Models.API.Semantic.UMA.Result.ColumnData>>();
			columns = resl.ResultData.TablesParseResults[0].ColumnResults;

			List<string> drug_internal_id = new List<string>();
			List<string> drug_id = new List<string>();
			List<string> drugstore_login = new List<string>();
			List<string> barcode = new List<string>();
			List<string> name = new List<string>();
			List<string> trade_name = new List<string>();
			List<string> composition = new List<string>();
			List<string> form_dosage = new List<string>();
			List<string> form = new List<string>();
			List<string> dosage = new List<string>();
			List<string> pack = new List<string>();
			List<string> manufacturer_country = new List<string>();
			List<string> manufacturer = new List<string>();
			List<string> quantity = new List<string>();
			List<string> country = new List<string>();
			List<string> type_product = new List<string>();
			List<string> comment = new List<string>();
			List<string> selling_price = new List<string>();
			List<string> selling_price_type = new List<string>();
			List<string> discount = new List<string>();

			List<UMKBRequests.Models.API.Semantic.UMA.Result.ColumnData>[] o = new List<UMKBRequests.Models.API.Semantic.UMA.Result.ColumnData>[columns.Count];
			int counter = 0;
			foreach (var i in columns)
			{
				o[counter] = i.Value;
				counter++;
			}
			foreach (var i in o)
			{
				if (i.Count > 0) if (i[0].Code == "drug_internal_id")
						foreach (var j in i[0].FieldParseResult.OriginData) drug_internal_id.Add(j.Value);
				//
				// Вот этот кусок должен был заполнять drug_id idшником ТНа(?!!), чтобы потом быть заменённым idшником ФП((?!!)x2). Не оптимально. 
				// Оптимизировать. А пока должно работать и так.
				//
				/*if (i.Count > 0) if (i[0].Code == "trade_name")
						foreach (var j in i[0].FieldParseResult.LibParseData)
						{
							if (j.Count > 0) foreach (var l in j)
									if (l.Value.Count > 0) foreach (var k in l.Value) drug_id.Add(k.id.ToString());
									else drug_id.Add("0");
						}*/

				if (i.Count > 0) if (i[0].Code == "drugstore_login")
						foreach (var j in i[0].FieldParseResult.OriginData) drugstore_login.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "barcode")
						foreach (var j in i[0].FieldParseResult.OriginData) barcode.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "name")
						foreach (var j in i[0].FieldParseResult.OriginData) name.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "trade_name")
						foreach (var j in i[0].FieldParseResult.OriginData) trade_name.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "composition")
						foreach (var j in i[0].FieldParseResult.OriginData) composition.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "form_dosage")
						foreach (var j in i[0].FieldParseResult.OriginData) form_dosage.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "form")
						foreach (var j in i[0].FieldParseResult.OriginData) form.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "dosage")
						foreach (var j in i[0].FieldParseResult.OriginData) dosage.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "pack")
						foreach (var j in i[0].FieldParseResult.OriginData) pack.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "manufacturer_country")
						foreach (var j in i[0].FieldParseResult.OriginData) manufacturer_country.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "manufacturer")
						foreach (var j in i[0].FieldParseResult.OriginData) manufacturer.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "quantity")
						foreach (var j in i[0].FieldParseResult.OriginData) quantity.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "country")
						foreach (var j in i[0].FieldParseResult.OriginData) country.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "type_product")
						foreach (var j in i[0].FieldParseResult.OriginData) type_product.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "comment")
						foreach (var j in i[0].FieldParseResult.OriginData) comment.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "selling_price_type")
						foreach (var j in i[0].FieldParseResult.OriginData) selling_price_type.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "discount")
						foreach (var j in i[0].FieldParseResult.OriginData) discount.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "selling_price")
						foreach (var j in i[0].FieldParseResult.OriginData) selling_price.Add(j.Value);
			}
			List<Core.Data.AssortmentItemData> retlist = new List<Data.AssortmentItemData>();
			for (int i = 0; i < drugstore_login.Count; i++)
			{

				//SMBarcode.Configuration.Setup(PharmTaxiAPI.Constants.Consts.SQLConnectionString, PharmTaxiAPI.Constants.Consts.TableGetKey, PharmTaxiAPI.Constants.Consts.GraphKey, PharmTaxiAPI.Constants.Consts.ConceptKey, new SMBarcode.Configuration.SetupBase(), Crypto, PharmTaxiAdm.Constants.Keys.GraphListKey, PharmTaxiAdm.Constants.Keys.ConceptListKey);
			//	SMBarcode.Configuration.getNames = PharmTaxiAPI.Constants.Consts.NamesKey;
				SMBarcode.Drug drug = new SMBarcode.Drug();

				/*if (trade_name.Count > 0 && manufacturer.Count > 0)
				{
					if (barcode.Count > 0) drug.GTIN = barcode[i];
					if (trade_name.Count > 0) drug.TorgName = trade_name[i];
					if (dosage.Count > 0) drug.Dosage = dosage[i];
					if (pack.Count > 0) drug.Quantity = pack[i];
					if (manufacturer.Count > 0) drug.Manuf = manufacturer[i];
					drug.GetMasterIdLib67(ref drug);
					try
					{
						if (drug.master_id_lib67 is null || drug.master_id_lib67 == "0") drug.master_id_lib67 = SMBarcode.UMKBGetInfo.GetMasterIDByGTIN(drug.GTIN);
					}
					catch (Exception e)
					{
						try
						{
							if (drug.master_id_lib67 is null || drug.master_id_lib67 == "0") drug.master_id_lib67 = SMBarcode.UMKBGetInfo.GetMasterIDByGTIN73(drug.GTIN);
						}
						catch (Exception e2) { }
					}
				}*/
				/*if (barcode.Count > 0) drug.GTIN = barcode[i];
				try
				{
					if (drug.master_id_lib67 is null || drug.master_id_lib67 == "0") drug.master_id_lib67 = SMBarcode.UMKBGetInfo.GetMasterIDByGTIN(drug.GTIN);
				}
				catch (Exception e)
				{
					try
					{
						if (drug.master_id_lib67 is null || drug.master_id_lib67 == "0") drug.master_id_lib67 = SMBarcode.UMKBGetInfo.GetMasterIDByGTIN73(drug.GTIN);
					}
					catch (Exception e2) { }
				}*/
				/*	if(drug.master_id_lib67=="0")
					{
						FpFinder fpFinder = new FpFinder(Data.ConnectionMode.Work);
						Data.PharmPositionData dt = new Data.PharmPositionData();
						dt.Barcode = barcode[i];
						dt.Manufacturer = manufacturer[i];
						dt.TradeName = trade_name[i];
						dt.Dosage = dosage[i];
						dt.ManufacturerCountry = manufacturer_country[i];
						dt.Country = country[i];
					//FpFinderResult ffr=new FpFinderResult()
						FpFinderResult fpFinderResult = fpFinder.GetFpIdByPharmPositionData(dt, true);
					}*/

				Core.Data.AssortmentItemData rt = new Data.AssortmentItemData();
				rt.PharmPackId = drug_internal_id[i];
				rt.PharmId = (uint.TryParse(drugstore_login[i], out uint x) ? x : 0);
				if (drug_id.Count > 0) rt.PackId = (uint.TryParse(drug.fpIdShort, out uint y) ? y : 0); 
				if (barcode.Count > 0) rt.Barcode = barcode[i];
				if (name.Count > 0) rt.Name = name[i];
				if (trade_name.Count > 0) rt.TradeName = trade_name[i];
				if (composition.Count > 0) rt.Composition = composition[i];
				if (form_dosage.Count > 0) rt.FormDosage = form_dosage[i];
				if (form.Count > 0) rt.Form = form[i];
				if (dosage.Count > 0) rt.Dosage = dosage[i];
				if (pack.Count > 0) rt.Pack = pack[i];
				rt.PharmLogin = defaultLogin;
				if (quantity.Count > 0) rt.Quantity = (uint.TryParse(quantity[i], out uint y) ? y : 0);
				if (country.Count > 0) rt.Country = country[i];
				if (type_product.Count > 0) rt.PharmacoTherapeuticGroup = type_product[i];
				if (comment.Count > 0) rt.Comment = comment[i];
				if (manufacturer_country.Count > 0) rt.ManufacturerCountry = manufacturer_country[i];
				if (manufacturer.Count > 0) rt.Manufacturer = manufacturer[i];
				if (selling_price.Count > 0) rt.SellingPrice = (decimal.TryParse(selling_price[i], out decimal y) ? y : 0);
				if (selling_price_type.Count > 0) rt.SellingPriceType = selling_price_type[i];
				if (discount.Count > 0) rt.Discount = (float.TryParse(discount[i], out float y) ? y : 0);
				retlist.Add(rt);
			}
		/*	string[] first = new string[tbl.Rows.Count];
			string[] second = new string[tbl.Rows.Count];
			for (int cnt = 0; cnt < tbl.Rows.Count; cnt++)
			{
				first[cnt] = tbl.Rows[cnt].ItemArray.GetValue(1).ToString();
				second[cnt] = !(retlist[cnt].PharmId is null) ? retlist[cnt].PharmPackId : null; //tbl.Rows[cnt].ItemArray.GetValue(1).ToString();
			}
			var dif = first.Except(second);*/
			return retlist;
		}

		public static IEnumerable<Core.Data.PriceItemData> AbdulToPrice(DataTable tbl, string defaultLogin)
		{
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
			resl = prs.Get(rpt);

			Dictionary<int, List<UMKBRequests.Models.API.Semantic.UMA.Result.ColumnData>> columns = new Dictionary<int, List<UMKBRequests.Models.API.Semantic.UMA.Result.ColumnData>>();
			columns = resl.ResultData.TablesParseResults[0].ColumnResults;

			List<string> drug_internal_id = new List<string>();
			List<string> drug_id = new List<string>();
			List<string> drugstore_login = new List<string>();
			List<string> barcode = new List<string>();
			List<string> name = new List<string>();
			List<string> trade_name = new List<string>();
			List<string> composition = new List<string>();
			List<string> form_dosage = new List<string>();
			List<string> form = new List<string>();
			List<string> dosage = new List<string>();
			List<string> pack = new List<string>();
			List<string> manufacturer_country = new List<string>();
			List<string> manufacturer = new List<string>();
			List<string> selling_price = new List<string>();
			List<string> country = new List<string>();
			List<string> selling_price_type = new List<string>();
			List<string> discount = new List<string>();
			List<string> comment = new List<string>();

			List<UMKBRequests.Models.API.Semantic.UMA.Result.ColumnData>[] o = new List<UMKBRequests.Models.API.Semantic.UMA.Result.ColumnData>[columns.Count];
			int counter = 0;
			foreach (var i in columns)
			{
				o[counter] = i.Value;
				counter++;
			}
			foreach (var i in o)
			{
				if (i.Count > 0) if (i[0].Code == "drug_internal_id")
						foreach (var j in i[0].FieldParseResult.OriginData) drug_internal_id.Add(j.Value);
				//Вот этот кусок должен был заполнять drug_id idшником ТНа(?!!), чтобы потом быть заменённым idшником ФП((?!!)x2). Не оптимально. 
				//Оптимизировать. А пока должно работать и так.
				/*
				if (i.Count > 0) if (i[0].Code == "trade_name")
						foreach (var j in i[0].FieldParseResult.LibParseData)
						{
							if (j.Count > 0) foreach (var l in j)
									if (l.Value.Count > 0) foreach (var k in l.Value) drug_id.Add(k.id.ToString());
									else drug_id.Add("0");
						}*/
				if (i.Count > 0) if (i[0].Code == "drugstore_login")
						foreach (var j in i[0].FieldParseResult.OriginData) drugstore_login.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "barcode")
						foreach (var j in i[0].FieldParseResult.OriginData) barcode.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "name")
						foreach (var j in i[0].FieldParseResult.OriginData) name.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "trade_name")
						foreach (var j in i[0].FieldParseResult.OriginData) trade_name.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "composition")
						foreach (var j in i[0].FieldParseResult.OriginData) composition.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "form_dosage")
						foreach (var j in i[0].FieldParseResult.OriginData) form_dosage.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "form")
						foreach (var j in i[0].FieldParseResult.OriginData) form.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "dosage")
						foreach (var j in i[0].FieldParseResult.OriginData) dosage.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "pack")
						foreach (var j in i[0].FieldParseResult.OriginData) pack.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "manufacturer_country")
						foreach (var j in i[0].FieldParseResult.OriginData) manufacturer_country.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "manufacturer")
						foreach (var j in i[0].FieldParseResult.OriginData) manufacturer.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "selling_price")
						foreach (var j in i[0].FieldParseResult.OriginData) selling_price.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "country")
						foreach (var j in i[0].FieldParseResult.OriginData) country.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "comment")
						foreach (var j in i[0].FieldParseResult.OriginData) comment.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "selling_price_type")
						foreach (var j in i[0].FieldParseResult.OriginData) selling_price_type.Add(j.Value);
				if (i.Count > 0) if (i[0].Code == "discount")
						foreach (var j in i[0].FieldParseResult.OriginData) discount.Add(j.Value);
			}
			List<Core.Data.PriceItemData> retlist = new List<Core.Data.PriceItemData>();
			for (int i = 0; i < drugstore_login.Count; i++)
			{
				//SMBarcode.Configuration.Setup(PharmTaxiAPI.Constants.Consts.SQLConnectionString, PharmTaxiAPI.Constants.Consts.TableGetKey, PharmTaxiAPI.Constants.Consts.GraphKey, PharmTaxiAPI.Constants.Consts.ConceptKey, new SMBarcode.Configuration.SetupBase(), Crypto, PharmTaxiAdm.Constants.Keys.GraphListKey, PharmTaxiAdm.Constants.Keys.ConceptListKey);
				//SMBarcode.Configuration.getNames = PharmTaxiAPI.Constants.Consts.NamesKey;
				SMBarcode.Drug drug = new SMBarcode.Drug();

				/*if (trade_name.Count > 0 && manufacturer.Count > 0)
				{
					if (barcode.Count > 0) drug.GTIN = barcode[i];
					if (trade_name.Count>0)drug.TorgName = trade_name[i];
					if (dosage.Count > 0) drug.Dosage = dosage[i];
					if (pack.Count > 0) drug.Quantity = pack[i];
					if (manufacturer.Count > 0) drug.Manuf = manufacturer[i];
					drug.GetMasterIdLib67(ref drug);
					try
					{
						if (drug.master_id_lib67 is null || drug.master_id_lib67 == "0") drug.master_id_lib67 = SMBarcode.UMKBGetInfo.GetMasterIDByGTIN(drug.GTIN);
					}
					catch (Exception e)
					{
						try
						{
							if (drug.master_id_lib67 is null || drug.master_id_lib67 == "0") drug.master_id_lib67 = SMBarcode.UMKBGetInfo.GetMasterIDByGTIN73(drug.GTIN);
						}
						catch (Exception e2) { }
					}
				}*/
				/*	if (barcode.Count > 0) drug.GTIN = barcode[i];
					try
					{
						if (drug.master_id_lib67 is null || drug.master_id_lib67 == "0") drug.master_id_lib67 = SMBarcode.UMKBGetInfo.GetMasterIDByGTIN(drug.GTIN);
					}
					catch (Exception e)
					{
						try
						{
							if (drug.master_id_lib67 is null || drug.master_id_lib67 == "0") drug.master_id_lib67 = SMBarcode.UMKBGetInfo.GetMasterIDByGTIN73(drug.GTIN);
						}
						catch (Exception e2) { }
					}*/
				/*	if (drug.master_id_lib67 == "0")
					{
						FpFinder fpFinder = new FpFinder(Data.ConnectionMode.Work);
						Data.PharmPositionData dt = new Data.PharmPositionData();
						dt.Barcode = barcode[i];
						dt.Manufacturer = manufacturer[i];
						dt.TradeName = trade_name[i];
						dt.Dosage = dosage[i];
						dt.ManufacturerCountry = manufacturer_country[i];
						dt.Country = country[i];
						FpFinderResult fpFinderResult = fpFinder.GetFpIdByPharmPositionData(dt, true);
					}*/

				Core.Data.PriceItemData rt = new Core.Data.PriceItemData();
				rt.PharmPackId = drug_internal_id[i];
				rt.PharmId = (uint.TryParse(drugstore_login[i], out uint x) ? x : 0);
				if (drug_id.Count > 0) rt.PackId = (uint.TryParse(drug.fpIdShort, out uint y) ? y : 0); 
				if (barcode.Count > 0) rt.Barcode = barcode[i];
				if (name.Count > 0) rt.Name = name[i];
				if (trade_name.Count > 0) rt.TradeName = trade_name[i];
				if (composition.Count > 0) rt.Composition = composition[i];
				if (form_dosage.Count > 0) rt.FormDosage = form_dosage[i];
				if (form.Count > 0) rt.Form = form[i];
				if (dosage.Count > 0) rt.Dosage = dosage[i];
				if (pack.Count > 0) rt.Pack = pack[i];
				rt.PharmLogin = defaultLogin;
				if (selling_price.Count > 0) rt.SellingPrice = (decimal.TryParse(selling_price[i], out decimal y) ? y : 0);
				if (country.Count > 0) rt.Country = country[i];
				if (selling_price_type.Count > 0) rt.SellingPriceType = selling_price_type[i];
				if (discount.Count > 0) rt.Discount = (float.TryParse(discount[i], out float y) ? y : 0);
				if (comment.Count > 0) rt.Comment = comment[i];
				if (manufacturer_country.Count > 0) rt.ManufacturerCountry = manufacturer_country[i];
				if (manufacturer.Count > 0) rt.Manufacturer = manufacturer[i];
				retlist.Add(rt);
			}
			/*string[] first = new string[tbl.Rows.Count];
			string[] second = new string[tbl.Rows.Count];
			for(int cnt = 0;cnt< tbl.Rows.Count;cnt++)
            {
				first[cnt] = tbl.Rows[cnt].ItemArray.GetValue(1).ToString();
				second[cnt] = !(retlist[cnt].PharmId is null)?retlist[cnt].PharmPackId:null; //tbl.Rows[cnt].ItemArray.GetValue(1).ToString();
			}
			var dif = first.Except(second);*/
			return retlist;
		}


		public static IEnumerable<Core.Data.AssortmentItemData> ToAssortmentItemData(DataTable tbl, string defaultLogin)
		{
			DataTableColumnsIndex index = tbl.BuildColumnsIndex();
			try
			{
				return AbdulToAss(tbl, defaultLogin);
			}
			catch
            {
				foreach (DataRow r in tbl.Rows)
				{
					uint? phid = (uint)index.TryGetValue(r, DrugstoreIdFieldName);
					string phlogin = (string)index.TryGetValue(r, DrugstoreLoginFieldName);
					long? vl = (long)index.TryGetValue(r, QuantityFieldName);

					if (String.IsNullOrEmpty(phlogin))
						phlogin = defaultLogin;

					// Пропускаем всякий мусор
					if (vl == null)
						continue;

					Core.Data.AssortmentItemData ret= (new Core.Data.AssortmentItemData()
					{
						RegistrationCertificate = (string)index.TryGetValue(r, RegistrationCertificateFieldName),
						RegistrationDate = (DateTime)index.TryGetValue(r, RegistrationDateFieldName),
						RegistrationEndDate = (DateTime)index.TryGetValue(r, RegistrationEndDateFieldName),
						RegistrationCancellationDate = (DateTime)index.TryGetValue(r, RegistrationCancellationDateFieldName),
						RegistrationOwner = (string)index.TryGetValue(r, RegistrationOwnerFieldName),
						ActiveSubstance = (string)index.TryGetValue(r, ActiveSubstanceFieldName),
						ProductionStages = (string)index.TryGetValue(r, ProductionStagesFieldName),
						PharmacoTherapeuticGroup = (string)index.TryGetValue(r, PharmacoTherapeuticGroupFieldName),
						ConsignmentNumber = (string)index.TryGetValue(r, ConsignmentNumberFieldName),
						BestBefore = (DateTime)index.TryGetValue(r, BestBeforeFieldName),
						DeliveryDate = (DateTime)index.TryGetValue(r, DeliveryDateFieldName),
						Quantity = vl.Value,
						PurchasePrice = (decimal)index.TryGetValue(r, PurchasePriceFieldName),
						Comment = (string)index.TryGetValue(r, CommentFieldName),

						Barcode = (string)index.TryGetValue(r, BarcodeFieldName),
						Name = (string)index.TryGetValue(r, NameFieldName),
						TradeName = (string)index.TryGetValue(r, TradeNameFieldName),
						Composition = (string)index.TryGetValue(r, CompositionFieldName),
						FormDosage = (string)index.TryGetValue(r, FormDosageFieldName),
						Form = (string)index.TryGetValue(r, FormFieldName),
						Dosage = (string)index.TryGetValue(r, DosageFieldName),
						Pack = (string)index.TryGetValue(r, PackFieldName),
						ManufacturerCountry = (string)index.TryGetValue(r, ManufacturerCountryFieldName),
						Manufacturer = (string)index.TryGetValue(r, ManufacturerFieldName),
						Country = (string)index.TryGetValue(r, CountryFieldName),

						PharmId = phid,
						PharmLogin = phlogin,
						PackId = (uint)index.TryGetValue(r, DrugIdFieldName),
						PharmPackId = (string)index.TryGetValue(r, DrugInternalIdFieldName)
					});
					List<Core.Data.AssortmentItemData> retlist = new List<Data.AssortmentItemData>();
					retlist.Add(ret);
					return retlist;
				}
			}

			return null;

		}

		public static IEnumerable<Core.Data.AssortmentItemData> ToAssortmentItemData(DataTable tbl, string defaultLogin, string key)
		{
			DataTableColumnsIndex index = tbl.BuildColumnsIndex();
			try
			{
				return AbdulToAss(tbl, defaultLogin);
			}
			catch
			{
				foreach (DataRow r in tbl.Rows)
				{
					uint? phid = (uint)index.TryGetValue(r, DrugstoreIdFieldName);
					string phlogin = (string)index.TryGetValue(r, DrugstoreLoginFieldName);
					long? vl = (long)index.TryGetValue(r, QuantityFieldName);

					if (String.IsNullOrEmpty(phlogin))
						phlogin = defaultLogin;

					// Пропускаем всякий мусор
					if (vl == null)
						continue;

					Core.Data.AssortmentItemData ret = (new Core.Data.AssortmentItemData()
					{
						RegistrationCertificate = (string)index.TryGetValue(r, RegistrationCertificateFieldName),
						RegistrationDate = (DateTime)index.TryGetValue(r, RegistrationDateFieldName),
						RegistrationEndDate = (DateTime)index.TryGetValue(r, RegistrationEndDateFieldName),
						RegistrationCancellationDate = (DateTime)index.TryGetValue(r, RegistrationCancellationDateFieldName),
						RegistrationOwner = (string)index.TryGetValue(r, RegistrationOwnerFieldName),
						ActiveSubstance = (string)index.TryGetValue(r, ActiveSubstanceFieldName),
						ProductionStages = (string)index.TryGetValue(r, ProductionStagesFieldName),
						PharmacoTherapeuticGroup = (string)index.TryGetValue(r, PharmacoTherapeuticGroupFieldName),
						ConsignmentNumber = (string)index.TryGetValue(r, ConsignmentNumberFieldName),
						BestBefore = (DateTime)index.TryGetValue(r, BestBeforeFieldName),
						DeliveryDate = (DateTime)index.TryGetValue(r, DeliveryDateFieldName),
						Quantity = vl.Value,
						PurchasePrice = (decimal)index.TryGetValue(r, PurchasePriceFieldName),
						Comment = (string)index.TryGetValue(r, CommentFieldName),

						Barcode = (string)index.TryGetValue(r, BarcodeFieldName),
						Name = (string)index.TryGetValue(r, NameFieldName),
						TradeName = (string)index.TryGetValue(r, TradeNameFieldName),
						Composition = (string)index.TryGetValue(r, CompositionFieldName),
						FormDosage = (string)index.TryGetValue(r, FormDosageFieldName),
						Form = (string)index.TryGetValue(r, FormFieldName),
						Dosage = (string)index.TryGetValue(r, DosageFieldName),
						Pack = (string)index.TryGetValue(r, PackFieldName),
						ManufacturerCountry = (string)index.TryGetValue(r, ManufacturerCountryFieldName),
						Manufacturer = (string)index.TryGetValue(r, ManufacturerFieldName),
						Country = (string)index.TryGetValue(r, CountryFieldName),

						PharmId = phid,
						PharmLogin = phlogin,
						PackId = (uint)index.TryGetValue(r, DrugIdFieldName),
						PharmPackId = (string)index.TryGetValue(r, DrugInternalIdFieldName)
					});
					List<Core.Data.AssortmentItemData> retlist = new List<Data.AssortmentItemData>();
					retlist.Add(ret);
					return retlist;
				}
			}

			return null;

		}


		public static IEnumerable<ModifyPriceItemData> ToModifyPriceItemData(DataTable tbl, string defaultLogin)
		{
			DataTableColumnsIndex index = tbl.BuildColumnsIndex();

			foreach (DataRow r in tbl.Rows)
			{
				//uint? phid = index.TryGetValue(r, DrugstoreIdFieldName).ConvertToStruct<uint>();
				//string phlogin = index.TryGetValue(r, DrugstoreLoginFieldName).ConvertToClass<string>();
				//long? vl = index.TryGetValue(r, QuantityFieldName).ConvertToStruct<long>();
				//decimal? vl2 = index.TryGetValue(r, SellingPriceFieldName).ConvertToStruct<decimal>();

				//if (String.IsNullOrEmpty(phlogin))
				//	phlogin = defaultLogin;

				//// Пропускаем всякий мусор
				//if ((vl == null) || (vl2 == null))
				//	continue;

				yield return(new ModifyPriceItemData()
				{
					//SellingPrice = vl2.Value,
					//Quantity = vl.Value,

					//PharmId = phid,
					//PharmLogin = phlogin,
					//PackId = index.TryGetValue(r, DrugIdFieldName).ConvertToStruct<uint>(),
					//PharmPackId = index.TryGetValue(r, DrugInternalIdFieldName).ConvertToClass<string>()
				});
			}
		}

		public static IEnumerable<RemovePriceItemData> ToRemovePriceItemData(DataTable tbl, string defaultLogin)
		{
			DataTableColumnsIndex index = tbl.BuildColumnsIndex();

			foreach (DataRow r in tbl.Rows)
			{
				//uint? phid = index.TryGetValue(r, DrugstoreIdFieldName).ConvertToStruct<uint>();
				//string phlogin = index.TryGetValue(r, DrugstoreLoginFieldName).ConvertToClass<string>();
				//uint? pckvak = index.TryGetValue(r, DrugIdFieldName).ConvertToStruct<uint>();
				//string ppckval = index.TryGetValue(r, DrugInternalIdFieldName).ConvertToClass<string>();

				//if (String.IsNullOrEmpty(phlogin))
				//	phlogin = defaultLogin;

				//// Пропускаем всякий мусор
				//if ((pckvak == null) && String.IsNullOrEmpty(ppckval))
				//	continue;

				yield return(new RemovePriceItemData()
				{
					//Comment = index.TryGetValue(r, CommentFieldName) as string,

					//PharmId = phid,
					//PharmLogin = phlogin,
					//PackId = pckvak,
					//PharmPackId = ppckval
				});
			}
		}
	}
}
