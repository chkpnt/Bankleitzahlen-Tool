using Bankleitzahlen.Commons;
using Bankleitzahlen.GL;
using Caliburn.Micro;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bankleitzahlen.GUI.CM.ViewModels
{
    class MainViewModel : PropertyChangedBase
    {
        #region Properties für den View
        public string WindowTitle { get; } = "Bankleitzahlen-Tool: GUI mit Caliburn.Micro";

        public BindableCollection<Bankleitzahlenänderungsdatei> ÄnderungsdateiAuswahl { get; } = new BindableCollection<Bankleitzahlenänderungsdatei>();

        public Bankleitzahlenänderungsdatei SelectedÄnderungsdateiAuswahl
        {
            get { return _selectedÄnderungsdateiAuswahl; }
            set
            {
                _selectedÄnderungsdateiAuswahl = value;
                NotifyOfPropertyChange(() => SelectedÄnderungsdateiAuswahl);

                ÄnderungsdateiUri = _selectedÄnderungsdateiAuswahl?.Uri.AbsoluteUri;
                NotifyOfPropertyChange(() => ÄnderungsdateiUri);
            }
        }

        // Per Konvention mit ÄnderungsdateiAuswahl verbunden :-)
        public bool CanÄnderungsdateiAuswahl => ÄnderungsdateiAuswahl.Any();

        public string ÄnderungsdateiUri { get; private set; } = "";

        public BankenViewModel Banken { get; } = new BankenViewModel();

        #endregion

        #region Actions für den View
        public void AddFromFilesystem()
        {
            var fd = new OpenFileDialog();
            fd.ShowDialog();

            var änderungsdatei = new Bankleitzahlenänderungsdatei { Dateiname = fd.SafeFileName, Uri = new Uri(fd.FileName) };
            ÄnderungsdateiAuswahl.Add(änderungsdatei);

            NotifyOfPropertyChange(() => CanÄnderungsdateiAuswahl);
            SelectedÄnderungsdateiAuswahl = SelectedÄnderungsdateiAuswahl ?? ÄnderungsdateiAuswahl.FirstOrDefault();
        }

        public void ParseWebsite()
        {
            Task.Run(() =>
            {
                var bundesbankService = new BundesbankService();
                var änderungsdateien = bundesbankService.GetBankleitzahlenänderungsdateien().Result.Dateien;

                Execute.OnUIThread(() =>
                {
                    ÄnderungsdateiAuswahl.AddRange(änderungsdateien);
                    NotifyOfPropertyChange(() => CanÄnderungsdateiAuswahl);
                    SelectedÄnderungsdateiAuswahl = SelectedÄnderungsdateiAuswahl ?? ÄnderungsdateiAuswahl.FirstOrDefault();
                });
            }).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    MessageBox.Show("Obala ...");
                }
            });
        }

        public void LadeÄnderungsdatei()
        {
            Banken.LadeÄnderungsdatei(SelectedÄnderungsdateiAuswahl);
        }

        public void OpenAbout()
        {
            var windowManager = new WindowManager();
            windowManager.ShowDialog(new AboutViewModel());
        }
        #endregion

        #region Backing-Fields für Properties
        private Bankleitzahlenänderungsdatei _selectedÄnderungsdateiAuswahl;
        #endregion
    }
}
