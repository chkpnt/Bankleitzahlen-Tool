using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bankleitzahlen.Commons;
using Bankleitzahlen.GL;
using Caliburn.Micro;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows;

namespace Bankleitzahlen.GUI.CM.ViewModels
{
    class BankenViewModel : PropertyChangedBase
    {
        #region Properties für den View
        public BindableCollection<HighlightableTextBlockViewModel> Banken { get; } = new BindableCollection<HighlightableTextBlockViewModel>();

        public string Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                NotifyOfPropertyChange(() => Filter);
                _bankenView.Refresh();
            }
        }
        #endregion

        private ICollectionView _bankenView;

        public BankenViewModel()
        {
            _bankenView = CollectionViewSource.GetDefaultView(Banken);
            _bankenView.Filter = new Predicate<object>(Banken_FilterUndHighlighter);
            _bankenView.SortDescriptions.Add(
                new SortDescription(nameof(HighlightableTextBlockViewModel.Text), ListSortDirection.Ascending)
            );
        }

        private bool Banken_FilterUndHighlighter(object o)
        {
            var highlightableTextBlock = o as HighlightableTextBlockViewModel;
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
                                        select new HighlightableTextBlockViewModel
                                        {
                                            Text = bank.ToString()
                                        };

                Execute.OnUIThread(() =>
                {
                    Banken.Clear();
                    Banken.AddRange(banken_UIElements);
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


        #region Backing-Fields für Properties
        private string _filter = string.Empty;
        #endregion
    }
}
