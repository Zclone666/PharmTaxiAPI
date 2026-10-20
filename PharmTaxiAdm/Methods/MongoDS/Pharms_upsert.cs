//using MongoDB.Bson;
//using MongoDB.Driver;
//using PharmTaxiAdm.Constants;
//using PharmTaxiAdm.Models.MongoDS;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PharmTaxiAdm.Methods.MongoDS
//{
//    public static class Pharms_upsert
//    {
//        public static Exception UpSert(Pharms phms)
//        {
//            try
//            {
//                var doc = Services.ExternalServices.DB.GetCollection<Pharms>(Consts.Pharms);
//                var filter = new BsonDocument();
//                //  var filter = Builders<Models.LSBase>.Filter.Eq("serial", lsinf.serial); // тут это для примера - аналог sql-овского "WHERE id=666"
//                if (!(phms.PharmId ==0))// || ass.PackId == 0))
//                    filter = new BsonDocument("$or", new BsonArray{
//                        new BsonDocument("PharmId",phms.PharmId),
//                        new BsonDocument("PharmLogin",phms.PharmLogin),
//                       // new BsonDocument("user_id", User_ID)
//                    });
//                else if (!(phms.PharmId ==0) && (phms.latitude ==0 || phms.longitude==0))
//                {
//                    filter = new BsonDocument("$or", new BsonArray{
//                        new BsonDocument("PharmId",phms.PharmId),
//                        new BsonDocument("PharmLogin",phms.PharmLogin),
//                    });
//                }
//                //else if (phms.PharmLogin is null && (phms.PharmId==0))
//                //    filter = new BsonDocument("$or", new BsonArray{
//                //        new BsonDocument("longitude",phms.longitude),
//                //        new BsonDocument("latitude",phms.latitude),
//                //       // new BsonDocument("user_id", User_ID)
//                //    });
                
//                var result = doc.Find(filter).ToList();
//                if (doc.Find(filter).CountDocuments() > 0) //Если количество документов с таким id больше нуля
//                {
//                    var update = Builders<Pharms>.Update.Set("PackId", phms.PackId);
//                    //doc[0].
//                    doc.UpdateOne(filter, update);
//                    //doc.UpdateOne(x => x.user_id == user_id, update); //обновить строку в базе
//                }
//                else { doc.InsertOne(phms); } // если таких строк не найдено, вставить новую.                
//                return null;
//            }
//            catch (Exception ex) { return ex; }

//        }

//        public static Exception UpdateAss(Core.Data.Pharms phms)
//        {
//            try
//            {
//                var doc = StaticObjects.ExternalServices.DB.GetCollection<Pharms>(PharmTaxiAPI.Constants.Consts.Pharms);
//                var filter = new BsonDocument();
//                //  var filter = Builders<Models.LSBase>.Filter.Eq("serial", lsinf.serial); // тут это для примера - аналог sql-овского "WHERE id=666"
//                if (phms.PharmId ==0)
//                    filter = new BsonDocument("$and", new BsonArray{
//                        new BsonDocument("PharmLogin",phms.PharmLogin),
//                        new BsonDocument("latitude",phms.latitude),
//                        new BsonDocument("longitude",phms.longitude),
//                       // new BsonDocument("user_id", User_ID)
//                    });
//                else
//                {
//                    filter = new BsonDocument("$and", new BsonArray{
//                        new BsonDocument("PharmId",phms.PharmId),
//                        new BsonDocument("PharmLogin",phms.PharmLogin),
//                    });
//                }
//                var result = doc.Find(filter).ToList();
//                if (doc.Find(filter).CountDocuments() > 0) //Если количество документов с таким id больше нуля
//                {
//                    var update = Builders<Pharms>.Update.Set("PackId", phms.PackId);
//                    foreach (var i in result)
//                    {
//                        if (i.PackId != phms.PackId)
//                        {
//                            update = Builders<Pharms>.Update.Set("PackId", phms.PackId);
//                            doc.UpdateOne(filter, update);
//                        }
                        
//                    }
//                    //doc[0].
                    
//                   // 
//                    //doc.UpdateOne(x => x.user_id == user_id, update); //обновить строку в базе
//                }
//                else { doc.InsertOne(phms); } // если таких строк не найдено, вставить новую.                
//                return null;
//            }
//            catch (Exception ex) { return ex; }

//        }

//    }
//}
