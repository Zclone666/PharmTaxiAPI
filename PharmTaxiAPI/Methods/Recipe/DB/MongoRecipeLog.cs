using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PTConstants;

namespace PharmTaxiAPI.Methods.Recipe
{
    public static class MongoRecipeLog
    { 
        public static void Initialization() { _ = DB; }

        private static MongoClient _client;
        private static MongoClient Get()
        {
            if (_client != null) { return _client; }
            _client = new MongoClient(Mongo.ReciepesConString);
            return _client;
        }
        private static IMongoDatabase _db = null;

        private static HashSet<string> collectionNames = new HashSet<string>();

        public static ReaderWriterLockSlim locker = new ReaderWriterLockSlim();

        public static IMongoDatabase DB
        {
            get
            {
                try
                {
                    if (_db == null)
                    {
                        _db = Get().GetDatabase("RecipeLogs");
                    }

                    return _db;
                }
                catch { }
                return null;
            }
        }

        public static IMongoCollection<Model.RecipeLog> GetCollection(string name)
        {
            if (name.Length != 2) { throw new Exception("Bad collection name"); }

            if (!CheckCollection(name))
            {
                if (!collectionNames.Contains(name))
                {
                    _db.CreateCollection(name);

                    var indexes = new List<CreateIndexModel<Model.RecipeLog>>()
                            {
                                new CreateIndexModel<Model.RecipeLog>(Builders<Model.RecipeLog>.IndexKeys.Hashed(proto=>proto.IdClinic), new CreateIndexOptions() { Unique = false }),
                                new CreateIndexModel<Model.RecipeLog>(Builders<Model.RecipeLog>.IndexKeys.Hashed(proto=>proto.IdDep), new CreateIndexOptions() { Unique = false }),
                                new CreateIndexModel<Model.RecipeLog>(Builders<Model.RecipeLog>.IndexKeys.Hashed(proto=>proto.UserId), new CreateIndexOptions() { Unique = false }),
                                new CreateIndexModel<Model.RecipeLog>(Builders<Model.RecipeLog>.IndexKeys.Ascending(proto=>proto.Date), new CreateIndexOptions() { Unique = false }),
                                new CreateIndexModel<Model.RecipeLog>(Builders<Model.RecipeLog>.IndexKeys.Hashed(proto=>proto.PHId), new CreateIndexOptions() { Unique = false }),
                                new CreateIndexModel<Model.RecipeLog>(Builders<Model.RecipeLog>.IndexKeys.Hashed(proto=>proto.RecipeId), new CreateIndexOptions() { Unique = false }),
                            };

                    _db.GetCollection<Model.RecipeLog>(name).Indexes.CreateMany(indexes);
                }
            }

            return DB.GetCollection<Model.RecipeLog>(name);
        }



        public static bool CheckCollection(string collectionName)
        {
            collectionNames = new HashSet<string>(DB.ListCollectionNames().ToList());

            if (!string.IsNullOrWhiteSpace(collectionName) && collectionNames.Contains(collectionName)) { return true; }

            return false;
        }

        public static Exception Insert(Model.RecipeLog logModel)
        {
            try
            {
                if (logModel.RecipeId.Length < 3)// || string.IsNullOrWhiteSpace(logModel.UserId) || string.IsNullOrWhiteSpace(logModel.PHId) || string.IsNullOrWhiteSpace(logModel.IdClinic))
                { return new Exception("Bad Ids"); }

                string collName = logModel.RecipeId[0..2];
                logModel.Id = ObjectId.GenerateNewId().ToString();

                GetCollection(collName).InsertOne(logModel);

                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public static List<Model.RecipeLog> Select(Model.RecipeLogFilter modelFilter)
        {
            var filterarray = new BsonArray();

            if (!string.IsNullOrEmpty(modelFilter.RecipeId)) { filterarray.Add(new BsonDocument("RecipeId", modelFilter.RecipeId)); }
            if (!string.IsNullOrEmpty(modelFilter.IdClinic)) { filterarray.Add(new BsonDocument("IdClinic", modelFilter.IdClinic)); }
            if (!string.IsNullOrEmpty(modelFilter.IdDep)) { filterarray.Add(new BsonDocument("IdDep", modelFilter.IdDep)); }
            if (!string.IsNullOrEmpty(modelFilter.PHId)) { filterarray.Add(new BsonDocument("PHId", modelFilter.PHId)); }
            if (!string.IsNullOrEmpty(modelFilter.UserId)) { filterarray.Add(new BsonDocument("UserId", modelFilter.UserId)); }


            BsonDocument filter = new BsonDocument("$and", filterarray);

            List<Model.RecipeLog> logs = new List<Model.RecipeLog>();
            CheckCollection("");

            foreach (var collectionName in collectionNames)
            {
                logs.AddRange(GetCollection(collectionName).FindSync(filter).ToList());
            }

            return logs;
        }
    }
}
