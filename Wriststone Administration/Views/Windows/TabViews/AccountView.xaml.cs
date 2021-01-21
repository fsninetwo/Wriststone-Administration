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

namespace Wriststone_Administration.Views.Windows.TabViews
{
    /// <summary>
    /// Interaction logic for Accounts.xaml
    /// </summary>
    public partial class AccountView : UserControl
    {
        private readonly Context db = new Context();
        public AccountView()
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
            AccountTable.ItemsSource = db.Accounts.ToList();
        }

        public void AddItemsBySearch()
        {
            AccountTable.ItemsSource = db.Accounts.Where(e => e.Login.Contains(Search.Text)).ToList();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            Initialize();
        }

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            AccountEditView.IsEnabled = true;
            AccountEditView.Visibility = Visibility.Visible;
            AccountEditView.Initialize();
            EditHeight.Height = new GridLength(120);
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (AccountTable.SelectedIndex >= 0)
            {
                Account Item = (dynamic)AccountTable.SelectedItem;
                AccountEditView.IsEnabled = true;
                AccountEditView.Visibility = Visibility.Visible;
                AccountEditView.Initialize(Item);
                EditHeight.Height = new GridLength(120);
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure to delete selected data?", "Требуется подстверждение!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes && AccountTable.SelectedIndex >= 0)
            {
                Account Item = (dynamic)AccountTable.SelectedItem;
                db.Accounts.Remove(db.Accounts.Where(e => e.Id == Item.Id).Single());
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
