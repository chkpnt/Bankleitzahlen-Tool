using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bankleitzahlen.Lizenzen;

namespace Bankleitzahlen.GUI.CM.ViewModels
{
    class LizenzViewModel
    {
        #region Properties für den View
        public string Projektname { get; private set; } = "Projekt A";
        public string LizenzText { get; private set; } = "GPL bla bla";
        #endregion

        public LizenzViewModel() {}

        public LizenzViewModel(GenutztesProjekt projekt)
        {
            Projektname = projekt.Projektname;
            LizenzText = projekt.LizenzText;
        }

    }
}
