using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Wriststone_Administration.Views.Windows.TabViews.Edit
{
    /// <summary>
    /// Interaction logic for AccountEditView.xaml
    /// </summary>
    public partial class AccountEditView : UserControl
    {
        readonly Context db = new Context();
        private long SelectedId { get; set; }

        public AccountEditView()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (CheckFields())
            {
                string Sol = MD5Hash.RandomString();
                if (SelectedId == 0)
                {
                    db.Accounts.Add(new Account
                    {
                        Login = Login.Text,
                        Fullname = Fullname.Text,
                        Email = Email.Text,
                        Sol = Sol,
                        Password = MD5Hash.GetMd5Hash(Password.Password + Sol)
                    });
                }
                else
                {
                    Account account = db.Accounts.Where(e => e.Id == SelectedId).Single();
                    account.Login = Login.Text;
                    account.Fullname = Fullname.Text;
                    account.Email = Email.Text;
                    account.Sol = Sol;
                    account.Password = MD5Hash.GetMd5Hash(Password.Password + Sol);
                }
                db.SaveChanges();
                Exit();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Exit();
        }

        public void Initialize()
        {
            CleanFields();
        }

        public void Initialize(Account entity)
        {
            Initialize();
            SelectedId = entity.Id;
            Login.Text = entity.Login;
            Fullname.Text = entity.Fullname;
            Email.Text = entity.Email;
        }

        private void CleanFields()
        {
            Login.Text = "";
            Fullname.Text = "";
            Email.Text = "";
            Password.Password = "";
        }

        private bool CheckFields()
        {
            if (!Regex.IsMatch(Login.Text, ("\\w+")))
            {
                MessageBox.Show("Login is empty!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Regex.IsMatch(Fullname.Text, "\\w+"))
            {
                MessageBox.Show("Fullname is empty!", "Неверные данные!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Regex.IsMatch(Email.Text, RegexData.Email))
            {
                MessageBox.Show("Enter correct email name!", "Неверные данные!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Regex.IsMatch(Password.Password, RegexData.Password))
            {
                MessageBox.Show("Password must have at least one digit, lower, upper case, special symbol(@#$%^&-+<>=()), no white space and be in range 8-20 symbols!", "Неверные данные!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        private void Exit()
        {
            SelectedId = 0;
            IsEnabled = false;
            Visibility = Visibility.Hidden;
        }
    }
}
