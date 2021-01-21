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
    /// Interaction logic for OrderDetailsView.xaml
    /// </summary>
    public partial class OrderDetailsView : UserControl
    {
        private readonly Context db = new Context();
        public OrderDetailsView()
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
            var result = from orderdetails in db.OrderDetails
                         join product in db.Products on orderdetails.Product equals product.Id
                         select new OrderDetailsCase
                         {
                             Id = orderdetails.Id,
                             Order = orderdetails.Order,
                             Product = product.Name,
                             Price = orderdetails.Price
                         };
            OrderDetailsTable.ItemsSource = result.ToList();
        }

        public void AddItemsBySearch()
        {
            var result = from orderdetails in db.OrderDetails
                         join product in db.Products on orderdetails.Product equals product.Id
                         where product.Name.Contains(Search.Text)
                         select new OrderDetailsCase
                         {
                             Id = orderdetails.Id,
                             Order = orderdetails.Order,
                             Product = product.Name,
                         };
            OrderDetailsTable.ItemsSource = result.ToList();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            Initialize();
        }

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            OrderDetailsEditView.IsEnabled = true;
            OrderDetailsEditView.Visibility = Visibility.Visible;
            OrderDetailsEditView.Initialize();
            EditHeight.Height = new GridLength(120);
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (OrderDetailsTable.SelectedIndex >= 0)
            {
                OrderDetailsCase Item = (dynamic)OrderDetailsTable.SelectedItem;
                OrderDetailsEditView.IsEnabled = true;
                OrderDetailsEditView.Visibility = Visibility.Visible;
                OrderDetailsEditView.Initialize(Item);
                EditHeight.Height = new GridLength(120);
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure to delete selected data?", "Требуется подстверждение!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes && OrderDetailsTable.SelectedIndex >= 0)
            {
                Account Item = (dynamic)OrderDetailsTable.SelectedItem;
                db.OrderDetails.Remove(db.OrderDetails.Where(e => e.Id == Item.Id).Single());
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
