using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UMKBRequests;

namespace PharmTaxiApp.Models
{
    public class Answer
    {
#pragma warning disable IDE1006 // Стили именования
        /// <summary>
        /// Стандартный алерт для передачи состояния
        /// </summary>
        public Alert alert { get; set; }
        public List<Concept> concepts { get; set; } = new List<Concept>();
#pragma warning restore IDE1006 // Стили именования
    }

}
