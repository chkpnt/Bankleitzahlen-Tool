using Bankleitzahlen.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
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
        private static readonly Uri BankleitzahlenänderungsdateiUri = new Uri("https://www.bundesbank.de/Redaktion/DE/Standardartikel/Aufgaben/Unbarer_Zahlungsverkehr/bankleitzahlen_download.html");

        private static readonly Uri BankleitzahlenänderungsdateiUri_Base = new Uri("https://www.bundesbank.de");

        private static readonly string UriPrefix = "/Redaktion/DE/Downloads/Aufgaben/Unbarer_Zahlungsverkehr/Bankleitzahlen";

        private static readonly string UriSuffix = "?__blob=publicationFile";

        public IWebClientFactory WebClientFactory { private get; set; } = new WebClientFactory();

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
                    Uris = from datei in dateien
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
                          select new Bankleitzahlenänderungsdatei()
                          {
                              Dateiname = Path.GetFileName(href.Replace(UriSuffix, "")),
                              Uri = new Uri(BankleitzahlenänderungsdateiUri_Base, href)
                          };

            return dateien;
        }
    }
}
