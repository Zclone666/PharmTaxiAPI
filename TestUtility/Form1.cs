using ECPLib.Common.Drugs.Models;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestUtility.Constants;
using ECPLib;
using UMKBUtils;
using System.Threading;

namespace TestUtility
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            this.groupBox1.Width = this.Width / 2-25;
            this.groupBox1.Height = this.Height-63;
            this.groupBox2.Left = this.groupBox1.Width+22;
            this.groupBox2.Width = this.Width / 2-25;
            this.groupBox2.Height = this.Height - 63;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void check_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.barcode.Text))
            {
                CheckReq ch = new CheckReq() { text = this.barcode.Text, auth_key = this.authkeyUser.Text };
                this.response.Text = UMKBRequests.WebUtils.Post(Constants.URLs.PHTUrl_Local + URLs.Check, Newtonsoft.Json.JsonConvert.SerializeObject(ch), "application/json").Answer;
            }
        }

        private void GetReceipJson_Click(object sender, EventArgs e)
        {
            try
            {
                SMReceipt.Global.Models.SearchReceipt sr = new SMReceipt.Global.Models.SearchReceipt()
                {
                    DateOfReceipt = this.DateOfRec.Text,
                    FIO = this.FIO.Text,
                    SNILS = this.snils.Text,
                    ReceiptCode = this.receiptcode.Text,
                    authkey=this.authkeyUser.Text,
                    PharmId=this.PharmIdForOrder.Text
                };
                this.response.Text = UMKBRequests.WebUtils.Post(Constants.URLs.PHTUrl_Local + URLs.GetRecipeJson, Newtonsoft.Json.JsonConvert.SerializeObject(sr), "application/json").Answer;
            }catch { }
        }

        private void GetReceiptUrl_Click(object sender, EventArgs e)
        {
            try
            {
                SMReceipt.Global.Models.SearchReceipt sr = new SMReceipt.Global.Models.SearchReceipt()
                {
                    DateOfReceipt = this.DateOfRec.Text,
                    FIO = this.FIO.Text,
                    SNILS = this.snils.Text,
                    ReceiptCode = this.receiptcode.Text,
                    authkey = this.authkeyUser.Text,
                    PharmId = this.PharmIdForOrder.Text
                };
                this.response.Text = UMKBRequests.WebUtils.Post(Constants.URLs.PHTUrl_Local + URLs.GetRecipeURL, Newtonsoft.Json.JsonConvert.SerializeObject(sr), "application/json").Answer;
                this.RecFrame.Url = new Uri(this.response.Text);
                this.RecFrame.Navigate(this.response.Text);
            }
            catch { }
        }

        private void MakeOrder_Click(object sender, EventArgs e)
        {
            try
            {
                
                SMReceipt.Global.Models.OrderToSave ordts = new SMReceipt.Global.Models.OrderToSave()
                {
                    //Alert = new UMKBRequests.Alert(),

                };

                List<SMReceipt.Global.Models.Order> ord = new List<SMReceipt.Global.Models.Order>();
                List<SMReceipt.Global.Models.Meds> meds = new List<SMReceipt.Global.Models.Meds>();
                SMReceipt.Global.Models.Meds med0 = new SMReceipt.Global.Models.Meds() { dosage = "100мг", id = "302806", packs_string = "1", price = 500 };
                SMReceipt.Global.Models.Meds med1 = new SMReceipt.Global.Models.Meds() { dosage = "100мг", drugstore_drug_id = "70002645", packs_string = "10", price = 5000 };
                SMReceipt.Global.Models.Meds med2 = new SMReceipt.Global.Models.Meds() { dosage = "100мг", id = "56421", packs_string = "1", price = 987.5M };
                SMReceipt.Global.Models.Meds med3 = new SMReceipt.Global.Models.Meds() { dosage = "100мг", drugstore_drug_id = "990000633", packs_string = "10", price = 222 };
                meds.Add(med0);
                meds.Add(med1);
                meds.Add(med2);
                meds.Add(med3);
                ordts.drugs = meds;
                SMReceipt.Global.Models.Order el = new SMReceipt.Global.Models.Order() { order_to_save=ordts, PharmID=this.PharmIdForOrder.Text, UserID=this.UserId.Text };

                ord.Add(el);

                SMReceipt.Global.Models.SaveOrder so = new SMReceipt.Global.Models.SaveOrder()
                {
                    order = ord,
                    authkey="_"
                    //new List<SMReceipt.Global.Models.Order>()
                    //{        
                    //    new SMReceipt.Global.Models.Order()
                    //    {
                    //        PharmID=this.PharmIdForOrder.Text,
                    //        order_to_save=ordts
                    //        //new SMReceipt.Global.Models.OrderToSave(){
                    //        //    Patient= null ,
                    //        //    drugs=null
                    //        //}
                    //    } 
                    //}
                };
                SaveOrderD so2 = new SaveOrderD() { authkey = "_", order = new OrderToSaveD() { drugs = new List<MedsD>() { new MedsD() { } }, pharm_id = uint.Parse(this.PharmIdForOrder.Text) } };
                string ordr = "{ \"authkey\"=null, \"order\":{" +
                                "\"Delivery\": null,"+
                                "\"DeliveryAddress\": null,"+
                                "\"drugs\":"+
                                "["+
                                    "{ \"id\": \"+zdW4t3WAponqCBOEKnjGw==\", \"packs\": 1, \"price\": 168 },"+
                                    "{ \"id\": \"hTMl1CrUs08vXRO3GKnW8A==\", \"packs\": 2, \"price\": 85 },"+
                                "], \"pharm_id\": 1291,  }}";
                this.response.Text = UMKBRequests.WebUtils.Post(Constants.URLs.PHTUrl_Local + URLs.SaveOrder, Newtonsoft.Json.JsonConvert.SerializeObject(so2), "application/json").Answer;
            }
            catch { }
        }

        private void Register_Click(object sender, EventArgs e)
        {
            try
            {
                UserRegistrationReq ur = new UserRegistrationReq()
                {
                    email = this.AuthEmail.Text,
                    fio = this.FIO.Text,
                    familia = this.RegSurname.Text,
                    name = this.RegName.Text,
                    password = this.AuthPass.Text,
                    lic_key = this.LicCode.Text
                };
                this.response.Text = UMKBRequests.WebUtils.Post(Constants.URLs.PHTUrl_Local + URLs.Register, Newtonsoft.Json.JsonConvert.SerializeObject(ur), "application/json").Answer;
            }
            catch { }
        }

        private void Authorize_Click(object sender, EventArgs e)
        {
            try
            {
                Authorize au = new Authorize()
                {
                    email = this.AuthEmail.Text,
                    password = this.AuthPass.Text,
                    service = this.Service.Text
                };
                this.PharmsResp.Text = UMKBRequests.WebUtils.Post(Constants.URLs.PHTUrl_Local + URLs.Authorize, Newtonsoft.Json.JsonConvert.SerializeObject(au), "application/json").Answer;
            }
            catch { }
        }

        private void LoadAss_Click(object sender, EventArgs e)
        {
            //    FileStream fs = new FileStream(this.FilePath.Text, FileMode.Open, FileAccess.Read);
            //    byte[] data = new byte[fs.Length];
            //    fs.Read(data, 0, data.Length);
            //    string FlName = fs.Name.Substring(fs.Name.LastIndexOf('\\') + 1, fs.Name.Length - fs.Name.LastIndexOf('\\') - 1);
            //    fs.Close();
            //    Dictionary<string, object> postParamter = new Dictionary<string, object>();
            //    postParamter.Add("operation", this.operation.Text);
            //    postParamter.Add("file", new SMBarcode.Methods.FormUpload.FileParameter(data, FlName, "binary"));
            //    postParamter.Add("auth_key", "vRSCdjTRSij5s9sfKYLPs28Z");
            //    postParamter.Add("lang", "ru");
            //    string fullResponse = "";
            //    string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.135 Safari/537.36";
            //    HttpWebResponse webResponse = SMBarcode.Methods.FormUpload.MultipartFormDataPost(this.URL_To.Text, userAgent, postParamter);
            //    try
            //    {
            //        StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
            //        fullResponse = responseReader.ReadToEnd();
            //        webResponse.Close();
            //    }
            //    catch (Exception ex)
            //    {
            //        fullResponse = ex.Message;
            //    }

            //    this.Response.Text = fullResponse;
            //}
            ///string Lass = "{\"Lass\":[{\"PharmId\":343343,\"PharmLogin\":\"\", \"PackId\":null,\"PharmPackId\":\"58436\",\"Barcode\":\"8595026463387\",\"Quantity\":50,\"Name\":null,\"TradeName\":\"Урсосан (урсофальк), капс. 250мг №100\",\"Dosage\":\" 250мг \",\"Pack\":\"12\",\"Manufacturer\":\"Pro.Med.CS Praha\",\"Country\":\"Чешская республика\" }],\"authkey\":\"" + this.authkeyPharms.Text+"\"}";
            string Lass = "{\"Lass\":[{\"PharmId\":343343,\"PharmLogin\":\"\", \"PackId\":null,\"PharmPackId\":\"58434\",\"Barcode\":\"4605059012576\",\"Quantity\":50,\"Name\":null,\"TradeName\":\"5 дней крем, 30г д/ног заживляющий с маслом чайного дерева\",\"Dosage\":\" 100мг \",\"Pack\":\"12\",\"Manufacturer\":\"С-Петербургская ФФ/ ГаленоФарм\",\"Country\":\"Россия\" }],\"authkey\":\"" + this.PharmAuthkey.Text+"\"}";
            this.PharmsResp.Text = UMKBRequests.WebUtils.Post(Constants.URLs.PHTUrl_Local + URLs.LoadAssortiment, Newtonsoft.Json.JsonConvert.SerializeObject(Lass), "application/json").Answer;
        }

        private void AddUser_Click(object sender, EventArgs e)
        {
            try
            {
                AuthK au = new AuthK()
                {
                    authkey = this.authkeyUser.Text
                };
               
                this.response.Text = UMKBRequests.WebUtils.Post(Constants.URLs.PHTUrl_Local + URLs.GenerateUser, Newtonsoft.Json.JsonConvert.SerializeObject(au), "application/json").Answer;
            }
            catch { }
        }

        private void AttachPharm_Click(object sender, EventArgs e)
        {
            try
            {
                AttacheDrug au = new AttacheDrug()
                {
                    authkey = this.PharmAuthkey.Text,
                    drugstoreId = String.IsNullOrEmpty(this.PharmId.Text) ? 0 : uint.TryParse(this.PharmId.Text, out uint xex) ? xex : 0,
                    lic_code=this.LicCode.Text
                };
                if (au.drugstoreId == 0) au.drugstoreId = null;
                this.response.Text = UMKBRequests.WebUtils.Post(Constants.URLs.PHTUrl_Local + URLs.AttachDrugstoreToUser, Newtonsoft.Json.JsonConvert.SerializeObject(au), "application/json").Answer;
            }
            catch { }
        }

        private void LoadOrders_Click(object sender, EventArgs e)
        {
            try
            {
                AuthK au = new AuthK()
                {
                    authkey = this.PharmAuthkey.Text,
                   
                };

                this.PharmsResp.Text=(UMKBRequests.WebUtils.Post(Constants.URLs.PHTUrl_Local + URLs.GetPharmOrders, Newtonsoft.Json.JsonConvert.SerializeObject(au), "application/json").Answer);
            }
            catch { }
        }

        private void FillMedkit_Click(object sender, EventArgs e)
        {
            try
            {

                SMReceipt.Global.Models.PrescriptToSave ordts = new SMReceipt.Global.Models.PrescriptToSave()
                {
                    //Alert = new UMKBRequests.Alert(),
                    

                };
                List<Drug> dr = new List<Drug>();
                string strDr= "[{\"key\":\"Drug_0001\",\"id\":\"Maqz+E82Y+rBlYCOUwm4LA==\",\"name\":\"Полидекса\",\"level\":240,\"fpid\":null,\"type\":\"TN\",\"edit\":false,\"group\":0,\"text\":null,\"specials\":null,\"freq\":null,\"freq_id\":null,\"duration\":null,\"duration_id\":null,\"recipenameid\":null,\"solute\":false,\"dosage\":{\"form\":\"jDIkUiVsBH7XvnnwvO1zWw==\",\"method\":\"JOvxOPEc7+I1R/biMdNJgg==\",\"dosage\":null,\"pack\":null,\"value\":[0]},\"calc\":{\"dosageBaseUnit\":\"42\",\"dosage\":[0.001],\"countBaseUnit\":\"112\",\"count\":1,\"dosageUnit\":\"248\",\"dayCount\":1,\"fullDosage\":[0.001],\"weightDosage\":[0.000012048192771084337],\"dayDosage\":[0.001],\"dayWeightDosage\":[0.000012048192771084337],\"durationCount\":null,\"durationUnit\":null,\"fullSpeed\":null,\"liquidSpeed\":null,\"tintSpeed\":null,\"weightSpeed\":null},\"schedule\":{\"dateStart\":1614286800000,\"dateFinish\":null,\"finishAlternate\":null,\"interval\":null,\"once\":null},\"helpers\":{\"key\":null,\"way\":\"lf\",\"step\":\"4\",\"dosage\":{\"small\":\"нет данных\",\"full\":\"Дозировка не указана\",\"id\":\"248\"},\"pack\":{\"small\":\"\",\"full\":\"\",\"id\":\"168\"}},\"mnnid\":null,\"mnnname\":null,\"namesearch\":null,\"liquid\":false,\"proto\":\"6039396c1a0ada12b082c6a0\",\"proto_date\":\"2021_02\",\"proto_key\":\"1614362206011\",\"pdate\":null,\"pproto\":null,\"pkey\":null,\"repeal\":false,\"repealdoc\":null,\"repealdate\":null,\"repealproto\":null,\"recipes\":[\"24Q\"],\"lastrecipe\":\"24Q\"},{\"key\":\"Drug_0002\",\"id\":\"deb34FZylTlubEYmNddmdw==\",\"name\":\"Валидол с глюкозой\",\"level\":240,\"fpid\":null,\"type\":\"TN\",\"edit\":false,\"group\":0,\"text\":null,\"specials\":null,\"freq\":\"5 раз в день\",\"freq_id\":\"Ft9B9+D8Yr0lbepkIW5m9A==\",\"duration\":\"4 дня\",\"duration_id\":\"Vm2onoNvpZ4m/cOuqWcGqQ==\",\"recipenameid\":null,\"solute\":false,\"dosage\":{\"form\":\"aEJd0HuLPFsMG/QDaKiwSg==\",\"method\":\"azk9D8yXCUT1fH5MCHl0mg==\",\"dosage\":null,\"pack\":null,\"value\":[0]},\"calc\":{\"dosageBaseUnit\":\"42\",\"dosage\":[0.001],\"countBaseUnit\":\"342\",\"count\":1,\"dosageUnit\":\"248\",\"dayCount\":1,\"fullDosage\":[0.001],\"weightDosage\":[0.000012048192771084337],\"dayDosage\":[0.001],\"dayWeightDosage\":[0.000012048192771084337],\"durationCount\":null,\"durationUnit\":null,\"fullSpeed\":null,\"liquidSpeed\":null,\"tintSpeed\":null,\"weightSpeed\":null},\"schedule\":{\"dateStart\":1614632400000,\"dateFinish\":null,\"finishAlternate\":null,\"interval\":null,\"once\":null},\"helpers\":{\"key\":null,\"way\":\"lf\",\"step\":\"4\",\"dosage\":{\"small\":\"нет данных\",\"full\":\"Дозировка не указана\",\"id\":\"248\"},\"pack\":{\"small\":\"\",\"full\":\"\",\"id\":\"342\"}},\"mnnid\":null,\"mnnname\":null,\"namesearch\":null,\"liquid\":false,\"proto\":\"603e295db111e3d49db2743a\",\"proto_date\":\"2021_03\",\"proto_key\":\"Drug_0003\",\"pdate\":\"2021_03\",\"pproto\":\"603e2586b111e3d49db27433\",\"pkey\":\"Drug_0002\",\"repeal\":false,\"repealdoc\":null,\"repealdate\":null,\"repealproto\":null,\"recipes\":[],\"lastrecipe\":null},{\"key\":\"Drug_0003\",\"id\":\"JBoea4IgyjGzv8HkNhCFFQ==\",\"name\":\"Конкор\",\"level\":240,\"fpid\":null,\"type\":\"TN\",\"edit\":false,\"group\":0,\"text\":null,\"specials\":null,\"freq\":null,\"freq_id\":null,\"duration\":null,\"duration_id\":null,\"recipenameid\":null,\"solute\":false,\"dosage\":{\"form\":\"+qbdqiTg3nODLu1K4yKQDQ==\",\"method\":\"azk9D8yXCUT1fH5MCHl0mg==\",\"dosage\":[{\"type\":\"value\",\"value_1\":10,\"value_2\":null,\"id_1\":\"248\",\"id_2\":null,\"id\":null}],\"pack\":null,\"value\":[10]},\"calc\":{\"dosageBaseUnit\":\"42\",\"dosage\":[0.01],\"countBaseUnit\":\"342\",\"count\":1,\"dosageUnit\":\"248\",\"dayCount\":1,\"fullDosage\":[0.01],\"weightDosage\":[0.00012048192771084338],\"dayDosage\":[0.01],\"dayWeightDosage\":[0.00012048192771084338],\"durationCount\":null,\"durationUnit\":null,\"fullSpeed\":null,\"liquidSpeed\":null,\"tintSpeed\":null,\"weightSpeed\":null},\"schedule\":{\"dateStart\":1614632400000,\"dateFinish\":null,\"finishAlternate\":null,\"interval\":null,\"once\":null},\"helpers\":{\"key\":null,\"way\":\"lf\",\"step\":\"4\",\"dosage\":{\"small\":\"10 мг\",\"full\":\"10 миллиграмм\",\"id\":\"248\"},\"pack\":{\"small\":\"\",\"full\":\"\",\"id\":\"342\"}},\"mnnid\":null,\"mnnname\":null,\"namesearch\":null,\"liquid\":false,\"proto\":\"603e29feb111e3d49db2743b\",\"proto_date\":\"2021_03\",\"proto_key\":\"1614686589823\",\"pdate\":null,\"pproto\":null,\"pkey\":null,\"repeal\":false,\"repealdoc\":null,\"repealdate\":null,\"repealproto\":null,\"recipes\":[],\"lastrecipe\":null},{\"key\":\"Drug_0005\",\"id\":\"EmebBviJ9SkZrDjJGWWREw==\",\"name\":\"Парацетамол\",\"level\":241,\"fpid\":null,\"type\":\"DV\",\"edit\":false,\"group\":1,\"text\":null,\"specials\":null,\"freq\":null,\"freq_id\":null,\"duration\":null,\"duration_id\":null,\"recipenameid\":null,\"solute\":true,\"dosage\":{\"form\":\"rrxJLiuZFIJspFNJkG3MnQ==\",\"method\":\"pYdWqJsu//JDSQ5A0RyW6A==\",\"dosage\":[{\"type\":\"value\",\"value_1\":10,\"value_2\":null,\"id_1\":\"250\",\"id_2\":null,\"id\":null}],\"pack\":{\"id\":\"Cf1VqzJVbZvCMtaqhTNGcg==\",\"dosage\":{\"type\":\"value\",\"value_1\":50,\"value_2\":null,\"id_1\":\"168\",\"id_2\":null,\"id\":null}},\"value\":[10]},\"calc\":{\"dosageBaseUnit\":\"42\",\"dosage\":[0.01],\"countBaseUnit\":\"112\",\"count\":60,\"dosageUnit\":\"248\",\"dayCount\":1,\"fullDosage\":[0.6],\"weightDosage\":[0.007228915662650602],\"dayDosage\":[0.6],\"dayWeightDosage\":[0.007228915662650602],\"durationCount\":null,\"durationUnit\":null,\"fullSpeed\":null,\"liquidSpeed\":null,\"tintSpeed\":null,\"weightSpeed\":null},\"schedule\":{\"dateStart\":1614632400000,\"dateFinish\":null,\"finishAlternate\":null,\"interval\":null,\"once\":null},\"helpers\":{\"key\":null,\"way\":\"lf\",\"step\":\"4\",\"dosage\":{\"small\":\"10 мг/мл\",\"full\":\"10 миллиграмм на миллилитр\",\"id\":\"250\"},\"pack\":{\"small\":\"фл, 50 мл\",\"full\":\"флакон, 50 мл\",\"id\":\"168\"}},\"mnnid\":null,\"mnnname\":null,\"namesearch\":null,\"liquid\":false,\"proto\":\"603e657a0c470dd01b87535d\",\"proto_date\":\"2021_03\",\"proto_key\":\"Drug_0007\",\"pdate\":\"2021_03\",\"pproto\":\"603e64ff0c470dd01b87535c\",\"pkey\":\"Drug_0006\",\"repeal\":false,\"repealdoc\":null,\"repealdate\":null,\"repealproto\":null,\"recipes\":[],\"lastrecipe\":null},{\"key\":\"Drug_0006\",\"id\":\"XOUVnhJ7nkqAtWomJAxlAQ==\",\"name\":\"Диклофенак\",\"level\":239,\"fpid\":null,\"type\":\"DV\",\"edit\":false,\"group\":1,\"text\":null,\"specials\":\"Опа\",\"freq\":null,\"freq_id\":null,\"duration\":null,\"duration_id\":null,\"recipenameid\":null,\"solute\":true,\"dosage\":{\"form\":\"K2li5YyvKt+j6U0L7wJPDg==\",\"method\":\"IZe8rTWANcPvr3+HTQdkuA==\",\"dosage\":[{\"type\":\"ratio\",\"value_1\":75,\"value_2\":3,\"id_1\":\"250\",\"id_2\":null,\"id\":null}],\"pack\":{\"id\":\"Js8fEvknttP8ADWi50bj3Q==\",\"dosage\":{\"type\":\"value\",\"value_1\":3,\"value_2\":null,\"id_1\":\"168\",\"id_2\":null,\"id\":null}},\"value\":[25]},\"calc\":{\"dosageBaseUnit\":\"42\",\"dosage\":[0.025],\"countBaseUnit\":\"112\",\"count\":5,\"dosageUnit\":\"248\",\"dayCount\":1,\"fullDosage\":[0.125],\"weightDosage\":[0.0015060240963855422],\"dayDosage\":[0.125],\"dayWeightDosage\":[0.0015060240963855422],\"durationCount\":null,\"durationUnit\":null,\"fullSpeed\":null,\"liquidSpeed\":null,\"tintSpeed\":null,\"weightSpeed\":null},\"schedule\":{\"dateStart\":1614632400000,\"dateFinish\":null,\"finishAlternate\":null,\"interval\":null,\"once\":null},\"helpers\":{\"key\":null,\"way\":\"lf\",\"step\":\"4\",\"dosage\":{\"small\":\"25 мг/мл\",\"full\":\"25 миллиграмм на миллилитр\",\"id\":\"250\"},\"pack\":{\"small\":\"амп, 3 мл\",\"full\":\"ампула, 3 мл\",\"id\":\"168\"}},\"mnnid\":null,\"mnnname\":null,\"namesearch\":null,\"liquid\":false,\"proto\":\"603e657a0c470dd01b87535d\",\"proto_date\":\"2021_03\",\"proto_key\":\"Drug_0008\",\"pdate\":\"2021_03\",\"pproto\":\"603e64ff0c470dd01b87535c\",\"pkey\":\"Drug_0008\",\"repeal\":false,\"repealdoc\":null,\"repealdate\":null,\"repealproto\":null,\"recipes\":[],\"lastrecipe\":null},{\"key\":\"Drug_0007\",\"id\":\"QAZ6c/Zq5h/tQyp9pAsZkg==\",\"name\":\"Оливковое масло\",\"level\":241,\"fpid\":null,\"type\":\"DV\",\"edit\":false,\"group\":0,\"text\":null,\"specials\":null,\"freq\":null,\"freq_id\":null,\"duration\":null,\"duration_id\":null,\"recipenameid\":null,\"solute\":false,\"dosage\":{\"form\":null,\"method\":null,\"dosage\":[],\"pack\":{\"id\":null,\"dosage\":null},\"value\":null},\"calc\":{\"dosageBaseUnit\":\"42\",\"dosage\":[0],\"countBaseUnit\":\"42\",\"count\":1,\"dosageUnit\":\"248\",\"dayCount\":1,\"fullDosage\":[0],\"weightDosage\":[0],\"dayDosage\":[0],\"dayWeightDosage\":[0],\"durationCount\":null,\"durationUnit\":null,\"fullSpeed\":null,\"liquidSpeed\":null,\"tintSpeed\":null,\"weightSpeed\":null},\"schedule\":{\"dateStart\":1615237200000,\"dateFinish\":null,\"finishAlternate\":null,\"interval\":null,\"once\":null},\"helpers\":{\"key\":null,\"way\":null,\"step\":null,\"dosage\":{\"small\":null,\"full\":null,\"id\":null},\"pack\":{\"small\":null,\"full\":null,\"id\":null}},\"mnnid\":null,\"mnnname\":null,\"namesearch\":null,\"liquid\":false,\"proto\":\"60475148f43c0904b7efa86d\",\"proto_date\":\"2021_03\",\"proto_key\":\"Drug_0004\",\"pdate\":\"2021_03\",\"pproto\":\"603e4e68807d66e3e342b3b5\",\"pkey\":\"1614696026539\",\"repeal\":false,\"repealdoc\":null,\"repealdate\":null,\"repealproto\":null,\"recipes\":[],\"lastrecipe\":null},{\"key\":\"Drug_0008\",\"id\":\"kD1AcC5OV7kYWbNtRtcQnw==\",\"name\":\"Нурофен\",\"level\":240,\"fpid\":null,\"type\":\"TN\",\"edit\":false,\"group\":0,\"text\":null,\"specials\":null,\"freq\":null,\"freq_id\":null,\"duration\":\"3 дня\",\"duration_id\":\"Jwy4qdjtdVf0PjkWRlIhCA==\",\"recipenameid\":null,\"solute\":false,\"dosage\":{\"form\":\"+qbdqiTg3nODLu1K4yKQDQ==\",\"method\":\"azk9D8yXCUT1fH5MCHl0mg==\",\"dosage\":[{\"type\":\"value\",\"value_1\":200,\"value_2\":null,\"id_1\":\"248\",\"id_2\":null,\"id\":null}],\"pack\":null,\"value\":[200]},\"calc\":{\"dosageBaseUnit\":\"42\",\"dosage\":[0.2],\"countBaseUnit\":\"342\",\"count\":1,\"dosageUnit\":\"248\",\"dayCount\":1,\"fullDosage\":[0.2],\"weightDosage\":[0.0024096385542168677],\"dayDosage\":[0.2],\"dayWeightDosage\":[0.0024096385542168677],\"durationCount\":null,\"durationUnit\":null,\"fullSpeed\":null,\"liquidSpeed\":null,\"tintSpeed\":null,\"weightSpeed\":null},\"schedule\":{\"dateStart\":1615237200000,\"dateFinish\":1615410000000,\"finishAlternate\":null,\"interval\":null,\"once\":null},\"helpers\":{\"key\":null,\"way\":\"lf\",\"step\":\"4\",\"dosage\":{\"small\":\"200 мг\",\"full\":\"200 миллиграмм\",\"id\":\"248\"},\"pack\":{\"small\":\"\",\"full\":\"\",\"id\":\"342\"}},\"mnnid\":null,\"mnnname\":null,\"namesearch\":null,\"liquid\":false,\"proto\":\"60475148f43c0904b7efa86d\",\"proto_date\":\"2021_03\",\"proto_key\":\"Drug_0008\",\"pdate\":\"2021_03\",\"pproto\":\"6046b6b1f43c0904b7efa86b\",\"pkey\":\"Drug_0001\",\"repeal\":false,\"repealdoc\":null,\"repealdate\":null,\"repealproto\":null,\"recipes\":[],\"lastrecipe\":null}]";
                dr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Drug>>(strDr);
                ordts.drugs=dr;
                List<SMReceipt.Global.Models.Prescript> ord = new List<SMReceipt.Global.Models.Prescript>();
                SMReceipt.Global.Models.Prescript el = new SMReceipt.Global.Models.Prescript() { order_to_save = ordts, UserID = this.UserId.Text };
                ord.Add(el);

                SMReceipt.Global.Models.SavePrescript so = new SMReceipt.Global.Models.SavePrescript()
                {
                    order = ord,
                    authkey="_testWinForm"
                    //new List<SMReceipt.Global.Models.Order>()
                    //{        
                    //    new SMReceipt.Global.Models.Order()
                    //    {
                    //        PharmID=this.PharmIdForOrder.Text,
                    //        order_to_save=ordts
                    //        //new SMReceipt.Global.Models.OrderToSave(){
                    //        //    Patient= null ,
                    //        //    drugs=null
                    //        //}
                    //    } 
                    //}
                };
                this.response.Text = UMKBRequests.WebUtils.Post(Constants.URLs.PHTUrl_Local + URLs.SavePrescript, Newtonsoft.Json.JsonConvert.SerializeObject(so), "application/json").Answer;
            }
            catch { }
        }

        private void AddPharm_Click(object sender, EventArgs e)
        {
            try
            {
                string phrm = "{\"LongId\":0,\"ParentId\":null,\"TypeText\":null,\"UserId\":\"\",\"Address\":\"Москва, ул. Нобеля 1\",\"AddressVCId\":0,\"License\":\"1111111йв\",\"LicenseVCId\":0,\"Phone\":\"+7 926 000 00 11\",\"PhoneVCId\":0,\"Title\":\"TEST_APT666\",\"NameLongId\":0,\"Users\":[],\"latitude\":75.14,\"longitude\":55.14}";
                DrugstoreModel phr = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.DrugstoreModel>(phrm);
                phr.authkey = this.PharmAuthkey.Text;
                phr.location = new MongoDB.Driver.GeoJsonObjectModel.GeoJson2DGeographicCoordinates(0, 0);
            //    phr.location = new MongoDB.Driver.GeoJsonObjectModel.GeoJson2DGeographicCoordinates(95.4321, -45.2543);
                this.response.Text = UMKBRequests.WebUtils.Post(Constants.URLs.PHTUrl_Local + URLs.CreateDrugstoreWL, Newtonsoft.Json.JsonConvert.SerializeObject(phr), "application/json").Answer;
            }
            catch { }
        }

        private void CreateWNPass_Click(object sender, EventArgs e)
        {
            AuthKeyByLicKey au = new AuthKeyByLicKey() {
                email = this.SlaveEmail.Text,
                master_authkey = this.PharmAuthkey.Text,
                slave_authkey = this.SlaveAuthkey.Text, 
                drugstore_id = this.SlavePharmId.Text 
            };
            try
            {
                this.response.Text = UMKBRequests.WebUtils.Post(Constants.URLs.PHTUrl_Local + URLs.CreateSlaveWithEmail, Newtonsoft.Json.JsonConvert.SerializeObject(au), "application/json").Answer;
            }
            catch { }
        }

        private void CreateGuestButton_Click(object sender, EventArgs e)
        {
            var answer = UMKBRequests.WebUtils.Post(Constants.URLs.PHTUrl_Local + URLs.CreateGuestUser, "application/json").Answer;
            this.response.Text = answer;
        }

        static async Task<string> WebReq(string req)
        {
            return UMKBRequests.WebUtils.Post("https://service.socmedica.com:9025/check", req, "application/json").Answer;
        }

        static async void ThreadPl(Object stateInfo)
        {
            string rrrrr = "{     \"code\": {         \"text\": \"01046070277626502100000OTW2S14G 91EE06 92vQfiZmMrSHJejB4VfONpq+Dk7DNI4CYbOzyEccRLjvE=\",         \"format\": \"DATA_MATRIX\"     },     \"text\": \"01046070277626502100000OTW2S14G 91EE06 92vQfiZmMrSHJejB4VfONpq+Dk7DNI4CYbOzyEccRLjvE=\",     \"format\": \"DATA_MATRIX\",     \"auth_key\": \"\" }";
            for (int i = 0; i < 30; i++)
            {
                await Task.Run(() => WebReq(rrrrr));
            }
        }

        static void NotMain()
        {
            try
            {
                using (BigIdCryptography service = new BigIdCryptography("DESKTOP-0JFBQ9R"))//DESKTOP-0JFBQ9R
                {
                    string patid = "LZQxAAtk5h9UB8Sv1UvaKg==";
                    var pid = service.DecryptUInt64(patid);

                    ThreadPool.SetMinThreads(30, 30);
                    ThreadPool.QueueUserWorkItem(ThreadPl);
                }
            }
            catch (Exception e) { }
        }


        private void UserFromGuest_Click(object sender, EventArgs e)
        {
            UserFromGuest ufg = new UserFromGuest()
            {
                key = "1",
                application = "5",
                authkey = this.authkeyUser.Text,
                email = "zclone@yandex.ru",
                password = "Kirill1991",
                firstName = "EE",
                lastName = "MM",
                middleName = "ttt"
            };
            string ask = "{\"authkey\": \"9dozKqMBeu4Hyg6zJ7v9wKD9\",\"email\": \"testphtu55@ycopmail.com\",\"password\": \"1234abc\",\"firstName\": \"Иван\",\"lastName\": \"Иванов\",\"middleName\": \"Иванович\"}";
            try
            {
                this.response.Text = UMKBRequests.WebUtils.Post(Constants.URLs.PHTUrl_Local + URLs.CreateUserFromGuest, Newtonsoft.Json.JsonConvert.SerializeObject(ufg), "application/json").Answer; //Newtonsoft.Json.JsonConvert.SerializeObject(ufg)
            }
            catch
            {

            }
        }

        private void LoadOrdersOld_Click(object sender, EventArgs e)
        {
            try
            {
                AuthK au = new AuthK()
                {
                    authkey = this.PharmAuthkey.Text,

                };

                this.PharmsResp.Text = (UMKBRequests.WebUtils.Post(Constants.URLs.PHTUrl_Local + URLs.GetPharmOrdersOld, Newtonsoft.Json.JsonConvert.SerializeObject(au), "application/json").Answer);
            }
            catch { }
        }

        private void MustHaveButton_Click(object sender, EventArgs e)
        {
            RequestMustHave rmh = new RequestMustHave()
            {
                fp_id = "BVPbTyk9oSV6hVfdDUq+tQ=="
            };

            var a = UMKBRequests.WebUtils.Post(Constants.URLs.PHTUrl_Local + URLs.CheckMustHave, Newtonsoft.Json.JsonConvert.SerializeObject(rmh), "application/json").Answer;
            PharmsResp.Text = a;
        }

        private void GetDrugstoresByUserId_Click(object sender, EventArgs e)
        {
            try
            {
                AuthK au = new AuthK()
                {
                    authkey = this.PharmAuthkey.Text,

                };

                this.PharmsResp.Text = (UMKBRequests.WebUtils.Post(Constants.URLs.PHTUrl_Local + URLs.GetDrugstoresByUserId, Newtonsoft.Json.JsonConvert.SerializeObject(au), "application/json").Answer);
            }
            catch { }
        }
        
    }
}
