using PharmTaxiAdm.Models.User;
using System.Collections.Generic;

namespace PharmTaxiAdm.Models.Drugstore
{
    public interface IBaseDrugNet
    {
        string Address { get; set; }
        int? AddressVCId { get; set; }
        bool IsActive { get; set; }
        bool IsHidden { get; set; }
        string License { get; set; }
        int? LicenseVCId { get; set; }
        ulong? LongId { get; set; }
        ulong? NameLongId { get; set; }
        ulong? ParentId { get; set; }
        uint PharmId { get; set; }
        string Phone { get; set; }
        int? PhoneVCId { get; set; }
        string Title { get; set; }
        string TypeText { get; set; }
        string UserId { get; set; }
        List<UserModel> Users { get; set; }
    }
}