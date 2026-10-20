using MongoDB.Driver.GeoJsonObjectModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiAdm.Models.DrugLic
{
    public class LicInfo
    {
         
        /// <summary>
        /// айди лицензии с системе РЗН:
        /// </summary>
        public uint dT_RowId { get; set; }

        /// <summary>
        ///Регистрационный номер
        ////// </summary>
        //
        public string regNumb { get; set; }

        /// <summary>
        /// Дата регистрации
        /// </summary> 
        /// 
        public double regDate { get; set; }

        /// <summary>
        /// Сведения о лицензиате(это юр лицо, например "Аптечная сеть Оз") 
        /// </summary>
        public string licensee { get; set; }

        /// <summary>
        /// Наименование лицензирующего органа
        /// </summary>

        public string licIssuer { get; set; }

        /// <summary>
        ///  Место нахождения лицензиата
        /// </summary>
        /// 
        public string licenseeAddress { get; set; }

        /// <summary>
        /// Код ОГРН/ОГРИП
        /// </summary>
        public uint ogrn_ogrip { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        public uint inn { get; set; }

        /// <summary>
        ///  Код ОКПО
        /// </summary>
        public uint okpo { get; set; }

        /// <summary>
        /// Номер лицензии
        /// </summary>
         public string license { get; set; }

        /// <summary>
        ///Дата принятия решения о предоставлении лицензии 
        /// </summary>
        public double licIssueDate { get; set; }

        /// <summary>
        /// Дата начала действия лицензии
        /// </summary>
        public double licStartDate { get; set; }

        /// <summary>
        /// Дата окончания действия лицензии
        /// </summary>
        public double licEndDate { get; set; }

        /// <summary>
        /// Сведения о переоформлении документа, подтверждающего наличие лицензии
        /// </summary>
        public string licConfUpdate { get; set; }

        /// <summary>
        /// Сведения о приостановлении действия лицензии
        /// </summary>
        public string licStoped { get; set; }

        /// <summary>
        /// Сведения о возобновлении действия лицензии 
        /// </summary>
        public string licContinued { get; set; }

        /// <summary>
        /// Сведения об аннулировании лицензии
        /// </summary>
        public string licAnnul { get; set; }

        /// <summary>
        /// Сведения о прекращении действия лицензии
        /// </summary>
        public string licCanceled { get; set; }

        /// <summary>
        ///Сведения о документе, подтверждающем наличие лицензии
        /// </summary>
        public string docLicConfirm { get; set; }

        /// <summary>
        /// Выписка
        /// </summary>
        public string excerpt { get; set; }
    }

    public class SimpleLicInfo : ISimpleLicInfo
    {
        /// <summary>
        /// ID сети (аптеки) из системы ЮМКБ, к которой относится лицензия
        /// </summary>
        [JsonRequired]
        public uint? parent_id { get; set; }
        [JsonRequired]
        public string UserId { get; set; }
        [JsonRequired]
        public string license { get; set; }
        [JsonRequired]
        public string name { get; set; }
        [JsonRequired]
        public string issuing { get; set; }
        [JsonRequired]
        public string address { get; set; }
        [JsonRequired]
        public string dateStart { get; set; }
        [JsonRequired]
        public string dateEnd { get; set; }
        [JsonRequired]
        public string ogrn { get; set; }
        [JsonRequired]
        public string inn { get; set; }
        [JsonRequired]
        public string order { get; set; }
        [JsonRequired]
        public string orderFresh { get; set; }
    }

    public class RozZdravInfo : IRozZdravInfo
    {
       // public GeoJson2DGeographicCoordinates location { get; set; }
        public string name { get; set; }

        /// <summary>
        ///  (вид деятельности на которую выдана лицензия)
        /// </summary>
        public string activity { get; set; }

        /// <summary>
        ///  (полный адрес: индекс + город + улица + дом)
        /// </summary>
        public string address_fact { get; set; }

        /// <summary>
        ///  (короткий адрес: улица + дом)
        /// </summary>
        public string address_fact_street { get; set; }

        /// <summary>
        ///  (индекс)
        /// </summary>
        public string address_fact_zip { get; set; }

        public string appartment_num { get; set; }
        public string building_num { get; set; }
        public string city { get; set; }

        /// <summary>
        ///   (хз че за код внутри росздравнадзора, но пусть будет, выглядит так: "40c70f85-2502-4a92-8aa9-4efc03c2d521")
        /// </summary>
        public string code_fias { get; set; }
        public string district_label { get; set; }

        /// <summary>
        /// "2017-09-21 10:09:17" (дата создания)
        /// </summary>
        public string dt_created { get; set; }

        /// <summary>
        ///  "2017-09-21 10:09:17" (дата последнего изменения)
        /// </summary>
        public string dt_edited { get; set; }
        public string ext_city_id { get; set; }
        public string ext_db { get; set; }
        public string ext_id { get; set; }
        public string ext_org { get; set; }
        public string ext_region_id { get; set; }
        public string ext_type_id { get; set; }
        public string fake { get; set; }
        public string from_b2b { get; set; }
        public string house_num { get; set; }

        /// <summary>
        /// "766954" (айдишник внутри Росздравнадзора)
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// "81841"
        /// </summary>
        public string id_create_user { get; set; }
        public string id_department { get; set; }

        /// <summary>
        ///"3638603"
        /// </summary>
        public string id_licenses_apps_object { get; set; }
        public string id_organization { get; set; }
        public string id_revision { get; set; }

        /// <summary>
        /// "2"
        /// </summary>
        public string id_type_org { get; set; }
        public string id_user_edited { get; set; }
        public string id_voc_city { get; set; }
        public string id_voc_district { get; set; }
        public string id_voc_licenser { get; set; }
        public string id_voc_producer { get; set; }
        public string id_voc_region { get; set; }

        /// <summary>
        /// "14687,14685,14683,14686"
        /// </summary>
        public string id_voc_revision_activity_items { get; set; }
        public string id_voc_revision_object_type { get; set; }
        public string id_voc_settlements { get; set; }
        public string ids_voc_revision_activities { get; set; }
        public string is_active_b2b { get; set; }
        public string is_actual { get; set; }
        public string is_foreign { get; set; }
        public string is_med_business_check { get; set; }
        public string is_not_code_fias { get; set; }
        public string is_save_region { get; set; }
        public string kladr { get; set; }

        /// <summary>
        ///  "Общество с ограниченной ответственностью \"Аптечная сеть Оз\""
        /// </summary>
        public string label { get; set; }
        public string label_firm { get; set; }
        public string label_short { get; set; }
        public string okato { get; set; }
        public string oktmo { get; set; }
        public string region { get; set; }
        public string street { get; set; }
    }

    public class RozZdravWPhoto:RozZdravInfo, IRozZdravInfo
    {
        public byte[] photo { get; set; }
    }
}
