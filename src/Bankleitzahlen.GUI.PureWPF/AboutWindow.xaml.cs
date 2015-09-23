using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace Bankleitzahlen.GUI.PureWPF
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public List<Lizenzen.GenutztesProjekt> GenutzteProjekte { get; private set; }

        public AboutWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            var projekte = new Lizenzen.GenutzteProjekte();
            GenutzteProjekte = projekte.Projekte.ToList();

            GenutzteProjekteListBox.ItemContainerStyle = new Style(typeof(ListBoxItem));
            GenutzteProjekteListBox.ItemContainerStyle.Setters.Add(
                new EventSetter
                {
                    Event = MouseDoubleClickEvent,
                    Handler = new MouseButtonEventHandler(GenutzteProjekteListBoxItem_MouseDoubleClick)
                });
        }

        private void GenutzteProjekteListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listBoxItem = sender as ListBoxItem;
            var projekt = listBoxItem?.DataContext as Lizenzen.GenutztesProjekt;

            if (projekt != null)
            {
                var lizenzWindow = new LizenzWindow(projekt);
                lizenzWindow.ShowDialog();
            }
        }

        private void GenutzteProjekteListBoxItem_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var projekt = button?.DataContext as Lizenzen.GenutztesProjekt;

            if (projekt != null)
            {
                var uri = projekt.Homepage.AbsoluteUri;
                Debug.WriteLine(string.Format("Öffne Webseite von Projekt \"{0}\": {1}", projekt.Projektname, uri));
                Process.Start(new ProcessStartInfo(uri));
            }
        }
    }
}
