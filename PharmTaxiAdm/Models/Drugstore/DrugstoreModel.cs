using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.IO;
using MongoDB.Driver.GeoJsonObjectModel;
using GeoJSON.Net.Geometry;

namespace PharmTaxiAdm.Models.Drugstore
{
    public class BaseDrugNet : Models.DrugLic.CompanyInfo, IBaseDrugNet
    {
        public BaseDrugNet()
        {

        }
        public ulong? LongId { get; set; }
        /// <summary>
        /// idшник аптеки-родителя
        /// </summary>
        public ulong? ParentId { get; set; }
        /// <summary>
        /// Сюда посылаем "Аптека"
        /// </summary>
        public string TypeText { get; set; } // Аптека
        /// <summary>
        /// Юзверь аптеки
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// адресс
        /// </summary>
        public string Address { get; set; }
        public int? AddressVCId { get; set; }

        /// <summary>
        /// Номер лицензии
        /// </summary>
        public string License { get; set; }
        public int? LicenseVCId { get; set; }
        public string Phone { get; set; }
        public int? PhoneVCId { get; set; }
        /// <summary>
        /// название аптеки
        /// </summary>
        public string Title { get; set; }
        public ulong? NameLongId { get; set; }
        /// <summary>
        /// ID аптеки в системе UMKB
        /// </summary>
        public uint PharmId { get; set; }
        public List<User.UserModel> Users { get; set; } = new List<User.UserModel>();


        /// <summary>
        /// Активирована ли аптека в системе ФармТакси
        /// </summary>
        public bool IsActive { get; set; } = false;

        /// <summary>
        /// Поле, чтоб прятать аптеку
        /// </summary>
        public bool IsHidden { get; set; } = false;
    }

    public class DrugNetModel : BaseDrugNet, IBaseDrugNet
    {
        public DrugNetModel() : base()
        {

        }
        [BsonIgnore]
        public string authkey { get; set; }
        [BsonIgnore]
        public string key { get; set; }

    }


    public class DrugstoreModel : DrugNetModel, IBaseDrugNet, IDrugstoreModel
    {
        public DrugstoreModel() : base()
        {

        }
        /// <summary>
        /// Координаты аптеки
        /// </summary>
        /// 
        public double latitude { get; set; }
        public double longitude { get; set; }

        //[BsonIgnore]
        //public GeoJson2DGeographicCoordinates loc { get { return new GeoJson2DGeographicCoordinates(longitude, latitude); } set { location = value; } } //{ get; set; }//{ get { return location is null ? new GeoJson2DGeographicCoordinates(longitude, latitude):location; } set { location = value; } } 

        public GeoJson2DGeographicCoordinates location { get { return new GeoJson2DGeographicCoordinates(longitude, latitude); } set { } }

        public override bool Equals(object obj)
        {
            DrugstoreModel drugstoreModel = (obj as DrugstoreModel);
            if (drugstoreModel == null || drugstoreModel.LongId != LongId) return false;
            return true;
        }
        public override int GetHashCode()
        {
            return LongId.GetHashCode();
        }
        public Day mon { get; set; }
        public Day tue { get; set; }
        public Day wed { get; set; }
        public Day thu { get; set; }
        public Day fri { get; set; }
        public Day sat { get; set; }
        public Day sun { get; set; }
    }

    public class DrugstoreModelList
    {
        public List<DrugstoreModel> pharms { get; set; }
    }

    public class CreatedDrugStore
    {
        public UMKBRequests.Modelling.U_AUD_Lib.UAlert alert { get; set; }
        /// <summary>
        /// Заготовка лицензионного ключа
        /// </summary>
        public string lic_code { get; set; }
    }

    public class Day
    {
        public uint? StartTime { get; set; }
        public uint? EndTime { get; set; }
        public uint? BreakStart { get; set; }
        public uint? BreakEnd { get; set; }
    }

    public class MongoLocation
    {

    }
}
