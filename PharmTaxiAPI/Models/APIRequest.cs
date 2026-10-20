using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PharmTaxiAPI.Models
{
    public class ForceLoadAPIRequestAss : APIRequestAss
    {
        public bool ForceLoad { get; set; } = false;
    }
    public class APIRequestAss
    {
        /// <summary>
        /// Лист объектов типа AssortimentItemData (объект типа ассортимента - с информацией о количестве и ФП нижнего уровня)
        /// </summary>
        public List<AUpload.Global.Models.AssortmentItemData> Lass { get; set; }
        /// <summary>
        /// ключ
        /// </summary>
        public string authkey { get; set; }
        /// <summary>
        /// лицц ключ
        /// </summary>
        public string key { get; set; }
    }

    public class APIChangeAss
    {
        /// <summary>
        /// Лист объектов типа AssortimentItemData (объект типа ассортимента - с информацией о количестве и ФП нижнего уровня)
        /// </summary>
        public List<Models.ChangeAss> lass { get; set; }
        /// <summary>
        /// ключ
        /// </summary>
        public string authkey { get; set; }
        /// <summary>
        /// лицц ключ
        /// </summary>
        public string key { get; set; }
    }

    public class APIRequestPrice
    {
        /// <summary>
        /// Лист объектов типа PriceItemData (объект типа цен - с информацией о цене и ФП нижнего уровня)
        /// </summary>
        public List<AUpload.Global.Models.AssortmentItemData> Pass { get; set; }
        /// <summary>
        /// ключ
        /// </summary>
        public string authkey { get; set; }
        /// <summary>
        /// лицц ключ
        /// </summary>
        public string key { get; set; }
    }
}