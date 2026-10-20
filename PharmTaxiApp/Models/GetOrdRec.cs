using ECPLib.Common.Drugs.Models;
using ECPLib.Common.General.Models;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using ECPLib;
using UMKBNeedStuff;

namespace PharmTaxiApp.Models
{
    public class GetOrdRec
    {
        public bool IsRec { get; set; }
        public SMReceipt.Global.Models.SaveOrder Order { get; set; }
        public ModelRecipeLS Prescript { get; set; }
    }

    public class AllRecipes
    {
        public Alert alert { get; set; }
        public List<ModelRecipeLS> recipes { get; set; }
    }

    public class ModelRecipeLS
    {
        public Alert alert { get; set; } = new Alert() { code = "200", level = "success" };
        /// <summary>
        /// Назначеная фармакотерапия
        /// </summary>
        public string recipe_code { get; set; }
        public List<LS> liquids { get; set; }
        /// <summary>
        /// Имена к айдишникам (если их нет в кэше) в фармакотерапии в нужной локали
        /// </summary>
        public Dictionary<string, string> names { get; set; }
        /// <summary>
        /// Пациент
        /// </summary>
        public ECPLib.Common.Patient.Models.Patient patient { get; set; }
        /// <summary>
        /// Id врача выписавшего рецепт
        /// </summary>
        public string idDoc { get; set; }
        /// <summary>
        /// Id юзера, выписавшего рецепт
        /// </summary>
        public string idUser { get; set; }
        /// <summary>
        /// Id отделения, где выписали рецепт
        /// </summary>
        public string idDep { get; set; }
        /// <summary>
        /// Id клиники, где выписали рецепт
        /// </summary>        
        public string idClinic { get; set; }
        /// <summary>
        /// Дата выписки рецепта (UnixTime мс.)
        /// </summary>
        public long dateStart { get; set; }
        /// <summary>
        /// Дата окончания действия рецепта (UnixTime мс.)
        /// </summary>
        public long? dateEnd { get; set; }
        /// <summary>
        /// Табличное значение продолжительности действия рецепта
        /// </summary>
        public ECPLib.Common.TableValueItem dateEndItem { get; set; }
        /// <summary>
        /// ID Рецептурной формы
        /// </summary>
        public string idForm { get; set; }
        /// <summary>
        /// ID Рецептурной формы
        /// </summary>
        public string NameForm { get; set; }
        /// <summary>
        /// ID статуса рецепта
        /// </summary>
        public uint IdStatus { get; set; }
        /// <summary>
        /// Диагноз по МКБ10
        /// </summary>
        public string dCode { get; set; }
        /// <summary>
        /// Дата рождения пациента (на момент выписки)
        /// </summary>
        public string pBirthday { get; set; }
        /// <summary>
        /// СНИЛС пациента (на момент выписки)
        /// </summary>
        public string pSnils { get; set; }
        /// <summary>
        /// Коментарий врача
        /// </summary>
        public string dTextComment { get; set; }
        /// <summary>
        /// Рецепт для взр
        /// </summary>
        public bool? IsAdult { get; set; }
        /// <summary>
        /// Список тегов шаблона
        /// </summary>
        public Dictionary<string, List<string>> dText { get; set; }
    }

    public class LS
    {
        public Drug liquid { get; set; }
        public List<Drug> drugs { get; set; }
    }

    public class TableValueItem
    {
        /// <summary>
        /// ID концепта
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// Наименование концепта
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Параметрическое значение
        /// </summary>
        public double? value { get; set; }
        /// <summary>
        /// Единица измерения значения
        /// </summary>
        public uint? unit { get; set; }
    }

    /// <summary>
    /// Информация о пациенте
    /// </summary>
    public class Patient
    {
#pragma warning disable IDE1006 // Стили именования
        /// <summary>
        /// ulong id пациента в системе UMKB
        /// </summary>
        [BsonId]
        public string id { get; set; }
        /// <summary>
        /// Имя пациента (ник)
        /// </summary>
        [BsonElement]
        public string name { get; set; }
        /// <summary>
        /// e-mail пациента
        /// </summary>
        [BsonElement]
        public string email { get; set; }
        /// <summary>
        /// на будущее ID в МИСе
        /// </summary>
        [BsonElement]
        public string miscode { get; set; }
        /// <summary>
        /// на будущее ID МИСа
        /// </summary>
        [BsonElement]
        public string misid { get; set; }
        /// <summary>
        /// ФИО
        /// </summary>
        [BsonElement]
        public string fio { get; set; }
        /// <summary>
        /// СНИЛС
        /// </summary>
        [BsonElement]
        public string snils { get; set; }

        [BsonElement]
        public string key { get; set; }

        /// <summary>
        /// Дата рождения (UNIXTIME). до 1970 года - отрицательная
        /// </summary>
        [BsonElement]
        public Int64? birthdate { get; set; }
        /// <summary>
        /// Пол (male|female)
        /// </summary>
        [BsonElement]
        public string sex { get; set; }
        /// <summary>
        ///  Рост (см)
        /// </summary>
        [BsonElement]
        public double? growth { get; set; }
        /// <summary>
        ///  Вес (кг)
        /// </summary>
        [BsonElement]
        public double? weight { get; set; }
        /// <summary>
        ///  Основные заболевания (lib 61|25)
        /// </summary>
        [BsonElement]
        public List<Concept> disease { get; set; }
        /// <summary>
        ///  Сопутствующие заболевания (lib 61|25)
        /// </summary>
        [BsonElement]
        public List<Concept> co_disease { get; set; }
        /// <summary>
        ///  Флаг беременности
        /// </summary>
        [BsonElement]
        public bool pregnancy { get; set; } = false;
        /// <summary>
        ///  Неделя беременности
        /// </summary>
        [BsonElement]
        public int? gestation { get; set; }
        /// <summary>
        ///  Триместр беременности
        /// </summary>
        [BsonElement]
        public int? trimester { get; set; }
        /// <summary>
        ///  Флаг кормления грудью
        /// </summary>
        [BsonElement]
        public bool lactation { get; set; } = false;
        /// <summary>
        ///  Флаг вождения
        /// </summary>
        [BsonElement]
        public bool driving { get; set; } = false;
        /// <summary>
        ///  Флаг порверки на алкоголь
        /// </summary>
        [BsonElement]
        public bool alcohol { get; set; } = false;
        /// <summary>
        ///  Аллергия пациента (lib 62|52|37|60)
        /// </summary>
        [BsonElement]
        public List<Concept> allergy { get; set; }

        private List<string> _labparams;
        /// <summary>
        ///  Лабораторные показатели
        /// </summary>        
        [BsonElement]
        public List<string> labparams
        {
            get { if (_labparams is null) { _labparams = new List<string>(); } return _labparams; }
            set { if (value is null) { _labparams = new List<string>(); } else { _labparams = value; } }
        }
        /// <summary>
        /// Список актуальных протоколов фармтерапии.
        /// Ключ - имя коллекции, тело - список BsonId.
        /// </summary>
        //[BsonIgnoreIfNull]
        [BsonElement]
        [JsonIgnore]
        public Dictionary<string, HashSet<string>> actualproto { get; set; } = new Dictionary<string, HashSet<string>>();
#pragma warning restore IDE1006 // Стили именования
    }

}
