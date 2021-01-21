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
    /// Interaction logic for ProductView.xaml
    /// </summary>
    public partial class ProductView : UserControl
    {
        private readonly Context db = new Context();
        public ProductView()
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
            var result = from product in db.Products
                         join category in db.ProductCategories on product.Category equals category.Id
                         select new ProductCase
                         {
                             Id = product.Id,
                             Name = product.Name,
                             Description = product.Description,
                             Price = product.Price,
                             Publisher = product.Publisher,
                             Developer = product.Developer,
                             Category = category.Name
                         };
            ProductTable.ItemsSource = result.ToList();
        }

        public void AddItemsBySearch()
        {
            var result = from product in db.Products
                         join category in db.ProductCategories on product.Category equals category.Id
                         where product.Name.Contains(Search.Text) || product.Developer.Contains(Search.Text) 
                         || product.Publisher.Contains(Search.Text) || category.Name.Contains(Search.Text)
                         select new ProductCase
                         {
                             Id = product.Id,
                             Name = product.Name,
                             Description = product.Description,
                             Price = product.Price,
                             Publisher = product.Publisher,
                             Developer = product.Developer,
                             Category = category.Name
                         };
            ProductTable.ItemsSource = result.ToList();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            Initialize();
        }

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            ProductEditView.IsEnabled = true;
            ProductEditView.Visibility = Visibility.Visible;
            ProductEditView.Initialize();
            EditHeight.Height = new GridLength(210);
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (ProductTable.SelectedIndex >= 0)
            {
                ProductCase Item = (dynamic)ProductTable.SelectedItem;
                ProductEditView.IsEnabled = true;
                ProductEditView.Visibility = Visibility.Visible;
                ProductEditView.Initialize(Item);
                EditHeight.Height = new GridLength(120);
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure to delete selected data?", "Требуется подстверждение!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes && ProductTable.SelectedIndex >= 0)
            {
                ProductCase Item = (dynamic)ProductTable.SelectedItem;
                db.Products.Remove(db.Products.Where(e => e.Id == Item.Id).Single());
                db.SaveChanges();
                Initialize();
            }
        }

        private void ProductEditView_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            EditHeight.Height = new GridLength(0);
            Initialize();
        }
    }
}
