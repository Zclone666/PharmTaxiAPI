using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Methods.Recipe.Model
{
    public class ModelDrug : ECPLib.Common.Drugs.Models.Drug
    {
        public ModelDrug() { }

        public ModelDrug(ECPLib.Common.Drugs.Models.Drug drug)
        {
            atx_id = drug.atx_id;
            atx_name = drug.atx_name;
            calc = drug.calc;
            clinic_code = drug.clinic_code;
            date = drug.date;
            phantom = drug.phantom;
            dep_code = drug.dep_code;
            doctor_code = drug.doctor_code;
            dosage = drug.dosage;
            duration = drug.duration;
            duration_id = drug.duration_id;
            edit = drug.edit;
            fa_id = drug.fa_id;
            fa_name = drug.fa_name;
            fg_id = drug.fg_id;
            fg_name = drug.fg_name;
            fields = drug.fields;
            fpid = drug.fpid;
            freq = drug.freq;
            freq_id = drug.freq_id;
            group = drug.group;
            id = drug.id;
            key = drug.key;
            level = drug.level;
            liquid = drug.liquid;
            mnnid = drug.mnnid;
            mnnname = drug.mnnname;
            name = drug.name;
            namesearch = drug.namesearch;
            parsedosage = drug.parsedosage;
            pdate = drug.pdate;
            pkey = drug.pkey;
            pproto = drug.pproto;
            proto = drug.proto;
            proto_date = drug.proto_date;
            proto_key = drug.proto_key;
            recipenameid = drug.recipenameid;
            recipes = drug.recipes;
            repeal = drug.repeal;
            repealdate = drug.repealdate;
            repealdoc = drug.repealdoc;
            repealproto = drug.repealproto;
            schedule = drug.schedule;
            solute = drug.solute;
            specials = drug.specials;
            text = drug.text;
            type = drug.type;
            umkb10_id = drug.umkb10_id;
            umkb10_name = drug.umkb10_name;
        }
        /// <summary>
        /// статус драга
        /// </summary>
        public int IdStatus { get; set; }
        /// <summary>
        /// дата изменения драга
        /// </summary>
        public long DateСhange { get; set; }
    }
}
