using MongoDB.Driver.GeoJsonObjectModel;

namespace PharmTaxiAdm.Models.DrugLic
{
    public interface IRozZdravInfo
    {
        string activity { get; set; }
        string address_fact { get; set; }
        string address_fact_street { get; set; }
        string address_fact_zip { get; set; }
        string appartment_num { get; set; }
        string building_num { get; set; }
        string city { get; set; }
        string code_fias { get; set; }
        string district_label { get; set; }
        string dt_created { get; set; }
        string dt_edited { get; set; }
        string ext_city_id { get; set; }
        string ext_db { get; set; }
        string ext_id { get; set; }
        string ext_org { get; set; }
        string ext_region_id { get; set; }
        string ext_type_id { get; set; }
        string fake { get; set; }
        string from_b2b { get; set; }
        string house_num { get; set; }
        string id { get; set; }
        string id_create_user { get; set; }
        string id_department { get; set; }
        string id_licenses_apps_object { get; set; }
        string id_organization { get; set; }
        string id_revision { get; set; }
        string id_type_org { get; set; }
        string id_user_edited { get; set; }
        string id_voc_city { get; set; }
        string id_voc_district { get; set; }
        string id_voc_licenser { get; set; }
        string id_voc_producer { get; set; }
        string id_voc_region { get; set; }
        string id_voc_revision_activity_items { get; set; }
        string id_voc_revision_object_type { get; set; }
        string id_voc_settlements { get; set; }
        string ids_voc_revision_activities { get; set; }
        string is_active_b2b { get; set; }
        string is_actual { get; set; }
        string is_foreign { get; set; }
        string is_med_business_check { get; set; }
        string is_not_code_fias { get; set; }
        string is_save_region { get; set; }
        string kladr { get; set; }
        string label { get; set; }
        string label_firm { get; set; }
        string label_short { get; set; }
       // GeoJson2DGeographicCoordinates location { get; set; }
        string name { get; set; }
        string okato { get; set; }
        string oktmo { get; set; }
        string region { get; set; }
        string street { get; set; }
    }
}