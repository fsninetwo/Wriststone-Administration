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
    /// Interaction logic for OrderDetailsEditView.xaml
    /// </summary>
    public partial class OrderDetailsEditView : UserControl
    {
        readonly Context db = new Context();
        private long SelectedId { get; set; }

        public OrderDetailsEditView()
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
                    db.OrderDetails.Add(new OrderDetails
                    {
                        Order = Convert.ToInt64(Order.Text),
                        Product = db.Products.Where(e => e.Name.Equals(Product.SelectedItem)).Single().Id,
                        Price = Convert.ToDecimal(Price.Text)
                    });
                }
                else
                {
                    OrderDetails item = db.OrderDetails.Where(e => e.Id == SelectedId).Single();
                    item.Order = Convert.ToInt64(Order.Text);
                    item.Product = db.Products.Where(e => e.Name.Equals(Product.SelectedItem)).Single().Id;
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
            Product.ItemsSource = db.Products.Select(e => e.Name).ToList();
        }

        public void Initialize(OrderDetailsCase entity)
        {
            Initialize();
            SelectedId = entity.Id;
            Order.Text = entity.Order.ToString();
            Product.SelectedItem = db.Products.Where(e => e.Name.Equals(entity.Product)).Single().Name;
        }

        private void CleanFields()
        {
            Order.Text = "";
            Product.SelectedItem = null;
        }

        private bool CheckFields()
        {
            if (!Regex.IsMatch(Order.Text, ("\\d+")))
            {
                MessageBox.Show("Order must be in digits!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Regex.IsMatch(Price.Text, ("\\d+")))
            {
                MessageBox.Show("Price must be in digits!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (Product.SelectedItem == null)
            {
                MessageBox.Show("Product!", "Неверные данные!", MessageBoxButton.OK, MessageBoxImage.Error);
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
