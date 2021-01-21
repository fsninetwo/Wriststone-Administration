using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Wriststone_Administration.DB;
using Wriststone_Administration.DB.ApplicationTables;

namespace Wriststone_Administration.Views.Windows.TabViews
{
    /// <summary>
    /// Interaction logic for OrderView.xaml
    /// </summary>
    public partial class OrderView : UserControl
    {
        private readonly Context db = new Context();
        public OrderView()
        {
            InitializeComponent();

        }
        public void Initialize()
        {
            if (Search.Text.Equals("")) AddItems();
            else AddItemsBySearch();
        }

        public void AddItems()
        {
            var result = from order in db.Orders
                         join account in db.Accounts on order.Account equals account.Id
                         select new OrderCase
                         {
                             Id = order.Id,
                             Date = order.Date,
                             Account = account.Login,
                             Payment = order.Payment,
                             Price = order.Price
                         };
            OrderTable.ItemsSource = result.ToList();
        }

        public void AddItemsBySearch()
        {
            var result = from order in db.Orders
                         join account in db.Accounts on order.Account equals account.Id
                         where account.Login.Contains(Search.Text)
                         select new OrderCase
                         {
                             Id = order.Id,
                             Date = order.Date,
                             Account = account.Login,
                             Payment = order.Payment,
                             Price = order.Price
                         };
            OrderTable.ItemsSource = result.ToList();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            Initialize();
        }

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            OrderEditView.IsEnabled = true;
            OrderEditView.Visibility = Visibility.Visible;
            OrderEditView.Initialize();
            EditHeight.Height = new GridLength(120);
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (OrderTable.SelectedIndex >= 0)
            {
                OrderCase Item = (dynamic)OrderTable.SelectedItem;
                OrderEditView.IsEnabled = true;
                OrderEditView.Visibility = Visibility.Visible;
                OrderEditView.Initialize(Item);
                EditHeight.Height = new GridLength(120);
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure to delete selected data?", "Требуется подстверждение!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes && OrderTable.SelectedIndex >= 0)
            {
                OrderCase Item = (dynamic)OrderTable.SelectedItem;
                db.Orders.Remove(db.Orders.Where(e => e.Id == Item.Id).Single());
                db.SaveChanges();
                Initialize();
            }
        }

        private void AccountEditView_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            EditHeight.Height = new GridLength(0);
            Initialize();
        }
    }
}
