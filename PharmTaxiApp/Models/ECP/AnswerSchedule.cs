using System;
using System.Collections.Generic;
using System.Text;
using UMKBCoreAPI.Models.Api.Responses;
using ECPLib.Common.MIS.Models;

namespace PharmTaxiApp.Models.ECP
{

    public class AnswerSchedule
    {
        /// <summary>
        /// Стандартный класс алерта - информация о состоянии обработки запроса.
        /// </summary>
        public Alert alert { get; set; } = new Alert() { Code = "200", Message = null, Sticky = false, Title = string.Empty, Level = "success" };
        public Dictionary<string, List<ScheduleItem>> freq { get; set; } = new Dictionary<string, List<ScheduleItem>>();
        public Dictionary<string, List<ScheduleItem>> duration { get; set; } = new Dictionary<string, List<ScheduleItem>>();
        public Dictionary<string, string> names { get; set; } = new Dictionary<string, string>();
    }
}
