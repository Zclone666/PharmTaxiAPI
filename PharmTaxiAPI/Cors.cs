using Microsoft.AspNetCore.Cors.Infrastructure;

namespace PharmTaxiAPI
{
    public static class Cors
    {
        public static class AllowAll
        {
            public const string Name = "CorsAllowAll";
            public readonly static CorsPolicy Policy = new CorsPolicyBuilder().AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().Build();
        }
    }
}
