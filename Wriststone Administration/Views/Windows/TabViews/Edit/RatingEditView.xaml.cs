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
using Wriststone_Administration.DB.ApplicationTables;

namespace Wriststone_Administration.Views.Windows.TabViews.Edit
{
    /// <summary>
    /// Interaction logic for RatingEditView.xaml
    /// </summary>
    public partial class RatingEditView : UserControl
    {
        readonly Context db = new Context();
        private long SelectedId { get; set; }

        public RatingEditView()
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
                    db.Ratings.Add(new Rating
                    {
                        Rate = Convert.ToInt32(Rate.Text),
                        Message = Message.Text,
                        Account = db.Accounts.Where(e => e.Login.Equals(Account.SelectedItem)).Single().Id,
                        Product = db.Products.Where(e => e.Name.Equals(Product.SelectedItem)).Single().Id,
                    });
                }
                else
                {
                    Rating item = db.Ratings.Where(e => e.Id == SelectedId).Single();
                    item.Rate = Convert.ToInt32(Rate.Text);
                    item.Message = Message.Text;
                    item.Account = db.Accounts.Where(e => e.Login.Equals(Account.SelectedItem)).Single().Id;
                    item.Product = db.Products.Where(e => e.Name.Equals(Product.SelectedItem)).Single().Id;
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
            Account.ItemsSource = db.Accounts.Select(e => e.Login).ToList();
            Product.ItemsSource = db.Products.Select(e => e.Name).ToList();
        }

        public void Initialize(RatingCase entity)
        {
            Initialize();
            SelectedId = entity.Id.Value;
            Rate.Text = entity.Rate.ToString();
            Message.Text = entity.Message;
            Account.SelectedItem = entity.Account;
            Product.SelectedItem = entity.Product;
        }

        private void CleanFields()
        {
            Rate.Text = "";
            Message.Text = "";
            Account.SelectedItem = null;
            Product.SelectedItem = null;
        }

        private bool CheckFields()
        {
            if (!Rate.Text.Equals("") && !Regex.IsMatch(Rate.Text, ("\\d+")))
            {
                MessageBox.Show("Login must have a only digits!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (Account.SelectedItem == null)
            {
                MessageBox.Show("Account mustn't be empty!", "Неверные данные!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (Product.SelectedItem == null)
            {
                MessageBox.Show("Product mustn't be empty!", "Неверные данные!", MessageBoxButton.OK, MessageBoxImage.Error);
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
