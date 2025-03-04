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
    /// Interaction logic for AddSubcategoryWindow.xaml
    /// </summary>
    public partial class AddSubcategoryWindow : Window
    {
        ContextClass db = new ContextClass();
        Subcategory subcategory = null;
        public AddSubcategoryWindow(Subcategory u)
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
            if (u != null)
            {
                subcategory = u;
                Name.Text = u.Name;
                Category.SelectedValue = db.Category.FirstOrDefault(first => first.CategoryId == u.CategoryId).Name;
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
                if (subcategory != null)
                {

                    Subcategory u = owner.db.Subcategory.FirstOrDefault(first => first.SubcategoryId == subcategory.SubcategoryId);
                    u.Name = Name.Text;
                    u.CategoryId = db.Category.FirstOrDefault(first => first.Name == Category.SelectedValue.ToString()).CategoryId;
                    owner.SubcategoriesDG.ItemsSource = null;
                    owner.SubcategoriesDG.ItemsSource = owner.db.Subcategory.Local.ToBindingList();
                }
                else
                {
                    owner.db.Subcategory.Add(new Subcategory
                    {
                        Name = Name.Text,
                        CategoryId = db.Category.FirstOrDefault(first => first.Name == Category.SelectedValue.ToString()).CategoryId,
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
