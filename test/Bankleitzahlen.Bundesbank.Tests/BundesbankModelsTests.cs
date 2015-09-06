using FileHelpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankleitzahlen.Bundesbank.Tests
{
    [TestFixture]
    public class BundesbankModelsTests
    {
        private FileHelperEngine<BundesbankBankleitzahlenÄnderungsdateieintrag> _engine;

        [SetUp]
        public void SetUp()
        {
            _engine = new FileHelperEngine<BundesbankBankleitzahlenÄnderungsdateieintrag>();
        }
        
        [Test]
        public void Bundesbank_Models_KompletteZeile()
        {
            var zeile = "750690761Raiffeisenbank Kallmünz -alt-                             93183Kallmünz                           Raiffbk Kallmünz -alt-     67232GENODEF1KLM88027595U175069061";

            var eintrag = _engine.ReadStringAsList(zeile).Single();

            Assert.That(eintrag.Bankleitzahl, Is.EqualTo(75069076));
            Assert.That(eintrag.Merkmal, Is.EqualTo(1));
            Assert.That(eintrag.Bezeichnung, Is.EqualTo("Raiffeisenbank Kallmünz -alt-"));
            Assert.That(eintrag.Postleitzahl, Is.EqualTo(93183));
            Assert.That(eintrag.Ort, Is.EqualTo("Kallmünz"));
            Assert.That(eintrag.Kurzbezeichnung, Is.EqualTo("Raiffbk Kallmünz -alt-"));
            Assert.That(eintrag.PAN, Is.EqualTo(67232));
            Assert.That(eintrag.BIC, Is.EqualTo("GENODEF1KLM"));
            Assert.That(eintrag.Prüfzifferberechnungsmethode, Is.EqualTo(88));
            Assert.That(eintrag.Datensatznummer, Is.EqualTo(27595));
            Assert.That(eintrag.Änderungskennzeichen, Is.EqualTo("U"));
            Assert.That(eintrag.Bankleitzahlenlöschung, Is.True);
            Assert.That(eintrag.Nachfolgebankleitzahl, Is.EqualTo(75069061));
        }

        [Test]
        public void Bundesbank_Models_ZeileOhneBICundNachfolgebankleitzahl()
        {
            var zeile = "660912002Volksbank Ettlingen Zw Karlsbad                           76307Karlsbad                           Volksbank Ettlingen        96078           06022797U000000000";

            var eintrag = _engine.ReadStringAsList(zeile).Single();

            Assert.That(eintrag.Bankleitzahl, Is.EqualTo(66091200));
            Assert.That(eintrag.Merkmal, Is.EqualTo(2));
            Assert.That(eintrag.Bezeichnung, Is.EqualTo("Volksbank Ettlingen Zw Karlsbad"));
            Assert.That(eintrag.Postleitzahl, Is.EqualTo(76307));
            Assert.That(eintrag.Ort, Is.EqualTo("Karlsbad"));
            Assert.That(eintrag.Kurzbezeichnung, Is.EqualTo("Volksbank Ettlingen"));
            Assert.That(eintrag.PAN, Is.EqualTo(96078));
            Assert.That(eintrag.BIC, Is.Null);
            Assert.That(eintrag.Prüfzifferberechnungsmethode, Is.EqualTo(6));
            Assert.That(eintrag.Datensatznummer, Is.EqualTo(22797));
            Assert.That(eintrag.Änderungskennzeichen, Is.EqualTo("U"));
            Assert.That(eintrag.Bankleitzahlenlöschung, Is.False);
            Assert.That(eintrag.Nachfolgebankleitzahl, Is.Null);
        }
    }
}
