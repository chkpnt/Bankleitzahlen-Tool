using Bankleitzahlen.Commons;
using Bankleitzahlen.GL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
    /// Interaction logic for BankenView.xaml
    /// </summary>
    public partial class BankenView : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<HighlightableTextBlock> BankenListBoxItems { get; } = new ObservableCollection<HighlightableTextBlock>();

        private string _filter = string.Empty;

        private ICollectionView BankenListBoxCollectionView;

        public string Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Filter)));
                BankenListBoxCollectionView.Refresh();
            }
        }


        public BankenView()
        {
            InitializeComponent();

            BankenListBox.ItemsSource = BankenListBoxItems;

            BankenListBoxCollectionView = CollectionViewSource.GetDefaultView(BankenListBoxItems);
            BankenListBoxCollectionView.Filter = new Predicate<object>(Banken_FilterUndHighlighter);
            BankenListBoxCollectionView.SortDescriptions.Add(
                new SortDescription(nameof(HighlightableTextBlock.Text), ListSortDirection.Ascending)
            );
        }

        private bool Banken_FilterUndHighlighter(object o)
        {
            var highlightableTextBlock = o as HighlightableTextBlock;
            var showBank = highlightableTextBlock.Contains(Filter);
            if (showBank)
            {
                highlightableTextBlock.HighlightText(Filter);
            }
            return showBank;
        }

        public void LadeÄnderungsdatei(Bankleitzahlenänderungsdatei selectedÄnderungsdateiAuswahl)
        {
            Task.Run(() =>
            {
                var bundesbankService = new BundesbankService();
                var banken = bundesbankService.GetBanken(selectedÄnderungsdateiAuswahl).Result;

                var banken_UIElements = from bank in banken
                                        select new HighlightableTextBlock(bank.ToString());

                // Muss im UI-Thread ausgeführt werden:
                Dispatcher.Invoke(() =>
                {
                    BankenListBoxItems.Clear();
                    banken_UIElements.ToList().ForEach(ui => BankenListBoxItems.Add(ui));
                });
            }).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    var hasFileHelperException = false;

                    task.Exception.Flatten().Handle(e =>
                    {
                        if (e.Source == "FileHelpers") hasFileHelperException = true;
                        return true;
                    });

                    if (hasFileHelperException)
                    {
                        MessageBox.Show("Datei konnte nicht geparst werden!");
                    }
                    else
                    {
                        MessageBox.Show("Ein Fehler ist passiert!");
                    }
                }
            });
        }
    }
}
