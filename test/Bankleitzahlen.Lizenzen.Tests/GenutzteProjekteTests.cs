using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankleitzahlen.Lizenzen.Tests
{
    [TestFixture]
    public class GenutzteProjekteTests
    {

        private GenutzteProjekte _projekte;

        [SetUp]
        public void SetUp()
        {
            _projekte = new GenutzteProjekte();
        }

        [Test]
        public void GenutzteProjekte_AnzahlDerFremdprojekte()
        {
            var anzahl = _projekte.Projekte.Count();
            Assert.That(anzahl, Is.EqualTo(4));
        }

        [Test, Explicit]
        public void GenutzteProjekte_AusgabeDerLizenzinformationen()
        {
            var ausgabe = from projekt in _projekte.Projekte
                          select string.Format(@"Projekt: {0}
Autor: {1}
Homepage: {2}
Lizenz:
{3}
", projekt.Projekt, projekt.Autor, projekt.Homepage, projekt.LizenzText);

            Console.WriteLine(string.Join(Environment.NewLine, ausgabe.ToArray()));
        }
    }
}
