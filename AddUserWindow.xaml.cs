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
    /// Interaction logic for AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        ContextClass db = new ContextClass();
        User user;
        bool UserType;
        string EmailUpdate = "";
        public AddUserWindow(User u, bool t)
        {
            InitializeComponent();
            UserType = t;
            user = u;
            if (u != null)
            {
                Name.Text = u.Name;
                Phone.Text = u.Phone;
                EmailUpdate = Email.Text = u.Email;
                Address.Text = u.Address;
                Password.Password = u.Password;
                ConfirmPassword.Password = u.Password;
                Feminin.IsChecked = u.Gender == "Feminin";
                add.Content = "Salvează modificările";
            }
            db.User.Load();
            Closing += AddUserWindow_Closing;
        }

        private void AddUserWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db.Dispose();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool isOk = true;
            if (Name.Text == "" || !new Regex(@"^([A-Z][a-z]+)((\s)+([A-Z][a-z]+)((\s)+)?)*$").IsMatch(Name.Text))
            {
                isOk = false;
                (Name.Parent as Border).Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#d16989");
            }
            if (Phone.Text == "" || !new Regex(@"^[0][0-9]{8}$").IsMatch(Phone.Text))
            {
                isOk = false;
                (Phone.Parent as Border).Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#d16989");
            }
            if (Password.Password == "" || Password.Password.Length < 8)
            {
                isOk = false;
                (Password.Parent as Border).Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#d16989");
            }
            if (ConfirmPassword.Password == "" || ConfirmPassword.Password != Password.Password)
            {
                isOk = false;
                (ConfirmPassword.Parent as Border).Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#d16989");
            }
            if (Email.Text == "" || !new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$").IsMatch(Email.Text))
            {
                isOk = false;
                (Email.Parent as Border).Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#d16989");
            }
            else
            {
                if (EmailUpdate != Email.Text)
                    if (db.User.FirstOrDefault(first => first.Email == Email.Text) != null)
                    {
                        isOk = false;
                        (Email.Parent as Border).Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#d16989");
                        MessageBox.Show("Email deja existant", "Email", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
            }
            if (isOk)
            {
                if (user != null)
                {
                    if (Owner is ProductsManagementWindow)
                    {
                        ProductsManagementWindow owner = Owner as ProductsManagementWindow;
                        User u = owner.db.User.FirstOrDefault(first => first.UserId == user.UserId);
                        string gender = "Masculin";
                        if (Feminin.IsChecked == true) gender = "Feminin";
                        u.Name = Name.Text;
                        u.Gender = gender;
                        u.Phone = Phone.Text;
                        u.Address = Address.Text;
                        u.Email = Email.Text;
                        u.Password = Password.Password;
                        u.UserType = UserType;
                        owner.db.SaveChanges();
                        owner.UserDG.ItemsSource = null;
                        owner.UserDG.ItemsSource = owner.db.User.Local.ToBindingList();
                        if (u.UserId == owner.thisUser.UserId)
                        {
                            owner.username.Content = u.Name;
                        }
                    }
                    else
                    {
                        DimElectronics owner = Owner as DimElectronics;
                        User u = owner.db.User.FirstOrDefault(first => first.UserId == user.UserId);
                        string gender = "Masculin";
                        if (Feminin.IsChecked == true) gender = "Feminin";
                        u.Name = Name.Text;
                        u.Gender = gender;
                        u.Phone = Phone.Text;
                        u.Address = Address.Text;
                        u.Email = Email.Text;
                        u.Password = Password.Password;
                        u.UserType = UserType;
                        owner.db.SaveChanges();
                        if (u.UserId == owner.thisUser.UserId)
                        {
                            owner.userName.Content = u.Name;
                        }
                    }
                }
                else
                {
                    if (Owner is ProductsManagementWindow)
                    {
                        ProductsManagementWindow owner = Owner as ProductsManagementWindow;
                        string gender = "Masculin";
                        if (Feminin.IsChecked == true) gender = "Feminin";

                        owner.db.User.Add(new User
                        {
                            Name = Name.Text,
                            Gender = gender,
                            Phone = Phone.Text,
                            Address = Address.Text,
                            Email = Email.Text,
                            Password = Password.Password,
                            UserType = UserType,
                        });
                        owner.db.SaveChanges();
                    }
                    else
                    {
                        LoginWindow owner = Owner as LoginWindow;
                        string gender = "Masculin";
                        if (Feminin.IsChecked == true) gender = "Feminin";
                        owner.db.User.Add(new User
                        {
                            Name = Name.Text,
                            Gender = gender,
                            Phone = Phone.Text,
                            Address = Address.Text,
                            Email = Email.Text,
                            Password = Password.Password,
                            UserType = UserType,
                        });
                        owner.db.SaveChanges();
                    }
                }
                db.Dispose();
                Close();
            }
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            ((sender as TextBox).Parent as Border).Background = Brushes.White;
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            ((sender as PasswordBox).Parent as Border).Background = Brushes.White;
        }
    }
}
