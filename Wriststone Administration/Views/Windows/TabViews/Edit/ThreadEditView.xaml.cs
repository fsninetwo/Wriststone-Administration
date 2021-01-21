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
    /// Interaction logic for ThreadEditView.xaml
    /// </summary>
    public partial class ThreadEditView : UserControl
    {
        readonly Context db = new Context();
        private long SelectedId { get; set; }

        public ThreadEditView()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (CheckFields())
            {
                if (SelectedId == 0)
                {
                    db.Threads.Add(new Thread
                    {
                        Subject = Subject.Text,
                        Created = Created.SelectedDate.Value,
                        Category = db.ForumCategories.Where(e => e.Name.Equals(Category.SelectedItem)).Single().Id,
                        Account = db.Accounts.Where(e => e.Login.Equals(Account.SelectedItem)).Single().Id,
                    });
                }
                else
                {
                    Thread item = db.Threads.Where(e => e.Id == SelectedId).Single();
                    item.Subject = Subject.Text;
                    item.Created = Created.SelectedDate.Value;
                    item.Category = db.ForumCategories.Where(e => e.Name.Equals(Category.SelectedItem)).Single().Id;
                    item.Account = db.Accounts.Where(e => e.Login.Equals(Account.SelectedItem)).Single().Id;
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
            Category.ItemsSource = db.ForumCategories.Select(e => e.Name).ToList();
            Account.ItemsSource = db.Accounts.Select(e => e.Login).ToList();
        }

        public void Initialize(ThreadCase entity)
        {
            Initialize();
            SelectedId = entity.Id;
            Subject.Text = entity.Subject;
            Created.SelectedDate = entity.Created;
            Category.SelectedItem = entity.Category;
            Account.SelectedItem = entity.Account;
        }

        private void CleanFields()
        {
            Subject.Text = "";
            Created.SelectedDate = null;
            Category.SelectedItem = null;
            Account.SelectedItem = null;
        }

        private bool CheckFields()
        {
            if (!Regex.IsMatch(Subject.Text, ("\\w+")))
            {
                MessageBox.Show("Context mustn't be empty!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (Created.SelectedDate == null)
            {
                MessageBox.Show("Created date mustn't be empty!", "Неверные данные!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (Category.SelectedItem == null)
            {
                MessageBox.Show("Thread mustn't be empty!", "Неверные данные!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (Account.SelectedItem == null)
            {
                MessageBox.Show("Account mustn't be empty", "Неверные данные!", MessageBoxButton.OK, MessageBoxImage.Error);
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
