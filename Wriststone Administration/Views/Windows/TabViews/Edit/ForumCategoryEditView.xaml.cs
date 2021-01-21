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
    /// Interaction logic for ForumCategoryEditView.xaml
    /// </summary>
    public partial class ForumCategoryEditView : UserControl
    {
        private readonly Context db = new Context();
        private long SelectedId { get; set; }

        public ForumCategoryEditView()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (CheckFields())
            {
                var category = db.ForumCategories.Where(e => e.Name.Equals(Category.SelectedItem)).FirstOrDefault();
                if (SelectedId == 0)
                {
                    if (category != null)
                    {
                        db.ForumCategories.Add(new ForumCategory
                        {
                            Name = Name.Text,
                            Category = category.Id
                        });
                    }
                    else db.ForumCategories.Add(new ForumCategory { Name = Name.Text });
                }
                else
                {
                    ForumCategory item = db.ForumCategories.Where(e => e.Id == SelectedId).Single();
                    item.Name = Name.Text;
                    if (category != null) item.Category = category.Id;
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
        }

        public void Initialize(ForumCategory entity)
        {
            Initialize();
            SelectedId = entity.Id;
            Name.Text = entity.Name;
            var category = db.ForumCategories.Where(e => e.Id == entity.Category).FirstOrDefault();
            if(category != null) Category.SelectedItem = category.Name;
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
                MessageBox.Show("Name is empty!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
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
