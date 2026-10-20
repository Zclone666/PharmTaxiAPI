using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PharmTaxiAdm.Models.Drugstore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PTConstants;
using UMKBRequests.Modelling.U_AUD_Lib;
using MySqlConnector;
using PharmTaxiAdm.Models;
using AUpload.Upload.Mongo.Models;
using PharmTaxiAdm.Models.User;
using UMKBNeedStuff.Models;

namespace PharmTaxiAdm.Methods.Drugstore
{
    public class DrugstoreMethods
    {
        /// <summary>
        /// Создание сети аптек
        /// </summary>
        /// <param name="cdsModel"></param>
        public static Models.MongoDS.PharmNets CreateDrugNet(DrugNetModel cdsModel)
        {
            DrugstoreModel Drugstore = Newtonsoft.Json.JsonConvert.DeserializeObject<DrugstoreModel>(Newtonsoft.Json.JsonConvert.SerializeObject(cdsModel));
            Models.MongoDS.Pharms phr=UMKBCreateDrugNet(Drugstore);
            PharmTaxiAdm.Models.MongoDS.PharmNets phnet = new Models.MongoDS.PharmNets();
            phnet.Address = phr.Address;
            phnet.AddressVCId = phr.AddressVCId;
            phnet.authkey = phr.authkey;
            //phnet.IsActive = phr.IsActive;
            // Временный workaround - надо бы при загрузке ассортимента это делать... наверное            
            phnet.IsActive = true;
            phnet.IsHidden = false;
            //
            phnet.key = phr.key;
            phnet.License = phr.License;
            phnet.LicenseVCId = phr.LicenseVCId;
            phnet.LongId = phr.LongId;
            phnet.NameLongId = phr.NameLongId;
            phnet.ParentId = phr.ParentId;
            phnet.PharmId = phr.PharmId;
            phnet.PharmLogin = phr.PharmLogin;
            phnet.Phone = phr.Phone;
            phnet.PhoneVCId = phr.PhoneVCId;
            phnet.Title = phr.Title;
            phnet.TypeText = phr.TypeText;
            phnet.UserId = phr.UserId;
            phnet.Users = phr.Users;
            phnet._id = phr._id;
            MongoDS.PharmInsert.InSertDrugNet(phnet);
            return phnet;
        }


        public static Models.MongoDS.Pharms UMKBCreateDrugNet(DrugstoreModel cdsModel)
        {
            Models.MongoDS.Pharms phr = new Models.MongoDS.Pharms();
            if (cdsModel.PharmId == 0)
            {
                int cnt = 0;
                UMKB_Packet UP = new UMKB_Packet(UMKB.Modelling);
                UP.user = PTConstants.Constants.ModellingUserSverdlov;
                UError hz = new UError();
                UMKB_Field APTEK = new UMKB_Field(null, null, null, null, null, 95, 948, ref cnt, out hz);

                ////  UMKB_Field Anunak = UP.BelCreatePrev("902", cdsModel.Id.ToString(), APTEK, "9112", 905, ref cnt, null);  // 
                //UMKB_Field Anunak = APTEK.BelAddInverse(uint.Parse(cdsModel.UserId), null, 902, 920, 9112, ref cnt, out hz);
                ////UMKB_Field Anunak = APTEK.bel

                //UP.modeling.Add(APTEK);
                //UP.modeling.Add(Anunak);
                UP.modeling.Add(APTEK);
                UMKB_Field Anunak;
                //  UMKB_Field Anunak = UP.BelCreatePrev("902", cdsModel.Id.ToString(), APTEK, "9112", 905, ref cnt, null);  // 
                if (!(cdsModel.UserId is null))
                {
                    Anunak = APTEK.BelAddInverse(uint.TryParse(cdsModel.UserId, out uint r) ? r : 0, null, 902, 920, 9112, ref cnt, out hz);
                    //UMKB_Field Anunak = APTEK.bel


                    UP.modeling.Add(Anunak);
                }
                UP.modeling.Add(APTEK.CrNamesToBel(cdsModel.Title));
                UP.modeling.Add(APTEK.AddNewVarcharTo(cdsModel.License, null, "license", APTEK.lib));

                UP.modeling.Add(APTEK.AddNewVarcharTo(cdsModel.Phone, null, "phone", APTEK.lib));
                UP.modeling.Add(APTEK.AddNewVarcharTo(cdsModel.Address, null, "address", APTEK.lib));
                UP.cache = true;
                string shit0 = Newtonsoft.Json.JsonConvert.SerializeObject(UP, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                UP.UMKBRequestSP(URLs.Modelling);
                UMKB_RespStat resp = UP.URresult;
                string shit1 = Newtonsoft.Json.JsonConvert.SerializeObject(UP.URresult, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                phr = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.MongoDS.Pharms>(Newtonsoft.Json.JsonConvert.SerializeObject(cdsModel));
                if (resp.log.Count > 0) phr.PharmId = (uint)resp.log[0].id;
            }
            else phr.PharmId = cdsModel.PharmId;
            phr._id = SMReceipt.Global.Methods.RGenerator.PharmIdCode().Result; //BsonString.Create(BsonObjectId.GenerateNewId().AsString).AsString;
            return phr;
        }


        public static void CreateDrugstore(DrugstoreModel cdsModel)
        {
            Models.MongoDS.Pharms phr = new Models.MongoDS.Pharms();
            if (cdsModel.PharmId ==0)
            {
                int cnt = 0;
                UMKB_Packet UP = new UMKB_Packet(UMKB.Modelling);
                UP.user = PTConstants.Constants.ModellingUserSverdlov;
                UError hz = new UError();
                UMKB_Field APTEK = new UMKB_Field(null, null, null, null, null, 95, 944, ref cnt, out hz);

                ////  UMKB_Field Anunak = UP.BelCreatePrev("902", cdsModel.Id.ToString(), APTEK, "9112", 905, ref cnt, null);  // 
                //UMKB_Field Anunak = APTEK.BelAddInverse(uint.Parse(cdsModel.UserId), null, 902, 920, 9112, ref cnt, out hz);
                ////UMKB_Field Anunak = APTEK.bel
                if (cdsModel.ParentId.HasValue) APTEK.parent_id = cdsModel.ParentId.Value.ToString();
                //UP.modeling.Add(APTEK);
                //UP.modeling.Add(Anunak);
                UP.modeling.Add(APTEK);
                UMKB_Field Anunak;
                //  UMKB_Field Anunak = UP.BelCreatePrev("902", cdsModel.Id.ToString(), APTEK, "9112", 905, ref cnt, null);  // 
                if (!(cdsModel.UserId is null))
                {
                    Anunak = APTEK.BelAddInverse(uint.TryParse(cdsModel.UserId, out uint r) ? r : 0, null, 902, 920, 9112, ref cnt, out hz);
                    //UMKB_Field Anunak = APTEK.bel


                    UP.modeling.Add(Anunak);
                }
                UP.modeling.Add(APTEK.CrNamesToBel(cdsModel.Title));
                UP.modeling.Add(APTEK.AddNewVarcharTo(cdsModel.License, null, "license", APTEK.lib));

                UP.modeling.Add(APTEK.AddNewVarcharTo(cdsModel.Phone, null, "phone", APTEK.lib));
                UP.modeling.Add(APTEK.AddNewVarcharTo(cdsModel.Address, null, "address", APTEK.lib));
              //  UP.modeling.Add();
                UP.cache = true;
                string shit0 = Newtonsoft.Json.JsonConvert.SerializeObject(UP, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                UP.UMKBRequestSP(URLs.Modelling);
                UMKB_RespStat resp = UP.URresult;
                string shit1 = Newtonsoft.Json.JsonConvert.SerializeObject(UP.URresult, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                phr = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.MongoDS.Pharms>(Newtonsoft.Json.JsonConvert.SerializeObject(cdsModel));
                if (resp.log.Count > 0) phr.PharmId = (uint)resp.log[0].id;
            }
            else phr.PharmId = cdsModel.PharmId;
            phr._id = SMReceipt.Global.Methods.RGenerator.PharmIdCode().Result; //BsonString.Create(BsonObjectId.GenerateNewId().AsString).AsString;
            MongoDS.PharmInsert.InSert(phr);
            Task.Run(()=>MongoDS.PharmInsert.InSertForDanya(phr));
            //Task.Run(() => MongoDS.PharmInsert.InSertForDanya(phr));
            if (MongoDS.GetPharmIdByCode.Get(phr._id) == 0) MongoDS.PharmInsert.InSert(phr);
            //  if (MongoDS.GetPharmIdByCode.GetShortFD(phr._id) == 0 || MongoDS.GetPharmIdByCode.GetLongFD(phr._id) == 0) MongoDS.PharmInsert.InSertForDanya(phr);
            if (MongoDS.GetPharmIdByCode.GetLongFD(phr._id) == 0 || MongoDS.GetPharmIdByCode.GetShortFD(phr._id) == 0) MongoDS.PharmInsert.InSertForDanya(phr);
          //  if (MongoDS.GetPharmIdByCode.GetShortFD(phr._id) == 0) MongoDS.PharmInsert.InSertShortForDanya(phr);

            #region oldCode
            return;
            // 1. Create drugstore in lib 95.
            //List<object> modelling = new List<object> {
            /*new SMCore.Modeling.Models.FieldsUMKB
            {
                action = "add",
                model = "concept",
                lib = 95,
                level = "944",
                sort="1",
                text=cdsModel.Title,
            },*/
            /*
            new 
            {
                action = "add",
                csv_id="0",
                model = "bel",
                lib=95,
                level = "944",
                sort = "1"
            },
            new 
            {
                action = "add",
                model = "names",
                lib=95,
                csv_master_id = "0",
                text = cdsModel.Title
            },
            new 
            {
                action="add",
                model="varchar",
                lib=95,
                csv_master_id="0",
                field="address",
                value=cdsModel.Address,
            },
            new 
            {
                action="add",
                model="varchar",
                lib=95,
                csv_master_id="0",
                field="license",
                value=cdsModel.License,
            } };
        if (cdsModel.TypeText != null)
        {
            modelling.Add(
            new 
            {
                action = "add",
                model = "varchar",
                lib = 95,
                csv_master_id = "0",
                field = "type",
                value = cdsModel.TypeText,
            });
        }
        modelling.Add(
        new 
        {
            action = "add",
            model = "varchar",
            lib = 95,
            csv_master_id = "0",
            field = "phone",
            value = cdsModel.Phone,
        });
        modelling.Add(new // Link drugstore to the user
        {
            action = "add",
            model = "bel",
            lib = 905,
            level = "9112",
            lib_a = "902",
            id_a = cdsModel.UserId,
            lib_b = "95",
            csv_id_b = "0",
            sort = "1",
        });
        var request_modelling = new //new SMCore.Modeling.Models.Requests.RequestModeling
        {
            key = PharmTaxiAdm.Constants.ModellingKey,
            modeling = modelling,
            cache=true,
            mode = 0
        };
        var jsss = JsonConvert.SerializeObject(request_modelling);
        var response = UMKBRequests.WebUtils.Post(PharmTaxiAdm.Constants.UriModellingDEV, request_modelling.ToJson());*/
            #endregion
        }

        /// <summary>
        /// Создание Аптеки с Генерацией Кода
        /// </summary>
        /// <param name="cdsModel"></param>
        /// <returns></returns>
        public static async Task<CreatedDrugStore> CreateDrugstoreWCode(DrugstoreModel cdsModel)
        {
            int cnt = 0;
            UMKB_Packet UP = new UMKB_Packet(UMKB.Modelling);
            UP.user = PTConstants.Constants.ModellingUserSverdlov;
            UError hz = new UError();
            UMKB_Field APTEK = new UMKB_Field(null, null, null, null, null, 95, 944, ref cnt, out hz);
            string lic_code = SMReceipt.Global.Methods.RGenerator.PharmIdCode().Result;
            APTEK.code = lic_code;
            if (cdsModel.ParentId.HasValue) APTEK.parent_id = cdsModel.ParentId.Value.ToString();
            UP.modeling.Add(APTEK);
            UMKB_Field Anunak;
            //  UMKB_Field Anunak = UP.BelCreatePrev("902", cdsModel.Id.ToString(), APTEK, "9112", 905, ref cnt, null);  // 
            if (!(cdsModel.UserId is null))
            {
                Anunak = APTEK.BelAddInverse(uint.TryParse(cdsModel.UserId, out uint r) ? r : 0, null, 902, 920, 9112, ref cnt, out hz);
                //UMKB_Field Anunak = APTEK.bel


                UP.modeling.Add(Anunak);
            }
            UP.modeling.Add(APTEK.CrNamesToBel(cdsModel.Title));
            UP.modeling.Add(APTEK.AddNewVarcharTo(cdsModel.License, null, "license", APTEK.lib));

            UP.modeling.Add(APTEK.AddNewVarcharTo(cdsModel.Phone, null, "phone", APTEK.lib));
            UP.modeling.Add(APTEK.AddNewVarcharTo(cdsModel.Address, null, "address", APTEK.lib));
            UP.cache = true;
            string shit0 = Newtonsoft.Json.JsonConvert.SerializeObject(UP, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            string h = URLs.Modelling;
            UMKB_RespStat resp=new UMKB_RespStat();
            if (cdsModel.PharmId == 0)
            {
                UP.UMKBRequest(URLs.Modelling);
                string rrr = UMKBRequests.WebUtils.Post(URLs.Modelling, shit0, "application/json").Answer;
                resp = UP.URresult;
                string shit1 = Newtonsoft.Json.JsonConvert.SerializeObject(resp, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            if (cdsModel.PharmId != 0) Task.Run(()=>SetPharmCode(cdsModel.PharmId, lic_code));
            //string lic_code="";
            try
            {
                uint idApt = cdsModel.PharmId==0?(uint)resp.log[0].id:cdsModel.PharmId;
               // if (!(resp.log[0].id is null) || !(resp.log is null) || resp.log[0].id != "" || !(resp.log[0].id is null)) lic_code = SetPharmCode(idApt);
                Models.MongoDS.Pharms phr = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.MongoDS.Pharms>(Newtonsoft.Json.JsonConvert.SerializeObject(cdsModel));
                if(resp.log.Count>0) phr.PharmId = (uint)resp.log[0].id;
                else phr.PharmId = cdsModel.PharmId == 0 ? 0 : cdsModel.PharmId;
                phr._id = lic_code;
                MongoDS.PharmInsert.InSert(phr);
                await Task.Run(() => MongoDS.PharmInsert.InSertForDanya(phr));
                if (MongoDS.GetPharmIdByCode.Get(lic_code)==0)MongoDS.PharmInsert.InSert(phr);
                //if (MongoDS.GetPharmIdByCode.GetShortFD(lic_code) == 0 || MongoDS.GetPharmIdByCode.GetLongFD(lic_code) == 0) MongoDS.PharmInsert.InSertForDanya(phr);
               // if (MongoDS.GetPharmIdByCode.GetLongFD(lic_code) == 0 || MongoDS.GetPharmIdByCode.GetShortFD(lic_code) == 0) MongoDS.PharmInsert.InSertForDanya(phr);
               // if (MongoDS.GetPharmIdByCode.GetShortFD(lic_code) == 0) MongoDS.PharmInsert.InSertShortForDanya(phr);
            }
            catch
            {
                UMKBRequests.Modelling.U_AUD_Lib.UAlert al = new UAlert() { code = "400", title = "error", message = "No Drugstore created!" };
                CreatedDrugStore CdRes = new CreatedDrugStore() { alert = al, lic_code = lic_code };
                Models.MongoDS.Pharms phr = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.MongoDS.Pharms>(Newtonsoft.Json.JsonConvert.SerializeObject(cdsModel));
                phr.PharmId = cdsModel.PharmId==0?0:cdsModel.PharmId;
                phr._id = lic_code;
                MongoDS.PharmInsert.InSert(phr);
                await Task.Run(() => MongoDS.PharmInsert.InSertForDanya(phr));
                if (MongoDS.GetPharmIdByCode.Get(lic_code) == 0) MongoDS.PharmInsert.InSert(phr);
               // if (MongoDS.GetPharmIdByCode.GetLongFD(lic_code)==0 || MongoDS.GetPharmIdByCode.GetShortFD(lic_code) == 0) MongoDS.PharmInsert.InSertForDanya(phr);
              //  if (MongoDS.GetPharmIdByCode.GetShortFD(lic_code)==0) MongoDS.PharmInsert.InSertShortForDanya(phr);
                return CdRes;
            }
            #region oldCode
            CreatedDrugStore CdS = new CreatedDrugStore() { alert = new UAlert() { code = "200", level = "success", message = "drugstore created!", title = "success" }, lic_code = lic_code }; //{ alert = (resp.Alerts.Count==0)?new UAlert() { code="200", level="success", message="drugstore created!", title="success"}:resp.Alerts[0], lic_code = lic_code };
            return CdS;
        }


        public static void UpdateDrugstore(DrugstoreModel udsModel)
        {
            int cnt = 0;
            UMKB_Packet UP = new UMKB_Packet(UMKB.Modelling);
            UP.user = PTConstants.Constants.ModellingUserSverdlov;
            UError hz = new UError();
            UMKB_Field APTEK = new UMKB_Field("", "", "", "", udsModel.Title, 0, out cnt, 95, "944",null,"concept","add");
            APTEK.mode = 3;
            APTEK.id = (int)(udsModel.LongId*0.0001);
            APTEK.action = "updat";
            //UP.modeling.Add(APTEK);
            var nameBel = APTEK.CrNamesToBel(udsModel.Title);
            nameBel.id = (int)(udsModel.NameLongId * 0.0001) ;
            nameBel.field = "name";
            nameBel.mode = 3;
            nameBel.value = udsModel.Title;
            nameBel.action = "updat";
            UP.modeling.Add(nameBel);
            var vc1 = APTEK.AddNewVarcharTo(udsModel.License, null, "license", APTEK.lib);
            vc1.id = udsModel.LicenseVCId.GetValueOrDefault();
            //vc1.action = "updat";
            UP.modeling.Add(vc1);
            var vc2 = APTEK.AddNewVarcharTo(udsModel.Phone, null, "phone", APTEK.lib);
            vc2.id = udsModel.PhoneVCId.GetValueOrDefault();
            //vc2.action = "updat";
            UP.modeling.Add(vc2);
            var vc3 = APTEK.AddNewVarcharTo(udsModel.Address, null, "address", APTEK.lib);
            vc3.id = udsModel.AddressVCId.GetValueOrDefault();
            //vc3.action = "updat";
            UP.modeling.Add(vc3);
            UP.cache = false;
            string shit0 = Newtonsoft.Json.JsonConvert.SerializeObject(UP, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            UP.UMKBRequestSP(URLs.Modelling);
            string shit1 = Newtonsoft.Json.JsonConvert.SerializeObject(UP.URresult, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
        public static HashSet<DrugstoreModel> GetDrugstoresByUserId(string userid)
        {
            if (string.IsNullOrWhiteSpace(userid)) { throw new Exception("userid is null"); }

            if (userid == "0") { throw new Exception("userid = 0"); }

            HashSet<DrugstoreModel> result = new HashSet<DrugstoreModel>();
            //Dictionary <ulong, (ulong? parent_id, string type, string title)> result = new Dictionary<ulong, (ulong? parent_id, string type, string title)>();

            var libid = new HashSet<string> { userid + "0902" };
            UMKBRequests.GetSemanticNewBigId getSemantic = new UMKBRequests.GetSemanticNewBigId();
            var req_sem = new UMKBRequests.Models.API.Semantic.RequestSemantic
            {
                key= UMKB.Graph,
                lib = new HashSet<ushort> { 95, 902},
                libid = libid,//new HashSet<string> { "75430902", "3433220095", "22100920" },
                //SelectLevelb = new HashSet<ushort> { 9112, 944 },
                //libid = new HashSet<string> { "950343322", },
                level = new List<ushort> { 944, 951, 9112, 9111, 9113, 9114, 1050, 1051 },//, 9115, 9110 },
                deep = 10,
                route = 0,
            };
            //var reqSemJson = req_sem.ToJson();
            var resultSemantic = getSemantic.Get(req_sem,false);
            //var resSemJson = JsonConvert.SerializeObject(resultSemantic);
            var libids95 = new HashSet<string>(); //< (ulong libid, ushort level)>();
            var pairs = new HashSet<UMKBNeedStuff.Pair>();
            HashSet<string> longids = new HashSet<string>();
            foreach (var graph in resultSemantic.graph)
            {
                if (graph.idb % 10000 == 95)
                {
                    libids95.Add(graph.idb.ToString());//, graph.levelb));
                    result.Add(new DrugstoreModel { LongId = graph.idb, ParentId = graph.parent_id, TypeText = resultSemantic.levels[graph.levelb] });
                    longids.Add(graph.idb.ToString());
                    pairs.Add(new UMKBNeedStuff.Pair { id = (uint)(graph.idb*0.0001), lib = 95 });
                }
            }
            var req_names = new SMCore.Names.Models.Requests.RequestNames
            {
                permit = new UMKBNeedStuff.DataContainer.Billing.Permit { key = UMKB.GetNamesDB },
                libids = pairs.ToList()
            };
            //UMKBNeedStuff.ResponseGetNamesMoreDB resultNames = SMCore.Names.Methods.Names.GetNamesDB(req_names, SMCore.Config.Server.Dev);
            var resultNames = SMUtility.WebUtility.Post(URLs.GetNamesDB, JsonConvert.SerializeObject(req_names), PTConstants.Constants.ContentTypeJson);
            var dResultNames = JsonConvert.DeserializeObject<Dictionary<string, List<FieldsNames>>>(JObject.Parse(resultNames.Answer)["result"].ToString());

            foreach (var resName in dResultNames)
            {
                foreach(var res in result)
                {
                    if( res.LongId.ToString() == resName.Key)
                    {
                        foreach(var resN in resName.Value)
                        {
                            if (resN.field == "name")
                            {
                                res.NameLongId = (ulong)resN.id*10000+95;
                                res.Title = resN.text;
                                break;
                            }
                        }
                        //res.Title = resName.Value[0].;
                        
                    }
                }
            }
            var req = new {
                Key = UMKB.GetVarcharWhere,
                Request = new Dictionary < ushort, Dictionary<string, HashSet<string>> >()
            };
            foreach(var pair in pairs)
            {
                HashSet<string> filter_field = new HashSet<string> { "address", "license", "type", "phone", "site" };
                if (req.Request.TryAdd(pair.lib, new Dictionary<string, HashSet<string>> { { "field" , filter_field },{ "master_id", new HashSet<string> { pair.id.ToString() } } })) continue;
                req.Request[pair.lib]["master_id"].Add(pair.id.ToString());
            }
            var requJson = JsonConvert.SerializeObject(req);
            var resp = UMKBRequests.WebUtils.Post(URLs.GetVarcharWhere, requJson);
            //var resultVC = JsonConvert.DeserializeObject<new {<string,Dictionary<string, List< UMKBNeedStuff.fieldsVarchar>>>>(resp.Answer);
            var resJO = JObject.Parse(resp.Answer);
            var resultVC = JsonConvert.DeserializeObject<List<UMKBNeedStuff.Models.FieldsVarchar>>(resJO["result"]["95"].ToString()) ;
            foreach (var resVC in resultVC)
            {

                //foreach (var resVC in resVC1)
                {
                    foreach (var res in result)
                    {
                        if ((int)(res.LongId * 0.0001) == resVC.master_id)
                        {
                            res.PharmId = (uint)resVC.master_id;
                            switch (resVC.field)
                            {
                                case "address": res.Address = resVC.value; res.AddressVCId = (int?)resVC.id;  break;
                                case "license": res.License = resVC.value; res.LicenseVCId = (int?)resVC.id; break;
                                case "type": res.TypeText = resVC.value; break;
                                case "phone": res.Phone = resVC.value; res.PhoneVCId = (int?)resVC.id; break;
                                //case "site"
                            }
                        }
                    }
                }
            }
           
            
            libid = new HashSet<string>();
            foreach (var res in result)
            {
                libid.Add(res.LongId.ToString());
            }
            getSemantic = new UMKBRequests.GetSemanticNewBigId();
            req_sem = new UMKBRequests.Models.API.Semantic.RequestSemantic
            {
                key = UMKB.Graph,
                lib = new HashSet<ushort> { 920 },
                libid = libid,//new HashSet<string> { "75430902", "3433220095", "22100920" },
                //SelectLevelb = new HashSet<ushort> { 9112, 944 },
                //libid = new HashSet<string> { "950343322", },
                level = new List<ushort> { 9112 },
                deep = 10,
                route = 2,
            };
            //reqSemJson = req_sem.ToJson();
            resultSemantic = getSemantic.Get(req_sem, false);
            foreach(var res in resultSemantic.graph)
            {
                if(res.levelb==951 && res.idb % 10000 == 902)
                {
                    foreach(var res1 in result)
                    {
                        if (res1.LongId == res.ida) res1.Users.Add(new Models.User.UserModel { LongId = res.idb });
                    }
                }
            }
            //new UMKB_Field().BelAddInverse(123, null, 902, 920, 9112, ref cnt, out hz);
            #endregion
            return result;
        }

        public static ulong[] GetLinkPharmUserByPh(uint? drugstore_id, uint? user_id=null)
        {
            var libid = new HashSet<string> { (drugstore_id.Value.ToString().EndsWith("0095")?drugstore_id.Value.ToString():drugstore_id.Value.ToString()+"0095") };
            UMKBRequests.GetSemanticNewBigId getSemantic = new UMKBRequests.GetSemanticNewBigId();
            var req_sem = new UMKBRequests.Models.API.Semantic.RequestSemantic
            {
                key = UMKB.Graph,
                lib = new HashSet<ushort> { 95 },
                libid = libid,//new HashSet<string> { "75430902", "3433220095", "22100920" },
                SelectLevelb = (user_id.HasValue) ? new HashSet<ushort>() { 951 } : null,
                SelectLongIDB = (user_id.HasValue) ? new HashSet<ulong>() { user_id.Value.ToString().EndsWith("0095") ? (ulong)user_id.Value : ulong.TryParse(user_id.Value.ToString() + "0095", out ulong uzid)?uzid:(ulong)0 }:null,
                //libid = new HashSet<string> { "950343322", },
                level = new List<ushort> { 9112 },
                deep = 9,
                route = 2,
            };
            var reqSemJson = req_sem.ToJson();
            var resultSemantic = getSemantic.Get(req_sem, false);
            int grphCnt = (resultSemantic.graph is null)?0:resultSemantic.graph.Count;
           // List<uint> x = null;
            if (grphCnt == 0) return null;
            else return (resultSemantic.graph.Select(x=>x.id).ToArray());
        }

        public static HashSet<DrugstoreModel> GetDrugstoresByPharmId(string pharmid)
        {
            HashSet<DrugstoreModel> result = new HashSet<DrugstoreModel>();
            //Dictionary <ulong, (ulong? parent_id, string type, string title)> result = new Dictionary<ulong, (ulong? parent_id, string type, string title)>();

            var libid = new HashSet<string> { pharmid + "0095" };
            UMKBRequests.GetSemanticNewBigId getSemantic = new UMKBRequests.GetSemanticNewBigId();
            var req_sem = new UMKBRequests.Models.API.Semantic.RequestSemantic
            {
                key = UMKB.Graph,
                lib = new HashSet<ushort> { 95 },
                libid = libid,//new HashSet<string> { "75430902", "3433220095", "22100920" },
                //SelectLevelb = new HashSet<ushort> { 948, 944, 9112, 951, 9116 },
                //libid = new HashSet<string> { "950343322", },
                level = new List<ushort> { 944,948, 1050, 1051, 951, 9112, 9111, 9113, 9114, 9115, 9110 },
                deep = 9,
                route = 2,
            };
            var reqSemJson = req_sem.ToJson();
            var resultSemantic = getSemantic.Get(req_sem, false);

                req_sem = new UMKBRequests.Models.API.Semantic.RequestSemantic
                {
                    key = UMKB.Graph,
                    lib = new HashSet<ushort> { 95 },
                    libid = resultSemantic.graph.Where(x => x.levelb == 944 || x.levelb == 948 || x.levelb == 1).Select(x=>x.idb.ToString()).ToList(),//new HashSet<string> { "75430902", "3433220095", "22100920" },
                                  //SelectLevelb = new HashSet<ushort> { 948, 944, 9112, 951, 9116 },
                                  //libid = new HashSet<string> { "950343322", },
                    level = new List<ushort> { 944, 948, 1050, 1051, 951, 9112, 9111, 9113, 9114, 9115, 9110 },
                    deep = 5,
                    route = 0,
                };
            var rs = getSemantic.Get(req_sem, false);
            resultSemantic.graph.AddRange(rs.graph);
            resultSemantic.levels.Add(948, "Аптечная сеть");
           // resultSemantic.names.AddRange(rs.names);
            var resSemJson = JsonConvert.SerializeObject(resultSemantic);
            var libids95 = new HashSet<string>(); //< (ulong libid, ushort level)>();
            var pairs = new HashSet<UMKBNeedStuff.Pair>();
            HashSet<string> longids = new HashSet<string>();
            foreach (var graph in resultSemantic.graph)
            {
                if (graph.idb % 10000 == 95)
                {
                    libids95.Add(graph.idb.ToString());//, graph.levelb));
                    result.Add(new DrugstoreModel { LongId = graph.idb, ParentId = graph.parent_id, TypeText = resultSemantic.levels[graph.levelb] });
                    longids.Add(graph.idb.ToString());
                    pairs.Add(new UMKBNeedStuff.Pair { id = (uint)(graph.idb * 0.0001), lib = 95 });
                }
            }
            var req_names = new SMCore.Names.Models.Requests.RequestNames
            {
                permit = new UMKBNeedStuff.DataContainer.Billing.Permit { key = UMKB.GetNamesDB },
                libids = pairs.ToList()
            };
            //UMKBNeedStuff.ResponseGetNamesMoreDB resultNames = SMCore.Names.Methods.Names.GetNamesDB(req_names, SMCore.Config.Server.Dev);
            var resultNames = SMUtility.WebUtility.Post(URLs.GetNamesDB, JsonConvert.SerializeObject(req_names), PTConstants.Constants.ContentTypeJson);
            var dResultNames = JsonConvert.DeserializeObject<Dictionary<string, List<FieldsNames>>>(JObject.Parse(resultNames.Answer)["result"].ToString());

            foreach (var resName in dResultNames)
            {
                foreach (var res in result)
                {
                    if (res.LongId.ToString() == resName.Key)
                    {
                        foreach (var resN in resName.Value)
                        {
                            if (resN.field == "name")
                            {
                                res.NameLongId = (ulong)resN.id * 10000 + 95;
                                res.Title = resN.text;
                                break;
                            }
                        }
                        //res.Title = resName.Value[0].;

                    }
                }
            }
            var req = new
            {
                Key = UMKB.GetVarcharWhere,
                Request = new Dictionary<ushort, Dictionary<string, HashSet<string>>>()
            };
            foreach (var pair in pairs)
            {
                HashSet<string> filter_field = new HashSet<string> { "address", "license", "type", "phone", "site" };
                if (req.Request.TryAdd(pair.lib, new Dictionary<string, HashSet<string>> { { "field", filter_field }, { "master_id", new HashSet<string> { pair.id.ToString() } } })) continue;
                req.Request[pair.lib]["master_id"].Add(pair.id.ToString());
            }
            var requJson = JsonConvert.SerializeObject(req);
            var resp = UMKBRequests.WebUtils.Post(URLs.GetVarcharWhere, requJson);
            //var resultVC = JsonConvert.DeserializeObject<new {<string,Dictionary<string, List< UMKBNeedStuff.fieldsVarchar>>>>(resp.Answer);
            var resJO = JObject.Parse(resp.Answer);
            var resultVC = JsonConvert.DeserializeObject<List<UMKBNeedStuff.Models.FieldsVarchar>>(resJO["Result"]["95"].ToString());
            foreach (var resVC in resultVC)
            {

                //foreach (var resVC in resVC1)
                {
                    foreach (var res in result)
                    {
                        if ((int)(res.LongId * 0.0001) == resVC.master_id)
                        {
                            res.PharmId = (uint)resVC.master_id;
                            switch (resVC.field)
                            {
                                case "address": res.Address = resVC.value; res.AddressVCId = (int?)resVC.id; break;
                                case "license": res.License = resVC.value; res.LicenseVCId = (int?)resVC.id; break;
                                case "type": res.TypeText = resVC.value; break;
                                case "phone": res.Phone = resVC.value; res.PhoneVCId = (int?)resVC.id; break;
                                    //case "site"
                            }
                        }
                    }
                }
            }


            libid = new HashSet<string>();
            foreach (var res in result)
            {
                libid.Add(res.LongId.ToString());
            }
            getSemantic = new UMKBRequests.GetSemanticNewBigId();
            req_sem = new UMKBRequests.Models.API.Semantic.RequestSemantic
            {
                key = UMKB.Graph,
                lib = new HashSet<ushort> { 920 },
                libid = libid,//new HashSet<string> { "75430902", "3433220095", "22100920" },
                //SelectLevelb = new HashSet<ushort> { 9112, 944 },
                //libid = new HashSet<string> { "950343322", },
                level = new List<ushort> { 9112 },
                deep = 10,
                route = 2,
            };
            reqSemJson = req_sem.ToJson();
            resultSemantic = getSemantic.Get(req_sem, false);
            foreach (var res in resultSemantic.graph)
            {
                if (res.levelb == 951 && res.idb % 10000 == 902)
                {
                    foreach (var res1 in result)
                    {
                        if (res1.LongId == res.ida) res1.Users.Add(new Models.User.UserModel { LongId = res.idb });
                    }
                }
            }
            //new UMKB_Field().BelAddInverse(123, null, 902, 920, 9112, ref cnt, out hz);

            return result;
        }


        private static string SetPharmCode(uint id, string lic_code=null)
        {
            using (MySqlConnection cnn = new MySqlConnection(MySQL.UMKBConString))
            {
                cnn.Open();
                using (MySqlCommand cmd = new MySqlCommand(Methods.SQLRequests.SetCode, cnn))
                {
                    string ret = (String.IsNullOrEmpty(lic_code))?SMReceipt.Global.Methods.RGenerator.PharmIdCode().Result:lic_code;
                    cmd.Parameters.Add("code", MySqlDbType.VarChar);
                    cmd.Parameters["code"].Value = ret;
                    cmd.Parameters.Add("id", MySqlDbType.UInt64);
                    cmd.Parameters["id"].Value = id;
                    //if (mode == 0) this.TorgName = cmd.ExecuteScalar().ToString();
                    // else if (mode == 1) this.FarmPrep = cmd.ExecuteScalar().ToString();
                    cnn.Close();
                    return ret;
                }
            }
        }

        public static string GetPharmByCode(string code)
        {
            using (MySqlConnection cnn = new MySqlConnection(MySQL.UMKBConString)) 
            {
                cnn.Open();
                using (MySqlCommand cmd = new MySqlCommand(Methods.SQLRequests.GetByCode, cnn))
                {
                    cmd.Parameters.Add("code", MySqlDbType.VarChar);
                    cmd.Parameters["code"].Value = code;
                    string ret = cmd.ExecuteScalar() is null ? "" : cmd.ExecuteScalar().ToString();
                    //if (mode == 0) this.TorgName = cmd.ExecuteScalar().ToString();
                    // else if (mode == 1) this.FarmPrep = cmd.ExecuteScalar().ToString();
                    cnn.Close();
                    return ret;
                }
            }
        }

        public static string GetCodeByPharm(string pharmId)
        {
            using (MySqlConnection cnn = new MySqlConnection(MySQL.UMKBConString))
            {
                cnn.Open();
                using (MySqlCommand cmd = new MySqlCommand(Methods.SQLRequests.GetByPharmId, cnn))
                {
                    cmd.Parameters.Add("pharmId", MySqlDbType.VarChar);
                    cmd.Parameters["pharmId"].Value = pharmId;
                    string ret = cmd.ExecuteScalar() is null ? "" : cmd.ExecuteScalar().ToString();
                    //if (mode == 0) this.TorgName = cmd.ExecuteScalar().ToString();
                    // else if (mode == 1) this.FarmPrep = cmd.ExecuteScalar().ToString();
                    cnn.Close();
                    return ret;
                }
            }
        }


        public static uint GetUserIdByEmail(string email)
        {
            using (MySqlConnection cnn = new MySqlConnection(MySQL.UMKBConString))
            {
                cnn.Open();
                using (MySqlCommand cmd = new MySqlCommand(Methods.SQLRequests.GetUserByEmail, cnn))
                {
                    cmd.Parameters.Add("email", MySqlDbType.VarChar);
                    cmd.Parameters["email"].Value = email;
                    string ret = cmd.ExecuteScalar() is null ? "0" : cmd.ExecuteScalar().ToString();
                    //if (mode == 0) this.TorgName = cmd.ExecuteScalar().ToString();
                    // else if (mode == 1) this.FarmPrep = cmd.ExecuteScalar().ToString();
                    cnn.Close();
                    return uint.Parse(ret);
                }
            }
        }

        private static string CreateUserNoPass(string email)
        {
            using (MySqlConnection cnn = new MySqlConnection(MySQL.UMKBConString))
            {
                cnn.Open();
                using (MySqlCommand cmd = new MySqlCommand(Methods.SQLRequests.CreateUserWithNoPass, cnn))
                {
                    cmd.Parameters.Add("email", MySqlDbType.VarChar);
                    cmd.Parameters["email"].Value = email;
                    string ret = cmd.ExecuteScalar() is null ? "0" : cmd.ExecuteScalar().ToString();
                    //if (mode == 0) this.TorgName = cmd.ExecuteScalar().ToString();
                    // else if (mode == 1) this.FarmPrep = cmd.ExecuteScalar().ToString();
                    cnn.Close();
                    return ret;
                }
            }
        }

        private static string UpdateUserNoPass(string email, uint id)
        {
            using (MySqlConnection cnn = new MySqlConnection(MySQL.UMKBConString))
            {
                cnn.Open();
                using (MySqlCommand cmd = new MySqlCommand(Methods.SQLRequests.UpdateUserWithNoPass, cnn))
                {
                    cmd.Parameters.Add("email", MySqlDbType.VarChar);
                    cmd.Parameters["email"].Value = email;
                    cmd.Parameters.Add("id", MySqlDbType.UInt32);
                    cmd.Parameters["id"].Value = id;
                    string ret = cmd.ExecuteScalar() is null ? "0" : cmd.ExecuteScalar().ToString();
                    //if (mode == 0) this.TorgName = cmd.ExecuteScalar().ToString();
                    // else if (mode == 1) this.FarmPrep = cmd.ExecuteScalar().ToString();
                    cnn.Close();
                    return ret;
                }
            }
        }

        public static string GetVerCode(uint id)
        {
            using (MySqlConnection cnn = new MySqlConnection(MySQL.UMKBConString))
            {
                cnn.Open();
                using (MySqlCommand cmd = new MySqlCommand(Methods.SQLRequests.GetUserConfirCode, cnn))
                {
                    cmd.Parameters.Add("id", MySqlDbType.UInt32);
                    cmd.Parameters["id"].Value = id;
                    string ret = cmd.ExecuteScalar() is null ? "0" : cmd.ExecuteScalar().ToString();
                    //if (mode == 0) this.TorgName = cmd.ExecuteScalar().ToString();
                    // else if (mode == 1) this.FarmPrep = cmd.ExecuteScalar().ToString();
                    cnn.Close();
                    return ret;
                }
            }
        }

        public static string GetEmailById(uint id)
        {
            using (MySqlConnection cnn = new MySqlConnection(MySQL.UMKBConString))
            {
                cnn.Open();
                using (MySqlCommand cmd = new MySqlCommand(Methods.SQLRequests.GetUserEmailByUserID, cnn))
                {
                    cmd.Parameters.Add("id", MySqlDbType.UInt32);
                    cmd.Parameters["id"].Value = id;
                    string ret = cmd.ExecuteScalar() is null ? "0" : cmd.ExecuteScalar().ToString();
                    //if (mode == 0) this.TorgName = cmd.ExecuteScalar().ToString();
                    // else if (mode == 1) this.FarmPrep = cmd.ExecuteScalar().ToString();
                    cnn.Close();
                    return ret;
                }
            }
        }

        private static string MakingRealSlave(uint slave_id, uint master_id, string email)
        { 
            using (MySqlConnection cnn = new MySqlConnection(MySQL.UMKBConString))
            {
                cnn.Open();
                using (MySqlCommand cmd = new MySqlCommand(Methods.SQLRequests.MakingRealSlave, cnn))
                {
                    cmd.Parameters.Add("email", MySqlDbType.VarChar);
                    cmd.Parameters["email"].Value = email;
                    cmd.Parameters.Add("id", MySqlDbType.UInt32);
                    cmd.Parameters["id"].Value = slave_id;
                    cmd.Parameters.Add("parent_id", MySqlDbType.UInt32);
                    cmd.Parameters["parent_id"].Value = master_id;
                    string ret = cmd.ExecuteScalar() is null ? "0" : cmd.ExecuteScalar().ToString();
                    //if (mode == 0) this.TorgName = cmd.ExecuteScalar().ToString();
                    // else if (mode == 1) this.FarmPrep = cmd.ExecuteScalar().ToString();
                    cnn.Close();
                    return ret;
                }
            }
        }

        public static string GetIdByCode(string code, string service)
        {
            SettingGraph sg = new SettingGraph() 
            {
                key = UMKB.Graph, 
                route = 2, 
                SelectLongIDB = new HashSet<ulong>(70099), 
                libid = new List<string>() { "70099", "990000007" }, 
                lib=new List<ushort>() { 99, 905, 902 },
                level = new List<ushort>() { 9115, 951 } 
            };

            UMKBRequests.GetSemanticNewBigId gs = new UMKBRequests.GetSemanticNewBigId();
            string res= UMKBRequests.WebUtils.Post("https://cs.socmedica.com/api/umkb/getsemanticnewbigid", Newtonsoft.Json.JsonConvert.SerializeObject(sg), "application/json").Answer;
          //  UMKBRequests.Models.API.Semantic.PlexNewBigId req=new UMKBRequests.Models.API.Semantic.PlexNewBigId() { ke=Constants.Keys.GraphKey, route=2,  }

            return "";
        }

        public static SMReceipt.Global.Models.SaveOrder GetPharmOrders(uint? PharmId)
        {
            return Task.Run(()=>SMReceipt.Global.Methods.Mongo.GetPharmOrders(PharmId)).Result;
        }

        public static async Task<InviteResp> SlaveCreation2(AuthKeyByLicKey data)
        {
            InviteReq Req = new InviteReq();
            Req.application = data.slave_service;
            Req.authkey = data.authkey;
            Req.key = UMKB.Invite;
            Req.invitedEmail = data.email;
            Req.invitedRoleCodes.Add(GetCodeByPharm(data.drugstore_id));
            var resp = UMKBRequests.WebUtils.Post(URLs.Invite, Newtonsoft.Json.JsonConvert.SerializeObject(Req), "application/json").Answer;
            InviteResp crG = new InviteResp();
            try
            {
                crG = Newtonsoft.Json.JsonConvert.DeserializeObject<InviteResp>(resp);
            }catch(Exception e)
            {
                crG = new InviteResp() { code = e.Message, title = e.Message, message = e.StackTrace };
                //crG.alerts.Add(new Models.User.Alert() { code = e.Message, title=e.Message, message = e.StackTrace });
            }
            return crG;
        }

        public static async Task<InviteResp> SlaveCreation3(InviteViaEmail data)
        {
            InviteReq Req = new InviteReq();
            Req.application = data.application;
            Req.authkey = data.authkey;
            Req.key = UMKB.Invite;
            Req.invitedEmail = data.invitedEmail;
            Req.invitedRoleCodes=data.invitedRoleCodes;
            var resp = UMKBRequests.WebUtils.Post(URLs.Invite, Newtonsoft.Json.JsonConvert.SerializeObject(Req), "application/json").Answer;
            InviteResp crG = new InviteResp();
            try
            {
                crG = Newtonsoft.Json.JsonConvert.DeserializeObject<InviteResp>(resp);
            }
            catch (Exception e)
            {
                crG = new InviteResp() { code = e.Message, title = e.Message, message = e.StackTrace };
                //crG.alerts.Add(new Models.User.Alert() { code = e.Message, title = e.Message, message = e.StackTrace });
            }
            return crG;
        }

        public static async Task<UMKBRequests.Alert> SlaveCreation(AuthKeyByLicKey data)
        {


            UMKBRequests.Alert ret = new UMKBRequests.Alert();
            uint CheckUsId = GetUserIdByEmail(data.email);
            //UMKBNeedStuff.Responses.ResponseAlertData<RDGenerateUserByAuthkey> UId = Newtonsoft.Json.JsonConvert.DeserializeObject<UMKBNeedStuff.Responses.ResponseAlertData<RDGenerateUserByAuthkey>>(Methods.User.UserMethods.GenerateUser(data.authkey).Answer);
            string rsp;
            if (CheckUsId == 0) 
                rsp = User.UserMethods.Register(new UserRegistrationReq() { 
                    email = data.email, 
                    lastName = data.email, 
                    middleName = data.email, 
                   // fio = data.email, 
                    firstName = data.email, 
                    password = "password", 
                    service = data.slave_service 
                }).Answer;
            else
            {
                ret.code = "400";
                ret.level = "error";
                ret.message = "User with this email already registred";
                ret.title = "error";
                //ret.sticky = true;
                return ret;
            }
            string Code = GetVerCode(CheckUsId);
            string req = "https://db.socmedica.com/pharmtaxi/confirm/" + Code + '/' + data.email + '/' + data.slave_service;
            var xz = UMKBRequests.WebUtils.Get(req);
            try
            {
               // string xx= (Methods.User.UserMethods.GenerateUser(data.authkey).Answer);
                //string CrUnP = UpdateUserNoPass(data.email, UId.data.user_id);
                ret.code = "200";
                ret.level = "success" ;
                ret.message = "success";
                ret.title = "success";
                SlaveEmailing(data.email, data.slave_service);
                if (!String.IsNullOrEmpty(data.drugstore_id)) User.UserMethods.AttachDrugstoreToUser(CheckUsId, uint.TryParse(data.drugstore_id, out uint x) ? x : 0);
                if (!String.IsNullOrEmpty(data.authkey))
                {
                    Models.User.AlertUserID AuId = User.UserMethods.GetUserIdByAuthkey(data.authkey);
                    MakingRealSlave(CheckUsId, AuId.userid, data.email);
                }
                return ret;
            }
            catch (Exception e)
            {
                ret.code = "400";
                ret.level = "error";
                ret.message = e.Message;
                ret.title ="error";
                return ret;
            }
            
        }

        private static async void SlaveEmailing(string email, string service="6")
        {
            Models.User.PassRecovery pr = new PassRecovery() { email = email, service = service, key= UMKB.PassReset };
            string ser=UMKBRequests.WebUtils.Post(URLs.PassRecover, Newtonsoft.Json.JsonConvert.SerializeObject(pr), "application/json").Answer;
        }

        public static UMKBUtils.BigIdCryptography Crypto = new UMKBUtils.BigIdCryptography(PharmTaxiAdm.Models.Crypto.Password);
        public static void WriteOff(WriteOffRequest data)
        {
            #region getting order meds
            SMReceipt.Global.Models.OrderToSave Order = new SMReceipt.Global.Models.OrderToSave();
            UsefullStuff.Common.Methods.MongoTools.Methods.Write<AUpload.Global.Models.AssortmentItemData> BigAssWrt = new UsefullStuff.Common.Methods.MongoTools.Methods.Write<AUpload.Global.Models.AssortmentItemData>();
            UsefullStuff.Common.Methods.MongoTools.Methods.Write<AUpload.Global.Models.AssFD> SmallAssWrt = new UsefullStuff.Common.Methods.MongoTools.Methods.Write<AUpload.Global.Models.AssFD>();
            if (!String.IsNullOrEmpty(data.recipe))
            {
                try
                {
                    SMReceipt.Global.Models.OrderToSave Filtr = new SMReceipt.Global.Models.OrderToSave() { order_id = data.recipe, pharm_id = uint.Parse(data.PharmId) };
                    UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Mongo.MongoDB, Mongo.Orders_DataBase, Mongo.PHTFORSTFR);

                    var Fndd = UsefullStuff.Common.Methods.MongoTools.Methods.Read<SMReceipt.Global.Models.OrderToSave>.AllFiltered(Filtr);
                    if (!(Fndd is null))
                    {
                        Order = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<SMReceipt.Global.Models.OrderToSave>(Fndd.FirstOrDefault());
                    }
                }
                catch (Exception ExT)
                {

                }
            }
            #endregion
            if (!(data.medicaments is null))
            {
                foreach (var i in data.medicaments)
                {
                    //if(!(Order.drugs is null))
                    //{
                    //    if (!String.IsNullOrEmpty(i.drugstore_drug_id))
                    //    {
                    //        if (!Order.drugs.Select(x => x.drugstore_drug_id).Contains(i.drugstore_drug_id))
                    //        {
                    //            AUpload.Global.Models.AssortmentItemData assB = new AUpload.Global.Models.AssortmentItemData();
                    //        }
                    //    }
                    //    else if (!String.IsNullOrEmpty(i.id))
                    //    {
                    //        if (!Order.drugs.Select(x => x.id).Contains(i.id))
                    //        {

                    //        }
                    //    }
                    //    else if (!String.IsNullOrEmpty(i.tn))
                    //    {
                    //        if (!Order.drugs.Select(x => x.tn).Contains(i.tn))
                    //        {

                    //        }
                    //    }
                    //}


                    AssItemData ass = new AssItemData();
                    if (!(i.drugstore_drug_id is null)) ass.PharmPackId = i.drugstore_drug_id;
                    else ass.TradeName = i.name;
                    if (data.drugstore_id is null)
                    {
                        //Dictionary<string, uint> dct = PharmApi.GetPharmMapping(data.auth_key, Core.Data.ConnectionMode.Work);
                        ass.PharmId = PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.GetDrugstoresByUserId(PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(data.authkey).userid.ToString()).FirstOrDefault().PharmId;//dct.Values.FirstOrDefault();
                    }
                    else ass.PharmId = uint.TryParse(data.drugstore_id, out uint x) ? x : 0;
                    ass.Quantity = long.TryParse(i.packs_string, out long z) ? z : 0;
                    if (!String.IsNullOrEmpty(i.drugstore_drug_id)) ass.PharmPackId = i.drugstore_drug_id;
                    if (!String.IsNullOrEmpty(i.id)) ass.PackId = i.id;
                    ass.SellingPrice = (float?)i.price;
                    AUpload.Global.Models.AssortmentItemData AssToPass = Newtonsoft.Json.JsonConvert.DeserializeObject<AUpload.Global.Models.AssortmentItemData>(Newtonsoft.Json.JsonConvert.SerializeObject(ass));
                    //  AUpload.Config.Setup(Constants.Consts.SQLConnectionString, Constants.Keys.AbdParseKey, Constants.Keys.TableGetKey, Constants.Keys.GraphKey, Constants.Keys.ConceptKey, Crypto, Constants.Keys.GraphListKey, Constants.Keys.ConceptListKey);           
                    AUpload.Config.MongoDb = Mongo.MongoDB;
                    AUpload.Config.ConfigMongoAddress(true);
                    AUpload.Config.MongoBase = Mongo.PHTFORSTFR;
                    if (data.status == "2")
                    {
                        string test = Newtonsoft.Json.JsonConvert.SerializeObject(new AUpload.Global.Models.Input() { Lass = new List<AUpload.Global.Models.AssortmentItemData>() { AssToPass }, AvalIds = new List<uint>() { (uint)ass.PharmId } });
                        var res = AUpload.Global.Methods.ChangeAssortiment.WriteOff(new AUpload.Global.Models.Input() { Lass = new List<AUpload.Global.Models.AssortmentItemData>() { AssToPass }, AvalIds = new List<uint>() { (uint)ass.PharmId } }).Result;
                        data.auth_key = res.message;
                    }
                    if (!String.IsNullOrEmpty(data.recipe)) SMReceipt.Global.Methods.Mongo.CloseOrder(data.recipe, (uint)ass.PharmId, int.Parse(data.status));
                    //AUpload.Upload.Mongo.Methods.Ass_upsert.UpSert(ass);
                    //AUpload.Global.Methods.RemoveAssortiment.Remove(new List<AUpload.Global.Models.AssortmentItemData>() { ass }, new List<uint>() { (uint)ass.PharmId });
                    //Core.MongoDB.Methods.Ass_upsert.UpSert(ass);
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(data.recipe)) SMReceipt.Global.Methods.Mongo.CloseOrder(data.recipe, uint.Parse(data.PharmId), int.Parse(data.status));
            }

        }

    }
}
