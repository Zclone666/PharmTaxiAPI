using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;
using PTConstants;
using PharmTaxiAdm.Models.User;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UMKBNeedStuff.Responses.Billing.ResponseData;
using UMKBRequests.Modelling.U_AUD_Lib;

namespace PharmTaxiAdm.Methods.User
{
    public class UserMethods
    {

        public static string Authorize(string email, string password, string service = null) //UMKBRequests.WebUtils.Result
        {
            AuthParameter authParameter = new AuthParameter
            {
                email = email,
                password = password,
                key = UMKB.Authorization,
                service = string.IsNullOrEmpty(service) ? PTConstants.Constants.FTServiceID : service
            };
            string data = authParameter.ToJson();
            var webResult = UMKBRequests.WebUtils.Post(URLs.Authorization, data, UMKBRequests.WebUtils.WebContentType.JSON);
            AuthK res = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthK>(webResult.Answer);
            if (String.IsNullOrEmpty(service))
            {
                AlertUserID AUid = GetUserIdByAuthkey(res.authkey);
                service = AUid.role;
            }
            res.role = (service == "7" || service == "6") ? (service == "7" ? "Admin" : "Provisor") : service;
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }

        public static string Authorize2(string email, string password, string service = null) //UMKBRequests.WebUtils.Result
        {
            InvitedUserToMongo UserMongo = new InvitedUserToMongo();
            UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Mongo.MongoDB, Mongo.UsersCollection, Mongo.PHTFORSTFR);
            InvitedUserToMongo Flt = new InvitedUserToMongo() { email = email };
            foreach (var i in UsefullStuff.Common.Methods.MRTools.GetFields(Flt)) { if (i != "email") UsefullStuff.Common.Methods.MRTools.SetVal(i, null, Flt); }
            Task<List<BsonDocument>> UsTsk = Task<List<BsonDocument>>.Run(() => UsefullStuff.Common.Methods.MongoTools.Methods.Read<InvitedUserToMongo>.AllFiltered(Flt));
            if (String.IsNullOrEmpty(service))
            {
                ResponseGetUserInfoByEmail ToGetServ = GetUserInfoByEmail(new GetUserInfo() { email = email });
                if (!(ToGetServ.services is null)) service = (ToGetServ.services.Contains(7)) ? "7" : (ToGetServ.services.Contains(6)) ? "6" : (ToGetServ.services.Contains(5)) ? "5" : null;
            }

            AuthParameter authParameter = new AuthParameter
            {
                email = email,
                password = password,
                key = UMKB.Authorization,
                service = string.IsNullOrEmpty(service) ? PTConstants.Constants.FTServiceID6 : service
            };
            string data = authParameter.ToJson();
            var webResult = UMKBRequests.WebUtils.Post(URLs.Authorization, data, UMKBRequests.WebUtils.WebContentType.JSON);
            AuthK res = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthK>(webResult.Answer);
            AlertUserID AUid;
            if (String.IsNullOrEmpty(service))
            {
                AUid = GetUserIdByAuthkey(res.authkey);
                service = AUid.role;
            }
            AUid = GetUserIdByAuthkey(res.authkey);
            AuthKUserId res2 = new AuthKUserId();
            res.role = (service == "7" || service == "6") ? (service == "7" ? "Admin" : "Provisor") : service;
            res2.alert = res.alert;
            res2.userid = AUid.userid;
            res2.authkey = res.authkey;
            res2.role = res.role;
            var r = UsTsk.Result;
            if (service != "5" && r is null || (!(r is null) && r.Count == 0) && service != "5")
            {
                #region SAVE to MONGO
                List<uint> PharmsOfUser = new List<uint>();
                string dt = data;
                try
                {
                    var apt = Drugstore.DrugstoreMethods.GetDrugstoresByUserId(AUid.userid.ToString());
                    PharmsOfUser.AddRange(apt.Select(x => x.PharmId));
                }
                catch { }
                InvitedUserToMongo SaveUserMng = new InvitedUserToMongo() { email = email, createdAt = DateTime.Now, isGuest = false, permissions = new List<uint>() { uint.TryParse(service, out uint srvs) ? srvs : 7 }, distance = 10 };
                SaveUserMng.role = (service == "7" || service == "6") ? (service == "7" ? "Admin" : "Provisor") : service;
                SaveUserMng.user_id = AUid.userid;
                SaveUserMng.pharms = PharmsOfUser;
                SaveUserMng.isEmailConfirmed = true;
                UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Mongo.MongoDB, Mongo.UsersCollection, Mongo.PHTFORSTFR);
                UsefullStuff.Common.Methods.MongoTools.Methods.Write<InvitedUserToMongo> Wrt = new UsefullStuff.Common.Methods.MongoTools.Methods.Write<InvitedUserToMongo>();
                InvitedUserToMongo Filt = new InvitedUserToMongo() { email = email };
                foreach (var i in UsefullStuff.Common.Methods.MRTools.GetFields(Filt)) { if (i != "email") UsefullStuff.Common.Methods.MRTools.SetVal(i, null, Filt); }
                SaveUserMng._id = MongoDB.Bson.BsonObjectId.GenerateNewId().ToString(); ;//.ToString();
                var tx = Wrt.UpSert_Compare(Filt, SaveUserMng);
                #endregion Saved

            }
            else if (!(r is null) && r.Count > 0 || (r is null) && service != "5")
            {
                //Converter<BsonDocument, InvitedUserToMongo>.Combine(r[0], Delegate.CreateDelegate(Delegate, Methods. UserMongo) cn
                //var t = r.ConvertAll<InvitedUserToMongo>(BsonDocument, InvitedUserToMongo);//.Cast<InvitedUserToMongo>().FirstOrDefault());  //.FirstOrDefault().Elements.ToList().ConvertAll<string>(); //.Remove(new BsonElement("createAt".ForEach(x=>x.Value   //.E.ToString();
                UserMongo = BsonSerializer.Deserialize<InvitedUserToMongo>(r.FirstOrDefault()); //t.FirstOrDefault();
                                                                                                //  UserMongo = Newtonsoft.Json.JsonConvert.DeserializeObject<InvitedUserToMongo>(t);
            }
            if (service == "6")
            { UserMongo.isEmailConfirmed = UserMongo.isEmailConfirmed; }
            else { UserMongo.isEmailConfirmed = true; }
            if (UserMongo.isEmailConfirmed == true) return Newtonsoft.Json.JsonConvert.SerializeObject(res2);
            else return Newtonsoft.Json.JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "403", level = "error", message = "Anauthorized", sticky = false, title = "error" });
        }


        public static AuthK Authorize(string key)
        {
            AuthParameter authParameter = new AuthParameter
            {
                email = "",
                password = "",
                key = key,
                service = PTConstants.Constants.FTServiceID
            };
            string data = authParameter.ToJson();
            var webResult = UMKBRequests.WebUtils.Post(URLs.Authorization, data, UMKBRequests.WebUtils.WebContentType.JSON);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<AuthK>(webResult.Answer);
        }

        public static AlertUserID GetUserIdByAuthkey(string authkey, string service = "7", string key = null)
        {
            string data;
            if (String.IsNullOrEmpty(key)) data = new { authkey = authkey, application = service }.ToJson();
            else data = new { key = key, application = service }.ToJson();
            AlertUserID uid = new AlertUserID();
            var webResult = UMKBRequests.WebUtils.Post(URLs.CheckRighstUserId, data, UMKBRequests.WebUtils.WebContentType.JSON);
            try
            {
                uid = Newtonsoft.Json.JsonConvert.DeserializeObject<AlertUserID>(webResult.Answer);
            }
            catch (Exception e)
            {
                uid.alert = new Alert() { code = "202", level = "success", message = e.Message };
                return uid;
            }
            uid.role = (service == "7" ? "Admin" : (service == "5" ? "User" : "Provisor"));
            if (uid.alert.level != "success")
            {
                data = (new { authkey = authkey, application = PTConstants.Constants.FTServiceID6 }).ToJson();
                webResult = UMKBRequests.WebUtils.Post(URLs.CheckRighstUserId, data, UMKBRequests.WebUtils.WebContentType.JSON);
                uid = Newtonsoft.Json.JsonConvert.DeserializeObject<AlertUserID>(webResult.Answer);
                uid.role = "Provisor";
                if (uid.alert.level != "success")
                {
                    data = new { authkey = authkey, application = "5" }.ToJson();
                    webResult = UMKBRequests.WebUtils.Post(URLs.CheckRighstUserId, data, UMKBRequests.WebUtils.WebContentType.JSON);
                    uid = Newtonsoft.Json.JsonConvert.DeserializeObject<AlertUserID>(webResult.Answer);
                    uid.role = "Patient";
                    return uid;
                }
                return uid;
            }
            return uid;
        }

        public static (UMKBRequests.WebUtils.Result, UMKB_RespStat) GenerateUserAttachDrugstore(string authkey, uint? drugstoreId)
        {
            var webResult = PharmTaxiAdm.Methods.User.UserMethods.GenerateUser(authkey);
            var responseAlertData = JsonConvert.DeserializeObject<UMKBNeedStuff.Responses.ResponseAlertData<RDGenerateUserByAuthkey>>(webResult.Answer);
            Trace.WriteLine(JsonConvert.SerializeObject(webResult));
            uint? user_id = null;
            //var answerData = JObject.Parse(result.Answer)["data"];
            //var answerAlert = JObject.Parse(result.Answer)["alert"].ToString();
            if (responseAlertData.data != null && responseAlertData.data.user_id != 0)
            {
                user_id = responseAlertData.data.user_id;
            }
            if (user_id == null)
            {
                Trace.WriteLine($"User creation error {webResult.Answer}");
                return (webResult, null);
            }
            return (webResult, PharmTaxiAdm.Methods.User.UserMethods.AttachDrugstoreToUser(user_id, drugstoreId));
        }
        public static UMKBRequests.WebUtils.Result GenerateUser(string authkey)
        {
            string service_id = PTConstants.Constants.FTServiceID;
            var data = new
            {
                authkey,
                service = service_id,
                childService = service_id,
                key = UMKB.GenerateUserByAuthkey,
            }.ToJson();
            Trace.WriteLine(data);
            var webResult = UMKBRequests.WebUtils.Post(URLs.GenerateUserByAuthkey, data, UMKBRequests.WebUtils.WebContentType.JSON);
            /*var webResult = new UMKBRequests.WebUtils.Result(JsonConvert.SerializeObject(
                new UMKBNeedStuff.Responses.ResponseAlertData<RDGenerateUserByAuthkey>
                {
                    alert = new UMKBNeedStuff.Alert { code = "200", message = "OK", level = "success" },
                    data = new RDGenerateUserByAuthkey { user_id = 8122 }
                })
                , new Exception());*/
            return webResult;
        }

        public static UMKBRequests.WebUtils.Result ConfirmInvite(ConfirmInvite args)
        {
            args.key = UMKB.InviteAccept;
            args.application = "6";
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(args);
            AlertUserID AuId = GetUserIdByAuthkey(args.authkey);
            string email = "";
            if (AuId.userid != 0)
            {
                List<uint> ChildphrmsId = PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.GetDrugstoresByUserId(AuId.userid.ToString()).Select(x => x.PharmId).ToList();
                email = Drugstore.DrugstoreMethods.GetEmailById(AuId.userid);
                ResponseGetUserInfoByEmail UinfByEmail = GetUserInfoByEmail(new GetUserInfo() { email = email });
                #region Сохранение в монгу
                InvitedUserToMongo SaveUserMng = new InvitedUserToMongo() { email = email };
                SaveUserMng = Newtonsoft.Json.JsonConvert.DeserializeObject<InvitedUserToMongo>(JsonConvert.SerializeObject(UinfByEmail));
                List<uint> PharmsOfUser = new List<uint>();
                foreach (var i in ChildphrmsId) PharmsOfUser.Add(i);
                SaveUserMng.pharms = PharmsOfUser;
                SaveUserMng.isEmailConfirmed = true;
                if (!UinfByEmail.services.Contains(6)) UinfByEmail.services.Add(6);
                // if (!UinfByEmail.services.Contains(7)) UinfByEmail.services.Add(6);
                SaveUserMng.permissions = UinfByEmail.services;
                UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Mongo.MongoDB, Mongo.UsersCollection, Mongo.PHTFORSTFR);
                UsefullStuff.Common.Methods.MongoTools.Methods.Write<InvitedUserToMongo> Wrt = new UsefullStuff.Common.Methods.MongoTools.Methods.Write<InvitedUserToMongo>();
                InvitedUserToMongo Filt = new InvitedUserToMongo() { email = email };
                foreach (var i in UsefullStuff.Common.Methods.MRTools.GetFields(Filt)) { if (i != "email") UsefullStuff.Common.Methods.MRTools.SetVal(i, null, Filt); }
                Wrt.UpSert_Compare(Filt, SaveUserMng);
                #endregion

                return UMKBRequests.WebUtils.Post(URLs.ConfirmInvite, data, "application/json");
            }

            else
            {
                return UMKBRequests.WebUtils.Post(URLs.ConfirmInvite, data, "application/json");
            }
            return UMKBRequests.WebUtils.Post(URLs.ConfirmInvite, data, "application/json");
        }

        public static UMKBRequests.Modelling.U_AUD_Lib.UMKB_RespStat AttachDrugstoreToUser(uint? user_id, uint? drugstoreLongId)
        {
            #region Сохранение в монгу в коллекцию users
            InvitedUserToMongo SaveUserMng = new InvitedUserToMongo();
            UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Mongo.MongoDB, Mongo.UsersCollection, Mongo.PHTFORSTFR);
            UsefullStuff.Common.Methods.MongoTools.Methods.Write<InvitedUserToMongo> WrtUsr = new UsefullStuff.Common.Methods.MongoTools.Methods.Write<InvitedUserToMongo>();
            SaveUserMng.pharms = new List<uint>();
            SaveUserMng.pharms.Add(uint.TryParse(drugstoreLongId.Value.ToString().Substring(0, drugstoreLongId.Value.ToString().Length - 4), out uint x) ? x : 0);
            UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Mongo.MongoDB, Mongo.UsersCollection, Mongo.PHTFORSTFR);
            InvitedUserToMongo Filt = new InvitedUserToMongo() { user_id = user_id.Value };
            foreach (var i in UsefullStuff.Common.Methods.MRTools.GetFields(Filt)) { if (i != "user_id") UsefullStuff.Common.Methods.MRTools.SetVal(i, null, Filt); }
            BsonDocument UserBson = UsefullStuff.Common.Methods.MongoTools.Methods.Read<InvitedUserToMongo>.AllFiltered(Filt).FirstOrDefault();
            SaveUserMng = BsonSerializer.Deserialize<InvitedUserToMongo>(UserBson); // Newtonsoft.Json.JsonConvert.DeserializeObject<InvitedUserToMongo>(UserBson.ToJson());
            var phms = UserBson.GetValue("pharms").AsBsonArray.Values.ToList();
            List<uint> Phms = new List<uint>();
            phms.ForEach(x => Phms.Add((uint)x.AsInt32));
            SaveUserMng.pharms = Phms;
            SaveUserMng.pharms.Add(drugstoreLongId.Value);
            WrtUsr.UpSert_Compare(Filt, SaveUserMng);
            #endregion

            #region Сохранение в монгу аптек
            Models.Drugstore.DrugstoreModel SearchDrgStor = new Models.Drugstore.DrugstoreModel() { PharmId = uint.TryParse(drugstoreLongId.Value.ToString().Substring(0, drugstoreLongId.Value.ToString().Length - 4), out uint phm) ? phm : 0 };
            PharmTaxiAdm.Models.Drugstore.DrugstoreModel DrugStore = new PharmTaxiAdm.Models.Drugstore.DrugstoreModel();
            List<BsonDocument> FoundedDrStor = UsefullStuff.Common.Methods.MongoTools.Methods.Read<PharmTaxiAdm.Models.Drugstore.DrugstoreModel>.AllFiltered(Newtonsoft.Json.JsonConvert.DeserializeObject<PharmTaxiAdm.Models.Drugstore.DrugstoreModel>(Newtonsoft.Json.JsonConvert.SerializeObject(SearchDrgStor)), Mongo.PHTFORSTFR, Mongo.Pharms);
            var usrs = FoundedDrStor.FirstOrDefault().TryGetValue("", out BsonValue uzr) ? uzr.AsBsonArray.Values.ToList() : null;// .GetValue("Users").AsBsonArray.Values.ToList();
            //if (usrs is null) continue;
            UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Mongo.MongoDB, Mongo.Pharms, Mongo.PHTFORSTFR);
            UsefullStuff.Common.Methods.MongoTools.Methods.Write<PharmTaxiAdm.Models.Drugstore.DrugstoreModel> WrtPharm = new UsefullStuff.Common.Methods.MongoTools.Methods.Write<PharmTaxiAdm.Models.Drugstore.DrugstoreModel>();
            try
            {
                List<uint> Usrs = new List<uint>();
                usrs.ForEach(x => Usrs.Add((uint)x.AsInt32));
                DrugStore = BsonSerializer.Deserialize<Models.Drugstore.DrugstoreModel>(FoundedDrStor.FirstOrDefault()); ///Newtonsoft.Json.JsonConvert.DeserializeObject<PharmTaxiAdm.Models.Drugstore.DrugstoreModel>(FoundedDrStor.FirstOrDefault().AsString);
                DrugStore.Users = DrugStore.Users.Except(new List<UserModel>() { new UserModel() { LongId = user_id.Value } }).ToList();
                UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Mongo.MongoDB, Mongo.Pharms, Mongo.PHTFORSTFR);
                WrtPharm.UpSert_Compare(Newtonsoft.Json.JsonConvert.DeserializeObject<PharmTaxiAdm.Models.Drugstore.DrugstoreModel>(Newtonsoft.Json.JsonConvert.SerializeObject(SearchDrgStor)), DrugStore);
                UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Mongo.MongoDB, Mongo.PharmsFD, Mongo.PHTFORSTFR);
                WrtPharm.UpSert_Compare(Newtonsoft.Json.JsonConvert.DeserializeObject<PharmTaxiAdm.Models.Drugstore.DrugstoreModel>(Newtonsoft.Json.JsonConvert.SerializeObject(SearchDrgStor)), DrugStore);
            }
            catch { }
            #endregion

            int cnt = 0;
            UMKB_Packet UP = new UMKB_Packet(UMKB.Modelling);
            UP.user = PTConstants.Constants.ModellingUserSmoley;
            UError hz = new UError();
            UMKB_Field APTEK = new UMKB_Field(user_id, drugstoreLongId, null, 902, 95, 920, 9112, ref cnt, out hz);
            UP.cache = true;
            UP.modeling.Add(APTEK);
            string shit0 = Newtonsoft.Json.JsonConvert.SerializeObject(UP, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            UP.UMKBRequestSP(URLs.Modelling);
            return UP.URresult;
        }

        public static UMKBRequests.Modelling.U_AUD_Lib.UMKB_RespStat DetachDrugstoreFromUser(uint? user_id, uint? drugstoreLongId)
        {
            int cnt = 0;
            UMKB_Packet UP = new UMKB_Packet(UMKB.Modelling);
            UP.action = "delet";
            UP.mode = 1;
            UP.model = "bel";
            UP.lib = 920;
            UP.user = PTConstants.Constants.ModellingUserSmoley;
            UError hz = new UError();
            ulong[] bels = Drugstore.DrugstoreMethods.GetLinkPharmUserByPh(drugstoreLongId, user_id);
            if (bels is null) return new UMKB_RespStat() { coment = "No Bels Found" };
            int NumOfBel = bels.Length;
            for (int i = 0; i < NumOfBel; i++)
            {
                //  UMKB_Deleter AptDel = new UMKB_Deleter();
                UMKB_Field AptDel = new UMKB_Field();
                AptDel.action = "delet";
                AptDel.mode = 1;
                AptDel.model = "bel";
                AptDel.lib = 920;
                AptDel.id = (int)bels[i];
                UP.modeling.Add(AptDel);
            }
            // UMKB_Field APTEK = new UMKB_Field(user_id, drugstoreLongId, null, 902, 95, 920, 9112, ref cnt, out hz);
            UP.cache = true;

            // UP.mode = 3;
            string shit0 = Newtonsoft.Json.JsonConvert.SerializeObject(UP, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            UP.UMKBRequestSP(URLs.Modelling);
            return UP.URresult;
        }

        public static RegResp ConfirmReg(RegConfirmReqUMKB data)
        {
            RegResp ret = new RegResp();
            data.key = "7e649ecb733d2af1";
            ; string posttest = Newtonsoft.Json.JsonConvert.SerializeObject(data); //"{\"email\":\"" + data.email + "\",\"code\":\"" + data.code + "\",\"service\":\"7\",\"key\":\"7e649ecb733d2af1\"}";
            var xz = UMKBRequests.WebUtils.Post("https://cs.socmedica.com/api/registration/confirmemail", posttest, "application/json").Answer;
            ret = Newtonsoft.Json.JsonConvert.DeserializeObject<RegResp>(xz);
            return ret;
        }

        public static UMKBRequests.WebUtils.Result Register(IUserRegUMKB data)
        {
            Trace.WriteLine(data.ToJson());
            UserRegistrationReq test = (UserRegistrationReq)data;
            if (String.IsNullOrEmpty((string)data.GetVal("lic_key")) && String.IsNullOrEmpty((string)data.GetVal("roleCode")))
            {
                data.key = UMKB.Authorization;
                string createU = data.ToJson();
                var webResult = UMKBRequests.WebUtils.Post(URLs.Registration, createU, UMKBRequests.WebUtils.WebContentType.JSON);
                /*var webResult = new UMKBRequests.WebUtils.Result(JsonConvert.SerializeObject(
                    new UMKBNeedStuff.Responses.ResponseAlertData<RDGenerateUserByAuthkey>
                    {
                        alert = new UMKBNeedStuff.Alert { code = "200", message = "OK", level = "success" },
                        data = new RDGenerateUserByAuthkey { user_id = 8122 }
                    })
                    , new Exception());*/
                RegUserByUserResp rsp = Newtonsoft.Json.JsonConvert.DeserializeObject<RegUserByUserResp>(webResult.Answer);
                #region SAVE to MONGO
                InvitedUserToMongo SaveUserMng = new InvitedUserToMongo();
                SaveUserMng = Newtonsoft.Json.JsonConvert.DeserializeObject<InvitedUserToMongo>(Newtonsoft.Json.JsonConvert.SerializeObject(data));//UsefullStuff.Common.Methods.Converter<InvitedUserToMongo, RegUserByUserReq>.Convert(SaveUserMng, args);
                                                                                                                                                   //  List<uint> PharmsOfUser = new List<uint>();
                string dt = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                //var apt = uint.TryParse(PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.GetPharmByCode(data.roleCode), out uint xapt) ? xapt : 0;
                // PharmsOfUser.Add(apt);
                // if (!PharmsOfUser.Contains(PharmTaxiAdm.Methods.MongoDS.GetPharmIdByCode.Get(data.roleCode))) PharmsOfUser.Add(PharmTaxiAdm.Methods.MongoDS.GetPharmIdByCode.Get(data.roleCode));
                SaveUserMng.user_id = rsp.userId;
                SaveUserMng.role = (data.service == "7" || data.service == "6") ? (data.service == "7" ? "Admin" : "Provisor") : data.service;
                // SaveUserMng.pharms = PharmsOfUser;
                UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Mongo.MongoDB, Mongo.UsersCollection, Mongo.PHTFORSTFR);
                UsefullStuff.Common.Methods.MongoTools.Methods.Write<InvitedUserToMongo> Wrt = new UsefullStuff.Common.Methods.MongoTools.Methods.Write<InvitedUserToMongo>();
                InvitedUserToMongo Filt = new InvitedUserToMongo() { email = data.email };
                foreach (var i in UsefullStuff.Common.Methods.MRTools.GetFields(Filt)) { if (i != "email") UsefullStuff.Common.Methods.MRTools.SetVal(i, null, Filt); }
                #region Нам надо проверить, были ли юзер уже зареган; если не был - сразу ставим ему статус подтверждённого, так как к нам с обычного подтверждения никто не приходит
                var UsTsk = UsefullStuff.Common.Methods.MongoTools.Methods.Read<InvitedUserToMongo>.AllFiltered(Filt);
                if (UsTsk is null || UsTsk.Count == 0) SaveUserMng.isEmailConfirmed = true;
                else
                {
                    InvitedUserToMongo UserMongo = BsonSerializer.Deserialize<InvitedUserToMongo>(UsTsk.FirstOrDefault());// Newtonsoft.Json.JsonConvert.DeserializeObject<InvitedUserToMongo>(UsTsk.FirstOrDefault().ToJson());
                    if (!String.IsNullOrEmpty(UserMongo._id) && !(UserMongo.isEmailConfirmed is null) && UserMongo.isEmailConfirmed != false) SaveUserMng.isEmailConfirmed = true;
                }
                #endregion
                Wrt.UpSert_Compare(Filt, SaveUserMng);
                #endregion
                return webResult;
            }
            else
            {
                try
                {
                    data.roleCode = (string)data.GetVal("lic_key");
                    data.key = UMKB.Authorization;
                    string createU = data.ToJson();
                    var webResult = UMKBRequests.WebUtils.Post(URLs.Registration, createU, UMKBRequests.WebUtils.WebContentType.JSON);
                    RegResp rsp = Newtonsoft.Json.JsonConvert.DeserializeObject<RegResp>(webResult.Answer);
                    uint usid = Drugstore.DrugstoreMethods.GetUserIdByEmail(data.email);
                    #region SAVE to MONGO
                    InvitedUserToMongo SaveUserMng = new InvitedUserToMongo();
                    SaveUserMng = Newtonsoft.Json.JsonConvert.DeserializeObject<InvitedUserToMongo>(Newtonsoft.Json.JsonConvert.SerializeObject(data));//UsefullStuff.Common.Methods.Converter<InvitedUserToMongo, RegUserByUserReq>.Convert(SaveUserMng, args);
                    List<uint> PharmsOfUser = new List<uint>();
                    string dt = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                    var apt = uint.TryParse(PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.GetPharmByCode(data.roleCode), out uint xapt) ? xapt : 0;
                    PharmsOfUser.Add(apt);
                    try
                    {
                        if (!PharmsOfUser.Contains(PharmTaxiAdm.Methods.MongoDS.GetPharmIdByCode.Get(data.roleCode))) PharmsOfUser.Add(PharmTaxiAdm.Methods.MongoDS.GetPharmIdByCode.Get(data.roleCode));
                    }
                    catch { }
                    SaveUserMng.role = (data.service == "7" || data.service == "6") ? (data.service == "7" ? "Admin" : "Provisor") : data.service;
                    SaveUserMng.user_id = usid;
                    SaveUserMng.pharms = PharmsOfUser;
                    UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Mongo.MongoDB, Mongo.UsersCollection, Mongo.PHTFORSTFR);
                    UsefullStuff.Common.Methods.MongoTools.Methods.Write<InvitedUserToMongo> Wrt = new UsefullStuff.Common.Methods.MongoTools.Methods.Write<InvitedUserToMongo>();
                    InvitedUserToMongo Filt = new InvitedUserToMongo() { email = data.email };
                    foreach (var i in UsefullStuff.Common.Methods.MRTools.GetFields(Filt)) { if (i != "email") UsefullStuff.Common.Methods.MRTools.SetVal(i, null, Filt); }
                    #region Нам надо проверить, были ли юзер уже зареган; если не был - сразу ставим ему статус подтверждённого, так как к нам с обычного подтверждения никто не приходит
                    var UsTsk = UsefullStuff.Common.Methods.MongoTools.Methods.Read<InvitedUserToMongo>.AllFiltered(Filt);
                    InvitedUserToMongo UserMongo = BsonSerializer.Deserialize<InvitedUserToMongo>(UsTsk.FirstOrDefault()); //Newtonsoft.Json.JsonConvert.DeserializeObject<InvitedUserToMongo>(UsTsk.FirstOrDefault().ToJson());
                    if (!String.IsNullOrEmpty(UserMongo._id) && !(UserMongo.isEmailConfirmed is null) && UserMongo.isEmailConfirmed != false) SaveUserMng.isEmailConfirmed = true;
                    #endregion
                    var x = Wrt.UpSert_Compare(Filt, SaveUserMng);
                    #endregion
                    return webResult;
                    //if (webResult.Answer.Contains("Адрес уже зарегистрирован"))
                    //{
                    //    AuthK TryAuth2 = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthK>(Authorize(data.email, data.password).Answer);
                    //    uint? userid = PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.GetUserIdByEmail(data.email);
                    //    string PhId2 = Drugstore.DrugstoreMethods.GetPharmByCode(data.lic_key);
                    //    if (PhId2 == "") PhId2 = MongoDS.GetPharmIdByCode.Get(data.lic_key).ToString();

                    //    //AlertUserID wRes = Newtonsoft.Json.JsonConvert.DeserializeObject<AlertUserID>(GetUserIdByAuthkey(authD.authkey).Answer);
                    //    var rsp2 = AttachDrugstoreToUser(userid, uint.Parse(PhId2)); //GenerateUserAttachDrugstore(TryAuth2.authkey, uint.Parse(PhId2));
                    //    GenUsAttDr GenRes2 = new GenUsAttDr() { respStat = rsp2 }; //{ webres = rsp2.Item1, respStat = rsp2.Item2 };
                    //    return GenRes2.webres;
                    //}
                    //else
                    //{
                    //    uint? userid = PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.GetUserIdByEmail(data.email);



                    //    //AuthKeyByLicKey authD = new AuthKeyByLicKey() { authkey=Authorize(data.key).authkey}; //Получить 
                    //    //string createU = new UserRegUMKB() { email = data.email, password = data.password, service = data.service, lastName = data.familia, firstName = data.name, middleName = data.fathername, key = PharmTaxiAdm.Constants.Keys.AuthorizationKey }.ToJson();
                    //    //var webResult = UMKBRequests.WebUtils.Post(PharmTaxiAdm.Constants.URLS.Registration, createU, UMKBRequests.WebUtils.WebContentType.JSON);
                    //    string PhId = Drugstore.DrugstoreMethods.GetPharmByCode(data.lic_key);
                    //    if (PhId == "") PhId = MongoDS.GetPharmIdByCode.Get(data.lic_key).ToString();
                    //    AuthK TryAuth = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthK>(Authorize(data.email, data.password).Answer);
                    //    //AlertUserID wRes = Newtonsoft.Json.JsonConvert.DeserializeObject<AlertUserID>(GetUserIdByAuthkey(authD.authkey).Answer);
                    //    //var rsp = GenerateUserAttachDrugstore(TryAuth.authkey, uint.Parse(PhId));
                    //    //GenUsAttDr GenRes = new GenUsAttDr() { webres = rsp.Item1, respStat = rsp.Item2 };
                    //    var rsp = AttachDrugstoreToUser(userid, uint.Parse(PhId)); //GenerateUserAttachDrugstore(TryAuth2.authkey, uint.Parse(PhId2));
                    //    GenUsAttDr GenRes = new GenUsAttDr() { respStat = rsp }; //{ webres = rsp2.Item1, respStat = rsp2.Item2 };
                    //    return GenRes.webres;
                    //}
                }
                catch
                {
                    GenUsAttDr GenRes = new GenUsAttDr() { webres = null, respStat = null };
                    return GenRes.webres;
                }
            }
        }

        public static UMKBRequests.WebUtils.Result RegisterAndInvite(Models.User.RegUserByUserReq args)
        {
            args.key = UMKB.RegisterUserByUser;
            BaseRegUserByUserReq Bargs = new BaseRegUserByUserReq();
            Bargs = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseRegUserByUserReq>(Newtonsoft.Json.JsonConvert.SerializeObject(args));
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(Bargs);
            var x = UMKBRequests.WebUtils.Post(URLs.RegisterUserByUser, data, "application/json");
            return x;
        }

        public static Models.User.ResponseGetUserInfoByEmail GetUserInfoByEmail(GetUserInfo args)
        {
            args.key = UMKB.GetUserInfoByEmail;
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(args);
            var x = UMKBRequests.WebUtils.Post(URLs.GetUserInfoByEmail, data, "application/json");
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseGetUserInfoByEmail>(x.Answer);
        }

        public static HashSet<ulong> GetDrugstoresIdByUserId(string userid)
        {
            HashSet<ulong> result = new HashSet<ulong>();

            try
            {
                UMKBRequests.GetSemanticNewBigId getSemantic = new UMKBRequests.GetSemanticNewBigId();
                var req_sem = new UMKBRequests.Models.API.Semantic.RequestSemantic
                {
                    key = UMKB.Graph,
                    lib = new List<ushort>(),
                    libid = new List<string> { userid + "0902" },
                    level = new List<ushort> { 9112 },
                    deep = 1,
                    route = 1,
                };

                var resultSemantic = getSemantic.Get(req_sem);

                foreach (var graph in resultSemantic.graph)
                {
                    if (graph.idb % 10000 == 95)
                    {
                        result.Add(graph.idb);
                    }
                }
            }
            catch
            { result = null; }

            return result;
        }
    }
}
