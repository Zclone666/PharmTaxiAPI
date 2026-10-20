using PTConstants;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using PharmTaxiAdm.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Methods
{
    public static class UserChangeRemove
    {
        public static string UserMod(UserFIOReq data)
        {
            try
            {
                #region Сохранение в монгу
                InvitedUserToMongo SaveUserMng = new InvitedUserToMongo();
                AlertUserID AuID = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(data.authkey, "7", data.key);
                if (AuID.userid != 0)
                {
                    SaveUserMng = Newtonsoft.Json.JsonConvert.DeserializeObject<InvitedUserToMongo>(Newtonsoft.Json.JsonConvert.SerializeObject(data));//UsefullStuff.Common.Methods.Converter<InvitedUserToMongo, RegUserByUserReq>.Convert(SaveUserMng, args);
                    UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Mongo.MongoDB, Mongo.UsersCollection, Mongo.PHTFORSTFR);
                    UsefullStuff.Common.Methods.MongoTools.Methods.Write<InvitedUserToMongo> Wrt = new UsefullStuff.Common.Methods.MongoTools.Methods.Write<InvitedUserToMongo>();
                    InvitedUserToMongo Filt = new InvitedUserToMongo() { user_id = AuID.userid };
                    foreach (var i in UsefullStuff.Common.Methods.MRTools.GetFields(Filt)) { if (i != "user_id") UsefullStuff.Common.Methods.MRTools.SetVal(i, null, Filt); }
                    Exception ex_ups = Wrt.UpSert_Compare(Filt, SaveUserMng);
                    #endregion
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new Alert() { code = "200", level = "success", message = $"Result of operation: {ex_ups.Message}; \n{ex_ups.StackTrace}", sticky = false, title = "success" });
                }
                else
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(AuID.alert);
                }
            }catch(Exception e)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(new Alert() { code = "403", level = "error", message = e.StackTrace , sticky = false, title = e.Message});
            }
        }

        public static string UserDel(Models.DesactivateModel args)
        {
            try
            {
                #region Сохранение в монгу
                InvitedUserToMongo Filt = new InvitedUserToMongo() { user_id = args.user_id };
                foreach (var i in UsefullStuff.Common.Methods.MRTools.GetFields(Filt)) { if (i != "user_id") UsefullStuff.Common.Methods.MRTools.SetVal(i, null, Filt); }
                UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Mongo.MongoDB, Mongo.UsersCollection, Mongo.PHTFORSTFR);
                UsefullStuff.Common.Methods.MongoTools.Methods.Write<InvitedUserToMongo> WrtUsr = new UsefullStuff.Common.Methods.MongoTools.Methods.Write<InvitedUserToMongo>();
                UsefullStuff.Common.Methods.MongoTools.Methods.Write<PharmTaxiAdm.Models.Drugstore.DrugstoreModel> WrtPharm = new UsefullStuff.Common.Methods.MongoTools.Methods.Write<PharmTaxiAdm.Models.Drugstore.DrugstoreModel>();
                BsonDocument UserBson = new BsonDocument();
                Task<BsonDocument> UsrSrch = new Task<BsonDocument>(() => UsefullStuff.Common.Methods.MongoTools.Methods.Read<InvitedUserToMongo>.AllFiltered(Filt).FirstOrDefault());
                Task<Exception> RUsrUpd = new Task<Exception>(()=>null);
                InvitedUserToMongo SaveUserMng = new InvitedUserToMongo();
                AlertUserID AuID = PharmTaxiAdm.Methods.User.UserMethods.GetUserIdByAuthkey(args.authkey, "7", args.key);
                if (AuID.userid != 0 && AuID.role=="Admin")
                {
                    if (args.full_desactivate)
                    {
                        //ЗДЕСЬ И ДАЛЕЕ НАДО ЗАМУТИТЬ ПРОВЕРКУ, КОТОРАЯ СОПОСТАВЛЯЕТ АПТЕКИ ДЕЗАКТИВИРУЕМОГО ЮЗЕРА
                        //С АПТЕКАМИ ДЕЗАКТИВИРУЮЩЕГО ЕГО АДМИНА, ЧТОБЫ ПРОВЕРИТЬ ЯВЛЯЕТСЯ ЛИ ДЕЗАКТИВИРУЮЩИЙ АДМИН
                        //АДМИНОМ В АПТЕКЕ ЮЗЕРА
                        SaveUserMng.isEmailConfirmed = false;
                        WrtUsr.UpSert_Compare(Filt, SaveUserMng);
                    }else if(args.pharms_to_exclude is null || args.pharms_to_exclude.Count == 0) { }
                    else
                    {
                        #region здесь мы получаем из поиска юзера и удаляем ему аптеки
                        UserBson = UsrSrch.Result;
                        SaveUserMng = BsonSerializer.Deserialize<InvitedUserToMongo>(UserBson);  // Newtonsoft.Json.JsonConvert.DeserializeObject<InvitedUserToMongo>(UserBson.ToJson());
                        var phms= UserBson.GetValue("pharms").AsBsonArray.Values.ToList();
                        List<uint> Phms = new List<uint>();
                        phms.ForEach(x => Phms.Add((uint)x.AsInt32));
                        List<uint> NewPhms = Phms.Except(args.pharms_to_exclude).ToList();
                        SaveUserMng.pharms= NewPhms;
                        #endregion
                        #region тут мы параллельно пытаемся удалить его из соответствующих аптек
                        Task.Run(() =>
                        {
                            foreach (var i in Phms.Except(NewPhms))
                            {
                                Models.SearchDrugStorePharmId SearchDrgStor = new Models.SearchDrugStorePharmId() { PharmId = i };
                                PharmTaxiAdm.Models.Drugstore.DrugstoreModel DrugStore = new PharmTaxiAdm.Models.Drugstore.DrugstoreModel();
                                List<BsonDocument> FoundedDrStor = UsefullStuff.Common.Methods.MongoTools.Methods.Read<PharmTaxiAdm.Models.Drugstore.DrugstoreModel>.AllFiltered(Newtonsoft.Json.JsonConvert.DeserializeObject<PharmTaxiAdm.Models.Drugstore.DrugstoreModel>(Newtonsoft.Json.JsonConvert.SerializeObject(SearchDrgStor)), Mongo.PHTFORSTFR, Mongo.Pharms);
                                var usrs = FoundedDrStor.FirstOrDefault().TryGetValue("", out BsonValue uzr) ? uzr.AsBsonArray.Values.ToList() : null;// .GetValue("Users").AsBsonArray.Values.ToList();
                                if (usrs is null) continue;
                                else
                                {
                                    try
                                    {
                                        List<uint> Usrs = new List<uint>();
                                        usrs.ForEach(x => Usrs.Add((uint)x.AsInt32));
                                        DrugStore = BsonSerializer.Deserialize<PharmTaxiAdm.Models.Drugstore.DrugstoreModel>(FoundedDrStor.FirstOrDefault()); ///Newtonsoft.Json.JsonConvert.DeserializeObject<PharmTaxiAdm.Models.Drugstore.DrugstoreModel>(FoundedDrStor.FirstOrDefault().AsString);
                                        DrugStore.Users = DrugStore.Users.Except(new List<UserModel>() { new UserModel() { LongId = args.user_id } }).ToList();
                                        UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Mongo.MongoDB, Mongo.Pharms, Mongo.PHTFORSTFR);
                                        WrtPharm.UpSert_Compare(Newtonsoft.Json.JsonConvert.DeserializeObject<PharmTaxiAdm.Models.Drugstore.DrugstoreModel>(Newtonsoft.Json.JsonConvert.SerializeObject(SearchDrgStor)), DrugStore);
                                    }
                                    catch { }
                                }
                                // 
                            }
                        });
                        #endregion
                        #region а тут запускаем операцию в монге в коллекции users
                        RUsrUpd = Task<Exception>.Run(() => WrtUsr.UpSert_Compare(Filt, SaveUserMng));
                        #endregion
                        #region а тут ща пойдёт жара в юмкб
                        Task.Run(() =>
                        {
                            foreach (var i in args.pharms_to_exclude)
                            {
                                PharmTaxiAdm.Methods.User.UserMethods.DetachDrugstoreFromUser(args.user_id, i);
                            }
                        });
                        #endregion
                    }
                    Exception ex_ups = RUsrUpd.Result;
                    
                    #endregion
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new Alert() { code = "200", level = "success", message = $"Result of operation: {ex_ups.Message}; \n{ex_ups.StackTrace}", sticky = false, title = "success" });
                }
                else
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new Alert() { code = "403", level = "error", message = "You have no rights!", sticky = false, title = "error" });
                }
            }
            catch (Exception e)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(new Alert() { code = "403", level = "error", message = e.StackTrace, sticky = false, title = e.Message });
            }
        }
    }
}
