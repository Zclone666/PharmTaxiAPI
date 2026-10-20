using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Models
{
    public class UserInfoResp
    {
        public UserInfo userInfo; // данные о пользователе
        public PharmTaxiAdm.Models.User.Alert alert; // Коллекция параметров оповещения
    }

    public class UserInfo // информация о пользователе
    {
        public uint user_id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string middleName { get; set; }
        public bool emailConfirmed { get; set; } // Email подтвержден? (UserCore.status==0)
    }

}
