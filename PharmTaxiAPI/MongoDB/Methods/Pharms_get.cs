using Core.Data;
using Core.MongoDB.Models;
using PTConstants;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.MongoDB.Methods
{
    public static class Pharms_get
    {
        public static List<Core.Data.Pharms> Check(Core.Data.Pharms getP)
        {
            try
            {
                var doc = StaticObjects.ExternalServices.DB.GetCollection<Pharms>(Mongo.Pharms);
                var filter = new BsonDocument();
                if (getP.PharmId ==0)filter = new BsonDocument("PharmLogin", getP.PharmLogin);
                else filter = new BsonDocument("PharmId", getP.PharmId);
                //else filter = new BsonDocument("$and", new BsonArray { new BsonDocument("PharmId", getP.PharmId), new BsonDocument("TradeName", new BsonRegularExpression(new Regex(getP.TradeName, RegexOptions.IgnoreCase))) });
                // var filter = Builders<Core.Data.AssortmentItemData>.Filter.Eq("PharmId", getP.PharmId); // тут это для примера - аналог sql-овского "WHERE id=666"
                var result = doc.Find(filter).ToList();//.Skip(getP.SetSkip).Limit(getP.SetLimit).ToList();
                if (doc.Find(filter).CountDocuments() > 0) //Если количество документов с таким id больше нуля
                {
                    return result;
                    //doc.ReplaceOneReplaceOne(filter, userInfo);
                    //var update = Builders<Models.UserInfo>.Update.Set(a => a, userInfo); //обновить у структуры данных поле answer (кстати, это у меня другой, вложенный класс данных, зацени красоту)
                    //doc[0].
                    //doc.UpdateOne(x => x.user_id == user_id, update); //обновить строку в базе
                }
                // else { //doc.InsertOne(userInfo); } // если таких строк не найдено, вставить новую.                
                return null;
            }
            catch { return null; }
        }

        public static long getCount(Core.Data.Pharms getP)
        {
            try
            {
                var doc = StaticObjects.ExternalServices.DB.GetCollection<Pharms>(Mongo.Pharms);
                var filter = new BsonDocument("PharmId", getP.PharmId);
                // var filter = Builders<Core.Data.AssortmentItemData>.Filter.Eq("PharmId", getP.PharmId); // тут это для примера - аналог sql-овского "WHERE id=666"
                var result = doc.Find(filter).ToList();
                return doc.Find(filter).CountDocuments();
                /*if (doc.Find(filter).CountDocuments() > 0) //Если количество документов с таким id больше нуля
                {
                    return doc.Find(filter).CountDocuments();
                    //doc.ReplaceOneReplaceOne(filter, userInfo);
                    //var update = Builders<Models.UserInfo>.Update.Set(a => a, userInfo); //обновить у структуры данных поле answer (кстати, это у меня другой, вложенный класс данных, зацени красоту)
                    //doc[0].
                    //doc.UpdateOne(x => x.user_id == user_id, update); //обновить строку в базе
                }
                // else { //doc.InsertOne(userInfo); } // если таких строк не найдено, вставить новую.                
                return null;*/
            }
            catch { return 0; }
        }

    }
}
