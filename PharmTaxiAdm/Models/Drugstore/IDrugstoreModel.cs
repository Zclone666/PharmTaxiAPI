using MongoDB.Driver.GeoJsonObjectModel;
using System;

namespace PharmTaxiAdm.Models.Drugstore
{
    public interface IDrugstoreModel:IBaseDrugNet
    {
        double latitude { get; set; }
        double longitude { get; set; }
        GeoJson2DGeographicCoordinates location { get {
                try
                {
                    GeoJson2DGeographicCoordinates geo = new GeoJson2DGeographicCoordinates(this.longitude, this.latitude);
                    return geo;
                }
                catch (Exception e)
                {
                    return null;
                }
            } set {; } }
        Day mon { get; set; }
        Day tue { get; set; }
        Day wed { get; set; }
        Day thu { get; set; }
        Day fri { get; set; }
        Day sat { get; set; }
        Day sun { get; set; }

        bool Equals(object obj);
        int GetHashCode();
    }
}