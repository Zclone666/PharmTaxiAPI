using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UMKBCoreAPI.Models.Api.Responses;

namespace PharmTaxiApp.Models.ECP
{
    public class AnswerUIText
    {
        public AnswerUIText()
        {
            freq = new Dictionary<string, string>();
            duration = new Dictionary<string, string>();
            button = new Dictionary<string, string>();
            suptext = new Dictionary<string, string>();
            caution = new Dictionary<string, string>();
            ecptext = new Dictionary<string, string>();
        }

        public AnswerUIText(AnswerUIText answerUIText)
        {
            freq = answerUIText.freq.ToDictionary(t => string.Copy(t.Key), t => string.Copy(t.Value));
            duration = answerUIText.duration.ToDictionary(t => string.Copy(t.Key), t => string.Copy(t.Value));
            button = answerUIText.button.ToDictionary(t => string.Copy(t.Key), t => string.Copy(t.Value));
            suptext = answerUIText.suptext.ToDictionary(t => string.Copy(t.Key), t => string.Copy(t.Value));
            caution = answerUIText.caution.ToDictionary(t => string.Copy(t.Key), t => string.Copy(t.Value));
            ecptext = answerUIText.ecptext.ToDictionary(t => string.Copy(t.Key), t => string.Copy(t.Value));
            alert = new Alert() { Code = "200", Level = "success" };
        }

        public Alert alert { get; set; } = null;
        public Dictionary<string, string> freq { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> duration { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> button { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> suptext { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> caution { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> ecptext { get; set; } = new Dictionary<string, string>();
    }
}
