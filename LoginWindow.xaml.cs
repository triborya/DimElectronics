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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public ContextClass db = new ContextClass();
        public LoginWindow()
        {
            InitializeComponent();
            db.User.Load();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddUserWindow w = new AddUserWindow(null, false)
            {
                Owner = this,
            };
            w.ShowDialog();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            User u = db.User.FirstOrDefault(f => f.Email == email.Text && f.Password == password.Password);
            if (u != null)
            {
                if (u.UserType)
                {
                    ProductsManagementWindow w = new ProductsManagementWindow(u);
                    w.Show();
                }
                else
                {
                    DimElectronics w = new DimElectronics(u);
                    w.Show();
                }
                Close();
            }
            else
            {
                (email.Parent as Border).Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#d16989");
                (password.Parent as Border).Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#d16989");
                password.Password = "";
                isValid = false;
            }
        }
        bool isValid = true;
        private void email_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!isValid)
            {
                (email.Parent as Border).Background = Brushes.White;
                (password.Parent as Border).Background = Brushes.White;
                isValid = true;
            }
        }

        private void password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!isValid)
            {
                (email.Parent as Border).Background = Brushes.White;
                (password.Parent as Border).Background = Brushes.White;
                isValid = true;
            }
        }
    }
}
