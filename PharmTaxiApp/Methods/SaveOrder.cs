using System;
using PTConstants;
using System.Text;

namespace PharmTaxiApp.Methods
{
    /// <summary>
    /// Класс для сохранения заказа
    /// </summary>
    public static class SaveOrder
    {
        //public static UMKBUtils.BigIdCryptography Crypto = new UMKBUtils.BigIdCryptography(PharmTaxiApp.Models.Crypto.Password);
        /// <summary>
        /// Сохранение Листа Назначения
        /// </summary>
        /// <param name="order"></param>
        public static void Save(ref SMReceipt.Global.Models.SavePrescript order)
        {
            SMReceipt.Global.Methods.Mongo.InsertPrescript(ref order);
        }

        /// <summary>
        /// Сохранение Заказа
        /// </summary>
        /// <param name="order"></param>
        public static void OrderSave(ref SMReceipt.Global.Models.SaveOrder order)
        {
            string vvv = Newtonsoft.Json.JsonConvert.SerializeObject(order);
            SMReceipt.Global.Methods.Mongo.InsertOrder(ref order);
            foreach (var i in order.order) foreach (var ii in i.orderToSave.drugs) if (String.IsNullOrEmpty(ii.packs_string)) ii.packs_string = "1";
            
            AUpload.Config.ConfigMongoAddress(true);
            AUpload.Config.MongoBase = Mongo.PHTFORSTFR;
            AUpload.Global.Methods.GetAssortiment.Reserve(order);
            AUpload.Config.MongoBase = Mongo.PHT;
            AUpload.Global.Methods.GetAssortiment.Reserve(order);
        }
    }
}
