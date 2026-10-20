using MongoDB.Bson;
using MongoDB.Driver;
using PharmTaxiAdm.Models.MongoDS;
using System;
using System.Collections.Generic;
using PTConstants;
using System.Threading.Tasks;

namespace PharmTaxiAdm.Methods.MongoDS
{
    public static class PharmInsert
    {
        public static Exception InSert(Models.MongoDS.Pharms ass)
        {
            try
            {
                var doc = Services.ExternalServices.DB.GetCollection<Models.MongoDS.Pharms>(Mongo.Pharms);
                if(ass.latitude.ToString().Length>7)ass.latitude = double.Parse(ass.latitude.ToString().Substring(0, 7));
                if (ass.longitude.ToString().Length > 7) ass.longitude= double.Parse(ass.longitude.ToString().Substring(0, 7));
                //  var filter = Builders<Models.LSBase>.Filter.Eq("serial", lsinf.serial); // тут это для примера - аналог sql-овского "WHERE id=666"
                var filter = new BsonDocument("$and", new BsonArray{
                              new BsonDocument("latitude",ass.latitude),
                        new BsonDocument("longitude", ass.longitude)
                // new BsonDocument("PharmId", ass.PharmId);
                 });
                var result = doc.Find(filter).ToList();
                if (doc.Find(filter).CountDocuments() > 0) //Если количество документов с таким id больше нуля
                {
                    ass._id = result[0]._id;
                    ass.PharmId = result[0].PharmId;
                    ass.PharmLogin = result[0].PharmLogin;
                    doc.DeleteMany(filter);
                    doc.InsertOne(ass);
                    //foreach (var i in result)
                    //{
                    //    Pharms a = new Pharms();
                    //    a = Newtonsoft.Json.JsonConvert.DeserializeObject<Pharms>(Newtonsoft.Json.JsonConvert.SerializeObject(i));
                    //    if (a.latitude != ass.latitude && a.longitude != ass.longitude || a.location!=ass.location)
                    //    {
                    //        a.longitude = ass.longitude;
                    //        a.latitude = ass.latitude;
                    //        a.location = ass.location;
                    //    }
                    //    try
                    //    {

                    //    }
                    //    var update = Builders<Pharms>..Update   .Set("location", a.location);
                    //   // update.AddToSet("longitude", a.longitude);
                    //    doc..UpdateOne(filter, update);
                    //    //Core.MongoDB.Methods.Ass_upsert.UpdateAss(a);
                    //}
                    //var update = Builders<Models.UserInfo>.Update.Set(a => a, userInfo); //обновить у структуры данных поле answer (кстати, это у меня другой, вложенный класс данных, зацени красоту)
                    //doc[0].
                    //doc.UpdateOne(x => x.user_id == user_id, update); //обновить строку в базе
                }
                else
                {
                    Pharms a = new Pharms();
                    a = Newtonsoft.Json.JsonConvert.DeserializeObject<Pharms>(Newtonsoft.Json.JsonConvert.SerializeObject(ass));
                    doc.InsertOne(a);
                } // если таких строк не найдено, вставить новую.                
                return null;
            }
            catch (Exception ex) { return ex; }

        }

        public static Exception InSertForDanya(Models.MongoDS.Pharms ass)
        {
            try
            {
                InSertShortForDanya(ass);
                var doc = Services.ExternalServices.DBForStupidFront.GetCollection<Models.MongoDS.Pharms>(Mongo.Pharms);

                //  var filter = Builders<Models.LSBase>.Filter.Eq("serial", lsinf.serial); // тут это для примера - аналог sql-овского "WHERE id=666"
                var filter = new BsonDocument("$and", new BsonArray{
                              new BsonDocument("latitude",ass.latitude),
                        new BsonDocument("longitude", ass.longitude)
                // new BsonDocument("PharmId", ass.PharmId);
                 });
                var result = doc.Find(filter).ToList();
                if (doc.Find(filter).CountDocuments() > 0) //Если количество документов с таким id больше нуля
                {
                    ass._id = result[0]._id;
                    ass.PharmId = result[0].PharmId;
                    ass.PharmLogin = result[0].PharmLogin;
                    doc.DeleteMany(filter);
                    doc.InsertOne(ass);
                    //foreach (var i in result)
                    //{
                    //    Pharms a = new Pharms();
                    //    a = Newtonsoft.Json.JsonConvert.DeserializeObject<Pharms>(Newtonsoft.Json.JsonConvert.SerializeObject(i));
                    //    if (a.latitude != ass.latitude && a.longitude != ass.longitude || ass.location!=a.location)
                    //    {
                    //        a.longitude = ass.longitude;
                    //        a.latitude = ass.latitude;
                    //        a.location = ass.location;
                    //    }
                    //    var update = Builders<Pharms>.Update.Set("location", a.location);
                    //    //update.AddToSet("longitude", a.longitude);
                    //    doc.UpdateOne(filter, update);
                        //Core.MongoDB.Methods.Ass_upsert.UpdateAss(a);
                    //}
                    //var update = Builders<Models.UserInfo>.Update.Set(a => a, userInfo); //обновить у структуры данных поле answer (кстати, это у меня другой, вложенный класс данных, зацени красоту)
                    //doc[0].
                    //doc.UpdateOne(x => x.user_id == user_id, update); //обновить строку в базе
                }
                else
                {
                    Pharms a = new Pharms();
                    a = Newtonsoft.Json.JsonConvert.DeserializeObject<Pharms>(Newtonsoft.Json.JsonConvert.SerializeObject(ass));
                    doc.InsertOne(a);
                } // если таких строк не найдено, вставить новую.                
                return null;
            }
            catch (Exception ex) { return ex; }

        }

        public static Exception InSertDrugNet(Models.MongoDS.PharmNets ass)
        {
            try
            {
                var doc = Services.ExternalServices.DBForStupidFront.GetCollection<Models.MongoDS.PharmNets>(Mongo.PharmNets);

                //  var filter = Builders<Models.LSBase>.Filter.Eq("serial", lsinf.serial); // тут это для примера - аналог sql-овского "WHERE id=666"
                var filter =// new BsonDocument("$and", new BsonArray{
                            //  new BsonDocument("PharmPackId",ass.PharmPackId),
                        new BsonDocument("PharmId", ass.PharmId);
                // new BsonDocument("user_id", User_ID)
                // });
                var result = doc.Find(filter).ToList();
                if (doc.Find(filter).CountDocuments() > 0) //Если количество документов с таким id больше нуля
                {
                    foreach (var i in result)
                    {
                        PharmNets a = new PharmNets();
                        a = Newtonsoft.Json.JsonConvert.DeserializeObject<PharmNets>(Newtonsoft.Json.JsonConvert.SerializeObject(i));
                        var update = Builders<PharmNets>.Update.Set("Adress", a.Address);
                        // update.AddToSet("longitude", a.longitude);
                        doc.UpdateOne(filter, update);
                        //Core.MongoDB.Methods.Ass_upsert.UpdateAss(a);
                    }
                    //var update = Builders<Models.UserInfo>.Update.Set(a => a, userInfo); //обновить у структуры данных поле answer (кстати, это у меня другой, вложенный класс данных, зацени красоту)
                    //doc[0].
                    //doc.UpdateOne(x => x.user_id == user_id, update); //обновить строку в базе
                }
                else
                {
                    //PharmNets a = new PharmNets();
                    //a = Newtonsoft.Json.JsonConvert.DeserializeObject<PharmNets>(Newtonsoft.Json.JsonConvert.SerializeObject(ass));
                    doc.InsertOne(ass);
                } // если таких строк не найдено, вставить новую.                
                return null;
            }
            catch (Exception ex) { return ex; }

        }


        public static Exception InSertShortForDanya(Models.MongoDS.Pharms ass)
        {
            try
            {
                var doc = Services.ExternalServices.DBForStupidFront.GetCollection<Models.MongoDS.PharmsFD>(Mongo.PharmsFD);

                //  var filter = Builders<Models.LSBase>.Filter.Eq("serial", lsinf.serial); // тут это для примера - аналог sql-овского "WHERE id=666"
                //var filter = new BsonDocument("PharmId", ass.PharmId);
                //  new BsonDocument("PharmPackId",ass.PharmPackId),
                // new BsonDocument("$and", new BsonArray{
                //  new BsonDocument("PharmPackId",ass.PharmPackId),

                // new BsonDocument("user_id", User_ID)
                //});

                var filter = new BsonDocument("$and", new BsonArray{
                              new BsonDocument("latitude",ass.latitude),
                        new BsonDocument("longitude", ass.longitude)
                // new BsonDocument("PharmId", ass.PharmId);
                 });

                PharmsFD pharmsFD = new PharmsFD() { PharmId = ass.PharmId, IsActive = ass.IsActive, _id = ass._id, mon = ass.mon, tue = ass.tue, wed = ass.wed, thu = ass.thu, fri = ass.fri, sat = ass.sat, sun = ass.sun, latitude = ass.latitude, longitude = ass.longitude };

                var result = doc.Find(filter).ToList();
                if (result.Count == 0) doc.InsertOne(pharmsFD);
                else
                {  //doc.UpdateOne(filter, Builders<PharmsFD>.Update.Set("location", pharmsFD.location));
                    doc.DeleteMany(filter);
                    doc.InsertOne(pharmsFD);
                }
                 // если таких строк не найдено, вставить новую.                
                return null;
            }
            catch (Exception ex) { return ex; }

        }


        public static async Task<Exception> InSertAsync(Pharms ass)
        {
            try
            {
                Task.Run(() => InSertForDanya(ass));
                return await Task.Run(() => InSert(ass));
            }
            catch { return null; }
        }

        public static Exception ButchInSert(List<Pharms> Lass)
        {
            List<Pharms> ass = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Pharms>>(Newtonsoft.Json.JsonConvert.SerializeObject(Lass));
            if (Lass[0].latitude != ass[0].latitude && Lass[0].longitude != ass[0].longitude)
            {

                for (int cnt = 0; cnt < Lass.Count; cnt++)
                {
                    ass[cnt].longitude = Lass[cnt].longitude;
                    ass[cnt].latitude = Lass[cnt].latitude;
                }
                /* List<Pharms> ass2 = new List<Pharms>();
                foreach(var i in Lass)
                {
                    Pharms a = new Pharms(i);
                    ass2.Add(a);
                }*/
            }
            try
            {
                var doc = Services.ExternalServices.DB.GetCollection<Models.MongoDS.Pharms>(Mongo.Pharms);
                //  int flag = -1;
                //  var filter = Builders<Models.LSBase>.Filter.Eq("serial", lsinf.serial); // тут это для примера - аналог sql-овского "WHERE id=666"
                // for (int i = 0; i < ass.Count; i++) {
                //var filter = new BsonDocument("$and", new BsonArray{
                //       // new BsonDocument("PharmPackId",ass[0].PharmPackId),
                //        new BsonDocument("PharmId",ass[0].PharmId),
                //       // new BsonDocument("user_id", User_ID)
                //    });
                foreach (var i in ass)
                {
                    var filter = new BsonDocument("$and", new BsonArray{
                              new BsonDocument("latitude",i.latitude),
                        new BsonDocument("longitude", i.longitude)
                    });
                    var result = doc.Find(filter).ToList();
                    if (doc.Find(filter).CountDocuments() > 0) //Если количество документов с таким id больше нуля
                    {

                        i._id = result[0]._id;
                        i.PharmId = result[0].PharmId;
                        i.PharmLogin = result[0].PharmLogin;
                        doc.DeleteMany(filter);
                     

                        // return new Exception("There is the same data!");
                        //var update = Builders<Models.UserInfo>.Update.Set(a => a, userInfo); //обновить у структуры данных поле answer (кстати, это у меня другой, вложенный класс данных, зацени красоту)
                        //doc[0].
                        //doc.UpdateOne(x => x.user_id == user_id, update); //обновить строку в базе
                    }
                }
                    //else { flag = i; }
                    // }
                    // if (flag != -1) return new Exception("There is the same data!");
                    
                        doc.InsertMany(ass); //} // если таких строк не найдено, вставить новую.                
                        return null;
                    
                
            }
            catch (Exception ex) { return ex; }
        }

        public static Exception ButchInSertForDanya(List<Pharms> Lass)
        {
            List<Pharms> ass = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Pharms>>(Newtonsoft.Json.JsonConvert.SerializeObject(Lass));
            if (Lass[0].latitude != ass[0].latitude && Lass[0].longitude != ass[0].longitude)
            {

                for (int cnt = 0; cnt < Lass.Count; cnt++)
                {
                    ass[cnt].longitude = Lass[cnt].longitude;
                    ass[cnt].latitude = Lass[cnt].latitude;
                }
                /* List<Pharms> ass2 = new List<Pharms>();
                foreach(var i in Lass)
                {
                    Pharms a = new Pharms(i);
                    ass2.Add(a);
                }*/
            }
            try
            {
                ButchInSertShortForDanya(Lass);
                // new BsonDocument("user_id", User_ID)
                //});

                //PharmsFD pharmsFD = new PharmsFD() { IsActive = ass.IsActive, _id = ass._id, mon = ass.mon, tue = ass.tue, wed = ass.wed, thu = ass.thu, fri = ass.fri, sat = ass.sat, sun = ass.sun, latitude = ass.latitude, longitude = ass.longitude, location = ass.location };


                //////////////////////
                var doc = Services.ExternalServices.DBForStupidFront.GetCollection<Models.MongoDS.Pharms>(Mongo.Pharms);
                //  int flag = -1;
                //  var filter = Builders<Models.LSBase>.Filter.Eq("serial", lsinf.serial); // тут это для примера - аналог sql-овского "WHERE id=666"
                // for (int i = 0; i < ass.Count; i++) {
                foreach (var i in ass)
                {
                    var filter = new BsonDocument("$and", new BsonArray{
                              new BsonDocument("latitude",i.latitude),
                        new BsonDocument("longitude", i.longitude)
                    });
                    var result = doc.Find(filter).ToList();
                    if (doc.Find(filter).CountDocuments() > 0) //Если количество документов с таким id больше нуля
                    {

                        i._id = result[0]._id;
                        i.PharmId = result[0].PharmId;
                        i.PharmLogin = result[0].PharmLogin;
                        doc.DeleteMany(filter);
                    }
                }
                doc.InsertMany(ass); //} // если таких строк не найдено, вставить новую.                
                return null;
                
            }
            catch (Exception ex) { return ex; }
        }

        public static Exception ButchInSertShortForDanya(List<Pharms> Lass)
        {
            try
            {
                List<PharmsFD> ass = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PharmsFD>>(Newtonsoft.Json.JsonConvert.SerializeObject(Lass));
                var doc = Services.ExternalServices.DBForStupidFront.GetCollection<Models.MongoDS.PharmsFD>(Mongo.PharmsFD);
                foreach (var i in ass)
                {
                    var filter = new BsonDocument("$and", new BsonArray{
                              new BsonDocument("latitude",i.latitude),
                        new BsonDocument("longitude", i.longitude)
                    });
                    var result = doc.Find(filter).ToList();
                    if (doc.Find(filter).CountDocuments() > 0) //Если количество документов с таким id больше нуля
                    {

                        i._id = result[0]._id;
                        i.PharmId = result[0].PharmId;
                        doc.DeleteMany(filter);
                    }
                }
                doc.InsertMany(ass);
            //    else doc.UpdateOne(filter, Builders<PharmsFD>.Update.Set("latitude", ass[0].latitude));
                return null;
            }
            catch (Exception ex) { return ex; }
        }

        public static async Task<Exception> ButchInSertAsync(List<Pharms> ass)
        {
            try
            {
                Task.Run(() => ButchInSertForDanya(ass));
                return await Task.Run(() => ButchInSert(ass));
            }
            catch { return null; }
        }
    }
}
