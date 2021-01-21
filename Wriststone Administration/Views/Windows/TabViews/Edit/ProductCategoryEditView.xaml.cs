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

namespace Wriststone_Administration.Views.Windows.TabViews.Edit
{
    /// <summary>
    /// Interaction logic for ProductCategoryEditView.xaml
    /// </summary>
    public partial class ProductCategoryEditView : UserControl
    {
        readonly Context db = new Context();
        private long SelectedId { get; set; }

        public ProductCategoryEditView()
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
                    db.ProductCategories.Add(new ProductCategory
                    {
                        Name = Name.Text,
                        Category = db.ProductCategories.Where( e => e.Name.Equals(Category.SelectedItem)).Single().Id,
                    });
                }
                else
                {
                    ProductCategory item = db.ProductCategories.Where(e => e.Id == SelectedId).Single();
                    item.Name = Name.Text;
                    item.Category = db.ProductCategories.Where(e => e.Name.Equals(Category.SelectedItem)).Single().Id;
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
        }

        public void Initialize(ProductCategory entity)
        {
            Initialize();
            SelectedId = entity.Id;
            Name.Text = entity.Name;
            Category.SelectedItem = entity.Category;
        }

        private void CleanFields()
        {
            Name.Text = "";
            Category.SelectedItem = null;
        }

        private bool CheckFields()
        {
            if (!Regex.IsMatch(Name.Text, ("\\w+")))
            {
                MessageBox.Show("Category name is empty!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
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
