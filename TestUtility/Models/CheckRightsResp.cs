using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class AlertUserID
        {
            public uint userid; // id пользователя
            public UMKBRequests.Alert alert; // Коллекция параметров оповещения
        }

    public class AuthK
    {
        public string authkey { get; set; }
        public UMKBRequests.Alert alert { get; set; }
    }
}
