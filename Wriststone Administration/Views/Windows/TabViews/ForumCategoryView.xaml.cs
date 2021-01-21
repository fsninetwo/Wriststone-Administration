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
    /// Interaction logic for ForumCategoryView.xaml
    /// </summary>
    public partial class ForumCategoryView : UserControl
    {
        private readonly Context db = new Context();
        public ForumCategoryView()
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
            /*var result = from category in db.ForumCategories
                         join subcategory in db.ForumCategories on category.Id equals subcategory.Category
                         select new ForumCategoryCase
                         {
                             Id = category.Id,
                             Name = category.Name,
                             Category = subcategory.Name
                         };*/
            ForumCategoryTable.ItemsSource = db.ForumCategories.ToList();
        }

        public void AddItemsBySearch()
        {
            /*var result = from category in db.ForumCategories
                         join subcategory in db.ForumCategories on category.Id equals subcategory.Category
                         where category.Name.Contains(Search.Text)
                         select new ForumCategoryCase 
                         { 
                             Id = category.Id,
                             Name = category.Name,
                             Category = subcategory.Name
                         };*/
            ForumCategoryTable.ItemsSource = db.ForumCategories.Where(e => e.Name.Contains(Search.Text)).ToList();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            Initialize();
        }

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            ForumCategoryEditView.IsEnabled = true;
            ForumCategoryEditView.Visibility = Visibility.Visible;
            ForumCategoryEditView.Initialize();
            EditHeight.Height = new GridLength(120);
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (ForumCategoryTable.SelectedIndex >= 0)
            {
                ForumCategory Item = (dynamic)ForumCategoryTable.SelectedItem;
                ForumCategoryEditView.IsEnabled = true;
                ForumCategoryEditView.Visibility = Visibility.Visible;
                ForumCategoryEditView.Initialize(Item);
                EditHeight.Height = new GridLength(120);
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure to delete selected data?", "Требуется подстверждение!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes && ForumCategoryTable.SelectedIndex >= 0)
            {
                ForumCategory Item = (dynamic)ForumCategoryTable.SelectedItem;
                db.ForumCategories.Remove(db.ForumCategories.Where(e => e.Id == Item.Id).Single());
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
