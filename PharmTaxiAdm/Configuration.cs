using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiAdm
{
    public class Configuration
    {
        public enum ServerType
        {
            Dev,
            Prod,
        }
        public readonly static ServerType serverType;
        static Configuration()
        {
            serverType = ServerType.Prod;
        }
    }
}
