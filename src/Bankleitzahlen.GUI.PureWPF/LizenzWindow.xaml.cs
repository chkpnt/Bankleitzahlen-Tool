using Bankleitzahlen.Lizenzen;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for LizenzWindow.xaml
    /// </summary>
    public partial class LizenzWindow : Window
    {
        public LizenzWindow()
        {
            InitializeComponent();
        }

        public LizenzWindow(GenutztesProjekt projekt) : this()
        {
            this.DataContext = projekt;
        }
    }
}
