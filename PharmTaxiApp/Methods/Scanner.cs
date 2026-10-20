using Newtonsoft.Json;
using PharmTaxiApp.Models;
using PharmTaxiApp.Models.Barcodes;
using SMBarcode;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace PharmTaxiApp.Methods
{
    /// <summary>
    /// Класс с методами для работы со штрихкдами
    /// </summary>
    public static class Scanner
    {
        //public static UMKBUtils.BigIdCryptography Crypto = new UMKBUtils.BigIdCryptography(PharmTaxiApp.Models.Crypto.Password);
        static PharmTaxiApp.Methods.Logs lg = new Logs();
        public static int counter = 38;
        /// <summary>
        /// Проверка подлинности и получение данных по штрихкоду
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Drug Check(CheckReq data)
        {
            Drug dr = new Drug();
            try
            {
                string barcodeText = (data.code is null) ? data.text : data.code.text is null ? data.text : data.code.text;
                barcodeText = barcodeText.Trim();

                if (barcodeText.Contains(" "))
                    barcodeText = barcodeText.Replace(" ", "\x1d");

                // Убираем лишний спецсимвол в начале строки
                if (!barcodeText.StartsWith("01") && barcodeText.Length > 14)
                    barcodeText = barcodeText.Substring(barcodeText.IndexOf("01", StringComparison.Ordinal));

                // Если нет штрихкода, выбрасываем ошибку
                if (barcodeText is null)
                    throw new Exception("ErrorMessages.CantReadBarcode");

                string barcodeFormat = (data.code is null) ? data.format : data.code.format is null ? data.format : data.code.format;
                if (barcodeText.Length == 13 && barcodeText.StartsWith('0')) barcodeText = barcodeText.Substring(1);
                Barcode bc = new SMBarcode.Barcode(barcodeText);

                try
                {
                    dr = DrugRequests.Read(bc);
                }
                catch { }
                if (dr.format == "UPC" && dr.alert.code == 404 && !String.IsNullOrEmpty(dr.farmPrep))
                {
                    counter = counter - 1;
                    dr.alert.code = 200;
                    dr.alert.message = $"Зарубежный препарат, число оставшихся сканирований в день: {--counter}";
                }
                return dr;
            }
            catch (Exception e)
            {
                lg.Log("Check()", e.Message);
                dr.alert = new Alert()
                {
                    code = 404,
                    message = "Unknown Drug. Error: " + e.ToString(),
                    level = "error",
                    title = "error",
                    sticky = false,
                };
                return (dr);
            }

            /*
            try
            {
                BarcodeRequestData code = new BarcodeRequestData();
                double?[] coords = null;
                uint? accuracy = null;
                string file = null;
                string auth_key = null;
                string lang = "ru";
                string mode = "WORK";
                string country = null;

                try
                {

                    //API.Models.FormData tmp;
                    uint itmp;
                    code = data.code;
                    if (code is null)
                    {
                        code = new BarcodeRequestData();
                        code.text = data.text; //JsonConvert.DeserializeObject<BarcodeRequestData>(data.code);// JsonConvert.DeserializeObject<BarcodeRequestData>(((FormField)tmp).Value);
                        code.format = data.format;
                    }
                    //if (!(data.auth_key is null))
                    auth_key = data.auth_key;
                    //	else
                    //		throw new Exception("IncorrectAuthKey");
                    lang = data.lang;

                    mode = data.mode;

                    country = data.country;

                    coords = data.coords;

                    if (!(data.accuracy is null))
                        accuracy = data.accuracy;

                    //	country = "Москва";
                    //	code = new BarcodeRequestData();
                    //	code.text = "4601669002495";

                    SMBarcode.Configuration.Setup(Constants.Consts.SQLConnectionString, Constants.Consts.TableGetKey, Constants.Consts.GraphKey, Constants.Consts.ConceptKey, new SMBarcode.Configuration.SetupBase());
                    SMBarcode.Configuration.GS1_user = Constants.Consts.GS1_user;
                    SMBarcode.Configuration.GS1_pass = Constants.Consts.GS1_pass;
                    //BarcodeInfo bcinfo = new BarcodeInfo(String.Format("{0}10{1}", BarcodeInfo.FNC1, code), "EAN_128");
                    BarcodeInfo bcinfo = new BarcodeInfo(String.Format("{0}", code.text), code.format);
                    if ((code == null) && String.IsNullOrEmpty(file))
                        throw new Exception("NoContentDispositionValue_template_1");

                    SMBarcode.Barcode bc = new SMBarcode.Barcode(code.text);
                    Drug dr = new Drug();
                    try
                    {
                        dr = DrugRequests.Read(bc);
                    }
                    catch (Exception e) { }
                    //bc.IdentificationNumber = (code.text.Length > 13 ? code.text.Substring(3, 13) : code.text);
                    //dr.GTIN = (Regex.IsMatch(code.text, "[a-zA-Z]^[^x]+$") ? (code.text.Length > 13 ? code.text.Substring(4, 13) : code.text) : code.text);
                    if (code.text.Length == 12)
                    {
                        UPCRes res = UPCGoMF.Get(code.text);
                        /*if (res.total > 0)
                        {
                            ProcessesBarcodeCheckResponse resp = new ProcessesBarcodeCheckResponse(res.items[0].title);
                            resp.process.status = 0;
                            resp.fp = new API.Models.IdNamePair(0, res.items[0].title);
                            return (JsonConvert.SerializeObject(resp));
                        }
                        else
                        {
                            ProcessesBarcodeCheckResponse resp = new ProcessesBarcodeCheckResponse("Штрих код не распознан! Возможно фальсификат!");
                            resp.process.status = 1;
                            //resp.fp = new OL.Models.IdNamePair(0, res.items[0].title);
                            return (JsonConvert.SerializeObject(resp));
                        }*/
            /*
                        return (JsonConvert.SerializeObject(res));
                    }
                    if (code.text.Length > 14) 
                        dr.GTIN = (code.text.Length > 13 ? code.text.Substring(4, 13) : code.text);
                    else 
                        dr.GTIN = (code.text.Length > 13 ? code.text.Substring(1, 13) : code.text);
                    if (Regex.IsMatch(dr.GTIN, "[a-z]") || Regex.IsMatch(dr.GTIN, "[A-Z]"))
                    {
                        string a = code.text.Substring(3, 22);
                        decimal value = 0;
                        decimal dx = decimal.TryParse(string.Join("", a.Where(c => char.IsDigit(c))), out value) ? value : 0;
                        dr.GTIN = dx.ToString().Substring(1, 13);
                    }
                    dr.Format = Regex.IsMatch(code.text, "^[^x]+$") ? BarcodeType.DataMatrix : BarcodeType.EAN_13;
                    try
                    {
                        DrugRequests.Gs1Parse(ref dr);
                    }
                    catch (Exception e) { }
                    dr.GetMasterIdLib67(ref dr);
                    try
                    {
                        if (dr.master_id_lib67 is null || dr.master_id_lib67 == "0") dr.master_id_lib67 = SMBarcode.UMKBGetInfo.GetMasterIDByGTIN(dr.GTIN);
                        if (dr.master_id_lib67 is null || dr.master_id_lib67 == "0" || dr.master_id_lib67 == "") dr.master_id_lib67 = SMBarcode.UMKBGetInfo.GetMasterIDByGTIN73(dr.GTIN);
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            if (dr.master_id_lib67 is null || dr.master_id_lib67 == "0") dr.master_id_lib67 = SMBarcode.UMKBGetInfo.GetMasterIDByGTIN73(dr.GTIN);
                        }
                        catch (Exception e2)
                        {
                        }
                    }
                    dr.GetDeyVesch(ref dr);
                    //ProcessesBarcodeCheckResponse response;
                    //try
                    //{
                    //	bcinfo = (code == null)
                    //		? _barcodeProcessor.Read(file)
                    //		: new BarcodeInfo(code.text, code.format);
                    //}
                    //catch
                    //{
                    //	response = new ProcessesBarcodeCheckResponse("");
                    //	response = TemporaryFix.TempFix(ref dr, response);
                    //	/*#if LOG_ALL_REQUESTS
                    //							lg.Log("Check()", "RETURNED");
                    //	#endif

                    //							return (Json(response));*/
                    //}
                    //bc.Format = SMBarcode.BarcodeType.EAN_13;//;code.format;
                    //bc.RawCode=code.
                    /*

                    if (bcinfo == null)
                        throw new Exception("ErrorMessages.CantReadBarcode");
                    else if (!bcinfo.WasReaded)
                        throw new Exception("ErrorMessages.CantParseBarcode");

                    //ConnectionMode cmode = (mode == "TEST") ? ConnectionMode.Test : ConnectionMode.Work;

                    double? lat = ((coords != null) && (coords.Length > 0)) ? coords[0] : null;
                    double? lon = ((coords != null) && (coords.Length > 1)) ? coords[1] : null;

                    #region ЗАТЫЧКА

                    if (lat == 55.4521 && lon == 37.3704 && accuracy == 1000)
                    {
                        lat = null;
                        lon = null;
                        accuracy = null;
                    }

                    #endregion

#if LOG_ALL_REQUESTS
            					if (code != null)
            					{
            						lg.Log("Check()", String.Format("{0}({1}, {2}, {3}, {4})", code.format, code.text,
            							lat,
            							lon,
            							accuracy));
            					}
#endif

                    // Сохраняем в базу результат сканирования
                    //await UmkbDataSource.GetAdapters(cmode).SaveDrugScan(bcinfo, // ------------------------- 2 секунды
                    //	auth_key,
                    //	lat,
                    //	lon,
                    //	accuracy);

                    //uint? userId = await UmkbDataSource.GetAdapters(cmode).GetUserId(auth_key);



                }
                catch (Exception e)
                {
                    //	response = new ProcessesBarcodeCheckResponse(process, cmode, null);
                    //response=TemporaryFix.TempFix(ref dr,response);
                    /*response.gtin = dr.GTIN;
                    response.fp = new OL.Models.IdNamePair(UInt32.Parse(dr.master_id_lib67), dr.FarmPrep==null?dr.TorgName:dr.FarmPrep);
                    System.DateTime datetime = new DateTime();
                    if (System.DateTime.TryParse(dr.ExpiryDate, out datetime)) response.expiredate = datetime;
                    response.seria = dr.SerialNumber;
                    response.process.status = 0;
                    response.process.message = response.fp.name;
                    response.process.awaytable_values = new ProcessesBarcodeCheckAwaytableResponse[1];
                    response.process.awaytable_values[0] = new ProcessesBarcodeCheckAwaytableResponse();
                    response.process.awaytable_values[0].enums = null;
                    response.process.awaytable_values[0].prompt = "71:562(51)"; //response.fp.name;//
                    response.process.awaytable_values[0].key = "isConceptConfirmed";
                    response.process.awaytable_values[0].type = "boolean";
                    ProcessesBarcodeCheckStageResponse[] tmpstg = new ProcessesBarcodeCheckStageResponse[3];
                    tmpstg[0] = response.process.stages[0];
                    tmpstg[1] = response.process.stages[1];
                    tmpstg[2] = new ProcessesBarcodeCheckStageResponse();
                    tmpstg[2].name = "Идентификация фармпрепарата";
                    tmpstg[2].status = 2;
                    tmpstg[2].message = "71:562(51)";// response.fp.name;
                    response.process.stages = tmpstg;*/
                    /*
                }
#if LOG_ALL_REQUESTS
            				lg.Log("Check()", "RETURNED");
#endif

                return ("");


            }
            catch (Exception e)
            {
                lg.Log("Check()", e.Message);

                return (JsonConvert.SerializeObject((e.Message)));
            }
                    */
		}

        /// <summary>
        /// Какая-то фишгня, типо теста, осталась от старого проекта
        /// </summary>
        /// <returns></returns>
        public static string Detect()
        {
            try
            {
                BarcodeRequestData code = null;
                string file = null;
                string auth_key = null;
                string lang = "ru";
                string mode = "WORK";
                string country = null;

                try
                {

                }
                finally
                {
                    if (!String.IsNullOrEmpty(file))
                    {
                        //File.Delete(file);
                    }
                }
                return "";
            }
            catch (Exception e)
            {
                lg.Log("Detect()", e.Message);
                return (e.Message);
            }
        }
    }
}
