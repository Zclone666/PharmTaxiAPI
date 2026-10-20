using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MongoDB.Models
{
    public class GetPricesReq
    {
        public string PharmId { get; set; }
        public string price_id { get; set; }
        public string TradeName { get; set; }


    }
}
