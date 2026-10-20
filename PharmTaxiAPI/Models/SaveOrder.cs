using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using SMReceipt.Global.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UMKBRequests;

namespace PharmTaxiAPI.Models
{
    public class SaveOrderD
    {
        public string authkey { get; set; }
        public OrderToSaveD order { get; set; }
    }

    public class SOAns
    {
        public Alert alert { get; set; }
        public string order_id { get; set; }
    }
    public class SaveOrderL
    {
       // [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
      //  [BsonIgnore]
        public Alert Alert { get; set; }
        public string key { get; set; }
        public string authkey { get; set; }
        public string device_id { get; set; }
        public string service { get; set; }
        public string active_language { get; set; }
        public bool delivery { get { return order[0].order_to_save.Delivery; } set { order[0].order_to_save.Delivery = value; } }
        public long delivery_date { get { return order[0].order_to_save.IssueDate; } set { order[0].order_to_save.IssueDate = value; } }
        public List<MedsL> drugs { get { return order[0].order_to_save.drugs; } set { order[0].order_to_save.drugs=value; } }
        [JsonIgnore]
        public List<OrderL> order { get; set; } = new List<OrderL>() { new OrderL() };


    }

    public class OrderL 
    {
        public string PharmID { get { return order_to_save.pharm_id; } set { order_to_save.pharm_id = value; } }
        public string UserID { get { return order_to_save.user_id; } set { order_to_save.user_id = value; } }

        public OrderToSaveL order_to_save { get; set; } = new OrderToSaveL();
    }
    public class OrderToSaveL 
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string pharm_id { get; set; }
        public string user_id { get; set; }
        [JsonIgnore]
        public string order_id { get; set; }
        [JsonIgnore]
        public string recipe { get; set; } //= SMReceipt.Global.Methods.RGenerator.Code(4).Result;
        public List<MedsL> drugs { get; set; } = new List<MedsL>();
        // public List<Meds> liquids { get; set; } = new List<Meds>();
        [JsonIgnore]
        public bool IsActive { get; set; }

        public long CreationDate { get; set; } = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();

        public long IssueDate { get; set; } = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();

        public string DeliveryAddress { get; set; }
        /// <summary>
        /// status заказа:
        /// -1 - отменён
        /// 0 - в обработке
        /// 1 - готов к выдаче
        /// 2 - выдан
        /// </summary>
        /// 
        [JsonIgnore]
        public int status { get; set; }
        public bool Delivery { get; set; }
    }


    public class OrderToSaveD
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public uint pharm_id { get; set; }
        public string user_id { get; set; }
        [JsonIgnore]
        public string order_id { get; set; }
        [JsonIgnore]
        public string recipe { get; set; } //= SMReceipt.Global.Methods.RGenerator.Code(4).Result;
        public List<MedsD> drugs { get; set; } = new List<MedsD>();
        // public List<Meds> liquids { get; set; } = new List<Meds>();
        [JsonIgnore]
        public bool IsActive { get; set; }

        public long CreationDate { get; set; } = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();

        public long IssueDate { get; set; } = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();

        public string DeliveryAddress { get; set; }
        /// <summary>
        /// status заказа:
        /// -1 - отменён
        /// 0 - в обработке
        /// 1 - готов к выдаче
        /// 2 - выдан
        /// </summary>
        /// 
        [JsonIgnore]
        public int status { get; set; }
        public bool? Delivery { get; set; }
    }


    public class MedsL
    {
        /// <summary>
        /// ID ФП
        /// </summary>
        public string id { get; set; }
        public string table { get; set; }
        /// <summary>
        /// Имя ФП
        /// </summary>
        public string name { get; set; }
        public string value { get; set; }
        /// <summary>
        /// количество упаковое
        /// </summary>
        public string packs { get; set; }
        public string send { get; set; }
        public string removed { get; set; }
        public string dosage { get; set; }
        /// <summary>
        /// ID ТН
        /// </summary>
        public string tn { get; set; }
        /// <summary>
        /// ID из аптеки
        /// </summary>
        public string drugstore_drug_id { get; set; }
        public double price { get; set; }
    }

    public class MedsD
    {
        /// <summary>
        /// ID ФП
        /// </summary>
        public string id { get; set; }
        public string table { get; set; }
        /// <summary>
        /// Имя ФП
        /// </summary>
        public string name { get; set; }
        public string value { get; set; }
        /// <summary>
        /// количество упаковое
        /// </summary>
        public uint packs { get; set; }
        public string send { get; set; }
        public string removed { get; set; }
        public string dosage { get; set; }
        /// <summary>
        /// ID ТН
        /// </summary>
        public string tn { get; set; }
        /// <summary>
        /// ID из аптеки
        /// </summary>
        public string drugstore_drug_id { get; set; }
        public double price { get; set; }
    }
}
