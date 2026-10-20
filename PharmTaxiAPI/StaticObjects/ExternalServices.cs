using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UMKBRequests;
using Core;
using PTConstants;

namespace Core.StaticObjects
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExternalServices
    {

        /// <summary>
        /// Получение смысловых графов
        /// GetSemantic: http://wiki.socmedica.me/doku.php?id=umkb:api:get:semantic
        /// </summary>
        public static UMKBRequests.GetSemantic GetSemantic = new UMKBRequests.GetSemantic();




        private static MongoClient _client;
        private static MongoClient Get()
        {
            if (_client != null) { return _client; }
            _client = new MongoClient(Mongo.MongoDB);
            return _client;
        }
        private static IMongoDatabase _db;

        /// <summary>
        /// База данных СКОЛ (MongoDb)
        /// </summary>
        public static IMongoDatabase DB
        {
            get
            {
                if (_db == null)
                {
                    _db = Get().GetDatabase(Mongo.Pharms);
                    #region Настройка сериализаторов классов данных и создание коллекций
                    //UserInfo
                  //  if (!BsonClassMap.IsClassMapRegistered(typeof(UserInfo))) { BsonClassMap.RegisterClassMap<Models.UserInfo>(cm => { cm.AutoMap(); }); }
                    if (!BsonClassMap.IsClassMapRegistered(typeof(Core.Data.AssortmentItemData))) { BsonClassMap.RegisterClassMap<Core.Data.AssortmentItemData>(cm => { cm.AutoMap(); }); }
                    if (!BsonClassMap.IsClassMapRegistered(typeof(Core.Data.Pharms))) { BsonClassMap.RegisterClassMap<Core.Data.Pharms>(cm => { cm.AutoMap(); }); }

                    HashSet<string> tables = new HashSet<string>(_db.ListCollections().ToList().Select(x => x.GetElement("name").Value.ToString())); //.ToList().Any(x => x.GetElement("name").Value.ToString() == "UserInfo");
                    //UserInfo
          //          if (!tables.Contains(Constants.Consts.Assortiment))
          //          {
          //              _db.CreateCollection(Constants.Consts.Assortiment);
          //              _db.GetCollection<Core.Data.AssortmentItemData>(Constants.Consts.Assortiment).Indexes.CreateOne(new CreateIndexModel<Core.Data.AssortmentItemData>("Name", new CreateIndexOptions() { Unique = false }));
          //          }
                    // lib_67_dyn_codes
                    if (!tables.Contains(Mongo.Assortiment))
                    {
                        _db.CreateCollection(Mongo.Assortiment);
                        var inexes = new List<CreateIndexModel<Core.Data.AssortmentItemData>>() {
                            new CreateIndexModel<Core.Data.AssortmentItemData>(Builders<Core.Data.AssortmentItemData>.IndexKeys.Ascending(proto=>proto.Barcode), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Core.Data.AssortmentItemData>(Builders<Core.Data.AssortmentItemData>.IndexKeys.Ascending(proto=>proto.PharmId), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Core.Data.AssortmentItemData>(Builders<Core.Data.AssortmentItemData>.IndexKeys.Ascending(proto=>proto.PharmPackId), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Core.Data.AssortmentItemData>(Builders<Core.Data.AssortmentItemData>.IndexKeys.Ascending(proto=>proto.TradeName), new CreateIndexOptions() { Unique = false }),
                     //   new CreateIndexModel<Core.Data.AssortmentItemData>("Barcode", new CreateIndexOptions() { Unique = false }),
                     //   new CreateIndexModel<Core.Data.AssortmentItemData>("PharmId", new CreateIndexOptions() { Unique = false }),
                    //    new CreateIndexModel<Core.Data.AssortmentItemData>("PharmPackId", new CreateIndexOptions() { Unique = false }),
                    //    new CreateIndexModel<Core.Data.AssortmentItemData>("TradeName", new CreateIndexOptions() { Unique = false }),
                     //   new CreateIndexModel<Core.Data.AssortmentItemData>(nameof(LSBase.FarmPrep), new CreateIndexOptions() { Unique = false }),
                     //   new CreateIndexModel<Core.Data.AssortmentItemData>(nameof(LSBase.DeyVesch), new CreateIndexOptions() { Unique = false }),
                        };
                        _db.GetCollection<Core.Data.AssortmentItemData>(Mongo.Assortiment).Indexes.CreateMany(inexes);
                    }

                    if (!tables.Contains(Mongo.Pharms))
                    {
                        _db.CreateCollection(Mongo.Pharms);
                        var Phinexes = new List<CreateIndexModel<Core.Data.Pharms>>() {
                            new CreateIndexModel<Core.Data.Pharms>(Builders<Core.Data.Pharms>.IndexKeys.Ascending(proto=>proto._id), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Core.Data.Pharms>(Builders<Core.Data.Pharms>.IndexKeys.Ascending(proto=>proto.PharmId), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Core.Data.Pharms>(Builders<Core.Data.Pharms>.IndexKeys.Ascending(proto=>proto.PharmLogin), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Core.Data.Pharms>(Builders<Core.Data.Pharms>.IndexKeys.Ascending(proto=>proto.latitude), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Core.Data.Pharms>(Builders<Core.Data.Pharms>.IndexKeys.Ascending(proto=>proto.longitude), new CreateIndexOptions() { Unique = false }),
                     //   new CreateIndexModel<Core.Data.AssortmentItemData>("Barcode", new CreateIndexOptions() { Unique = false }),
                     //   new CreateIndexModel<Core.Data.AssortmentItemData>("PharmId", new CreateIndexOptions() { Unique = false }),
                    //    new CreateIndexModel<Core.Data.AssortmentItemData>("PharmPackId", new CreateIndexOptions() { Unique = false }),
                    //    new CreateIndexModel<Core.Data.AssortmentItemData>("TradeName", new CreateIndexOptions() { Unique = false }),
                     //   new CreateIndexModel<Core.Data.AssortmentItemData>(nameof(LSBase.FarmPrep), new CreateIndexOptions() { Unique = false }),
                     //   new CreateIndexModel<Core.Data.AssortmentItemData>(nameof(LSBase.DeyVesch), new CreateIndexOptions() { Unique = false }),
                        };
                        _db.GetCollection<Core.Data.Pharms>(Mongo.Pharms).Indexes.CreateMany(Phinexes);
                    }

                    #endregion
                }
                return _db;
            }
        }



    }
}
