using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class Pharms
    {
        public string _id { get; set; }
        public uint PharmId { get; set; }
        public string PharmLogin { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public List<uint> PackId { get; set; }
        public List<uint> PharmPackId { get; set; }

    }
}
