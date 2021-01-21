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
    /// Interaction logic for ProductCategoryView.xaml
    /// </summary>
    public partial class ProductCategoryView : UserControl
    {
        private readonly Context db = new Context();
        public ProductCategoryView()
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
            ProductCategoryTable.ItemsSource = db.ProductCategories.ToList();
        }

        public void AddItemsBySearch()
        {
            ProductCategoryTable.ItemsSource = db.ProductCategories.Where(e => e.Name.Contains(Search.Text)).ToList();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            Initialize();
        }

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            ProductCategoryEditView.IsEnabled = true;
            ProductCategoryEditView.Visibility = Visibility.Visible;
            ProductCategoryEditView.Initialize();
            EditHeight.Height = new GridLength(120);
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (ProductCategoryTable.SelectedIndex >= 0)
            {
                ProductCase Item = (dynamic)ProductCategoryTable.SelectedItem;
                ProductCategoryEditView.IsEnabled = true;
                ProductCategoryEditView.Visibility = Visibility.Visible;
                ProductCategoryEditView.Initialize(Item);
                EditHeight.Height = new GridLength(120);
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure to delete selected data?", "Требуется подстверждение!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes && ProductCategoryTable.SelectedIndex >= 0)
            {
                ProductCategory Item = (dynamic)ProductCategoryTable.SelectedItem;
                db.ProductCategories.Remove(db.ProductCategories.Where(e => e.Id == Item.Id).Single());
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
