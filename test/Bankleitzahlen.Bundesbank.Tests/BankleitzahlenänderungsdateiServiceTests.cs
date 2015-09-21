using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Reflection;
using Bankleitzahlen.Commons;
using System.IO.Abstractions.TestingHelpers;

namespace Bankleitzahlen.Bundesbank.Tests
{
    [TestFixture]
    public class BankleitzahlenänderungsdateiServiceTests
    {
        private BankleitzahlenänderungsdateiService _service;

        [SetUp]
        public void SetUp()
        {
            var stream_website = Assembly.GetExecutingAssembly().GetManifestResourceStream("Bankleitzahlen.Bundesbank.Tests.Resources.bankleitzahlen_download.html");
            var stream_änderungsdatei = Assembly.GetExecutingAssembly().GetManifestResourceStream("Bankleitzahlen.Bundesbank.Tests.Resources.blz_2015_09_07_txt.txt");

            var webclientMock = new Mock<IWebClient>();
            webclientMock.Setup(client => client.OpenReadTaskAsync(It.Is<Uri>(uri => uri == BankleitzahlenänderungsdateiService.BankleitzahlenänderungsdateiUri))).ReturnsAsync(stream_website);
            webclientMock.Setup(client => client.OpenReadTaskAsync(It.Is<Uri>(uri => uri == new Uri("https://foo.bla/blz_2015_09_07_txt.txt")))).ReturnsAsync(stream_änderungsdatei);

            var webclientFactoryMock = new Mock<IWebClientFactory>();
            webclientFactoryMock.Setup(factory => factory.CreateWebClient()).Returns(webclientMock.Object);

            var fileSystemMock = new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    { @"c:\data\Bundesbankänderungsdateien\blz_2015_09_07_txt.txt", new MockFileData(
@"100305001Bankhaus Löbbecke                                         10117Berlin                             Bankhaus Löbbecke Berlin   26225LOEBDEBBXXX09043961U000000000
100306001North Channel Bank                                        55118Mainz a Rhein                      North Channel Bank Mainz   26205GENODEF1OGK88000532U000000000
100307001Eurocity Bank                                             60311Frankfurt am Main                  Eurocity Bank              26535DLGHDEB1XXX16044339U000000000", Encoding.GetEncoding(1252)) }
                });

            //_service = new BankleitzahlenänderungsdateiService(fileSystemMock);
            _service = new BankleitzahlenänderungsdateiService();
            _service.WebClientFactory = webclientFactoryMock.Object;
            _service.FileSystem = fileSystemMock;
        }

        [Test]
        public async void Bundesbank_BankleitzahlenänderungsdateiService_LadeUris()
        {
            var uris = await _service.LadeUris();
            Assert.That(uris.Dateien.Count(), Is.EqualTo(2));

            Assert.That(uris.Dateien.First().Dateiname, Is.EqualTo("blz_2015_06_08_txt.txt"));
            Assert.That(uris.Dateien.First().Uri, Is.EqualTo(new Uri("https://www.bundesbank.de/Redaktion/DE/Downloads/Aufgaben/Unbarer_Zahlungsverkehr/Bankleitzahlen/2015_09_06/blz_2015_06_08_txt.txt?__blob=publicationFile")));

            Assert.That(uris.Dateien.Last().Dateiname, Is.EqualTo("blz_2015_09_07_txt.txt"));
            Assert.That(uris.Dateien.Last().Uri, Is.EqualTo(new Uri("https://www.bundesbank.de/Redaktion/DE/Downloads/Aufgaben/Unbarer_Zahlungsverkehr/Bankleitzahlen/2015_12_06/blz_2015_09_07_txt.txt?__blob=publicationFile")));
        }

        [Test]
        public async void Bundesbank_BankleitzahlenänderungsdateiService_LadeBankenAusDateiÜbersGemockteWeb()
        {
            var änderungsdatei = new Bankleitzahlenänderungsdatei
            {
                Uri = new Uri("https://foo.bla/blz_2015_09_07_txt.txt")
            };

            var banken = await _service.LadeBankenAusÄnderungsdatei(änderungsdatei);

            Assert.That(banken.Count(), Is.EqualTo(18424));

            var bundesbank = banken.First();
            Assert.That((int)bundesbank.Bankleitzahl, Is.EqualTo(10000000));
            Assert.That(bundesbank.Name, Is.EqualTo("Bundesbank"));
            Assert.That(bundesbank.Adresse.City, Is.EqualTo("Berlin"));
            Assert.That(bundesbank.Adresse.PostalCode, Is.EqualTo("10591"));

            var bankMitUmlaut = banken.ElementAt(17);
            Assert.That((int)bankMitUmlaut.Bankleitzahl, Is.EqualTo(10030500));
            Assert.That(bankMitUmlaut.Name, Is.EqualTo("Bankhaus Löbbecke"));
            Assert.That(bankMitUmlaut.Adresse.City, Is.EqualTo("Berlin"));
            Assert.That(bankMitUmlaut.Adresse.PostalCode, Is.EqualTo("10117"));
        }

        [Test]
        public async void Bundesbank_BankleitzahlenänderungsdateiService_LadeBankenAusLokalerDatei()
        {
            var änderungsdatei = new Bankleitzahlenänderungsdatei
            {
                Uri = new Uri(@"c:\data\Bundesbankänderungsdateien\blz_2015_09_07_txt.txt")
            };

            var banken = await _service.LadeBankenAusÄnderungsdatei(änderungsdatei);

            Assert.That(banken.Count(), Is.EqualTo(3));

            var bank = banken.First();
            Assert.That((int)bank.Bankleitzahl, Is.EqualTo(10030500));
            Assert.That(bank.Name, Is.EqualTo("Bankhaus Löbbecke"));
            Assert.That(bank.Adresse.City, Is.EqualTo("Berlin"));
            Assert.That(bank.Adresse.PostalCode, Is.EqualTo("10117"));
        }
    }
}
