using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PTConstants;
using PharmTaxiAdm.Models;
using PharmTaxiAdm.Models.User;

namespace PharmTaxiAPI.Methods
{
    public static class Inviter
    {
        public static string Get(PharmTaxiAdm.Models.User.RegUserByUserReq args)
        {
            AlertUserID AuID = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(args.authkey);
            if (AuID.userid != 0) {
                PharmTaxiAdm.Models.User.GetUserInfo GiArgs = new PharmTaxiAdm.Models.User.GetUserInfo();
                GiArgs= Newtonsoft.Json.JsonConvert.DeserializeObject<GetUserInfo>(Newtonsoft.Json.JsonConvert.SerializeObject(args));
                //UsefullStuff.Common.Methods.Converter<PharmTaxiAdm.Models.User.GetUserInfo, PharmTaxiAdm.Models.User.RegUserByUserReq>.Convert(GiArgs, args);
               // GiArgs.email = args.email;
                PharmTaxiAdm.Models.User.ResponseGetUserInfoByEmail FirstStep = PharmTaxiAdm.Methods.User.UserMethods.GetUserInfoByEmail(GiArgs);
                if (FirstStep.userId != 0)
                {
                    // получение доступных аптек
                    List<uint> ChildphrmsId = PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.GetDrugstoresByUserId(FirstStep.userId.ToString()).Select(x => x.PharmId).ToList();

                   
                    #region Сохранение в монгу
                    InvitedUserToMongo SaveUserMng = new InvitedUserToMongo();
                    SaveUserMng = Newtonsoft.Json.JsonConvert.DeserializeObject<InvitedUserToMongo>(Newtonsoft.Json.JsonConvert.SerializeObject(args)); ///UsefullStuff.Common.Methods.Converter<InvitedUserToMongo, RegUserByUserReq>.Convert(SaveUserMng, args);
                    List<uint> PharmsOfUser = new List<uint>();
                    string dt = Newtonsoft.Json.JsonConvert.SerializeObject(args);
                    foreach (var i in ChildphrmsId) PharmsOfUser.Add(i);
                    var apt = uint.TryParse(PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.GetPharmByCode(args.roleCode), out uint xapt)?xapt:0;
                    if (!PharmsOfUser.Contains(apt)) PharmsOfUser.Add(apt);
                    if (!PharmsOfUser.Contains(PharmTaxiAdm.Methods.MongoDS.GetPharmIdByCode.Get(args.roleCode))) PharmsOfUser.Add(PharmTaxiAdm.Methods.MongoDS.GetPharmIdByCode.Get(args.roleCode));
                    SaveUserMng.pharms = PharmsOfUser;
                    SaveUserMng.isEmailConfirmed = FirstStep.isEmailConfirmed;
                    SaveUserMng.permissions = FirstStep.services;
                    SaveUserMng.inviter_id = AuID.userid;
                    SaveUserMng.user_id = (uint)FirstStep.userId;
                    if (String.IsNullOrWhiteSpace(SaveUserMng.firstname) || String.IsNullOrEmpty(SaveUserMng.firstname)) SaveUserMng.firstname = null;
                    if (String.IsNullOrWhiteSpace(SaveUserMng.middlename) || String.IsNullOrEmpty(SaveUserMng.middlename)) SaveUserMng.middlename = null;
                    if (String.IsNullOrWhiteSpace(SaveUserMng.lastname) || String.IsNullOrEmpty(SaveUserMng.lastname)) SaveUserMng.lastname = null;
                    UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Mongo.MongoDB, Mongo.UsersCollection, Mongo.PHTFORSTFR);
                    UsefullStuff.Common.Methods.MongoTools.Methods.Write<InvitedUserToMongo> Wrt = new UsefullStuff.Common.Methods.MongoTools.Methods.Write<InvitedUserToMongo>();
                    InvitedUserToMongo Filt = new InvitedUserToMongo() { email = args.email };
                    foreach (var i in UsefullStuff.Common.Methods.MRTools.GetFields(Filt)) { if (i != "email") UsefullStuff.Common.Methods.MRTools.SetVal(i, null, Filt); }
                    Wrt.UpSert_Compare(Filt, SaveUserMng);
                    #endregion
                    if (!(FirstStep.services is null))
                    {
                        if (FirstStep.services.Contains(6))
                        {
                            //List<uint> ParentphrmsId = PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.GetDrugstoresByUserId(PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(args.authkey).userid.ToString()).Select(x=>x.PharmId).ToList();
                            if (ChildphrmsId.Contains(UInt32.TryParse(PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.GetPharmByCode(args.roleCode), out uint x) ? x : 0))//ParentphrmsId.Intersect<uint>(ChildphrmsId).ToList().Count > 0)
                            {
                                return Newtonsoft.Json.JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "200", level = "success", sticky = false, title = "sucess", message = "Пользователь уже приглашён в эту аптеку." });
                            }
                            else
                            {
                                InviteViaEmail IvE = new InviteViaEmail() 
                                {
                                    authkey = args.authkey, 
                                    invitedEmail = args.email, 
                                    invitedRoleCodes = new List<string>() { args.roleCode }, 
                                    key = UMKB.Invite, 
                                    application = "6" 
                                };

                                InviteResp ret = PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.SlaveCreation3(IvE).Result;
                                return Newtonsoft.Json.JsonConvert.SerializeObject(ret);
                            }
                        }
                        else
                        {
                            ServiceCreationAuthK ScA = new ServiceCreationAuthK() { authkey = args.authkey, key = UMKB.AddServiceByAuthkey, service = "6" };
                            string data = Newtonsoft.Json.JsonConvert.SerializeObject(ScA);
                            return UMKBRequests.WebUtils.Post(URLs.AddServiceByAuthkey, data, "application/json").Answer;
                        }
                    }
                    else
                    {
                        ServiceCreationAuthK ScA = new ServiceCreationAuthK() { authkey = args.authkey, key = UMKB.AddServiceByAuthkey, service = "6" };
                        string data = Newtonsoft.Json.JsonConvert.SerializeObject(ScA);
                        return UMKBRequests.WebUtils.Post(URLs.AddServiceByAuthkey, data, "application/json").Answer;
                    }

                }
                else
                {
                    var res= PharmTaxiAdm.Methods.User.UserMethods.RegisterAndInvite(args).Answer;
                    RegUserByUserResp rsp = Newtonsoft.Json.JsonConvert.DeserializeObject<RegUserByUserResp>(res);
                    #region Сохранение в монгу
                    InvitedUserToMongo SaveUserMng = new InvitedUserToMongo();
                    SaveUserMng = Newtonsoft.Json.JsonConvert.DeserializeObject<InvitedUserToMongo>(Newtonsoft.Json.JsonConvert.SerializeObject(args));//UsefullStuff.Common.Methods.Converter<InvitedUserToMongo, RegUserByUserReq>.Convert(SaveUserMng, args);
                    List<uint> PharmsOfUser = new List<uint>();
                    string dt = Newtonsoft.Json.JsonConvert.SerializeObject(args);
                    var apt = uint.TryParse(PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.GetPharmByCode(args.roleCode), out uint xapt)?xapt:0;
                    PharmsOfUser.Add(apt);
                    if (!PharmsOfUser.Contains(PharmTaxiAdm.Methods.MongoDS.GetPharmIdByCode.Get(args.roleCode))) PharmsOfUser.Add(PharmTaxiAdm.Methods.MongoDS.GetPharmIdByCode.Get(args.roleCode));
                    SaveUserMng.user_id = rsp.userId;
                    SaveUserMng.inviter_id = AuID.userid;
                    SaveUserMng.pharms = PharmsOfUser;
                    UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Mongo.MongoDB, Mongo.UsersCollection, Mongo.PHTFORSTFR);
                    UsefullStuff.Common.Methods.MongoTools.Methods.Write<InvitedUserToMongo> Wrt = new UsefullStuff.Common.Methods.MongoTools.Methods.Write<InvitedUserToMongo>();
                    InvitedUserToMongo Filt = new InvitedUserToMongo() { email = args.email };
                    foreach (var i in UsefullStuff.Common.Methods.MRTools.GetFields(Filt)) { if (i != "email") UsefullStuff.Common.Methods.MRTools.SetVal(i, null, Filt); }
                    Wrt.UpSert_Compare(Filt, SaveUserMng);
                    #endregion
                    return res;
                }
            }
            return "";
        }

        public static string Confirm(RegConfirmReqPHT data)
        {
            #region Сохранение в монгу
            InvitedUserToMongo SaveUserMng = new InvitedUserToMongo();
            SaveUserMng = Newtonsoft.Json.JsonConvert.DeserializeObject<InvitedUserToMongo>(Newtonsoft.Json.JsonConvert.SerializeObject(data));//UsefullStuff.Common.Methods.Converter<InvitedUserToMongo, RegUserByUserReq>.Convert(SaveUserMng, args);
            UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Mongo.MongoDB, Mongo.UsersCollection, Mongo.PHTFORSTFR);
            UsefullStuff.Common.Methods.MongoTools.Methods.Write<InvitedUserToMongo> Wrt = new UsefullStuff.Common.Methods.MongoTools.Methods.Write<InvitedUserToMongo>();
            InvitedUserToMongo Filt = new InvitedUserToMongo() { email = data.email };
            foreach (var i in UsefullStuff.Common.Methods.MRTools.GetFields(Filt)) { if (i != "email") UsefullStuff.Common.Methods.MRTools.SetVal(i, null, Filt); }
            foreach (var i in UsefullStuff.Common.Methods.MRTools.GetFields(SaveUserMng)) { if (i != "firstname" && i != "lastname" && i != "middlename") UsefullStuff.Common.Methods.MRTools.SetVal(i, null, SaveUserMng); }
            SaveUserMng.isEmailConfirmed = true;

            var xx=Wrt.UpSert_Compare(Filt, SaveUserMng);
            #endregion
            RegConfirmReqUMKB args = new RegConfirmReqUMKB();
            args = Newtonsoft.Json.JsonConvert.DeserializeObject<RegConfirmReqUMKB>(Newtonsoft.Json.JsonConvert.SerializeObject(data));
          //  UsefullStuff.Common.Methods.Converter<RegConfirmReqUMKB, RegConfirmReqPHT>.Convert(args, data); //Newtonsoft.Json.JsonConvert.DeserializeObject<RegConfirmReqUMKB>(Newtonsoft.Json.JsonConvert.SerializeObject(data));
            return Newtonsoft.Json.JsonConvert.SerializeObject(PharmTaxiAdm.Methods.User.UserMethods.ConfirmReg(args));
        }

        public static string ReConfirm(ReConfirmEmailArgs args)
        {
            if (string.IsNullOrWhiteSpace(args.email)) { return Newtonsoft.Json.JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", level = "error", title = "Bad request", sticky = true }); }
            if (!args.email.Contains("@")) { return Newtonsoft.Json.JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", level = "error", title = "Bad request", sticky = true }); }
            if (!args.email.Contains(".")) { return  Newtonsoft.Json.JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "400", level = "error", title = "Bad request", sticky = true }); }

            try
            {
                UMKBNeedStuff.Requests.Billing.Auth.RequestGetConfirmEmail arg = new UMKBNeedStuff.Requests.Billing.Auth.RequestGetConfirmEmail()
                {
                    Email = args.email,
                    Key = "7e649ecb733d2af1",
                    Service = 7,
                };
                string posttest = Newtonsoft.Json.JsonConvert.SerializeObject(arg);

                return UMKBRequests.WebUtils.Post("https://cs.socmedica.com/api/registration/getconfirmemail", posttest, "application/json").Answer;
            }
            catch (Exception e)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(new UMKBRequests.Alert() { code = "403", level = "error", message = e.StackTrace, title = e.Message, sticky = false });
            }
        }
    }
}
