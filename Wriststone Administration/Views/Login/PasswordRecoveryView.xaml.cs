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
    /// Interaction logic for PasswordRecoveryView.xaml
    /// </summary>
    public partial class PasswordRecoveryView : UserControl
    {
        Context db = new Context();
        public PasswordRecoveryView()
        {
            InitializeComponent();
        }

        private void Recover_Click(object sender, RoutedEventArgs e)
        {
            if (IsChecked())
            {
                try
                {
                    string Sol = MD5Hash.RandomString();
                    Account user = db.Accounts.Where(e => e.Email.Equals(Email.Text)).Single();
                    user.Sol = Sol;
                    user.Password = MD5Hash.GetMd5Hash(Password.Password + Sol);
                    db.SaveChanges();
                    MessageBox.Show("Your password is successfully changed", "Email Error", MessageBoxButton.OK, MessageBoxImage.Information);
                    Exit();
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Your email is invaild. Please try again!", "Email Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    CleanFields();
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Exit();
        }

        private bool IsChecked()
        {
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
