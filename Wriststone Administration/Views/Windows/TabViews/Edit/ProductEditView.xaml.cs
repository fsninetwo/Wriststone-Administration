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
    /// Interaction logic for ProductEditView.xaml
    /// </summary>
    public partial class ProductEditView : UserControl
    {
        readonly Context db = new Context();
        private long SelectedId { get; set; }

        public ProductEditView()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (CheckFields())
            {
                string Sol = MD5Hash.RandomString();
                MessageBox.Show(Name.Text);
                MessageBox.Show(StringFromRichTextBox(Description));
                MessageBox.Show(Convert.ToDecimal(Price.Text).ToString());
                MessageBox.Show(Publisher.Text);
                MessageBox.Show(Developer.Text);
                if (SelectedId == 0)
                {
                    db.Products.Add(new Product
                    {
                        Name = Name.Text,
                        Description = StringFromRichTextBox(Description),
                        Price = Convert.ToDecimal(Price.Text),
                        Publisher = Publisher.Text,
                        Developer = Developer.Text,
                        Category = db.ProductCategories.Where(e => e.Name.Equals(Category.SelectedItem.ToString())).Single().Id
                    });
                }
                else
                {
                    Product item = db.Products.Where(e => e.Id == SelectedId).Single();
                    item.Name = Name.Text;
                    item.Description = StringFromRichTextBox(Description);
                    item.Price = Convert.ToDecimal(Price.Text);
                    item.Publisher = Publisher.Text;
                    item.Developer = Developer.Text;
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
            Category.ItemsSource = db.ProductCategories.Select(e => e.Name).ToList();
        }

        public void Initialize(ProductCase entity)
        {
            Initialize();
            SelectedId = entity.Id;
            Name.Text = entity.Name;
            Description.Document = StringToRichTextBox(entity.Description);
            Price.Text = entity.Price.ToString();
            Publisher.Text = entity.Publisher;
            Developer.Text = entity.Developer;
            Category.SelectedItem = entity.Category;
        }

        private void CleanFields()
        {
            Name.Text = "";
            Description.Document.Blocks.Clear();
            Price.Text = "";
            Publisher.Text = "";
            Developer.Text = "";
            Category.SelectedItem = null;
        }

        private bool CheckFields()
        {
            if (!Regex.IsMatch(Name.Text, ("\\w+")))
            {
                MessageBox.Show("Login is empty!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Regex.IsMatch(StringFromRichTextBox(Description), "\\w+"))
            {
                MessageBox.Show("Fullname is empty!", "Неверные данные!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Regex.IsMatch(Publisher.Text, "\\w+"))
            {
                MessageBox.Show("Enter correct email name!", "Неверные данные!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Regex.IsMatch(Developer.Text, "\\w+"))
            {
                MessageBox.Show("Password must have at least one digit, lower, upper case, special symbol(@#$%^&-+<>=()), no white space and be in range 8-20 symbols!", "Неверные данные!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (Category.SelectedItem == null)
            {
                MessageBox.Show("Password must have at least one digit, lower, upper case, special symbol(@#$%^&-+<>=()), no white space and be in range 8-20 symbols!", "Неверные данные!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private FlowDocument StringToRichTextBox(string text)
        {
            FlowDocument mcFlowDoc = new FlowDocument();
            Paragraph para = new Paragraph();
            para.Inlines.Add(new Run(text));
            mcFlowDoc.Blocks.Add(para);
            return mcFlowDoc;
        }
        private string StringFromRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            return textRange.Text;
        }
        private void Exit()
        {
            SelectedId = 0;
            IsEnabled = false;
            Visibility = Visibility.Hidden;
        }
    }
}
