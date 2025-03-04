using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for DimElectronics.xaml
    /// </summary>
    public partial class DimElectronics : Window
    {
        public ContextClass db = new ContextClass();
        ObservableCollection<ProductView> ProductsList = new ObservableCollection<ProductView>();
        ObservableCollection<ProductView> CartList = new ObservableCollection<ProductView>();
        public User thisUser;
        public DimElectronics(User u)
        {
            InitializeComponent();
            db.Category.Load();
            db.Subcategory.Load();
            db.Product.Load();
            db.User.Load();
            db.Favorite.Load();
            db.Cart.Load();
            sideMenu.ItemsSource = db.Category.Local.ToBindingList();
            thisUser = u;
            ContextClass context = new ContextClass();
            foreach (Product product in db.Product)
            {
                Subcategory subCat = context.Subcategory.FirstOrDefault(first => first.SubcategoryId == product.SubcategoryId);
                string fav = "FavoriteOutline";
                if (context.Favorite.FirstOrDefault(first => first.ProductId == product.ProductId && first.UserId == thisUser.UserId) != null)
                    fav = "Favorite";
                string cart = "CartOutline";
                if (context.Cart.FirstOrDefault(first => first.ProductId == product.ProductId && first.UserId == thisUser.UserId) != null)
                    cart = "Cart";
                ProductsList.Add(new ProductView
                {
                    Id = product.ProductId,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Image = product.Image,
                    Brand = context.Brand.FirstOrDefault(first => first.BrandId == product.BrandId).Name,
                    Subcategory = subCat.Name,
                    Category = context.Category.FirstOrDefault(first => first.CategoryId == subCat.CategoryId).Name,
                    KindFavorite = fav,
                    KindCart = cart,
                });
            }
            productsListBox.ItemsSource = ProductsList;
            cartListBox.ItemsSource = CartList;
            CartList.CollectionChanged += CartList_CollectionChanged;
        }

        private void CartList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            decimal s = 0;
            foreach(ProductView prod in CartList)
            {
                s += prod.Price;
            }
            totalPrice.Content = s;
        }

        private void Side_Menu_Click(object sender, RoutedEventArgs e)
        {
            ProductsTI.IsSelected = true;
            sideMenuBuitton.Visibility = Visibility.Collapsed;
            opacityBorder.Visibility = sideMenuBuitton1.Visibility = Visibility.Visible;
            CahngeButtonColor(sideMenuBuitton1);
        }
        private void Side_Menu_Click1(object sender, RoutedEventArgs e)
        {
            ProductsTI.IsSelected = true;
            sideMenuSubcategories.Visibility = opacityBorder.Visibility = sideMenuBuitton1.Visibility = Visibility.Collapsed;
            sideMenuBuitton.Visibility = Visibility.Visible;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sideMenuSubcategories.Visibility == Visibility.Collapsed)
                sideMenuSubcategories.Visibility = Visibility.Visible;
            int idCategory = Convert.ToInt32(((DockPanel)((Button)sender).Parent).Children.OfType<Label>().FirstOrDefault().Content);
            sideMenuSubcategories.ItemsSource = null;
            sideMenuSubcategories.Items.Clear();
            sideMenuSubcategories.ItemsSource = db.Subcategory.Local.ToBindingList().Where(w => w.CategoryId == idCategory);
        }
        private void sideMenuSubcategories_MouseLeave(object sender, MouseEventArgs e)
        {
            sideMenuSubcategories.Visibility = Visibility.Collapsed;
        }
        Subcategory selectedSubcategory;
        Category selectedCategory;
        private void Subcategory_Click(object sender, RoutedEventArgs e)
        {
            int idSubcategory = Convert.ToInt32(((DockPanel)((Button)sender).Parent).Children.OfType<Label>().FirstOrDefault().Content);
            selectedSubcategory = db.Subcategory.FirstOrDefault(f => f.SubcategoryId == idSubcategory);
            productsListBox.ItemsSource = null;
            productsListBox.ItemsSource = ProductsList.Where(w => w.Subcategory == selectedSubcategory.Name);
            sideMenuSubcategories.Visibility = Visibility.Collapsed;
            filt = 3;
        }
        private void Category_Click(object sender, RoutedEventArgs e)
        {
            int idCategory = Convert.ToInt32(((DockPanel)((Button)sender).Parent).Children.OfType<Label>().FirstOrDefault().Content);
            selectedCategory = db.Category.FirstOrDefault(f => f.CategoryId == idCategory);
            productsListBox.ItemsSource = null;
            productsListBox.ItemsSource = ProductsList.Where(w => w.Category == selectedCategory.Name);
            filt = 2;
        }

        private void AddProductInCart_Click(object sender, RoutedEventArgs e)
        {
            int idProduct = Convert.ToInt32(((((Button)sender).Parent as DockPanel).Parent as DockPanel).Children.OfType<Label>().FirstOrDefault().Content);
            Cart cart = db.Cart.FirstOrDefault(f => f.ProductId == idProduct && f.UserId == thisUser.UserId);
            ProductView prod = ProductsList.FirstOrDefault(f => f.Id == idProduct);
            if (cart == null)
            {
                db.Cart.Add(new Cart
                {
                    UserId = thisUser.UserId,
                    ProductId = idProduct,
                });
                prod.KindCart = "Cart";
            }
            else
            {
                db.Cart.Remove(cart);
                prod.KindCart = "CartOutline";
            }
            db.SaveChanges();
            ShowProducts();
        }
        private void ShowProducts()
        {
            productsListBox.ItemsSource = null;
            switch (filt)
            {
                case 0:
                    {
                        productsListBox.ItemsSource = ProductsList;
                    }
                    break;
                case 1:
                    {
                        productsListBox.ItemsSource = ProductsList.Where(w => w.Name.Contains(searchTextBox.Text) || w.Description.Contains(searchTextBox.Text) || w.Brand.Contains(searchTextBox.Text));
                    }
                    break;
                case 2:
                    {
                        productsListBox.ItemsSource = ProductsList.Where(w => w.Category == selectedCategory.Name);
                    }
                    break;
                case 3:
                    {
                        productsListBox.ItemsSource = ProductsList.Where(w => w.Subcategory == selectedSubcategory.Name);
                    }
                    break;
            }
        }
        private void AddProductToFavorite_Click(object sender, RoutedEventArgs e)
        {
            int idProduct = Convert.ToInt32(((((Button)sender).Parent as DockPanel).Parent as DockPanel).Children.OfType<Label>().FirstOrDefault().Content);
            Favorite fav = db.Favorite.FirstOrDefault(f => f.ProductId == idProduct && f.UserId == thisUser.UserId);
            if (fav == null)
            {
                db.Favorite.Add(new Favorite
                {
                    UserId = thisUser.UserId,
                    ProductId = idProduct,
                });
                ProductsList.FirstOrDefault(f => f.Id == idProduct).KindFavorite = "Favorite";
            }
            else
            {
                db.Favorite.Remove(fav);
                ProductsList.FirstOrDefault(f => f.Id == idProduct).KindFavorite = "FavoriteOutline";
            }
            db.SaveChanges();
            ShowProducts();
        }
        Button activeButton;
        private void CahngeButtonColor(object but)
        {
            if (activeButton != null)
            activeButton.Background = Brushes.Transparent;
            activeButton = but as Button;
            activeButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#D64774");
        }
        private void Cart_Click(object sender, RoutedEventArgs e)
        {
            CartTI.IsSelected = true;
            CartList.Clear();
            foreach (ProductView prod in ProductsList.Where(w => w.KindCart == "Cart"))
            {
                CartList.Add(prod);
            }
            CahngeButtonColor(sender);
        }

        private void Favorite_Click(object sender, RoutedEventArgs e)
        {
            FavoritesTI.IsSelected = true;
            favoritesListBox.ItemsSource = null;
            favoritesListBox.ItemsSource = ProductsList.Where(w => w.KindFavorite == "Favorite");
            CahngeButtonColor(sender);
        }

        private void opacityBorder_Click(object sender, RoutedEventArgs e)
        {
            sideMenuBuitton.Visibility = Visibility.Visible;
            sideMenuBuitton1.Visibility = Visibility.Collapsed;
            opacityBorder.Visibility = Visibility.Collapsed;
        }
        int filt = 0;
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            ProductsTI.IsSelected = true;
            productsListBox.ItemsSource = null;

            if (searchTextBox.Text == "")
            {
                productsListBox.ItemsSource = ProductsList;
                filt = 0;
            }
            else
            {
                filt = 1;
                productsListBox.ItemsSource = ProductsList.Where(w => w.Name.Contains(searchTextBox.Text) || w.Description.Contains(searchTextBox.Text) || w.Brand.Contains(searchTextBox.Text));
            }

            if (activeButton != null)
            {
                activeButton.Background = Brushes.Transparent;
            }
        }

        private void AddProductInCartFavorite_Click(object sender, RoutedEventArgs e)
        {
            int idProduct = Convert.ToInt32(((((Button)sender).Parent as DockPanel).Parent as DockPanel).Children.OfType<Label>().FirstOrDefault().Content);
            Cart cart = db.Cart.FirstOrDefault(f => f.ProductId == idProduct && f.UserId == thisUser.UserId);
            ProductView prod = ProductsList.FirstOrDefault(f => f.Id == idProduct);
            if (cart == null)
            {
                db.Cart.Add(new Cart
                {
                    UserId = thisUser.UserId,
                    ProductId = idProduct,
                });
                prod.KindCart = "Cart";
            }
            else
            {
                db.Cart.Remove(cart);
                prod.KindCart = "CartOutline";
            }
            db.SaveChanges();
            favoritesListBox.ItemsSource = null;
            favoritesListBox.ItemsSource = ProductsList.Where(w => w.KindFavorite == "Favorite");
        }

        private void Delete_From_Favorite_Click(object sender, RoutedEventArgs e)
        {
            int idProduct = Convert.ToInt32(((((Button)sender).Parent as DockPanel).Parent as DockPanel).Children.OfType<Label>().FirstOrDefault().Content);
            Favorite fav = db.Favorite.FirstOrDefault(f => f.ProductId == idProduct && f.UserId == thisUser.UserId);
            db.Favorite.Remove(fav);
            ProductsList.FirstOrDefault(f => f.Id == idProduct).KindFavorite = "FavoriteOutline";
            db.SaveChanges();
            favoritesListBox.ItemsSource = null;
            favoritesListBox.ItemsSource = ProductsList.Where(w => w.KindFavorite == "Favorite");
        }

        private void Delete_From_Cart_Click(object sender, RoutedEventArgs e)
        {
            int idProduct = Convert.ToInt32((((Button)sender).Parent as DockPanel).Children.OfType<Label>().FirstOrDefault().Content);
            Cart cart = db.Cart.FirstOrDefault(f => f.ProductId == idProduct && f.UserId == thisUser.UserId);
            ProductView prod = ProductsList.FirstOrDefault(f => f.Id == idProduct);
            db.Cart.Remove(cart);
            prod.KindCart = "CartOutline";
            CartList.Remove(prod);
            db.SaveChanges();
        }

        private void LogOut_CLick(object sender, RoutedEventArgs e)
        {
            db.Dispose();
            LoginWindow log = new LoginWindow();
            log.Show();
            Close();
        }
        public bool confirmshopping = false;
        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            if (CartList.Count > 0)
            {
                Confirm confirm = new Confirm(totalPrice.Content.ToString()) { Owner = this };
                confirm.ShowDialog();
                if (confirmshopping)
                {
                    foreach (ProductView prod in CartList)
                    {
                        db.Sale.Add(new Sale
                        {
                            ProductId = prod.Id,
                            UserId = thisUser.UserId,
                        });
                    }
                    db.SaveChanges();
                    CartList.Clear();
                }
                confirmshopping = false;
            }
        }

        private void User_Update_Click(object sender, RoutedEventArgs e)
        {
            AddUserWindow w = new AddUserWindow(thisUser,thisUser.UserType)
            {
                Owner = this
            };
            w.ShowDialog();
        }

        private void userInfo_MouseLeave(object sender, MouseEventArgs e)
        {
            userInfo.Visibility = Visibility.Collapsed;
        }

        private void User_Click(object sender, RoutedEventArgs e)
        {
            if (userInfo.Visibility == Visibility.Collapsed)
            {
                userInfo.Visibility = Visibility.Visible;
            }
            else
            {
                userInfo.Visibility = Visibility.Collapsed;
            }
        }
    }
}
