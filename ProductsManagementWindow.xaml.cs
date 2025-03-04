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
    /// Interaction logic for ProductsManagementWindow.xaml
    /// </summary>
    public partial class ProductsManagementWindow : Window
    {
        public ContextClass db = new ContextClass();
        public User thisUser;
        public ProductsManagementWindow(User u)
        {
            InitializeComponent();
            db.Product.Load();
            db.User.Load();
            db.Category.Load();
            db.Subcategory.Load();
            db.Brand.Load();
            ProductsDG.ItemsSource = db.Product.Local.ToBindingList();
            UserDG.ItemsSource = db.User.Local.ToBindingList();
            CategoriesDG.ItemsSource = db.Category.Local.ToBindingList();
            SubcategoriesDG.ItemsSource = db.Subcategory.Local.ToBindingList();
            BrandDG.ItemsSource = db.Brand.Local.ToBindingList();
            activeButton = productsButton;
            thisUser = u;
            username.Content = thisUser.Name;
        }
        private void User_Settings_Click(object sender, RoutedEventArgs e)
        {
            AddUserWindow w = new AddUserWindow(thisUser, thisUser.UserType)
            {
                Owner = this,
            };
            w.ShowDialog();
        }
        Button activeButton;
        private void CahngeButtonColor(object but)
        {
            activeButton.Background = Brushes.Transparent;
            activeButton = but as Button;
            activeButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#D64774");
        }
        private void Product_Click(object sender, RoutedEventArgs e)
        {
            ProductsTI.IsSelected = true;
            CahngeButtonColor(sender);
        }
        private void User_Click(object sender, RoutedEventArgs e)
        {
            UserTI.IsSelected = true;
            CahngeButtonColor(sender);
        }
        private void Category_Click(object sender, RoutedEventArgs e)
        {
            CahngeButtonColor(sender);
            CategoriesTI.IsSelected = true;
        }
        private void Subcategory_Click(object sender, RoutedEventArgs e)
        {
            CahngeButtonColor(sender);
            SubcategoriesTI.IsSelected = true;
        }
        private void Brand_Click(object sender, RoutedEventArgs e)
        {
            CahngeButtonColor(sender);
            BrandTI.IsSelected = true;
        }
        //------------------------------------------------------------------------------
        private void Add_User_ContextMenu(object sender, RoutedEventArgs e)
        {
            AddUserWindow w = new AddUserWindow(null, false)
            {
                Owner = this,
            };
            w.ShowDialog();
        }
        private void Add_Admin_ContextMenu(object sender, RoutedEventArgs e)
        {
            AddUserWindow w = new AddUserWindow(null, true)
            {
                Owner = this,
            };
            w.ShowDialog();
        }
        private void Updtate_User_Click(object sender, RoutedEventArgs e)
        {
            if (UserDG.SelectedItems.Count > 0)
            {
                User u = UserDG.SelectedItems[0] as User;
                AddUserWindow w = new AddUserWindow(u, u.UserType)
                {
                    Owner = this,
                };
                w.ShowDialog();
            }
        }
        private void Delete_User_Click(object sender, RoutedEventArgs e)
        {
            if (UserDG.SelectedItems.Count > 0)
            {
                int n = UserDG.SelectedItems.Count - 1;
                for (int i = n; i >= 0; i--)
                {
                    User u = UserDG.SelectedItems[0] as User;
                    db.User.Remove(u);
                }
                db.SaveChanges();
            }
        }
        //------------------------------------------------------------------------------
        private void Add_Product_ContextMenu(object sender, RoutedEventArgs e)
        {
            AddProductWindow w = new AddProductWindow(null)
            {
                Owner = this,
            };
            w.ShowDialog();
        }
        private void Updtate_Product_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDG.SelectedItems.Count > 0)
            {
                Product u = ProductsDG.SelectedItems[0] as Product;
                AddProductWindow w = new AddProductWindow(u)
                {
                    Owner = this,
                };
                w.ShowDialog();
            }
        }
        private void Delete_Product_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDG.SelectedItems.Count > 0)
            {
                int n = ProductsDG.SelectedItems.Count - 1;
                for (int i = n; i >= 0; i--)
                {
                    Product u = ProductsDG.SelectedItems[0] as Product;
                    db.Product.Remove(u);
                }
                db.SaveChanges();
            }
        }
        //------------------------------------------------------------------------------
        private void Add_Category_ContextMenu(object sender, RoutedEventArgs e)
        {
            AddCategoryWindow w = new AddCategoryWindow(null)
            {
                Owner = this,
            };
            w.ShowDialog();
        }
        private void Updtate_Category_Click(object sender, RoutedEventArgs e)
        {
            if (CategoriesDG.SelectedItems.Count > 0)
            {
                Category u = CategoriesDG.SelectedItems[0] as Category;
                AddCategoryWindow w = new AddCategoryWindow(u)
                {
                    Owner = this,
                };
                w.ShowDialog();
            }
        }
        private void Delete_Category_Click(object sender, RoutedEventArgs e)
        {
            if (CategoriesDG.SelectedItems.Count > 0)
            {
                int n = CategoriesDG.SelectedItems.Count - 1;
                for (int i = n; i >= 0; i--)
                {
                    Category u = CategoriesDG.SelectedItems[0] as Category;
                    db.Category.Remove(u);
                }
                db.SaveChanges();
            }
        }
        //------------------------------------------------------------------------------
        private void Add_Subcategory_ContextMenu(object sender, RoutedEventArgs e)
        {
            AddSubcategoryWindow w = new AddSubcategoryWindow(null)
            {
                Owner = this,
            };
            w.ShowDialog();
        }
        private void Updtate_Subcategory_Click(object sender, RoutedEventArgs e)
        {
            if (SubcategoriesDG.SelectedItems.Count > 0)
            {
                Subcategory u = SubcategoriesDG.SelectedItems[0] as Subcategory;
                AddSubcategoryWindow w = new AddSubcategoryWindow(u)
                {
                    Owner = this,
                };
                w.ShowDialog();
            }
        }
        private void Delete_Subcategory_Click(object sender, RoutedEventArgs e)
        {
            if (SubcategoriesDG.SelectedItems.Count > 0)
            {
                int n = SubcategoriesDG.SelectedItems.Count - 1;
                for (int i = n; i >= 0; i--)
                {
                    Subcategory u = SubcategoriesDG.SelectedItems[0] as Subcategory;
                    db.Subcategory.Remove(u);
                }
                db.SaveChanges();
            }
        }
        //------------------------------------------------------------------------------
        private void Add_Brand_ContextMenu(object sender, RoutedEventArgs e)
        {
            AddBrandWidow w = new AddBrandWidow(null)
            {
                Owner = this,
            };
            w.ShowDialog();
        }
        private void Updtate_Brand_Click(object sender, RoutedEventArgs e)
        {
            if (SubcategoriesDG.SelectedItems.Count > 0)
            {
                Brand u = BrandDG.SelectedItems[0] as Brand;
                AddBrandWidow w = new AddBrandWidow(u)
                {
                    Owner = this,
                };
                w.ShowDialog();
            }
        }
        private void Delete_Brand_Click(object sender, RoutedEventArgs e)
        {
            if (BrandDG.SelectedItems.Count > 0)
            {
                int n = BrandDG.SelectedItems.Count - 1;
                for (int i = n; i >= 0; i--)
                {
                    Brand u = BrandDG.SelectedItems[0] as Brand;
                    db.Brand.Remove(u);
                }
                db.SaveChanges();
            }
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            db.Dispose();
            LoginWindow log = new LoginWindow();
            log.Show();
            Close();
        }
    }
}
