using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiApp.Models
{
    public class AnswerUIText
    {
        public UMKBRequests.Alert alert { get; set; } = null;
        public Dictionary<string, string> freq { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> duration { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> button { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> suptext { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> caution { get; set; } = new Dictionary<string, string>();
    }
}
