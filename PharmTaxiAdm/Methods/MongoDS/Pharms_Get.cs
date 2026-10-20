using MongoDB.Bson;
using MongoDB.Driver;
using PharmTaxiAdm.Models.MongoDS;
using System;
using PTConstants;
using System.Text;

namespace PharmTaxiAdm.Methods.MongoDS
{
    public static class Pharms_Get
    {

        public static Exception GetAndActivate(Models.MongoDS.Pharms ass)
        {
            try
            {
                GetAndActivateLongFD(ass);
                var doc = Services.ExternalServices.DB.GetCollection<Models.MongoDS.Pharms>(Mongo.Pharms);

                //  var filter = Builders<Models.LSBase>.Filter.Eq("serial", lsinf.serial); // тут это для примера - аналог sql-овского "WHERE id=666"
                var filter = new BsonDocument("$and", new BsonArray{
                      //  new BsonDocument("PharmPackId",ass.PharmPackId),
                        new BsonDocument("PharmId",ass.PharmId),
                       // new BsonDocument("user_id", User_ID)
                    });
                var result = doc.Find(filter).ToList();
                if (doc.Find(filter).CountDocuments() > 0) //Если количество документов с таким id больше нуля
                {
                    foreach (var i in result)
                    {
                        Pharms a = new Pharms();
                        a = Newtonsoft.Json.JsonConvert.DeserializeObject<Pharms>(Newtonsoft.Json.JsonConvert.SerializeObject(i));
                        if (!a.IsActive)
                        {
                            a.IsActive = true;
                        }
                        var update = Builders<Pharms>.Update.Set("IsActive", a.IsActive);
                        doc.UpdateOne(filter, update);
                        //Core.MongoDB.Methods.Ass_upsert.UpdateAss(a);
                    }
                    //var update = Builders<Models.UserInfo>.Update.Set(a => a, userInfo); //обновить у структуры данных поле answer (кстати, это у меня другой, вложенный класс данных, зацени красоту)
                    //doc[0].
                    //doc.UpdateOne(x => x.user_id == user_id, update); //обновить строку в базе
                }
                else
                {
                    //Pharms a = new Pharms();
                    //a = Newtonsoft.Json.JsonConvert.DeserializeObject<Pharms>(Newtonsoft.Json.JsonConvert.SerializeObject(ass));
                    //doc.InsertOne(a);
                } // если таких строк не найдено, вставить новую.                
                return null;
            }
            catch (Exception ex) { return ex; }

        }

        public static Exception GetAndActivateLongFD(Models.MongoDS.Pharms ass)
        {
            try
            {
                GetAndActivateShortFD(ass);
                var doc = Services.ExternalServices.DBForStupidFront.GetCollection<Models.MongoDS.Pharms>(Mongo.Pharms);

                //  var filter = Builders<Models.LSBase>.Filter.Eq("serial", lsinf.serial); // тут это для примера - аналог sql-овского "WHERE id=666"
                var filter = new BsonDocument("$and", new BsonArray{
                      //  new BsonDocument("PharmPackId",ass.PharmPackId),
                        new BsonDocument("PharmId",ass.PharmId),
                       // new BsonDocument("user_id", User_ID)
                    });
                var result = doc.Find(filter).ToList();
                if (doc.Find(filter).CountDocuments() > 0) //Если количество документов с таким id больше нуля
                {
                    foreach (var i in result)
                    {
                        Pharms a = new Pharms();
                        a = Newtonsoft.Json.JsonConvert.DeserializeObject<Pharms>(Newtonsoft.Json.JsonConvert.SerializeObject(i));
                        if (!a.IsActive)
                        {
                            a.IsActive = true;
                        }
                        var update = Builders<Pharms>.Update.Set("IsActive", a.IsActive);
                        doc.UpdateOne(filter, update);
                        //Core.MongoDB.Methods.Ass_upsert.UpdateAss(a);
                    }
                    //var update = Builders<Models.UserInfo>.Update.Set(a => a, userInfo); //обновить у структуры данных поле answer (кстати, это у меня другой, вложенный класс данных, зацени красоту)
                    //doc[0].
                    //doc.UpdateOne(x => x.user_id == user_id, update); //обновить строку в базе
                }
                else
                {
                    //Pharms a = new Pharms();
                    //a = Newtonsoft.Json.JsonConvert.DeserializeObject<Pharms>(Newtonsoft.Json.JsonConvert.SerializeObject(ass));
                    //doc.InsertOne(a);
                } // если таких строк не найдено, вставить новую.                
                return null;
            }
            catch (Exception ex) { return ex; }

        }

        public static Exception GetAndActivateShortFD(Models.MongoDS.Pharms ass)
        {
            try
            {
                var doc = Services.ExternalServices.DBForStupidFront.GetCollection<Models.MongoDS.PharmsFD>(Mongo.PharmsFD);

                //  var filter = Builders<Models.LSBase>.Filter.Eq("serial", lsinf.serial); // тут это для примера - аналог sql-овского "WHERE id=666"
                var filter = new BsonDocument("$and", new BsonArray{
                      //  new BsonDocument("PharmPackId",ass.PharmPackId),
                        new BsonDocument("PharmId",ass.PharmId),
                       // new BsonDocument("user_id", User_ID)
                    });
                var result = doc.Find(filter).ToList();
                if (doc.Find(filter).CountDocuments() > 0) //Если количество документов с таким id больше нуля
                {
                    foreach (var i in result)
                    {
                        PharmsFD a = new PharmsFD();
                        a = Newtonsoft.Json.JsonConvert.DeserializeObject<PharmsFD>(Newtonsoft.Json.JsonConvert.SerializeObject(i));
                        if (!a.IsActive)
                        {
                            a.IsActive = true;
                        }
                        var update = Builders<PharmsFD>.Update.Set("IsActive", a.IsActive);
                        doc.UpdateOne(filter, update);
                        //Core.MongoDB.Methods.Ass_upsert.UpdateAss(a);
                    }
                    //var update = Builders<Models.UserInfo>.Update.Set(a => a, userInfo); //обновить у структуры данных поле answer (кстати, это у меня другой, вложенный класс данных, зацени красоту)
                    //doc[0].
                    //doc.UpdateOne(x => x.user_id == user_id, update); //обновить строку в базе
                }
                else
                {
                    //Pharms a = new Pharms();
                    //a = Newtonsoft.Json.JsonConvert.DeserializeObject<Pharms>(Newtonsoft.Json.JsonConvert.SerializeObject(ass));
                    //doc.InsertOne(a);
                } // если таких строк не найдено, вставить новую.                
                return null;
            }
            catch (Exception ex) { return ex; }

        }

    }
}
