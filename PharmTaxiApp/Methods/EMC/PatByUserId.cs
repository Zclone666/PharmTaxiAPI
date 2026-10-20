using System;
using System.Collections.Generic;
using System.Linq;
using PTConstants;

namespace PharmTaxiApp.Methods.EMCLocal
{
    public static class PatByUserId
    {
        public static uint GetPatId(uint UserId)
        {
            uint ret = 0;
            try
            {
                UMKBRequests.GetSemanticNewBigId GsNew = new UMKBRequests.GetSemanticNewBigId();

                UMKBRequests.Models.API.Semantic.RequestSemantic ReqSm = new UMKBRequests.Models.API.Semantic.RequestSemantic() 
                {
                    deep = 1, 
                    key = UMKB.Graph, 
                    level = new List<ushort>() { 951, 9112, 3001 }, 
                    lib = new List<ushort>() { 119 }, 
                    SelectLevelb = new HashSet<ushort>() { 3001 }, 
                    libid = new List<string>() { (UserId * 10000 + Libs.LibsUint.User).ToString() } 
                };

                UMKBRequests.Models.API.Semantic.ResultSemanticNewBigId Res = GsNew.Get(ReqSm);
                if (Res.alerts.code == 200) ret = (uint)Res.graph.LastOrDefault().idb;
            }
            catch(Exception e)
            {
                ret = 0;
            }

            return ret;
        }
    }
}
