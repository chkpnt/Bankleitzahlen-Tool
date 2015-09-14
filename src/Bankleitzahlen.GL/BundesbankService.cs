using Bankleitzahlen.Bundesbank;
using Bankleitzahlen.Commons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankleitzahlen.GL
{
    public class BundesbankService
    {
        public IBankleitzahlenänderungsdateiService BankleitzahlenänderungsdateiService { private get; set; } = new BankleitzahlenänderungsdateiService();

        public async Task<Bankleitzahlenänderungsdateien> GetBankleitzahlenänderungsdateien() => await BankleitzahlenänderungsdateiService.LadeUris();
    }
}
