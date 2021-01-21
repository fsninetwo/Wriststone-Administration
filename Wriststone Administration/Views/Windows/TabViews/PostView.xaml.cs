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
    /// Interaction logic for PostView.xaml
    /// </summary>
    public partial class PostView : UserControl
    {
        private readonly Context db = new Context();
        public PostView()
        {
            InitializeComponent();

        }
        public void Initialize()
        {
            if (Search.Equals("")) AddItems();
            else AddItemsBySearch();
        }

        public void AddItems()
        {
            var result = from post in db.Posts
                         join account in db.Accounts on post.Account equals account.Id
                         join thread in db.Threads on post.Thread equals thread.Id
                         select new PostCase
                         {
                             Id = post.Id,
                             Created = post.Created,
                             Context = post.Context,
                             Account = account.Login,
                             Thread = thread.Subject,
                         };
            PostTable.ItemsSource = result.ToList();
        }

        public void AddItemsBySearch()
        {
            var result = from post in db.Posts
                         join account in db.Accounts on post.Account equals account.Id
                         join thread in db.Threads on post.Thread equals thread.Id
                         where post.Context.Contains(Search.Text) || account.Login.Contains(Search.Text) || thread.Subject.Contains(Search.Text)
                         select new PostCase
                         {
                             Id = post.Id,
                             Created = post.Created,
                             Context = post.Context,
                             Account = account.Login,
                             Thread = thread.Subject,
                         };
            PostTable.ItemsSource = result.ToList();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            Initialize();
        }

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            PostEditView.IsEnabled = true;
            PostEditView.Visibility = Visibility.Visible;
            PostEditView.Initialize();
            EditHeight.Height = new GridLength(120);
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (PostTable.SelectedIndex >= 0)
            {
                PostCase Item = (dynamic)PostTable.SelectedItem;
                PostEditView.IsEnabled = true;
                PostEditView.Visibility = Visibility.Visible;
                PostEditView.Initialize(Item);
                EditHeight.Height = new GridLength(120);
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure to delete selected data?", "Требуется подстверждение!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes && PostTable.SelectedIndex >= 0)
            {
                PostCase Item = (dynamic)PostTable.SelectedItem;
                db.Posts.Remove(db.Posts.Where(e => e.Id == Item.Id).Single());
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
