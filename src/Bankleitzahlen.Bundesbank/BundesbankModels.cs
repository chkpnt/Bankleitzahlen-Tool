using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankleitzahlen.Bundesbank
{


    [FixedLengthRecord()]
    public class BankleitzahlenänderungsdateiEintrag
    {
        [FieldFixedLength(8)]
        public int Bankleitzahl;

        [FieldFixedLength(1)]
        public int Merkmal;

        [FieldFixedLength(58)]
        [FieldTrim(TrimMode.Right)]
        public string Bezeichnung;

        [FieldFixedLength(5)]
        public int Postleitzahl;

        [FieldFixedLength(35)]
        [FieldTrim(TrimMode.Right)]
        public string Ort;

        [FieldFixedLength(27)]
        [FieldTrim(TrimMode.Right)]
        public string Kurzbezeichnung;

        [FieldFixedLength(5)]
        public int? PAN;

        [FieldFixedLength(11)]
        [FieldConverter(typeof(EmptyStringIsNullConverter))]
        public string BIC;

        [FieldFixedLength(2)]
        public string Prüfzifferberechnungsmethode;

        [FieldFixedLength(6)]
        public int Datensatznummer;

        [FieldFixedLength(1)]
        public string Änderungskennzeichen;

        [FieldFixedLength(1)]
        [FieldConverter(ConverterKind.Boolean, "1", "0")]
        public bool Bankleitzahlenlöschung;

        [FieldFixedLength(8)]
        [FieldConverter(typeof(ZeroToNullConverter))]
        public int? Nachfolgebankleitzahl;

        internal class EmptyStringIsNullConverter : ConverterBase
        {
            public override object StringToField(string from)
            {
                if (string.IsNullOrWhiteSpace(from))
                {
                    return null;
                }

                return from;
            }
        }

        internal class ZeroToNullConverter : ConverterBase
        {
            public override object StringToField(string from)
            {
                var number = int.Parse(from);

                if (number == 0)
                {
                    return null;
                }

                return number;
            }
        }
    }
}
