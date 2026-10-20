using System;
using System.Collections.Generic;
using System.Text;

namespace PTConstants
{
    /// <summary>
    /// Все что относится к MySQL
    /// </summary>
    public class MySQL
    {
        /// <summary>
        /// Строка подключения к базе umkb
        /// </summary>
        #region возможно старые версии этой строки подключения
        //"server=db.socmedica.com;port=33066;database=umkb;user id=umkb_o;password='HG;ls;kg8934=12f';allowuservariables=True;
        //          CharSet=utf8;Convert Zero Datetime=True;Allow Zero Datetime=True; Pooling = false; Connect Timeout=60"; 
        //"server=db.socmedica.dev;port=33066;Initial Catalog=umkb;User ID=dssc_user;Password=o5BqvY3Til14xdXY; Pooling = false; Connect Timeout=60";
        //"server=umkb.socmedica.dev;port=33066;Initial Catalog=dccs;User ID=dssc_user;Password=o5BqvY3Til14xdXY"; что за база dccs хз, возможно это не отсюда
        #endregion
        public static string UMKBConString;
        /// <summary>
        /// Cтрока подключения к базе pharm
        /// </summary>
        #region возможно старые версии этой строки подключения
        //user id = dssc_user; password = 'PoHOvu9DYYZLMLWt'; Pooling = false;";
        #endregion
        public static string FPPriceConString;
    }
}
