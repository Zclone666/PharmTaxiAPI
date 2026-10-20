using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiApp.Methods
{
    public static class RecipeCodesToRecipeArgs
    {
        public static List<Models.RecipeArgs> RecArgsFromSNILS(string snils)
        {
            List<Models.RecipeArgs> ret = new List<Models.RecipeArgs>();
            List<string> Numbs=new List<string>();
            try
            {
                SMReceipt.Global.Models.SearchReceipt sr = new SMReceipt.Global.Models.SearchReceipt() { SNILS = snils };
                Numbs = SMReceipt.Global.Methods.Receipt.GetNumbFin(sr);
            }catch(Exception e) { }
            foreach (var i in Numbs)
            {
                ret.Add(new Models.RecipeArgs() { recipe = i });
            }
            return ret;
        }

    }
}
