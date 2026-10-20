using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Models
{
    public class RecipReleaseReq
    {
        public string authkey { get; set; }
        public string key { get; set; }
        public string localUid { get; set; }
        public string prescriptionUid { get; set; }
        public DateTime date { get; set; }
        public Pharmacy pharmacy { get; set; }
        public MedicationDispense medicationDispense { get; set; }
        public bool isPartialDispense { get; set; }
        public Cancel cancel { get; set; }
        public string software { get; set; }
        public List<Semd> semds { get; set; }
    }

    public class Pharmacist
    {
        public string name { get; set; }
        public string surname { get; set; }
        public string patronymic { get; set; }
        public string snils { get; set; }
        public int frmrMedicalStaffPostCode { get; set; }
    }

    public class Pharmacy
    {
        public string ogrn { get; set; }
        public string oid { get; set; }
        public string name { get; set; }
        public string inn { get; set; }
        public Pharmacist pharmacist { get; set; }
    }

    public class MedicationDispense
    {
        public string name { get; set; }
        public string code { get; set; }
        public string handbook { get; set; }
        public int count { get; set; }
        public int price { get; set; }
        public string signa { get; set; }
    }

    public class Cancel
    {
        public string reason { get; set; }
        public int reasonFederalCode { get; set; }
    }

    public class RemdError
    {
        public string key { get; set; }
        public string message { get; set; }
        public string code { get; set; }
    }

    public class RegistrationData
    {
        public string federalRegNumber { get; set; }
        public string remdState { get; set; }
        public DateTime remdStateDate { get; set; }
        public List<RemdError> remdErrors { get; set; }
    }

    public class PersonalSignature
    {
        public string signerRole { get; set; }
        public string signature { get; set; }
    }

    public class Semd
    {
        public string localUid { get; set; }
        public int version { get; set; }
        public string number { get; set; }
        public string departmentCode { get; set; }
        public string departmentName { get; set; }
        public DateTime createDateTime { get; set; }
        public string data { get; set; }
        public string orgSignature { get; set; }
        public RegistrationData registrationData { get; set; }
        public List<PersonalSignature> personalSignatures { get; set; }
    }

}
