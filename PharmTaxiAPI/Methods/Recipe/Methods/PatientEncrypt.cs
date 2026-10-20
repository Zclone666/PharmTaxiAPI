using ECPLib.Common.General.Models;
using ECPLib.Common.Patient.Models;
using PTConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Methods.Recipe
{
    public static partial class Metods
    {
        public static Patient PatientEncrypt(Patient patient)
        {
            if (!string.IsNullOrWhiteSpace(patient.id)) patient.id = Crypto.Сryptographer.Encrypt(patient.id);

            List<Concept> concepts = new List<Concept>();
            if (patient.disease != null) { concepts.AddRange(patient.disease); }
            if (patient.co_disease != null) { concepts.AddRange(patient.co_disease); }
            if (patient.allergy != null) { concepts.AddRange(patient.allergy); }

            foreach (var concept in concepts)
            {
                if (!string.IsNullOrWhiteSpace(concept.id)) concept.id = Crypto.Сryptographer.Encrypt(concept.id);
                if (!string.IsNullOrWhiteSpace(concept.fpid)) concept.fpid = Crypto.Сryptographer.Encrypt(concept.fpid);
                if (!string.IsNullOrWhiteSpace(concept.code)) concept.code = Crypto.Сryptographer.Encrypt(concept.code);
            }

            return patient;
        }
    }
}
