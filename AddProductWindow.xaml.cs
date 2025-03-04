using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        ContextClass db = new ContextClass();
        Product product = null;
        int selectedCategory = 1;
        public AddProductWindow(Product u)
        {
            InitializeComponent();
            foreach (Category x in db.Category)
            {
                Category.Items.Add(x.Name);
            }
            if (Category.Items.Count > 0)
            {
                Category.SelectedIndex = 0;
            }
            foreach (Brand x in db.Brand)
            {
                Brand.Items.Add(x.Name);
            }
            if (Brand.Items.Count > 0)
            {
                Brand.SelectedIndex = 0;
            }
            if (u != null)
            {
                product = u;
                Name.Text = u.Name;
                Description.Text = u.Description;
                Brand.SelectedValue = db.Brand.FirstOrDefault(first => first.BrandId == u.BrandId).Name;
                Subcategory s = db.Subcategory.FirstOrDefault(first => first.SubcategoryId == u.SubcategoryId);
                Subcategory.SelectedValue = s.Name;
                Category.SelectedValue = db.Category.FirstOrDefault(first => first.CategoryId == s.CategoryId).Name;
                Price.Text = u.Price.ToString();
                Image.Source = new BitmapImage(new Uri(u.Image));
                add.Content = "Salvează modificările";
            }
            Closing += AddProductWindow_Closing;
        }

        private void AddProductWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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
            if (Description.Text == "")
            {
                isOk = false;
                (Description.Parent as Border).Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#d16989");
            }
            if (!decimal.TryParse(Price.Text, out decimal price))
            {
                isOk = false;
                (Price.Parent as Border).Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#d16989");
            }
            if (isOk)
            {
                ProductsManagementWindow owner = Owner as ProductsManagementWindow;
                if (product != null)
                {
                   
                    Product u = owner.db.Product.FirstOrDefault(first => first.ProductId == product.ProductId);
                    u.Name = Name.Text;
                    u.Description = Description.Text;
                    u.Price = price;
                    u.BrandId = db.Brand.FirstOrDefault(first => first.Name == Brand.SelectedValue.ToString()).BrandId;
                    u.SubcategoryId = db.Subcategory.FirstOrDefault(first => first.Name == Subcategory.SelectedValue.ToString()).SubcategoryId;
                    u.Image = Image.Source.ToString();
                    owner.ProductsDG.ItemsSource = null;
                    owner.ProductsDG.ItemsSource = owner.db.Product.Local.ToBindingList();
                }
                else
                {
                    owner.db.Product.Add(new Product
                    {
                        Name = Name.Text,
                        Description = Description.Text,
                        Price = price,
                        BrandId = db.Brand.FirstOrDefault(first => first.Name == Brand.SelectedValue.ToString()).BrandId,
                        SubcategoryId = db.Subcategory.FirstOrDefault(first => first.Name == Subcategory.SelectedValue.ToString()).SubcategoryId,
                        Image = Image.Source.ToString(),
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            f.ShowDialog();
            if (f.FileName != "")
            {
                Image.Source = new BitmapImage(new Uri(f.FileName));
            }
        }

        private void Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedCategory = db.Category.FirstOrDefault(first => first.Name == Category.SelectedValue.ToString()).CategoryId;
            Subcategory.Items.Clear();
            foreach (Subcategory x in db.Subcategory.Where(w => w.CategoryId == selectedCategory))
            {
                Subcategory.Items.Add(x.Name);
            }
            if (Subcategory.Items.Count > 0)
            {
                Subcategory.SelectedIndex = 0;
            }
        }
    }
}
