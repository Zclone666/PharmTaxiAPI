using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;
using PharmTaxiAdm.Models.Drugstore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiAdm.Models.MongoDS
{
    public class PharmsFD
    {
        public string _id { get; set; }
        public uint PharmId { get; set; }
        [BsonIgnore]
        public double latitude { get; set; }
        [BsonIgnore]
        public double longitude { get; set; }

        public GeoJson2DGeographicCoordinates location { get { return new GeoJson2DGeographicCoordinates(longitude, latitude); } set {  } }

        public bool IsActive { get; set; } = false;
        public Day mon { get; set; }
        public Day tue { get; set; }
        public Day wed { get; set; }
        public Day thu { get; set; }
        public Day fri { get; set; }
        public Day sat { get; set; }
        public Day sun { get; set; }
    }
}
