using ECPLib.Common.Recipes.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using PharmTaxiAPI.Methods.Recipe.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PTConstants;

namespace PharmTaxiAPI.Methods.Recipe
{
    public static class MongoRecipe
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

        static HashSet<string> recipeCollections = new HashSet<string>();

        public static ReaderWriterLockSlim locker = new ReaderWriterLockSlim();

        private static IMongoDatabase DB
        {
            get
            {
                if (_db == null)
                {
                    _db = Get().GetDatabase(Mongo.ReciepesDBName);
                    locker.EnterWriteLock();
                    recipeCollections.UnionWith(_db.ListCollectionNames().ToList());
                    locker.ExitWriteLock();
                }
                return _db;
            }
        }

        private static IMongoCollection<ModelRecipeBase> GetCollection(string name)
        {
            if (name.Length != 2) { throw new Exception("Bad collection name"); }

            if (!recipeCollections.Contains(name))
            {
                if (CheckCollection(DB, name))
                {
                    recipeCollections.Add(name);
                }
            }

            return DB.GetCollection<ModelRecipeBase>(name);
        }

        private static bool CheckCollection(IMongoDatabase database, string collectionName)
        {
            var collectionCursor = database.ListCollections(new ListCollectionsOptions { Filter = new BsonDocument("name", collectionName) });
            return collectionCursor.Any();
        }

        public static ModelRecipeBase GetRecipe(string recipeNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(recipeNumber) || recipeNumber.Length < 3) { return null; }

                recipeNumber = recipeNumber.ToUpper();
                string nameCol = recipeNumber.Substring(0, 2);

                var result = GetCollection(nameCol).Find(Builders<ModelRecipeBase>.Filter.Eq("_id", recipeNumber));
                if (result != null) { return result.ToList().First(); }
                return null;
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "Sequence contains no elements") { return null; }

                throw ex;
            }
            catch (FormatException ex)
            {
                throw new Exception($"Ошибка получения рецепта: структура рецепта не соответствует необходимой({ex.Message})");
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка получения рецепта: " + ex.Message);
            }
        }

        public static Exception UpdateRecipe(ModelRecipeBase recipeNew)
        {
            string nameCol = recipeNew.id.Substring(0, 2);

            var result = GetCollection(nameCol).ReplaceOne(Builders<ModelRecipeBase>.Filter.Eq("_id", recipeNew.id), recipeNew);
            if (result.ModifiedCount < 1) { return new Exception("Not update document " + recipeNew.id); }
            return null;
        }
    }
}
