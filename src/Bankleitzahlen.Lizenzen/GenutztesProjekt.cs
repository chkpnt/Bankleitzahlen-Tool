using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankleitzahlen.Lizenzen
{
    public class GenutztesProjekt
    {
        public string Projektname { get; set; }
        public string Autor { get; set; }
        public Uri Homepage { get; set; }
        public string LizenzAbk { get; set; }
        public string LizenzText { get; set; }
    }
}
