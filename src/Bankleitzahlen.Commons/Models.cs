using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankleitzahlen.Commons
{
    public class Bank
    {
        public string Name { get; set; } = string.Empty;
        public BLZ Bankleitzahl { get; set; } = BLZ.Invalid;
        public CivicAddress Adresse { get; set; }

        public override string ToString()
        {
            var blz = Bankleitzahl.ToString();
            var name = string.IsNullOrEmpty(Name) ? "UNBENANNTE BANK" : Name;
            
            if (Adresse == null)
            {
                return string.Format("{0} {1}", blz, name);
            }

            var stadt = string.IsNullOrEmpty(Adresse.City) ? "ORTSNAME FEHLT" : Adresse.City;

            if (string.IsNullOrEmpty(Adresse.PostalCode))
            {
                return string.Format("{0} {1} ({2})", blz, name, stadt);
            }

            return string.Format("{0} {1} ({2} {3})", blz, name, Adresse.PostalCode, stadt);
        }
    }

    public class BLZ
    {
        public static readonly BLZ Invalid = -1;

        private int _value;

        public BLZ(int blz)
        {
            this._value = blz;
        }

        public static implicit operator BLZ(int blz) => new BLZ(blz);

        public static explicit operator int(BLZ blz) => blz._value;

        public override string ToString()
        {
            if (this == Invalid)
            {
                return "--------";
            }

            return _value.ToString();
        }
    }

    public class Bankleitzahlenänderungsdatei
    {
        public string Dateiname { get; set; }
        public Uri Uri { get; set; }
    }

    public class Bankleitzahlenänderungsdateien
    {
        public IEnumerable<Bankleitzahlenänderungsdatei> Dateien { get; set; }
    }
}
