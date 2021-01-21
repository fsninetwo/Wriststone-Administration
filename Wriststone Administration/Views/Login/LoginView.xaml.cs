using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wriststone.Models.Table;
using Wriststone_Administration.Cache;
using Wriststone_Administration.DB;

namespace Wriststone_Administration.Views.Login
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        private readonly Context db = new Context();
        public LoginView()
        {
            InitializeComponent();
        }

        public string Parameter { get; set; }

        private void LoginBox_Click(object sender, RoutedEventArgs e)
        {
            if(IsChecked())
            {
                try
                {
                    string Sol = db.Accounts.Where(e => e.Login.Equals(Login.Text)).Single().Sol;
                    Account user = db.Accounts.Where(e => e.Login.Equals(Login.Text) && e.Password.Equals(MD5Hash.GetMd5Hash(Password.Password + Sol))).Single();
                    Parameter = "MainWindow";
                    Exit();
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Your login and password is invaild. Please try again!");
                    CleanFields();
                }    
            }
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            Parameter = "SignUp";
            CleanFields();
            Exit();
        }

        private bool IsChecked()
        {
            if(!Regex.IsMatch(Password.Password, RegexData.Password))
            {
                MessageBox.Show("Password must have at least one digit, lower, upper case, special symbol(@#$%^&-+<>=()), no white space and be in range 8-20 symbols.", "Password Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void CleanFields()
        {
            Login.Text = "";
            Password.Password = "";
        }

        private void Exit()
        {
            Visibility = Visibility.Collapsed;
            IsEnabled = false;
        }

        private void RememberPassword_Click(object sender, RoutedEventArgs e)
        {
            Parameter = "Recover";
            CleanFields();
            Exit();
        }
    }
}
