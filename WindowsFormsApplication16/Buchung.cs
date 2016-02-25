using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication16
{
    public class Buchung
    {
        public string Betreff { get; set; }
        public DateTime Datum { get; set; }

        public string BuchungsTyp { get; set; }

        public double Betrag { get; set; }

        public string Vorzeichen { get; set; }
    }
}
