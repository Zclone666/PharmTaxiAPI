using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmTaxiAdm.Models.MongoDS
{
    public class Pharms : Models.Drugstore.DrugstoreModel
    {
        //[BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string PharmLogin { get; set; }

    }
}
