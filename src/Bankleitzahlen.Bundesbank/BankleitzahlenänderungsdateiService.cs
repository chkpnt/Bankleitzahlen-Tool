using Bankleitzahlen.Commons;
using FileHelpers;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bankleitzahlen.Bundesbank
{
    public interface IBankleitzahlenänderungsdateiService
    {
        Task<Bankleitzahlenänderungsdateien> LadeUris();
    }

    public class BankleitzahlenänderungsdateiService : IBankleitzahlenänderungsdateiService
    {
        public static readonly Uri BankleitzahlenänderungsdateiUri = new Uri("https://www.bundesbank.de/Redaktion/DE/Standardartikel/Aufgaben/Unbarer_Zahlungsverkehr/bankleitzahlen_download.html");

        private static readonly Uri BankleitzahlenänderungsdateiUri_Base = new Uri("https://www.bundesbank.de");

        private static readonly string UriPrefix = "/Redaktion/DE/Downloads/Aufgaben/Unbarer_Zahlungsverkehr/Bankleitzahlen";

        private static readonly string UriSuffix = "?__blob=publicationFile";

        public IWebClientFactory WebClientFactory { private get; set; } = new WebClientFactory();

        public IFileHelperEngine<BankleitzahlenänderungsdateiEintrag> FileHelperFürBankleitzahlenänderungsdatei { private get; set; } = new FileHelperEngine<BankleitzahlenänderungsdateiEintrag>();

        public async Task<Bankleitzahlenänderungsdateien> LadeUris()
        {
            using (var webclient = WebClientFactory.CreateWebClient())
            {
                var stream = await webclient.OpenReadTaskAsync(BankleitzahlenänderungsdateiUri);

                var doc = new HtmlDocument();
                doc.Load(stream);

                var dateien = ParseHtmlForBankleitzahlenänderungsdateien(doc);

                return new Bankleitzahlenänderungsdateien()
                {
                    Dateien = from datei in dateien
                           where datei.Dateiname.EndsWith(".txt")
                           select datei
                };
            }
        }

        private IEnumerable<Bankleitzahlenänderungsdatei> ParseHtmlForBankleitzahlenänderungsdateien(HtmlDocument doc)
        {
            var dateien = from href in (
                                  from link in doc.DocumentNode.SelectNodes("//a[@href]")
                                  select link.Attributes["href"].Value
                            )
                          where href.StartsWith(UriPrefix) && href.EndsWith(UriSuffix)
                          select new Bankleitzahlenänderungsdatei
                          {
                              Dateiname = Path.GetFileName(href.Replace(UriSuffix, "")),
                              Uri = new Uri(BankleitzahlenänderungsdateiUri_Base, href)
                          };

            return dateien;
        }

        public async Task<List<Bank>> LadeBankenAusÄnderungsdatei(Bankleitzahlenänderungsdatei änderungsdatei)
        {
            using (var webclient = WebClientFactory.CreateWebClient())
            {
                var stream = await webclient.OpenReadTaskAsync(änderungsdatei.Uri);

                using (var reader = new StreamReader(stream))
                {
                    var einträge =  FileHelperFürBankleitzahlenänderungsdatei.ReadStream(reader);

                    var result = from eintrag in einträge
                                 select new Bank
                                 {
                                     Name = eintrag.Bezeichnung,
                                     Bankleitzahl = eintrag.Bankleitzahl,
                                     Adresse = new CivicAddress
                                     {
                                         City = eintrag.Ort,
                                         PostalCode = eintrag.Postleitzahl.ToString()
                                     }
                                 };

                    return result.ToList();
                }
            }
        }
    }
}
