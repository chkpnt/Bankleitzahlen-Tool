using Bankleitzahlen.Commons;
using Bankleitzahlen.GL;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bankleitzahlen.GUI.PureWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Bankleitzahlenänderungsdatei> ÄnderungsdateiAuswahl { get; } = new ObservableCollection<Bankleitzahlenänderungsdatei>();

        public bool IstAuswahlMöglich => ÄnderungsdateiAuswahl.Any();

        private Bankleitzahlenänderungsdatei _selectedÄnderungsdateiAuswahl;

        public Bankleitzahlenänderungsdatei SelectedÄnderungsdateiAuswahl
        {
            get { return _selectedÄnderungsdateiAuswahl; }
            set
            {
                _selectedÄnderungsdateiAuswahl = value;
                // mit Hilfsmethode:
                RaisePropertyChanged();

                ÄnderungsdateiUri = _selectedÄnderungsdateiAuswahl?.Uri.AbsoluteUri;
            }
        }

        private string _änderungsdateiUri;
        public string ÄnderungsdateiUri
        {
            get { return _änderungsdateiUri; }
            set
            {
                _änderungsdateiUri = value;
                // Klassisch:
                PropertyChanged(this, new PropertyChangedEventArgs("ÄnderungsdateiUri"));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            AddFromFilesystemMenuItem.Click += AddFromFilesystemMenuItem_Click;
            ParseWebsiteMenuItem.Click += ParseWebsiteMenuItem_Click;

            LadeÄnderungsdateiButton.Click += (o, e) => Banken.LadeÄnderungsdatei(SelectedÄnderungsdateiAuswahl);
        }

        private void AddFromFilesystemMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var fd = new OpenFileDialog();
            fd.ShowDialog();

            var änderungsdatei = new Bankleitzahlenänderungsdatei { Dateiname = fd.SafeFileName, Uri = new Uri(fd.FileName) };
            ÄnderungsdateiAuswahl.Add(änderungsdatei);

            PropertyChanged(this, new PropertyChangedEventArgs(nameof(IstAuswahlMöglich)));
            SelectedÄnderungsdateiAuswahl = SelectedÄnderungsdateiAuswahl ?? ÄnderungsdateiAuswahl.FirstOrDefault();
        }

        private void ParseWebsiteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                var bundesbankService = new BundesbankService();
                var änderungsdateien = bundesbankService.GetBankleitzahlenänderungsdateien().Result.Dateien.ToList();

                // Muss in UI-Thread:
                Dispatcher.Invoke(() =>
                {
                    änderungsdateien.ForEach(datei => ÄnderungsdateiAuswahl.Add(datei));
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(IstAuswahlMöglich)));
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

        // Übers XAML verdrahtet
        private void OpenAbout_Click(object sender, RoutedEventArgs e)
        {
            var about = new AboutWindow();
            about.ShowDialog();
        }

        // Seit C# 5 möglich:
        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            // mit C# 6:
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));

            // davor:
            //if (PropertyChanged != null)
            //{
            //    PropertyChanged(this, new PropertyChangedEventArgs(caller));
            //}
        }
    }
}
