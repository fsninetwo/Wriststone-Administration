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
    /// Interaction logic for RatingView.xaml
    /// </summary>
    public partial class RatingView : UserControl
    {
        private readonly Context db = new Context();
        public RatingView()
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
            var result = from rating in db.Ratings
                         join account in db.Accounts on rating.Account equals account.Id
                         join product in db.Products on rating.Product equals product.Id
                         select new RatingCase
                         {
                             Id = rating.Id,
                             Rate = rating.Rate,
                             Message = rating.Message,
                             Account = account.Login,
                             Product = product.Name
                         };
            RatingTable.ItemsSource = result.ToList();
        }

        public void AddItemsBySearch()
        {
            var result = from rating in db.Ratings
                         join account in db.Accounts on rating.Account equals account.Id
                         join product in db.Products on rating.Product equals product.Id
                         where account.Login.Contains(Search.Text) || account.Login.Contains(Search.Text)
                         select new RatingCase
                         {
                             Id = rating.Id,
                             Rate = rating.Rate,
                             Message = rating.Message,
                             Account = account.Login,
                             Product = product.Name
                         };
            RatingTable.ItemsSource = result.ToList();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            Initialize();
        }

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            RatingEditView.IsEnabled = true;
            RatingEditView.Visibility = Visibility.Visible;
            RatingEditView.Initialize();
            EditHeight.Height = new GridLength(120);
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (RatingTable.SelectedIndex >= 0)
            {
                RatingCase Item = (dynamic)RatingTable.SelectedItem;
                RatingEditView.IsEnabled = true;
                RatingEditView.Visibility = Visibility.Visible;
                RatingEditView.Initialize(Item);
                EditHeight.Height = new GridLength(120);
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure to delete selected data?", "Требуется подстверждение!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes && RatingTable.SelectedIndex >= 0)
            {
                Account Item = (dynamic)RatingTable.SelectedItem;
                db.Ratings.Remove(db.Ratings.Where(e => e.Id == Item.Id).Single());
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
