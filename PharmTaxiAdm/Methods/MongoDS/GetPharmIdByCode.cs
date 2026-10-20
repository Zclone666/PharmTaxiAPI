using MongoDB.Bson;
using MongoDB.Driver;
using PTConstants;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiAdm.Methods.MongoDS
{
    public static class GetPharmIdByCode
    {
        public static uint Get(string lic_code)
        {
            var doc = Services.ExternalServices.DB.GetCollection<Models.MongoDS.Pharms>(Mongo.Pharms);
            var filter = new BsonDocument();
            filter = new BsonDocument("_id", lic_code);
            // var filter = Builders<Core.Data.AssortmentItemData>.Filter.Eq("PharmId", getP.PharmId); // тут это для примера - аналог sql-овского "WHERE id=666"
            var result = doc.Find(filter).ToList();
            if (doc.Find(filter).CountDocuments() > 0) //Если количество документов с таким id больше нуля
            {
                return result[0].PharmId;
                //doc.ReplaceOneReplaceOne(filter, userInfo);
                //var update = Builders<Models.UserInfo>.Update.Set(a => a, userInfo); //обновить у структуры данных поле answer (кстати, это у меня другой, вложенный класс данных, зацени красоту)
                //doc[0].
                //doc.UpdateOne(x => x.user_id == user_id, update); //обновить строку в базе
            }
            return 0;
        }

        public static uint GetShortFD(string lic_code)
        {
            var doc = Services.ExternalServices.DBForStupidFront.GetCollection<Models.MongoDS.PharmsFD>(Mongo.PharmsFD);
            var filter = new BsonDocument();
            filter = new BsonDocument("_id", lic_code);
            // var filter = Builders<Core.Data.AssortmentItemData>.Filter.Eq("PharmId", getP.PharmId); // тут это для примера - аналог sql-овского "WHERE id=666"
            var result = doc.Find(filter).ToList();
            if (doc.Find(filter).CountDocuments() > 0) //Если количество документов с таким id больше нуля
            {
                return result[0].PharmId;
                //doc.ReplaceOneReplaceOne(filter, userInfo);
                //var update = Builders<Models.UserInfo>.Update.Set(a => a, userInfo); //обновить у структуры данных поле answer (кстати, это у меня другой, вложенный класс данных, зацени красоту)
                //doc[0].
                //doc.UpdateOne(x => x.user_id == user_id, update); //обновить строку в базе
            }
            return 0;
        }

        public static uint GetLongFD(string lic_code)
        {
            var doc = Services.ExternalServices.DBForStupidFront.GetCollection<Models.MongoDS.Pharms>(Mongo.Pharms);
            var filter = new BsonDocument();
            filter = new BsonDocument("_id", lic_code);
            // var filter = Builders<Core.Data.AssortmentItemData>.Filter.Eq("PharmId", getP.PharmId); // тут это для примера - аналог sql-овского "WHERE id=666"
            var result = doc.Find(filter).ToList();
            if (doc.Find(filter).CountDocuments() > 0) //Если количество документов с таким id больше нуля
            {
                return result[0].PharmId;
                //doc.ReplaceOneReplaceOne(filter, userInfo);
                //var update = Builders<Models.UserInfo>.Update.Set(a => a, userInfo); //обновить у структуры данных поле answer (кстати, это у меня другой, вложенный класс данных, зацени красоту)
                //doc[0].
                //doc.UpdateOne(x => x.user_id == user_id, update); //обновить строку в базе
            }
            return 0;
        }

    }
}
