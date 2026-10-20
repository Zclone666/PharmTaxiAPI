using PTConstants;

namespace PharmTaxiAdm.Models.User
{
    public class PassRecovery
    {
        public string key { get; set; } = UMKB.Authorization;
        public string service { get; set; }
        public string email { get; set; }
    }
}
