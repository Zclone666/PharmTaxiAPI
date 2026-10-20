using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiApp.Models.eRp.Response
{
    public class PrescrLong
    {
        public string requestUid { get; set; }
        public List<Error> errors { get; set; }
        public List<Datum> data { get; set; }
    }

    public class Patient
    {
        public string localId { get; set; }
        public string surname { get; set; }
        public string name { get; set; }
        public string patronymic { get; set; }
        public DateTime birthday { get; set; }
        public string gender { get; set; }
        public string snils { get; set; }
        public string mPhone { get; set; }
    }

    public class Organization
    {
        public string shortName { get; set; }
        public string fullName { get; set; }
        public string ogrn { get; set; }
        public string inn { get; set; }
        public string directorFullName { get; set; }
        public string directorPhone { get; set; }
        public string oid { get; set; }
        public string address { get; set; }
    }

    public class Doctor
    {
        public string surname { get; set; }
        public string name { get; set; }
        public string patronymic { get; set; }
        public string snils { get; set; }
        public string phone { get; set; }
        public string localId { get; set; }
        public DateTime birthDate { get; set; }
        public string positionCode { get; set; }
        public string specialityCode { get; set; }
        public string departmentCode { get; set; }
    }

    public class Chairman
    {
        public string surname { get; set; }
        public string name { get; set; }
        public string patronymic { get; set; }
        public string snils { get; set; }
        public string phone { get; set; }
        public string localId { get; set; }
        public DateTime birthDate { get; set; }
        public string positionCode { get; set; }
        public string specialityCode { get; set; }
        public string departmentCode { get; set; }
    }

    public class Medication:MedicationBase
    {
        public string type { get; set; }
        public string smnnCode { get; set; }
        public bool isTrn { get; set; }
        public string latinPrescriptionName { get; set; }
        public string latinDosageForm { get; set; }
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

    public class SemdData
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

    public class Author
    {
        public string surname { get; set; }
        public string name { get; set; }
        public string patronymic { get; set; }
        public string snils { get; set; }
        public string phone { get; set; }
    }

    public class Cancel
    {
        public DateTime date { get; set; }
        public Author author { get; set; }
        public string reasonString { get; set; }
        public string reasonCode { get; set; }
    }

    public class Datum:DatumBase
    {
        public string uid { get; set; }
        public string prescribedMedicationGuid { get; set; }
        public string series { get; set; }
        public string number { get; set; }
        public DateTime dateCreate { get; set; }
        public DateTime dateEnd { get; set; }
        public bool isChronic { get; set; }
        public string formType { get; set; }
        public bool isPaper { get; set; }
        public Patient patient { get; set; }
        public Organization organization { get; set; }
        public Doctor doctor { get; set; }
        public Chairman chairman { get; set; }
        public Medication medication { get; set; }
        public string okatoCode { get; set; }
        public string dispencePeriod { get; set; }
        public string schemeUid { get; set; }
        public string representativeSnils { get; set; }
        public string representativeMPhone { get; set; }
        public string mkbCode { get; set; }
        public string tag { get; set; }
        public DateTime commissionDate { get; set; }
        public string commissionNumber { get; set; }
        public string softwareName { get; set; }
        public string state { get; set; }
        public SemdData semdData { get; set; }
        public Cancel cancel { get; set; }
    }

}
