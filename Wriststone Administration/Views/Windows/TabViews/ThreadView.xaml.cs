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
    /// Interaction logic for ThreadView.xaml
    /// </summary>
    public partial class ThreadView : UserControl
    {
        private readonly Context db = new Context();
        public ThreadView()
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
            var result = from thread in db.Threads
                         join account in db.Accounts on thread.Account equals account.Id
                         join category in db.ForumCategories on thread.Category equals category.Id
                         select new ThreadCase
                         {
                             Id = thread.Id,
                             Created = thread.Created,
                             Subject = thread.Subject,
                             Account = account.Login,
                             Category = category.Name,
                         };
            ThreadTable.ItemsSource = result.ToList();
        }

        public void AddItemsBySearch()
        {
            var result = from thread in db.Threads
                         join account in db.Accounts on thread.Account equals account.Id
                         join category in db.ForumCategories on thread.Category equals category.Id
                         where account.Login.Contains(Search.Text) || category.Name.Contains(Search.Text)
                         select new ThreadCase
                         {
                             Id = thread.Id,
                             Created = thread.Created,
                             Subject = thread.Subject,
                             Account = account.Login,
                             Category = category.Name,
                         };
            ThreadTable.ItemsSource = result.ToList();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            Initialize();
        }

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            ThreadEditView.IsEnabled = true;
            ThreadEditView.Visibility = Visibility.Visible;
            ThreadEditView.Initialize();
            EditHeight.Height = new GridLength(120);
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (ThreadTable.SelectedIndex >= 0)
            {
                ThreadCase Item = (dynamic)ThreadTable.SelectedItem;
                ThreadEditView.IsEnabled = true;
                ThreadEditView.Visibility = Visibility.Visible;
                ThreadEditView.Initialize(Item);
                EditHeight.Height = new GridLength(120);
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure to delete selected data?", "Требуется подстверждение!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes && ThreadTable.SelectedIndex >= 0)
            {
                ThreadCase Item = (dynamic)ThreadTable.SelectedItem;
                db.Threads.Remove(db.Threads.Where(e => e.Id == Item.Id).Single());
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
