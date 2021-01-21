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
    /// Interaction logic for OrderEditView.xaml
    /// </summary>
    public partial class OrderEditView : UserControl
    {
        readonly Context db = new Context();
        private long SelectedId { get; set; }

        public OrderEditView()
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
                    db.Orders.Add(new Order
                    {
                        Account = db.Accounts.Where(e => e.Login.Equals(Account.SelectedItem)).Single().Id,
                        Date = Date.SelectedDate.Value,
                        Payment = Payment.Text,
                        Price = Convert.ToDecimal(Price.Text)
                    });
                }
                else
                {
                    Order item = db.Orders.Where(e => e.Id == SelectedId).Single();
                    item.Account = db.Accounts.Where(e => e.Login.Equals(Account.SelectedItem)).Single().Id;
                    item.Date = Date.SelectedDate.Value;
                    item.Payment = Payment.Text;
                    item.Price = Convert.ToDecimal(Price.Text);
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
        }

        public void Initialize(OrderCase entity)
        {
            Initialize();
            SelectedId = entity.Id;
            Account.SelectedItem = entity.Account;
            Date.SelectedDate = entity.Date;
            Payment.Text = entity.Payment;
            Price.Text = entity.Price.ToString();
        }

        private void CleanFields()
        {
            Account.SelectedItem = null;
            Date.SelectedDate = null;
            Payment.Text = "";
            Price.Text = "";
        }

        private bool CheckFields()
        {
            if (Account.SelectedItem == null)
            {
                MessageBox.Show("Account mustn't be empty!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (Date.SelectedDate == null)
            {
                MessageBox.Show("Date mustn't be empty!", "Неверные данные!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Regex.IsMatch(Payment.Text, @"\w+"))
            {
                MessageBox.Show("Payment mustn't be empty!", "Неверные данные!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Regex.IsMatch(Price.Text, @"\d+\.\d+"))
            {
                MessageBox.Show("Price must be digital!", "Неверные данные!", MessageBoxButton.OK, MessageBoxImage.Error);
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
