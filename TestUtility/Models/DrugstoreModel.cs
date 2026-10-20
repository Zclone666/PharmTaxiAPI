using System.Collections.Generic;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Models
{
    public class DrugstoreModel
    {
        public string authkey { get; set; }
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
        /// Координаты аптеки
        /// </summary>
        /// 
        public double latitude { get; set; }
        public double longitude { get; set; }

        public GeoJson2DGeographicCoordinates location { get; set; } = null;//{ get { return new GeoJson2DGeographicCoordinates(longitude, latitude); } set { location = value; } }
        //public GeoJson2DGeographicCoordinates location { get; set; } 

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
        public List<UserModel> Users { get; set; } = new List<UserModel>();
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

        /// <summary>
        /// Активирована ли аптека в системе ФармТакси
        /// </summary>
        public bool IsActive { get; set; } = false;

        public Day mon { get; set; }
        public Day tue { get; set; }
        public Day wed { get; set; }
        public Day thu { get; set; }
        public Day fri { get; set; }
        public Day sat { get; set; }
        public Day sun { get; set; }
    }

    public class UserModel
    {
        public ulong LongId { get; set; }
        public override string ToString()
        {
            return $"LongId:{LongId}";
        }
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
