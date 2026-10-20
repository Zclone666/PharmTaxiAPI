using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiAdm.Models.User
{
    public class ResponseGetUserInfoByEmail
    {
        public Alert alert { get; set; } // Коллекция параметров оповещения
        public int userId { get; set; } // id пользователя
        public string firstName { get; set; } // Имя пользователя
        public string lastName { get; set; } // Фамилия пользователя
        public string middleName { get; set; } // Отчество пользователя
        public bool isEmailConfirmed { get; set; } // Email подтвержден? (UserCore.status==0)
        public List<uint> services { get; set; } // Список id сервисов пользователя
    }
}
