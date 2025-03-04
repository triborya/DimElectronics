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

namespace Magazin_de_Electronice
{
    /// <summary>
    /// Interaction logic for Confirm.xaml
    /// </summary>
    public partial class Confirm : Window
    {
        public Confirm(string p)
        {
            InitializeComponent();
            price.Content = p;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DimElectronics owner = Owner as DimElectronics;
            owner.confirmshopping = true;
            Close();
        }
    }
}
