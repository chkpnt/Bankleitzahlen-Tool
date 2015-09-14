using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Reflection;

namespace Bankleitzahlen.Bundesbank.Tests
{
    [TestFixture]
    public class BankleitzahlenänderungsdateiServiceTests
    {
        private BankleitzahlenänderungsdateiService _service;

        [SetUp]
        public void SetUp()
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Bankleitzahlen.Bundesbank.Tests.Resources.bankleitzahlen_download.html");

            var webclientMock = new Mock<IWebClient>();
            webclientMock.Setup(client => client.OpenReadTaskAsync(It.IsAny<Uri>())).ReturnsAsync(stream);

            var webclientFactoryMock = new Mock<IWebClientFactory>();
            webclientFactoryMock.Setup(factory => factory.CreateWebClient()).Returns(webclientMock.Object);

            _service = new BankleitzahlenänderungsdateiService();
            _service.WebClientFactory = webclientFactoryMock.Object;

        }

        [Test]
        public async void Bundesbank_BankleitzahlenänderungsdateiService_LadeUris()
        {
            var uris = await _service.LadeUris();
            Assert.That(uris.Uris.Count(), Is.EqualTo(2));

            Assert.That(uris.Uris.First().Dateiname, Is.EqualTo("blz_2015_06_08_txt.txt"));
            Assert.That(uris.Uris.First().Uri, Is.EqualTo(new Uri("https://www.bundesbank.de/Redaktion/DE/Downloads/Aufgaben/Unbarer_Zahlungsverkehr/Bankleitzahlen/2015_09_06/blz_2015_06_08_txt.txt?__blob=publicationFile")));

            Assert.That(uris.Uris.Last().Dateiname, Is.EqualTo("blz_2015_09_07_txt.txt"));
            Assert.That(uris.Uris.Last().Uri, Is.EqualTo(new Uri("https://www.bundesbank.de/Redaktion/DE/Downloads/Aufgaben/Unbarer_Zahlungsverkehr/Bankleitzahlen/2015_12_06/blz_2015_09_07_txt.txt?__blob=publicationFile")));
        }
    }
}
