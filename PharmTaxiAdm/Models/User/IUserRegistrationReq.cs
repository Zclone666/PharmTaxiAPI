namespace PharmTaxiAdm.Models.User
{
    public interface IUserRegistrationReq : IUserRegUMKB
    {
        string authkey { get; set; }
        string lic_key { get; set; }
    }
}