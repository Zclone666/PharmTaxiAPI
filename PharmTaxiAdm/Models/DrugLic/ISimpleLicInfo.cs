namespace PharmTaxiAdm.Models.DrugLic
{
    public interface ISimpleLicInfo
    {
        string address { get; set; }
        string dateEnd { get; set; }
        string dateStart { get; set; }
        string inn { get; set; }
        string issuing { get; set; }
        string license { get; set; }
        string name { get; set; }
        string ogrn { get; set; }
        string order { get; set; }
        string orderFresh { get; set; }
    }
}