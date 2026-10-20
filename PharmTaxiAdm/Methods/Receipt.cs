using MongoDB.Bson.Serialization;
using PharmTaxiAdm.Models;
using System;
using System.Collections.Generic;
using PTConstants;
using UMKBCoreAPI.Models.Api.Responses;

namespace PharmTaxiAdm.Methods
{
    public static class Receipt
    {
        public static string Get(SMReceipt.Global.Models.SearchReceipt args)
        {
            SMReceipt.Global.Consts.URLs.Reciepes_Connect = Mongo.ReciepesConString;
            List<string> recipeCodes = new List<string>();
            if (String.IsNullOrEmpty(args.ReceiptCode)) recipeCodes = SMReceipt.Global.Methods.Receipt.GetNumbFin(args);
            string key = String.IsNullOrEmpty(args.ReceiptCode) ? SMReceipt.Global.Methods.Receipt.GetNumb(args) : args.ReceiptCode;
            args.ReceiptCode = key;
            GetReceptJSON data = new GetReceptJSON() { key = key };
            SMReceipt.Global.Models.SaveOrder so = new SMReceipt.Global.Models.SaveOrder() { order = new List<SMReceipt.Global.Models.Order>() { new SMReceipt.Global.Models.Order() { pharmID =args.PharmId, orderToSave = new SMReceipt.Global.Models.OrderToSave() { order_id = args.ReceiptCode } } } };
            so = SMReceipt.Global.Methods.Mongo.GetOrder(args.ReceiptCode, uint.Parse(args.PharmId));
            if (so.alert.code != "666")
            {
                SMReceipt.Global.Consts.URLs.Reciepes_Connect = Mongo.MongoDB;
                return Newtonsoft.Json.JsonConvert.SerializeObject(so);
            }
            NewGetRecipeJSON dataNew = new NewGetRecipeJSON() { key = ECP.Key, active_language = "ru", recipe = key, service = ECP.Service };
            SMReceipt.Global.Consts.URLs.Reciepes_Connect = Mongo.MongoDB;
            return UMKBRequests.WebUtils.Post(URLs.RecipesCore, Newtonsoft.Json.JsonConvert.SerializeObject(dataNew), "application/json").Answer;
        }

        public static List<GetOrdRec> GetNew(SMReceipt.Global.Models.SearchReceipt args)
        {
            SMReceipt.Global.Consts.URLs.Reciepes_Connect = Mongo.ReciepesConString;
            HashSet<Models.Drugstore.DrugstoreModel> phms = new HashSet<Models.Drugstore.DrugstoreModel>();
            // if (!String.IsNullOrEmpty(args.authkey))phms= PharmTaxiAdm.Methods.Drugstore.DrugstoreMethods.GetDrugstoresByUserId(User.UserMethods.GetUserIdByAuthkey(args.authkey).userid.ToString());
            phms = null;
            List<string> recipeCodes = new List<string>();
            List<SMReceipt.Global.Models.SaveOrder> ret = new List<SMReceipt.Global.Models.SaveOrder>();
            List<GetOrdRec> ret2 = new List<GetOrdRec>();
            if (String.IsNullOrEmpty(args.ReceiptCode))
            {
                args.ReceiptsCodes = new List<string>();
                //var bd= (DateTime.TryParse("01.02.2012", out DateTime req))?req:DateTime.Now;
                args.Region = String.IsNullOrEmpty(args.Region) ? "Moscow" : args.Region;
                string xz = Newtonsoft.Json.JsonConvert.SerializeObject(args);
                recipeCodes = SMReceipt.Global.Methods.Receipt.GetNumbFin(args);
              if (recipeCodes[0] == "Patient not found")
                {
                    ret.Add(new SMReceipt.Global.Models.SaveOrder() { alert = new UMKBRequests.Alert() { code = "400", level = "error", message = recipeCodes[0] } });
                    ret2.Add(new GetOrdRec() { isRec = false, order = ret[0] });
                    SMReceipt.Global.Consts.URLs.Reciepes_Connect = Mongo.MongoDB;
                    return ret2;
                }
                else
                {
                    SMReceipt.Global.Consts.URLs.Reciepes_Connect = Mongo.ReciepesConString;
                    foreach (var i in recipeCodes)
                    {
                        SMReceipt.Global.Consts.URLs.Reciepes_Connect = Mongo.ReciepesConString;
                        string key = i;
                        args.ReceiptCode = key;
                        GetReceptJSON data = new GetReceptJSON() { key = key };
                        SMReceipt.Global.Models.SaveOrder so = new SMReceipt.Global.Models.SaveOrder() { order = new List<SMReceipt.Global.Models.Order>() { new SMReceipt.Global.Models.Order() { pharmID = args.PharmId, orderToSave = new SMReceipt.Global.Models.OrderToSave() { order_id = args.ReceiptCode } } } };
                        ModelRecipeLS sp = new ModelRecipeLS();
                        if (!String.IsNullOrEmpty(args.PharmId) && !(phms is null))
                        {
                            foreach (var ii in phms)
                            {
                                so = SMReceipt.Global.Methods.Mongo.GetOrder(args.ReceiptCode, ii.PharmId);
                                if (so.alert.code == "200") break;
                            }
                        }
                        else
                        {
                            if (uint.TryParse(args.PharmId, out uint ph)) so = SMReceipt.Global.Methods.Mongo.GetOrder(args.ReceiptCode, ph);
                            else so = SMReceipt.Global.Methods.Mongo.GetOrder(args.ReceiptCode);
                        }
                        if (so.alert.code != "666")
                        {
                            SMReceipt.Global.Consts.URLs.Reciepes_Connect = Mongo.MongoDB;
                            return new List<GetOrdRec>() { new GetOrdRec() { isRec = false, order = so } };
                        }
                        NewGetRecipeJSON dataNew = new NewGetRecipeJSON() { key = ECP.Key, active_language = "ru", recipe = key, service = ECP.Service };
                        string rStr = UMKBRequests.WebUtils.Post(URLs.RecipesCore, Newtonsoft.Json.JsonConvert.SerializeObject(dataNew), "application/json").Answer;
                        try
                        {
                            sp = Newtonsoft.Json.JsonConvert.DeserializeObject<ModelRecipeLS>(rStr);
                        }
                        catch (Exception e) { so.alert = new UMKBRequests.Alert() { code = "400", level = "error", message = e.Message }; }
                        SMReceipt.Global.Consts.URLs.Reciepes_Connect = Mongo.MongoDB;
                        if ((String.IsNullOrEmpty(sp.idStatus) || !sp.idStatus.Contains("отпущен")) && !sp.dateEnd.HasValue || (String.IsNullOrEmpty(sp.idStatus) || !sp.idStatus.Contains("отпущен") ) && sp.dateEnd.HasValue && sp.dateEnd > ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds()) ret2.Add(new GetOrdRec() { isRec = true, prescript = sp });
                        
                        //return new List<GetOrdRec>() { new GetOrdRec() { isRec = true, prescript = sp } };
                    }   
                    SMReceipt.Global.Consts.URLs.Reciepes_Connect = Mongo.MongoDB;
                    return ret2;
                }
            }
            else
            {
                string key = args.ReceiptCode;
                GetReceptJSON data = new GetReceptJSON() { key = key };
                SMReceipt.Global.Models.SaveOrder so = new SMReceipt.Global.Models.SaveOrder() { order = new List<SMReceipt.Global.Models.Order>() { new SMReceipt.Global.Models.Order() { pharmID = args.PharmId, orderToSave = new SMReceipt.Global.Models.OrderToSave() { order_id = args.ReceiptCode } } } };
                ModelRecipeLS sp = new ModelRecipeLS();
                sp.recipe_code = key;
                if (!String.IsNullOrEmpty(args.PharmId) && (phms is null))
                {
                    foreach (var ii in phms)
                    {
                        so = SMReceipt.Global.Methods.Mongo.GetOrder(args.ReceiptCode, ii.PharmId);
                        if (so.alert.code == "200") break;
                    }
                }
                else
                {
                    if (uint.TryParse(args.PharmId, out uint ph)) so = SMReceipt.Global.Methods.Mongo.GetOrder(args.ReceiptCode, ph);
                    else so = SMReceipt.Global.Methods.Mongo.GetOrder(args.ReceiptCode);
                }
                if (so.alert.code != "666")
                {
                    SMReceipt.Global.Consts.URLs.Reciepes_Connect = Mongo.MongoDB;
                    return new List<GetOrdRec>() { new GetOrdRec() { isRec = false, order = so } };
                }
                NewGetRecipeJSON dataNew = new NewGetRecipeJSON() { key = ECP.Key, active_language = "ru", recipe = key, service = ECP.Service };
                string req = Newtonsoft.Json.JsonConvert.SerializeObject(dataNew);
                string rStr = UMKBRequests.WebUtils.Post(URLs.RecipesCore, Newtonsoft.Json.JsonConvert.SerializeObject(dataNew), "application/json").Answer;
                try
                {
                    sp = Newtonsoft.Json.JsonConvert.DeserializeObject<ModelRecipeLS>(rStr);
                }
                catch (Exception e) { so.alert = new UMKBRequests.Alert() { code = "400", level = "error", message = e.Message }; }
                SMReceipt.Global.Consts.URLs.Reciepes_Connect = Mongo.MongoDB;
                return new List<GetOrdRec>() { new GetOrdRec() { isRec = true, prescript = sp } };
            }
            SMReceipt.Global.Consts.URLs.Reciepes_Connect = Mongo.MongoDB;
            return ret2;
        }

        public static string GetURL(SMReceipt.Global.Models.SearchReceipt args)
        {
            string key = String.IsNullOrEmpty(args.ReceiptCode)?SMReceipt.Global.Methods.Receipt.GetNumb(args):args.ReceiptCode;
            return URLs.RecipeByCode+key;
        }

        public static string GetURLNew(SMReceipt.Global.Models.SearchReceipt args)
        {
            List<string> recipeCodes = new List<string>();
            List<SMReceipt.Global.Models.SaveOrder> ret = new List<SMReceipt.Global.Models.SaveOrder>();
            if (String.IsNullOrEmpty(args.ReceiptCode))
            {
                SMReceipt.Global.Consts.URLs.Reciepes_Connect = Mongo.ReciepesConString;
                recipeCodes = SMReceipt.Global.Methods.Receipt.GetNumbFin(args);
                SMReceipt.Global.Consts.URLs.Reciepes_Connect = Mongo.MongoDB;
                return URLs.RecipeByCode + recipeCodes[0];
            }
            return URLs.RecipeByCode + args.ReceiptCode;
        }

        public static List<ECPLib.Common.Recipes.Models.ModelRecipeBase> ReleasePrescript(ECPLib.Common.Recipes.Models.ModelRecipeBase mrb)
        {
            string collectionName = mrb.id.Substring(0, 2);
            UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Mongo.ReciepesConString, collectionName, Mongo.ReciepesDBName);
            UsefullStuff.Common.Methods.MongoTools.Methods.Write<ECPLib.Common.Recipes.Models.ModelRecipeBase> Wr = new UsefullStuff.Common.Methods.MongoTools.Methods.Write<ECPLib.Common.Recipes.Models.ModelRecipeBase>();
            _idSearch srch = new _idSearch() { _id = mrb.id };
            List<ECPLib.Common.Recipes.Models.ModelRecipeBase> MrBList = UsefullStuff.Common.Methods.MongoTools.Methods.Read<ECPLib.Common.Recipes.Models.ModelRecipeBase>.Check(srch);
            foreach (var i in MrBList)
            {
                i.IdStatus = "отпущен";
                Wr.UpSert(i);
            }
            return MrBList;
        }

        public static List<ECPLib.Common.Recipes.Models.ModelRecipeBase> CheckPrescript(ECPLib.Common.Recipes.Models.ModelRecipeBase mrb)
        {
            string collectionName = mrb.id.Substring(0, 2);
            UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Mongo.ReciepesConString, collectionName, Mongo.ReciepesDBName);
            _idSearch srch = new _idSearch() { _id = mrb.id };
            //UsefullStuff.Common.Methods.MongoTools.Methods.Write<ModelRecipeBase> Wr = new UsefullStuff.Common.Methods.MongoTools.Methods.Write<ModelRecipeBase>();
            List<ECPLib.Common.Recipes.Models.ModelRecipeBase> MrBList = UsefullStuff.Common.Methods.MongoTools.Methods.Read<ECPLib.Common.Recipes.Models.ModelRecipeBase>.Check(srch);
            //List<ModelRecipeBase> MrBList = new List<ModelRecipeBase>();
            //BsonMrBList.ForEach(x => MrBList.Add(BsonSerializer.Deserialize<ModelRecipeBase>(x)));
            foreach (var i in MrBList)
            {
                if(String.IsNullOrEmpty(i.IdStatus) && i.DateEnd != 0 && i.DateEnd > ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds()) i.IdStatus = "рецепт может быть отоварен";
                else if (!String.IsNullOrEmpty(i.IdStatus) && i.IdStatus.Contains("отпущен") && i.DateEnd != 0 && i.DateEnd > ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds()) i.IdStatus = "рецепт ранее был отоварен";
                else if (String.IsNullOrEmpty(i.IdStatus) && i.DateEnd != 0 && i.DateEnd < ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds()) i.IdStatus = "срок действия рецепта истек";
                else if (!String.IsNullOrEmpty(i.IdStatus) && i.IdStatus.Contains("отпущен") && i.DateEnd != 0 && i.DateEnd < ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds()) i.IdStatus = "рецепт ранее был отоварен, срок действия истек";
            }
            return MrBList;
        }

        public static List<ECPLib.Common.Recipes.Models.ModelRecipeBase> ReleasePrescript(string RecipeCode)
        {
            string collectionName = RecipeCode.Substring(0, 2);
            UsefullStuff.Common.Methods.MongoTools.Init.MongoConst(Mongo.ReciepesConString, collectionName, Mongo.ReciepesDBName);
            UsefullStuff.Common.Methods.MongoTools.Methods.Write<ECPLib.Common.Recipes.Models.ModelRecipeBase> Wr = new UsefullStuff.Common.Methods.MongoTools.Methods.Write<ECPLib.Common.Recipes.Models.ModelRecipeBase>();

            ECPLib.Common.Recipes.Models.ModelRecipeBase mrB = new ECPLib.Common.Recipes.Models.ModelRecipeBase() { id = RecipeCode };
            _idSearch srch = new _idSearch() { _id = mrB.id };
            //var yopta= UsefullStuff.Common.Methods.MongoTools.Methods.Read<ModelRecipeBase>.AllFiltered(mrB);
            List<ECPLib.Common.Recipes.Models.ModelRecipeBase> MrBList =
                new List<ECPLib.Common.Recipes.Models.ModelRecipeBase>();
            try
            {
                var tt = UsefullStuff.Common.Methods.MongoTools.Methods.Read<ECPLib.Common.Recipes.Models.ModelRecipeBase>.AllFiltered(srch);//.FirstOrDefault();
                tt.ForEach(x=> MrBList.Add(BsonSerializer.Deserialize<ECPLib.Common.Recipes.Models.ModelRecipeBase>(x)));
              
            }
            catch
            {
                try
                {
                    var tt = UsefullStuff.Common.Methods.MongoTools.Methods.Read<ECPLib.Common.Recipes.Models.ModelRecipeBase>.Check(srch);
                    tt.ForEach(x => MrBList.Add(x));
                   
                }
                catch
                {
                    return new List<ECPLib.Common.Recipes.Models.ModelRecipeBase>() { new ECPLib.Common.Recipes.Models.ModelRecipeBase() { Alert = new Alert() { Code = "200", Level = "success", Message = "No Recipe Found", Title = "success", Sticky = false } } };
                }
            }
            
            foreach (var i in MrBList)
            {
                i.IdStatus = "отпущен";
                Wr.UpSert(i);
            }
            return MrBList.Count>0?MrBList: new List<ECPLib.Common.Recipes.Models.ModelRecipeBase>() { new ECPLib.Common.Recipes.Models.ModelRecipeBase() { Alert = new Alert() { Code = "200", Level = "success", Message = "No Recipe Found", Title = "success", Sticky = false } } }; ;
        }
    }
}
