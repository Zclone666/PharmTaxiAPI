using Core.Data;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using PTConstants;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MongoDB.Methods
{
    public static class Ass_upsert
    {
        public static Exception UpSert(Core.Data.AssItemData ass)
        {
            try
            {
                var doc = StaticObjects.ExternalServices.DB.GetCollection<AssItemData>(Mongo.Assortiment);
                var filter = new BsonDocument();
                //  var filter = Builders<Models.LSBase>.Filter.Eq("serial", lsinf.serial); // тут это для примера - аналог sql-овского "WHERE id=666"
                if (!(ass.PackId is null || ass.PackId == 0))
                    filter = new BsonDocument("$and", new BsonArray{
                        new BsonDocument("PharmId",ass.PharmId),
                        new BsonDocument("PackId",ass.PackId),
                       // new BsonDocument("user_id", User_ID)
                    });
                else if (!(ass.PharmPackId is null) && (ass.PackId is null || ass.PackId == 0))
                {
                    filter = new BsonDocument("$and", new BsonArray{
                        new BsonDocument("PharmId",ass.PharmId),
                        new BsonDocument("PharmPackId",ass.PharmPackId),
                    });
                }
                else if (ass.PharmPackId is null && (ass.PackId is null || ass.PackId==0))
                    filter = new BsonDocument("$and", new BsonArray{
                        new BsonDocument("PharmId",ass.PharmId),
                        new BsonDocument("TradeName",ass.TradeName),
                       // new BsonDocument("user_id", User_ID)
                    });
                
                var result = doc.Find(filter).ToList();
                if (doc.Find(filter).CountDocuments() > 0) //Если количество документов с таким id больше нуля
                {
                    var update = Builders<AssItemData>.Update.Set("Quantity", result[0].Quantity-ass.Quantity);
                    //doc[0].
                    doc.UpdateOne(filter, update);
                    //doc.UpdateOne(x => x.user_id == user_id, update); //обновить строку в базе
                }
                else { doc.InsertOne(ass); } // если таких строк не найдено, вставить новую.                
                return null;
            }
            catch (Exception ex) { return ex; }

        }

        public static Exception UpdateAss(Core.Data.AssItemData ass)
        {
            try
            {
                var doc = StaticObjects.ExternalServices.DB.GetCollection<AssItemData>(Mongo.Assortiment);
                var filter = new BsonDocument();
                //  var filter = Builders<Models.LSBase>.Filter.Eq("serial", lsinf.serial); // тут это для примера - аналог sql-овского "WHERE id=666"
                if (ass.PharmPackId is null)
                    filter = new BsonDocument("$and", new BsonArray{
                        new BsonDocument("PharmId",ass.PharmId),
                        new BsonDocument("TradeName",ass.TradeName),
                       // new BsonDocument("user_id", User_ID)
                    });
                else
                {
                    filter = new BsonDocument("$and", new BsonArray{
                        new BsonDocument("PharmId",ass.PharmId),
                        new BsonDocument("PharmPackId",ass.PharmPackId),
                    });
                }
                var result = doc.Find(filter).ToList();
                if (doc.Find(filter).CountDocuments() > 0) //Если количество документов с таким id больше нуля
                {
                    var update = Builders<AssItemData>.Update.Set("Quantity", ass.Quantity);
                    foreach (var i in result)
                    {
                        if (i.Quantity != ass.Quantity)
                        {
                            update = Builders<AssItemData>.Update.Set("Quantity", ass.Quantity);
                            doc.UpdateOne(filter, update);
                        }
                        else if (i.SellingPrice != ass.SellingPrice)
                        {
                            update = Builders<AssItemData>.Update.Set("SellingPrice", ass.SellingPrice);
                            doc.UpdateOne(filter, update);
                        }
                        else if (i.Quantity != ass.Quantity && i.SellingPrice != ass.SellingPrice)
                        {
                            var updateQ = Builders<AssItemData>.Update.Set("Quantity", ass.Quantity);
                            var updateP = Builders<AssItemData>.Update.Set("SellingPrice", ass.SellingPrice);
                            doc.UpdateOne(filter, updateQ);
                            doc.UpdateOne(filter, updateP);
                        }
                    }
                    //doc[0].
                    
                   // 
                    //doc.UpdateOne(x => x.user_id == user_id, update); //обновить строку в базе
                }
                else { doc.InsertOne(ass); } // если таких строк не найдено, вставить новую.                
                return null;
            }
            catch (Exception ex) { return ex; }

        }

        public static Exception UpdatePharmID(Core.Data.AssItemData ass)
        {
            try
            {
                var doc = StaticObjects.ExternalServices.DB.GetCollection<AssItemData>(Mongo.Assortiment);
                var filter = new BsonDocument();
                //  var filter = Builders<Models.LSBase>.Filter.Eq("serial", lsinf.serial); // тут это для примера - аналог sql-овского "WHERE id=666"
                if (ass.PharmPackId is null)
                    filter = new BsonDocument("$and", new BsonArray{
                        new BsonDocument("PharmId",ass.PharmId),
                        new BsonDocument("TradeName",ass.PharmPackId),
                       // new BsonDocument("user_id", User_ID)
                    });
                else
                {
                    filter = new BsonDocument("$and", new BsonArray{
                        new BsonDocument("PharmId",ass.PharmId),
                        new BsonDocument("PharmPackId",ass.PharmPackId),
                    });
                }
                var result = doc.Find(filter).ToList();
                if (doc.Find(filter).CountDocuments() > 0) //Если количество документов с таким id больше нуля
                {
                    var update = Builders<AssItemData>.Update.Set("PackId", ass.PackId);
                    foreach (var i in result)
                    {
                        if(i.PackId is null && !(i.PackID is null))i.PackId = i.PackID;
                        if (i.PackId != ass.PackId)
                        {
                            doc.UpdateOne(filter, update);
                        }
                    }
                    //doc[0].

                    // 
                    //doc.UpdateOne(x => x.user_id == user_id, update); //обновить строку в базе
                }
                else { doc.InsertOne(ass); } // если таких строк не найдено, вставить новую.                
                return null;
            }
            catch (Exception ex) { return ex; }

        }

    }
}
