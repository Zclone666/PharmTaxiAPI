using API.Models;
//using Core.Models;
//using Core.Processes;
//using OL.BuisnessProcess;
//using OL.Models;
using System;

namespace PharmTaxiAPI.Models
{
	public class ProcessBarcodeResponse
	{
		//id_ФП 
		//	название ФП 
		//id_ТН 
		//	назвние ТН 
		//id_ ЛФ 
		//	название ЛФ 
		//дозировка (если это комбинированный препарат то дозировок будет несколько) 
		//	id_единицы измерения дозировки (если это комбинированный препарат то дозировок будет несколько) 
		//	название единицы измерения дозировки/дозировок (если это комбинированный препарат, то для каждой дозировки будут соответствующие единицы измерения) 
		//id_фармпроизводителя 
		//	название фармпроизводителя 
		//id_страны производителя 
		//	страна производителя 
		//id_упаковки 
		//	название упаковки 
		//	количество лекарственных форм в упаковке 
		//Штрих-код 
		//Уникального идентификатора товара 
		//номер регистрационного удостоверения 
		//дата регистрации 
		//страна регистрации 
		//юридическое лицо, на имя которого выдано регистрационное удостоверение 
		//дата окончания действия регистрационного удостоверения 
		//дата аннулирования регистрационного удостоверения 
		//статус регистрационного удостоверения (1 разрешено; -1 срок истек; -2 анулирован; 0 или NULL - неизвестно) 

		//серийный номер Batch 
		//срок годности EXP 
		//уникальный код упаковки 


		public IdNamePair fp { get; set; }
		public string barcode { get; set; }
		public string gtin { get; set; }
		public string seria { get; set; }
		public string pack_number { get; set; }
		public DateTime? expiredate { get; set; }

		//public ProcessBarcodeResponse(BuisnessProcess<BarcodeInfo> buisnessProcess)
		//{
		//	object o;
		//	Concept c = null;

		//	if (buisnessProcess.AdvancedValues.TryGetValue(CheckWhatIsIt.ConceptValueName, out o))
		//		c = o as Concept;

		//	fp = (c != null) ? new IdNamePair(c.id, c.ToString()) : null;
		//	barcode = buisnessProcess.Data.PlainText;
		//	gtin = buisnessProcess.Data.GTIN;
		//	seria = buisnessProcess.Data.Seria;
		//	pack_number = buisnessProcess.Data.Number;
		//	expiredate = buisnessProcess.Data.UseBy;

		//	//if ((seria == null) && buisnessProcess.AdvancedValues.TryGetValue(CheckWithdrawns.SeriaFieldName, out o))
		//	//	seria = o as string;
		//}

		public ProcessBarcodeResponse(Exception err)
		{
			fp = null;
			barcode = null;
			gtin = null;
			seria = null;
			pack_number = null;
			expiredate = null;
		}

		public ProcessBarcodeResponse(string message)
		{
			fp = null;
			barcode = null;
			gtin = null;
			seria = null;
			pack_number = null;
			expiredate = null;
		}
	}
}

/*
{
	fp: {id: 0, name: ""},
	tn: {id: 0, name: ""},
	lf: {id: 0, name: ""},
	fk:  {id: 0, name: "", country: ""},
	dosages: [ {unit: {id: 0, name: ""}, values: [0.0, ...]}, {...}, ...],
	packages: [ {id: 0, name: "", quantity: 0.0, unit: {id: 0, name: ""}}, {...}, ...],
	certificate:
	{
		number: "",
		date: "",
		country: "",
		owner: "",
		date_end: "",
		date_annullment: "",
		status: 0
	},
	barcode: "",
	gtin: "",
	seria: "",
	pack_number: "",
	expiredate: ""
*/