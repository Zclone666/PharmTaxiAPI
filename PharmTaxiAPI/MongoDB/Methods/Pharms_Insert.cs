using Core.Data;
using PTConstants;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MongoDB.Methods
{
    public static class Pharms_Insert
    {
        public static Exception InSert(Core.Data.Pharms phms)
        {
            try
            {
                var doc = StaticObjects.ExternalServices.DB.GetCollection<Core.Data.Pharms>(Mongo.Pharms);

                //  var filter = Builders<Models.LSBase>.Filter.Eq("serial", lsinf.serial); // тут это для примера - аналог sql-овского "WHERE id=666"
                var filter = new BsonDocument("$and", new BsonArray{
                        new BsonDocument("PharmLogin",phms.PharmLogin),
                        new BsonDocument("PharmId",phms.PharmId),
                       // new BsonDocument("user_id", User_ID)
                    });
                var result = doc.Find(filter).ToList();
                if (doc.Find(filter).CountDocuments() > 0) //Если количество документов с таким id больше нуля
                {
                    foreach(var i in result)
                    {
                        Pharms a = new Pharms();
                        a = Newtonsoft.Json.JsonConvert.DeserializeObject<Pharms>(Newtonsoft.Json.JsonConvert.SerializeObject(i));
                     /*   if (a.SellingPrice != phms.SellingPrice)
                        {
                            a.SellingPrice = phms.SellingPrice;
                        }*/
                        Core.MongoDB.Methods.Pharms_upsert.UpdateAss(a);
                    }
                    //var update = Builders<Models.UserInfo>.Update.Set(a => a, userInfo); //обновить у структуры данных поле answer (кстати, это у меня другой, вложенный класс данных, зацени красоту)
                    //doc[0].
                    //doc.UpdateOne(x => x.user_id == user_id, update); //обновить строку в базе
                }
                else {
                    Pharms a = new Pharms();
                    a = Newtonsoft.Json.JsonConvert.DeserializeObject<Pharms>(Newtonsoft.Json.JsonConvert.SerializeObject(phms));
                    doc.InsertOne(a); 
                } // если таких строк не найдено, вставить новую.                
                return null;
            }
            catch (Exception ex) { return ex; }

        }

        public static async Task<Exception> InSertAsync(Core.Data.Pharms phms)
        {
            try {
                return await Task.Run(() => InSert(phms));
            }
            catch { return null; }
        }

        public static Exception ButchInSert(List<Core.Data.Pharms> Lass)
        {
            List<Pharms> ass = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Pharms>>(Newtonsoft.Json.JsonConvert.SerializeObject(Lass));
           /* if (Lass[0].SellingPrice != ass[0].SellingPrice)
            {
                
               for(int cnt = 0;cnt<Lass.Count;cnt++)
                {
                    ass[cnt].SellingPrice = Lass[cnt].SellingPrice;
                }
                List<AssItemData> ass2 = new List<AssItemData>();
                foreach(var i in Lass)
                {
                    AssItemData a = new AssItemData(i);
                    ass2.Add(a);
                }
            }*/
            try
            {
                var doc = StaticObjects.ExternalServices.DB.GetCollection<Core.Data.Pharms>(Mongo.Pharms);
                //  int flag = -1;
                //  var filter = Builders<Models.LSBase>.Filter.Eq("serial", lsinf.serial); // тут это для примера - аналог sql-овского "WHERE id=666"
                // for (int i = 0; i < ass.Count; i++) {
                var filter = new BsonDocument("$and", new BsonArray{
                        new BsonDocument("PharmLogin",ass[0].PharmLogin),
                        new BsonDocument("PharmId",ass[0].PharmId),
                       // new BsonDocument("user_id", User_ID)
                    });
                var result = doc.Find(filter).ToList();
                if (doc.Find(filter).CountDocuments() > 0) //Если количество документов с таким id больше нуля
                {
                    return new Exception("There is the same data!");
                    //var update = Builders<Models.UserInfo>.Update.Set(a => a, userInfo); //обновить у структуры данных поле answer (кстати, это у меня другой, вложенный класс данных, зацени красоту)
                    //doc[0].
                    //doc.UpdateOne(x => x.user_id == user_id, update); //обновить строку в базе
                }

                //else { flag = i; }
                // }
                // if (flag != -1) return new Exception("There is the same data!");
                else
                {
                    doc.InsertMany(ass); //} // если таких строк не найдено, вставить новую.                
                    return null;
                }
            }
            catch (Exception ex) { return ex; }
        }

        public static async Task<Exception> ButchInSertAsync(List<Core.Data.Pharms> ass)
        {
            try
            {
                return await Task.Run(() => ButchInSert(ass));
            }
            catch { return null; }
        }

    }
}
