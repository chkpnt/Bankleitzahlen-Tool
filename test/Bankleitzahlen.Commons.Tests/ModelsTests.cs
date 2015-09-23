using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankleitzahlen.Commons.Tests
{
    [TestFixture]
    public class ModelsTests
    {
        [Test]
        public void Models_BLZ_Conversion()
        {
            BLZ blz = 12345;
            Assert.That((int)blz, Is.EqualTo(12345));
        }

        [Test]
        public void Models_BLZ_ToString()
        {
            BLZ blz = 12345;
            Assert.That(blz.ToString(), Is.EqualTo("12345"));

            Assert.That(BLZ.Invalid.ToString(), Is.EqualTo("--------"));
        }

        [Test]
        public void Models_Bank_ToString_Default()
        {
            var bank = new Bank();
            Assert.That(bank.ToString(), Is.EqualTo("-------- UNBENANNTE BANK"));
        }

        [Test]
        public void Models_Bank_ToString_OhneAdresse()
        {
            var bank = new Bank { Bankleitzahl = 12345678, Name = "Parkbank" };
            Assert.That(bank.ToString(), Is.EqualTo("12345678 Parkbank"));
        }

        [Test]
        public void Models_Bank_ToString_MitAdresse()
        {
            var bank = new Bank { Bankleitzahl = 12345678, Name = "Parkbank", Adresse = new CivicAddress { City = "Entenhausen" } };
            Assert.That(bank.ToString(), Is.EqualTo("12345678 Parkbank (Entenhausen)"));

            bank = new Bank
            {
                Bankleitzahl = 12345678,
                Name = "Parkbank",
                Adresse = new CivicAddress { City = "Entenhausen", PostalCode = "12345" }
            };
            Assert.That(bank.ToString(), Is.EqualTo("12345678 Parkbank (12345 Entenhausen)"));

            bank = new Bank
            {
                Bankleitzahl = 12345678,
                Name = "Parkbank",
                Adresse = new CivicAddress { PostalCode = "12345" }
            };
            Assert.That(bank.ToString(), Is.EqualTo("12345678 Parkbank (12345 ORTSNAME FEHLT)"));
        }
    }
}
