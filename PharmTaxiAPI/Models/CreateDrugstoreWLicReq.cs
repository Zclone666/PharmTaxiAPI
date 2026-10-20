using PharmTaxiAdm.Models.Drugstore;
using PharmTaxiAdm.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Models
{
    public class CreateDrugstoreWLicReq
    {
        public string authkey { get; set; }
        public string key { get; set; }
        public uint? parent_id { get { if (data is null) { data = new List<SingleDrugWLic>() { new SingleDrugWLic() }; return data[0].parent_id; } else { return data[0].parent_id; } } set { if (data is null) { data = new List<SingleDrugWLic>() { new SingleDrugWLic() }; data[0].parent_id = value; } else { data[0].parent_id = value; } } }
        public string UserId { get { if (data is null) { data = new List<SingleDrugWLic>() { new SingleDrugWLic() }; return data[0].UserId; } else { return data[0].UserId; } } set { if (data is null) { data = new List<SingleDrugWLic>() { new SingleDrugWLic() }; data[0].UserId = value; } else { data[0].UserId = value; } } }
        public List<SingleDrugWLic> data { get; set; }
    }

    public class SingleDrugWLic : PharmTaxiAdm.Models.DrugLic.SimpleLicInfo, PharmTaxiAdm.Models.DrugLic.ISimpleLicInfo // PharmTaxiAdm.Models.DrugLic.RozZdravInfo, PharmTaxiAdm.Models.DrugLic.IRozZdravInfo
    {
        public List<ExtenendedDrugstore> pharms { get; set; }
    }

    public class ExtenendedDrugstore : PharmTaxiAdm.Models.DrugLic.RozZdravInfo, PharmTaxiAdm.Models.DrugLic.IRozZdravInfo, PharmTaxiAdm.Models.Drugstore.IDrugstoreModel
    {
        public string Address { get { return address_fact; } set { } }
        public int? AddressVCId {get;set;}
        public bool IsActive {get;set;}
        public string License { get; set; }
        public int? LicenseVCId {get;set;}
        public ulong? LongId {get;set;}
        public ulong? NameLongId {get;set;}
        public ulong? ParentId {get;set;}
        public uint PharmId {get;set;}
        public string Phone {get;set;}
        public int? PhoneVCId {get;set;}
        public string Title { get { return  name; } set {  } }
        public string TypeText {get;set;}
        public string UserId {get;set;}
        public List<UserModel> Users {get;set;}
        public double latitude {get;set;}
        public double longitude {get;set;}
        public Day mon {get;set;}
        public Day tue {get;set;}
        public Day wed {get;set;}
        public Day thu {get;set;}
        public Day fri {get;set;}
        public Day sat {get;set;}
        public Day sun {get;set;}
        public bool IsHidden { get; set; }
    }

}
