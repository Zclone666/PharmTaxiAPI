using API.Models;

using SMBarcode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PharmTaxiAPI.Methods
{
    public static class TemporaryFix
    {
  //      public static ProcessesBarcodeCheckResponse TempFix(ref Drug dr, ProcessesBarcodeCheckResponse response)
  //      {
		//		response.gtin = dr.GTIN;
		//		uint id = UInt32.TryParse(dr.master_id_lib67, out uint x) ? x : 0;
		//		response.fp = new OL.Models.IdNamePair(id, dr.FarmPrep == null ? dr.TorgName : dr.FarmPrep);
		//		System.DateTime datetime = new DateTime();
		//		if (System.DateTime.TryParse(dr.ExpiryDate, out datetime)) response.expiredate = datetime;
		//		response.seria = dr.SerialNumber;
		//		response.process.status = dr.isMed ? (uint)0 : (uint)1;
		//		response.process.message = response.fp.name;
		//		response.process.awaytable_values = new ProcessesBarcodeCheckAwaytableResponse[1];
		//		response.process.awaytable_values[0] = new ProcessesBarcodeCheckAwaytableResponse();
		//		response.process.awaytable_values[0].enums = null;
		//		response.process.awaytable_values[0].prompt = "71:562(51)"; //response.fp.name;//
		//		response.process.awaytable_values[0].key = "isConceptConfirmed";
		//		response.process.awaytable_values[0].type = "boolean";
		//		ProcessesBarcodeCheckStageResponse[] tmpstg = new ProcessesBarcodeCheckStageResponse[3];
		//		if (response.process.stages != null)
		//		{
		//			try
		//			{
		//				tmpstg[0] = response.process.stages[0] != null ? response.process.stages[0] : null;
		//				tmpstg[1] = response.process.stages[1] != null ? response.process.stages[1] : null;
		//			}
		//			catch (Exception e) { }
		//		}
		//		tmpstg[2] = new ProcessesBarcodeCheckStageResponse();
		//		tmpstg[2].name = "Идентификация фармпрепарата";
		//		tmpstg[2].status = 2;
		//		tmpstg[2].message = "71:562(51)";// response.fp.name;
		//		response.process.stages = tmpstg;
		//		if (!(response.process.stages[0] is null)) response.process.stages[0].message = response.process.stages[0].message == "Успешно" ? "" : response.process.stages[0].message;
		//	return response;
		//}

    }
}