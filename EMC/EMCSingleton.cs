using System;
using System.Threading.Tasks;
using EMC.ECPIntegration;
using Core = ECPLib.Common.CoreApi;

namespace EMC
{
    public static partial class Singleton
    {
        private static IECPRepository? Emc { get; set; }

        public static Task Initialize()
        {
            var useMetrics = PTConstants.EMC.UseMetrics;

            Emc = IECPRepository.CreateInstance(PTConstants.EMC.TarantoolConnectionString, PTConstants.EMC.MongoDbConnectionString, Core.UmkbApi, useMetrics);
            
            return Task.CompletedTask;
        }
    }
}
