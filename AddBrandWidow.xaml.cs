using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Interaction logic for AddBrandWidow.xaml
    /// </summary>
    public partial class AddBrandWidow : Window
    {
        ContextClass db = new ContextClass();
        Brand brand = null;
        public AddBrandWidow(Brand u)
        {
            InitializeComponent();
            if (u != null)
            {
                brand = u;
                Name.Text = u.Name;
                add.Content = "Salvează modificările";
            }
            Closing += AddCategoryWindow_Closing;
        }

        private void AddCategoryWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db.Dispose();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool isOk = true;
            if (Name.Text == "")
            {
                isOk = false;
                (Name.Parent as Border).Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#d16989");
            }
            if (isOk)
            {
                ProductsManagementWindow owner = Owner as ProductsManagementWindow;
                if (brand != null)
                {

                    Brand u = owner.db.Brand.FirstOrDefault(first => first.BrandId == brand.BrandId);
                    u.Name = Name.Text;
                    owner.BrandDG.ItemsSource = null;
                    owner.BrandDG.ItemsSource = owner.db.Brand.Local.ToBindingList();
                }
                else
                {
                    owner.db.Brand.Add(new Brand
                    {
                        Name = Name.Text,
                    });
                }
                owner.db.SaveChanges();
                Close();
            }
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            ((sender as TextBox).Parent as Border).Background = Brushes.White;
        }
    }
}
