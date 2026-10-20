using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAdm.Models.User
{
    public class UserModel
    {
        public ulong LongId { get; set; }
        public override string ToString()
        {
            return $"LongId:{LongId}";
        }
    }
}
