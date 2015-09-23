using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Bankleitzahlen.GUI.CM.ViewModels
{
    class AboutViewModel
    {
        public BindableCollection<Lizenzen.GenutztesProjekt> GenutzteProjekte { get; private set; } = new BindableCollection<Lizenzen.GenutztesProjekt>();

        public AboutViewModel()
        {
            // TODO: Umstellung auf IoC
            var projekte = new Lizenzen.GenutzteProjekte();
            GenutzteProjekte.AddRange(projekte.Projekte);
        
            var projekteView = CollectionViewSource.GetDefaultView(GenutzteProjekte);
            projekteView.SortDescriptions.Add(
                new SortDescription(nameof(Lizenzen.GenutztesProjekt.Projektname), ListSortDirection.Ascending)
            );
        }

        public void ShowLicense(Lizenzen.GenutztesProjekt projekt)
        {
            // TODO: Umstellung auf IoC
            var windowManager = new WindowManager();
            windowManager.ShowDialog(new LizenzViewModel(projekt));
        }

        public void NavigateTo(Lizenzen.GenutztesProjekt projekt)
        {
            var uri = projekt.Homepage.AbsoluteUri;
            Debug.WriteLine(string.Format("Öffne Webseite von Projekt \"{0}\": {1}", projekt.Projektname, uri));
            Process.Start(new ProcessStartInfo(uri));
        }
    }
}
