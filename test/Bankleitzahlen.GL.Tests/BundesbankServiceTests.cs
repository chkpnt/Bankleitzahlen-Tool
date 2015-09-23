using Bankleitzahlen.Bundesbank;
using Bankleitzahlen.Commons;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankleitzahlen.GL.Tests
{
    [TestFixture]
    public class BundesbankServiceTests
    {
        [Test]
        public async void GL_BundesbankService_GetBankleitzahlenänderungsdateien()
        {
            var service = new BundesbankService();
            service.BankleitzahlenänderungsdateiService = GetServiceMock().Object;

            var bankleitzahländerungsdateien = await service.GetBankleitzahlenänderungsdateien();

            Assert.That(bankleitzahländerungsdateien.Dateien.Count(), Is.EqualTo(1));
            Assert.That(bankleitzahländerungsdateien.Dateien.First().Dateiname, Is.EqualTo("a"));
            Assert.That(bankleitzahländerungsdateien.Dateien.First().Uri, Is.EqualTo(new Uri("http://beispiel/a")));
        }

        [Test]
        public async void GL_BundesbankService_GetBanken()
        {
            var service = new BundesbankService();
            service.BankleitzahlenänderungsdateiService = GetServiceMock().Object;

            var banken = await service.GetBanken(new Bankleitzahlenänderungsdatei());

            Assert.That(banken.Count(), Is.EqualTo(2));
            Assert.That(banken.First().Name, Is.EqualTo("Bank-1"));
            Assert.That(banken.Last().Name, Is.EqualTo("Bank-2"));
        }

        private Mock<IBankleitzahlenänderungsdateiService> GetServiceMock()
        {
            var serviceMock = new Mock<IBankleitzahlenänderungsdateiService>();


            var returnValue_LadeUris = new Bankleitzahlenänderungsdateien()
            {
                Dateien = new List<Bankleitzahlenänderungsdatei>()
                {
                    new Bankleitzahlenänderungsdatei
                    {
                        Dateiname = "a",
                        Uri = new Uri("http://beispiel/a")
                    }
                }
            };
            serviceMock.Setup(s => s.LadeUris()).ReturnsAsync(returnValue_LadeUris);

            var returnValue_LadeBanken = new List<Bank>
            {
                 new Bank { Name="Bank-1" },
                 new Bank { Name="Bank-2" } 
            };
            serviceMock.Setup(s => s.LadeBankenAusÄnderungsdatei(It.IsAny<Bankleitzahlenänderungsdatei>())).ReturnsAsync(returnValue_LadeBanken);

            return serviceMock;
        }
    }
}
