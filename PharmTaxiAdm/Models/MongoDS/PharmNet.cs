using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiAdm.Models.MongoDS
{
    public class PharmNets : Models.Drugstore.DrugNetModel
    {
        //[BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string PharmLogin { get; set; }

    }
}
