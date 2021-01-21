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
    /// Interaction logic for SignUpView.xaml
    /// </summary>
    public partial class SignUpView : UserControl
    {
        Context db = new Context();
        public SignUpView()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Exit();
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            if (IsChecked())
            {
                try
                {
                    string Sol = MD5Hash.RandomString();
                    db.Accounts.Add(new Account 
                    { 
                        Login = Login.Text, 
                        Email = Email.Text,
                        Fullname = Fullname.Text,
                        Sol = Sol,
                        Password = MD5Hash.GetMd5Hash(Password.Password + Sol),
                        Created = DateTime.Now 
                    });
                    db.SaveChanges();
                    Exit();
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("There's error in registring your account. Please try again!");
                    CleanFields();
                }
            }
        }

        private bool IsChecked()
        {
            if (!Regex.IsMatch(Login.Text, @"\w+"))
            {
                MessageBox.Show("Login mustn't be empty.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Regex.IsMatch(Fullname.Text, @"\w+"))
            {
                MessageBox.Show("Fullname mustn't be empty.", "Fullname Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Regex.IsMatch(Email.Text, RegexData.Email))
            {
                MessageBox.Show("Wrong email format.", "Email Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Regex.IsMatch(Password.Password, RegexData.Password))
            {
                MessageBox.Show("Password must have at least one digit, lower, upper case, special symbol(@#$%^&-+<>=()), no white space and be in range 8-20 symbols.", "Password Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!ConfirmPassword.Password.Equals(Password.Password))
            {
                MessageBox.Show("Confirm Password doesn't math password field.", "Password Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void CleanFields()
        {

        }

        private void Exit()
        {
            Visibility = Visibility.Collapsed;
            IsEnabled = false;
        }
    }
}
