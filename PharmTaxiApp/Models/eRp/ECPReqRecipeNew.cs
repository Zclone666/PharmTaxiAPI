using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiApp.Models.eRp
{
    public class ECPReqRecipeNew
    {
        public UMKBRequests.Alert alert { get; set; }
        public string key { get; set; }
        public string authkey { get; set; }
        public string device_id { get; set; }
        public string service { get; set; }
        public string misid { get; set; }
        public string misprotocol { get; set; }
        public string doctor_id { get; set; }
        public string doctor_code { get; set; }
        public string doctor_name { get; set; }
        public string clinic_id { get; set; }
        public string clinic_code { get; set; }
        public string clinic_name { get; set; }
        public string dep_id { get; set; }
        public string dep_code { get; set; }
        public string dep_name { get; set; }
        public PatientX PatientX { get; set; }
        public string active_language { get; set; }
        public List<Drug> drugs { get; set; }
        public List<Liquid> liquids { get; set; }
        public List<DeletedDrug> deletedDrugs { get; set; }
        public List<DeletedLiquid> deletedLiquids { get; set; }
        public bool return_subjects { get; set; }
        public string misurl { get; set; }
        public string protocol { get; set; }
        public int recipe_endtime { get; set; }
        public RecipeEndtimeitem recipe_endtimeitem { get; set; }
        public string recipe_formid { get; set; }
        public string recipe_text { get; set; }
        public RecipeData recipe_data { get; set; }
        public СlinicalGuidelines сlinical_guidelines { get; set; }
        public Checker checker { get; set; }
        public bool is_collector_available { get; set; }
    }

    public class MedcardNum
    {
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }

    public class Categories
    {
        public List<string> additionalProp1 { get; set; }
        public List<string> additionalProp2 { get; set; }
        public List<string> additionalProp3 { get; set; }
    }

    public class Addresses
    {
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }

    public class Disease
    {
        public string key { get; set; }
        public string id { get; set; }
        public string fpid { get; set; }
        public string type { get; set; }
        public int level { get; set; }
        public string name { get; set; }
        public string namesearch { get; set; }
        public int group { get; set; }
        public bool main { get; set; }
    }

    public class CoDisease
    {
        public string key { get; set; }
        public string id { get; set; }
        public string fpid { get; set; }
        public string type { get; set; }
        public int level { get; set; }
        public string name { get; set; }
        public string namesearch { get; set; }
        public int group { get; set; }
        public bool main { get; set; }
    }

    public class Allergy
    {
        public string key { get; set; }
        public string id { get; set; }
        public string fpid { get; set; }
        public string type { get; set; }
        public int level { get; set; }
        public string name { get; set; }
        public string namesearch { get; set; }
        public int group { get; set; }
        public bool main { get; set; }
    }

    public class Actualproto
    {
        public List<string> additionalProp1 { get; set; }
        public List<string> additionalProp2 { get; set; }
        public List<string> additionalProp3 { get; set; }
    }

    public class Mutation
    {
        public string id { get; set; }
        public string reference { get; set; }
        public string code { get; set; }
        public string idgene { get; set; }
        public string gene { get; set; }
        public string allele1 { get; set; }
        public string allele2 { get; set; }
        public List<string> alleles { get; set; }
        public bool newentry { get; set; }
    }

    public class PatientX
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string miscode { get; set; }
        public string misid { get; set; }
        public string keyid { get; set; }
        public string fio { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string patronymic { get; set; }
        public string policy_serial { get; set; }
        public string policy_num { get; set; }
        public MedcardNum medcard_num { get; set; }
        public Categories categories { get; set; }
        public string main_address { get; set; }
        public Addresses addresses { get; set; }
        public string snils { get; set; }
        public string phone { get; set; }
        public string passport { get; set; }
        public string key { get; set; }
        public int birthdate { get; set; }
        public string sex { get; set; }
        public int growth { get; set; }
        public int weight { get; set; }
        public List<Disease> disease { get; set; }
        public List<CoDisease> co_disease { get; set; }
        public bool pregnancy { get; set; }
        public int gestation { get; set; }
        public int trimester { get; set; }
        public bool lactation { get; set; }
        public bool driving { get; set; }
        public bool alcohol { get; set; }
        public List<Allergy> allergy { get; set; }
        public List<string> labparams { get; set; }
        public Actualproto actualproto { get; set; }
        public List<Mutation> mutations { get; set; }
        public bool genpassport_exist { get; set; }
    }

    public class Dosage
    {
        public string type { get; set; }
        public int value_1 { get; set; }
        public int value_2 { get; set; }
        public string id_1 { get; set; }
        public string id_2 { get; set; }
        public string id { get; set; }
        public string form { get; set; }
        public string method { get; set; }
        public List<Dosage> dosage { get; set; }
        public Pack pack { get; set; }
        public List<int> value { get; set; }
        public string small { get; set; }
        public string full { get; set; }
    }

    public class Pack
    {
        public string id { get; set; }
        public Dosage dosage { get; set; }
        public string small { get; set; }
        public string full { get; set; }
    }

    public class Calc
    {
        public string dosageBaseUnit { get; set; }
        public List<int> dosage { get; set; }
        public string countBaseUnit { get; set; }
        public string countUnit { get; set; }
        public int count { get; set; }
        public string dosageUnit { get; set; }
        public int dayCount { get; set; }
        public List<int> fullDosage { get; set; }
        public List<int> weightDosage { get; set; }
        public List<int> dayDosage { get; set; }
        public List<int> dayWeightDosage { get; set; }
        public int durationCount { get; set; }
        public string durationUnit { get; set; }
        public List<int> fullSpeed { get; set; }
        public int liquidSpeed { get; set; }
        public int tintSpeed { get; set; }
        public List<int> weightSpeed { get; set; }
    }

    public class Interval
    {
        public string type { get; set; }
        public int x { get; set; }
        public string timesx { get; set; }
    }

    public class Schedule
    {
        public int dateStart { get; set; }
        public int dateFinish { get; set; }
        public string finishAlternate { get; set; }
        public List<Interval> interval { get; set; }
        public bool once { get; set; }
    }

    public class Helpers
    {
        public string key { get; set; }
        public string way { get; set; }
        public string step { get; set; }
        public Dosage dosage { get; set; }
        public Pack pack { get; set; }
    }

    public class Drug
    {
        public string key { get; set; }
        public string id { get; set; }
        public string fpid { get; set; }
        public string mnnid { get; set; }
        public string mnnname { get; set; }
        public string type { get; set; }
        public int level { get; set; }
        public string name { get; set; }
        public string namesearch { get; set; }
        public int group { get; set; }
        public string recipenameid { get; set; }
        public string text { get; set; }
        public Dosage dosage { get; set; }
        public Calc calc { get; set; }
        public Schedule schedule { get; set; }
        public string duration { get; set; }
        public string duration_id { get; set; }
        public string freq { get; set; }
        public string freq_id { get; set; }
        public bool solute { get; set; }
        public string specials { get; set; }
        public bool liquid { get; set; }
        public string proto { get; set; }
        public string proto_date { get; set; }
        public string proto_key { get; set; }
        public string pdate { get; set; }
        public string pproto { get; set; }
        public string pkey { get; set; }
        public bool repeal { get; set; }
        public string repealdoc { get; set; }
        public string repealdate { get; set; }
        public string repealproto { get; set; }
        public bool edit { get; set; }
        public List<string> recipes { get; set; }
        public Helpers helpers { get; set; }
        public string doctor_code { get; set; }
        public string clinic_code { get; set; }
        public string dep_code { get; set; }
        public int date { get; set; }
        public bool phantom { get; set; }
        public int fg_id { get; set; }
        public string fg_name { get; set; }
        public int atx_id { get; set; }
        public string atx_name { get; set; }
        public int fa_id { get; set; }
        public string fa_name { get; set; }
        public int umkb10_id { get; set; }
        public string umkb10_name { get; set; }
    }

    public class Liquid
    {
        public string key { get; set; }
        public string id { get; set; }
        public string fpid { get; set; }
        public string mnnid { get; set; }
        public string mnnname { get; set; }
        public string type { get; set; }
        public int level { get; set; }
        public string name { get; set; }
        public string namesearch { get; set; }
        public int group { get; set; }
        public string recipenameid { get; set; }
        public string text { get; set; }
        public Dosage dosage { get; set; }
        public Calc calc { get; set; }
        public Schedule schedule { get; set; }
        public string duration { get; set; }
        public string duration_id { get; set; }
        public string freq { get; set; }
        public string freq_id { get; set; }
        public bool solute { get; set; }
        public string specials { get; set; }
        public bool liquid { get; set; }
        public string proto { get; set; }
        public string proto_date { get; set; }
        public string proto_key { get; set; }
        public string pdate { get; set; }
        public string pproto { get; set; }
        public string pkey { get; set; }
        public bool repeal { get; set; }
        public string repealdoc { get; set; }
        public string repealdate { get; set; }
        public string repealproto { get; set; }
        public bool edit { get; set; }
        public List<string> recipes { get; set; }
        public Helpers helpers { get; set; }
        public string doctor_code { get; set; }
        public string clinic_code { get; set; }
        public string dep_code { get; set; }
        public int date { get; set; }
        public bool phantom { get; set; }
        public int fg_id { get; set; }
        public string fg_name { get; set; }
        public int atx_id { get; set; }
        public string atx_name { get; set; }
        public int fa_id { get; set; }
        public string fa_name { get; set; }
        public int umkb10_id { get; set; }
        public string umkb10_name { get; set; }
    }

    public class DeletedDrug
    {
        public string key { get; set; }
        public string id { get; set; }
        public string fpid { get; set; }
        public string mnnid { get; set; }
        public string mnnname { get; set; }
        public string type { get; set; }
        public int level { get; set; }
        public string name { get; set; }
        public string namesearch { get; set; }
        public int group { get; set; }
        public string recipenameid { get; set; }
        public string text { get; set; }
        public Dosage dosage { get; set; }
        public Calc calc { get; set; }
        public Schedule schedule { get; set; }
        public string duration { get; set; }
        public string duration_id { get; set; }
        public string freq { get; set; }
        public string freq_id { get; set; }
        public bool solute { get; set; }
        public string specials { get; set; }
        public bool liquid { get; set; }
        public string proto { get; set; }
        public string proto_date { get; set; }
        public string proto_key { get; set; }
        public string pdate { get; set; }
        public string pproto { get; set; }
        public string pkey { get; set; }
        public bool repeal { get; set; }
        public string repealdoc { get; set; }
        public string repealdate { get; set; }
        public string repealproto { get; set; }
        public bool edit { get; set; }
        public List<string> recipes { get; set; }
        public Helpers helpers { get; set; }
        public string doctor_code { get; set; }
        public string clinic_code { get; set; }
        public string dep_code { get; set; }
        public int date { get; set; }
        public bool phantom { get; set; }
        public int fg_id { get; set; }
        public string fg_name { get; set; }
        public int atx_id { get; set; }
        public string atx_name { get; set; }
        public int fa_id { get; set; }
        public string fa_name { get; set; }
        public int umkb10_id { get; set; }
        public string umkb10_name { get; set; }
    }

    public class DeletedLiquid
    {
        public string key { get; set; }
        public string id { get; set; }
        public string fpid { get; set; }
        public string mnnid { get; set; }
        public string mnnname { get; set; }
        public string type { get; set; }
        public int level { get; set; }
        public string name { get; set; }
        public string namesearch { get; set; }
        public int group { get; set; }
        public string recipenameid { get; set; }
        public string text { get; set; }
        public Dosage dosage { get; set; }
        public Calc calc { get; set; }
        public Schedule schedule { get; set; }
        public string duration { get; set; }
        public string duration_id { get; set; }
        public string freq { get; set; }
        public string freq_id { get; set; }
        public bool solute { get; set; }
        public string specials { get; set; }
        public bool liquid { get; set; }
        public string proto { get; set; }
        public string proto_date { get; set; }
        public string proto_key { get; set; }
        public string pdate { get; set; }
        public string pproto { get; set; }
        public string pkey { get; set; }
        public bool repeal { get; set; }
        public string repealdoc { get; set; }
        public string repealdate { get; set; }
        public string repealproto { get; set; }
        public bool edit { get; set; }
        public List<string> recipes { get; set; }
        public Helpers helpers { get; set; }
        public string doctor_code { get; set; }
        public string clinic_code { get; set; }
        public string dep_code { get; set; }
        public int date { get; set; }
        public bool phantom { get; set; }
        public int fg_id { get; set; }
        public string fg_name { get; set; }
        public int atx_id { get; set; }
        public string atx_name { get; set; }
        public int fa_id { get; set; }
        public string fa_name { get; set; }
        public int umkb10_id { get; set; }
        public string umkb10_name { get; set; }
    }

    public class RecipeEndtimeitem
    {
        public string id { get; set; }
        public string name { get; set; }
        public int value { get; set; }
        public int unit { get; set; }
    }

    public class RecipeData
    {
        public string formid { get; set; }
        public int dateStart { get; set; }
        public string recipe_text { get; set; }
        public int recipe_endtime { get; set; }
        public RecipeEndtimeitem recipe_endtimeitem { get; set; }
        public string pdid { get; set; }
        public string pdcode { get; set; }
        public bool pchronic { get; set; }
        public string pcatlevel { get; set; }
        public string pcatcode { get; set; }
        public string pdiscount { get; set; }
        public string idStatus { get; set; }
        public bool cito { get; set; }
        public bool statim { get; set; }
        public bool special { get; set; }
        public string spectext { get; set; }
        public string docsign { get; set; }
        public string doccertify { get; set; }
        public string commsign { get; set; }
        public string commcertify { get; set; }
        public string cert_owner { get; set; }
        public string cert_code { get; set; }
        public string validity_start_date { get; set; }
        public string validity_end_date { get; set; }
        public string org_name { get; set; }
        public string address { get; set; }
    }

    public class СlinicalGuidelines
    {
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }

    public class AdditionalProp1
    {
        public int sort { get; set; }
        public int value { get; set; }
        public int unitid { get; set; }
        public string type { get; set; }
        public int page { get; set; }
        public string parent { get; set; }
    }

    public class AdditionalProp2
    {
        public int sort { get; set; }
        public int value { get; set; }
        public int unitid { get; set; }
        public string type { get; set; }
        public int page { get; set; }
        public string parent { get; set; }
    }

    public class AdditionalProp3
    {
        public int sort { get; set; }
        public int value { get; set; }
        public int unitid { get; set; }
        public string type { get; set; }
        public int page { get; set; }
        public string parent { get; set; }
    }

    public class Checker
    {
        public AdditionalProp1 additionalProp1 { get; set; }
        public AdditionalProp2 additionalProp2 { get; set; }
        public AdditionalProp3 additionalProp3 { get; set; }
    }

}
