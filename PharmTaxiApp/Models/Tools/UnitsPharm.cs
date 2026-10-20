using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiApp.Models.Tools
{
    public class UnitsPharm
    {
#pragma warning disable IDE1006 // Стили именования
            /// <summary>
            /// Лицензионный ключ клиента
            /// </summary>
            public string key { get; set; }
            /// <summary>
            /// Ключ авторизации (генерируется биллингом)
            /// </summary>
            public string authkey { get; set; }
            /// <summary>
            ///  индекс приложения/сервиса к которому обращается клиент
            /// </summary>
            public string service { get; set; }
            /// <summary>
            /// Индентификатор для набора получаемой информации
            /// </summary>
            public string active_language { get; set; } = "ru";
            /// <summary>
            /// Список Id комлплексных значений, которые надо разложить на элементы
            /// </summary>
            public List<string> units { get; set; } = new List<string>();
#pragma warning restore IDE1006 // Стили именования
    }
}
