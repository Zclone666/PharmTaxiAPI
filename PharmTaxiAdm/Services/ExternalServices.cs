using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UMKBRequests;
using PTConstants;

namespace PharmTaxiAdm.Services
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExternalServices
    {
        private static MongoClient _client;
        private static MongoClient Get()
        {
            if (_client != null) { return _client; }
            _client = new MongoClient(Mongo.MongoDB);
            return _client;
        }
        private static IMongoDatabase _db;
        public static void DBInit()
        {
            try
            {
                if (_db == null)
                {
                    _db = Get().GetDatabase(Mongo.PHT);
                    #region Настройка сериализаторов классов данных и создание коллекций
                    //UserInfo
                    //  if (!BsonClassMap.IsClassMapRegistered(typeof(UserInfo))) { BsonClassMap.RegisterClassMap<Models.UserInfo>(cm => { cm.AutoMap(); }); }
                    // if (!BsonClassMap.IsClassMapRegistered(typeof(Models.Drugstore.DrugstoreModel))) { BsonClassMap.RegisterClassMap<Models.Drugstore.DrugstoreModel>(cm => { cm.AutoMap(); }); }
                    if (!BsonClassMap.IsClassMapRegistered(typeof(Models.MongoDS.Pharms))) { BsonClassMap.RegisterClassMap<Models.MongoDS.Pharms>(cm => { cm.AutoMap(); }); }

                    HashSet<string> tables = new HashSet<string>(_db.ListCollections().ToList().Select(x => x.GetElement("name").Value.ToString())); //.ToList().Any(x => x.GetElement("name").Value.ToString() == "UserInfo");
                                                                                                                                                     //UserInfo
                                                                                                                                                     //          if (!tables.Contains(Constants.Consts.Assortiment))
                                                                                                                                                     //          {
                                                                                                                                                     //              _db.CreateCollection(Constants.Consts.Assortiment);
                                                                                                                                                     //              _db.GetCollection<Core.Data.AssortmentItemData>(Constants.Consts.Assortiment).Indexes.CreateOne(new CreateIndexModel<Core.Data.AssortmentItemData>("Name", new CreateIndexOptions() { Unique = false }));
                                                                                                                                                     //          }
                                                                                                                                                     // lib_67_dyn_codes
                    if (!tables.Contains(Mongo.Pharms))
                    {
                        _db.CreateCollection(Mongo.Pharms);

                        var indexes = new List<CreateIndexModel<Models.MongoDS.Pharms>>() 
                        {
                            new CreateIndexModel<Models.MongoDS.Pharms>(Builders<Models.MongoDS.Pharms>.IndexKeys.Ascending(proto=>proto.latitude), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Models.MongoDS.Pharms>(Builders<Models.MongoDS.Pharms>.IndexKeys.Ascending(proto=>proto.PharmId), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Models.MongoDS.Pharms>(Builders<Models.MongoDS.Pharms>.IndexKeys.Ascending(proto=>proto.longitude), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Models.MongoDS.Pharms>(Builders<Models.MongoDS.Pharms>.IndexKeys.Ascending(proto=>proto.PharmLogin), new CreateIndexOptions() { Unique = false }),
                            //new CreateIndexModel<Core.Data.AssortmentItemData>("Barcode", new CreateIndexOptions() { Unique = false }),
                            //new CreateIndexModel<Core.Data.AssortmentItemData>("PharmId", new CreateIndexOptions() { Unique = false }),
                            //new CreateIndexModel<Core.Data.AssortmentItemData>("PharmPackId", new CreateIndexOptions() { Unique = false }),
                            //new CreateIndexModel<Core.Data.AssortmentItemData>("TradeName", new CreateIndexOptions() { Unique = false }),
                            //new CreateIndexModel<Core.Data.AssortmentItemData>(nameof(LSBase.FarmPrep), new CreateIndexOptions() { Unique = false }),
                            //new CreateIndexModel<Core.Data.AssortmentItemData>(nameof(LSBase.DeyVesch), new CreateIndexOptions() { Unique = false }),
                        };

                        _db.GetCollection<Models.MongoDS.Pharms>(Mongo.Pharms).Indexes.CreateMany(indexes);
                        _db.GetCollection<Models.MongoDS.Pharms>(Mongo.Pharms).Indexes.CreateOne(new CreateIndexModel<Models.MongoDS.Pharms>(Builders<Models.MongoDS.Pharms>.IndexKeys.Ascending(proto => proto._id), new CreateIndexOptions() { Unique = false }));
                    }

                    #endregion
                }
            }
            catch { }// return _db; }
            //return _db;
        }
        /// <summary>
        /// База данных СКОЛ (MongoDb)
        /// </summary>
        public static IMongoDatabase DB
        {
            get
            {
                return _db;
            }
        }


        private static IMongoDatabase _dbForStupidFront;

        public static void DBForStupidFrontInit()
        {
            try
            {
                if (_dbForStupidFront == null)
                {
                    _dbForStupidFront = Get().GetDatabase(Mongo.PHTFORSTFR);
                    #region Настройка сериализаторов классов данных и создание коллекций
                    //UserInfo
                    //  if (!BsonClassMap.IsClassMapRegistered(typeof(UserInfo))) { BsonClassMap.RegisterClassMap<Models.UserInfo>(cm => { cm.AutoMap(); }); }
                    // if (!BsonClassMap.IsClassMapRegistered(typeof(Models.Drugstore.DrugstoreModel))) { BsonClassMap.RegisterClassMap<Models.Drugstore.DrugstoreModel>(cm => { cm.AutoMap(); }); }
                    if (!BsonClassMap.IsClassMapRegistered(typeof(Models.MongoDS.Pharms))) { BsonClassMap.RegisterClassMap<Models.MongoDS.Pharms>(cm => { cm.AutoMap(); }); }
                    if (!BsonClassMap.IsClassMapRegistered(typeof(Models.MongoDS.PharmsFD))) { BsonClassMap.RegisterClassMap<Models.MongoDS.PharmsFD>(cm => { cm.AutoMap(); }); }
                    //  HashSet<string> tables = new HashSet<string>(_db.ListCollections().ToList().Select(x => x.GetElement("name").Value.ToString())); //.ToList().Any(x => x.GetElement("name").Value.ToString() == "UserInfo");
                    #endregion

                    HashSet<string> tables2 = new HashSet<string>(_dbForStupidFront.ListCollections().ToList().Select(x => x.GetElement("name").Value.ToString())); //.ToList().Any(x => x.GetElement("name").Value.ToString() == "UserInfo");
                    if (!tables2.Contains(Mongo.Pharms))
                    {
                        _dbForStupidFront.CreateCollection(Mongo.Pharms);
                        var indexes = new List<CreateIndexModel<Models.MongoDS.Pharms>>() {
                            new CreateIndexModel<Models.MongoDS.Pharms>(Builders<Models.MongoDS.Pharms>.IndexKeys.Ascending(proto=>proto.latitude), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Models.MongoDS.Pharms>(Builders<Models.MongoDS.Pharms>.IndexKeys.Ascending(proto=>proto.PharmId), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Models.MongoDS.Pharms>(Builders<Models.MongoDS.Pharms>.IndexKeys.Ascending(proto=>proto.longitude), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Models.MongoDS.Pharms>(Builders<Models.MongoDS.Pharms>.IndexKeys.Ascending(proto=>proto.PharmLogin), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Models.MongoDS.Pharms>(Builders<Models.MongoDS.Pharms>.IndexKeys.Geo2DSphere(proto=>proto.location))
                     //   new CreateIndexModel<Core.Data.AssortmentItemData>("Barcode", new CreateIndexOptions() { Unique = false }),
                     //   new CreateIndexModel<Core.Data.AssortmentItemData>("PharmId", new CreateIndexOptions() { Unique = false }),
                    //    new CreateIndexModel<Core.Data.AssortmentItemData>("PharmPackId", new CreateIndexOptions() { Unique = false }),
                    //    new CreateIndexModel<Core.Data.AssortmentItemData>("TradeName", new CreateIndexOptions() { Unique = false }),
                     //   new CreateIndexModel<Core.Data.AssortmentItemData>(nameof(LSBase.FarmPrep), new CreateIndexOptions() { Unique = false }),
                     //   new CreateIndexModel<Core.Data.AssortmentItemData>(nameof(LSBase.DeyVesch), new CreateIndexOptions() { Unique = false }),
                        };
                        _dbForStupidFront.GetCollection<Models.MongoDS.Pharms>(Mongo.Pharms).Indexes.CreateMany(indexes);
                 //       _dbForStupidFront.GetCollection<Models.MongoDS.Pharms>(PharmTaxiAdm.Constants.Consts.Pharms).Indexes.CreateOne(new CreateIndexModel<Models.MongoDS.Pharms>(Builders<Models.MongoDS.Pharms>.IndexKeys.Ascending(proto => proto._id), new CreateIndexOptions() { Unique = false }));
                        //_dbForStupidFront.CreateCollection(PharmTaxiAdm.Constants.Consts.Pharms) reateIndex(Indexes.geo2dsphere("contact.location"));
                    }
                    if (!tables2.Contains(Mongo.PharmsFD))
                    {
                        tables2 = new HashSet<string>(_dbForStupidFront.ListCollections().ToList().Select(x => x.GetElement("name").Value.ToString()));
                        _dbForStupidFront.CreateCollection(Mongo.PharmsFD);
                        var indexes = new List<CreateIndexModel<Models.MongoDS.PharmsFD>>() {
                            new CreateIndexModel<Models.MongoDS.PharmsFD>(Builders<Models.MongoDS.PharmsFD>.IndexKeys.Ascending(proto=>proto.IsActive), new CreateIndexOptions() { Unique = false }),
                         //   new CreateIndexModel<Models.MongoDS.PharmsFD>(Builders<Models.MongoDS.PharmsFD>.IndexKeys.Ascending(proto=>proto.latitude), new CreateIndexOptions() { Unique = false }),
                         //   new CreateIndexModel<Models.MongoDS.PharmsFD>(Builders<Models.MongoDS.PharmsFD>.IndexKeys.Ascending(proto=>proto.longitude), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Models.MongoDS.PharmsFD>(Builders<Models.MongoDS.PharmsFD>.IndexKeys.Ascending(proto=>proto.mon), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Models.MongoDS.PharmsFD>(Builders<Models.MongoDS.PharmsFD>.IndexKeys.Ascending(proto=>proto.tue), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Models.MongoDS.PharmsFD>(Builders<Models.MongoDS.PharmsFD>.IndexKeys.Ascending(proto=>proto.wed), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Models.MongoDS.PharmsFD>(Builders<Models.MongoDS.PharmsFD>.IndexKeys.Ascending(proto=>proto.thu), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Models.MongoDS.PharmsFD>(Builders<Models.MongoDS.PharmsFD>.IndexKeys.Ascending(proto=>proto.fri), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Models.MongoDS.PharmsFD>(Builders<Models.MongoDS.PharmsFD>.IndexKeys.Ascending(proto=>proto.sat), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Models.MongoDS.PharmsFD>(Builders<Models.MongoDS.PharmsFD>.IndexKeys.Ascending(proto=>proto.sun), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Models.MongoDS.PharmsFD>(Builders<Models.MongoDS.PharmsFD>.IndexKeys.Geo2DSphere(proto=>proto.location))
                     //   new CreateIndexModel<Core.Data.AssortmentItemData>("Barcode", new CreateIndexOptions() { Unique = false }),
                     //   new CreateIndexModel<Core.Data.AssortmentItemData>("PharmId", new CreateIndexOptions() { Unique = false }),
                    //    new CreateIndexModel<Core.Data.AssortmentItemData>("PharmPackId", new CreateIndexOptions() { Unique = false }),
                    //    new CreateIndexModel<Core.Data.AssortmentItemData>("TradeName", new CreateIndexOptions() { Unique = false }),
                     //   new CreateIndexModel<Core.Data.AssortmentItemData>(nameof(LSBase.FarmPrep), new CreateIndexOptions() { Unique = false }),
                     //   new CreateIndexModel<Core.Data.AssortmentItemData>(nameof(LSBase.DeyVesch), new CreateIndexOptions() { Unique = false }),
                        };
                        _dbForStupidFront.GetCollection<Models.MongoDS.PharmsFD>(Mongo.PharmsFD).Indexes.CreateMany(indexes);
                //        _dbForStupidFront.GetCollection<Models.MongoDS.PharmsFD>(PharmTaxiAdm.Constants.Consts.PharmsFD).Indexes.CreateOne(new CreateIndexModel<Models.MongoDS.PharmsFD>(Builders<Models.MongoDS.PharmsFD>.IndexKeys.Ascending(proto => proto._id), new CreateIndexOptions() { Unique = false }));

                        //_dbForStupidFront.CreateCollection(PharmTaxiAdm.Constants.Consts.PharmsFD) reateIndex(Indexes.geo2dsphere("contact.location"));
                    }
                    else
                    {
                        if (_dbForStupidFront.GetCollection<Models.MongoDS.PharmsFD>(Mongo.PharmsFD).Indexes.List().ToList().Count < 9)
                        {
                            var indexes = new List<CreateIndexModel<Models.MongoDS.PharmsFD>>() {
                            new CreateIndexModel<Models.MongoDS.PharmsFD>(Builders<Models.MongoDS.PharmsFD>.IndexKeys.Ascending(proto => proto.IsActive), new CreateIndexOptions() { Unique = false }),
                        //    new CreateIndexModel<Models.MongoDS.PharmsFD>(Builders<Models.MongoDS.PharmsFD>.IndexKeys.Ascending(proto => proto.latitude), new CreateIndexOptions() { Unique = false }),
                         //   new CreateIndexModel<Models.MongoDS.PharmsFD>(Builders<Models.MongoDS.PharmsFD>.IndexKeys.Ascending(proto => proto.longitude), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Models.MongoDS.PharmsFD>(Builders<Models.MongoDS.PharmsFD>.IndexKeys.Ascending(proto => proto.mon), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Models.MongoDS.PharmsFD>(Builders<Models.MongoDS.PharmsFD>.IndexKeys.Ascending(proto => proto.tue), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Models.MongoDS.PharmsFD>(Builders<Models.MongoDS.PharmsFD>.IndexKeys.Ascending(proto => proto.wed), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Models.MongoDS.PharmsFD>(Builders<Models.MongoDS.PharmsFD>.IndexKeys.Ascending(proto => proto.thu), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Models.MongoDS.PharmsFD>(Builders<Models.MongoDS.PharmsFD>.IndexKeys.Ascending(proto => proto.fri), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Models.MongoDS.PharmsFD>(Builders<Models.MongoDS.PharmsFD>.IndexKeys.Ascending(proto => proto.sat), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Models.MongoDS.PharmsFD>(Builders<Models.MongoDS.PharmsFD>.IndexKeys.Ascending(proto => proto.sun), new CreateIndexOptions() { Unique = false }),
                            new CreateIndexModel<Models.MongoDS.PharmsFD>(Builders<Models.MongoDS.PharmsFD>.IndexKeys.Geo2DSphere(proto => proto.location))
                        };
                            _dbForStupidFront.GetCollection<Models.MongoDS.PharmsFD>(Mongo.PharmsFD).Indexes.CreateMany(indexes);
                          //  _dbForStupidFront.GetCollection<Models.MongoDS.PharmsFD>(PharmTaxiAdm.Constants.Consts.PharmsFD).Indexes.CreateOne(new CreateIndexModel<Models.MongoDS.PharmsFD>(Builders<Models.MongoDS.PharmsFD>.IndexKeys.Ascending(proto => proto._id), new CreateIndexOptions() { Unique = false }));

                        }
                    }
                }
            }
            catch { }// return _dbForStupidFront; }
            //return _dbForStupidFront;
        }

        /// <summary>
        /// База данных СКОЛ (MongoDb)
        /// </summary>
        public static IMongoDatabase DBForStupidFront
        {
            get
            {
                return _dbForStupidFront;
            }
        }

    }
}
