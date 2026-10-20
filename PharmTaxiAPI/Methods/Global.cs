using Core.Data;
using PTConstants;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Methods
{
    public class Global
    {

		/// <summary>
		/// Простая проверка Ключа авторизации
		/// </summary>
		/// <param name="authkey"></param>
		/// <returns></returns>
		public static UMKBRequests.Alert CheckAuth(string authkey)
		{
			if (authkey is null || !authkey.StartsWith("_") || authkey is "")
			{
				return new UMKBRequests.Alert
				{
					code = "400",
					level = "error",
					message = "Ошибка Авторизации"
				};
			}
			else
			{
				return new UMKBRequests.Alert
				{
					code = "200"
				};
			}
		}

		public static string GetLicKeyByPharmID(int PharmId)
        {
			string LicKey = "";

			string connectionString = Mongo.ReciepesConString + "/admin?authSource=admin&readPreference=primary&appname=MongoDB%20Compass%20Beta&ssl=false";
			var client = new MongoClient(connectionString);
			var database = client.GetDatabase("PHTForSLF");
			var collection = database.GetCollection<PharmTaxiAdm.Models.MongoDS.Pharms>("pharms");
			var filter = new BsonDocument("PharmId", PharmId);
			var pharms = collection.Find(filter).ToList();

			LicKey = pharms[0]._id;
			
			return LicKey;
        }
	}
}
