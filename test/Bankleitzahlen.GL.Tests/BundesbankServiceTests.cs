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

        private Mock<IBankleitzahlenänderungsdateiService> GetServiceMock()
        {
            var serviceMock = new Mock<IBankleitzahlenänderungsdateiService>();

            var returnValue = new Bankleitzahlenänderungsdateien()
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

            serviceMock.Setup(s => s.LadeUris()).ReturnsAsync(returnValue);

            return serviceMock;
        }
    }
}
