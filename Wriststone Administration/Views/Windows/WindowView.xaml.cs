using System;
using System.Collections.Generic;
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

namespace Wriststone_Administration.Views.Windows
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class WindowView : UserControl
    {
        public string Parameter { get; set; } = "";
        public WindowView()
        {
            InitializeComponent();
            Initialize();
        }

        public void Initialize()
        {
            if (AccountTab.IsSelected) AccountView.Search.Text = Search.Text;
            else if (ForumCategoryTab.IsSelected) ForumCategoryView.Search.Text = Search.Text;
            else if (OrderTab.IsSelected) OrderView.Search.Text = Search.Text;
            else if (OrderDetailsTab.IsSelected) OrderDetailsView.Search.Text = Search.Text;
            else if (ProductTab.IsSelected) ProductView.Search.Text = Search.Text;
            else if (ProductCategoryTab.IsSelected) ProductCategoryView.Search.Text = Search.Text;
            else if (RatingTab.IsSelected) RatingView.Search.Text = Search.Text;
            else if (ThreadTab.IsSelected) ThreadView.Search.Text = Search.Text;
            //else if (LogTab.IsSelected) LogView.Initialize();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Parameter = "Logout";
            IsEnabled = false;
            Visibility = Visibility.Collapsed;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Parameter = "Exit";
            IsEnabled = false;
            Visibility = Visibility.Collapsed;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Initialize();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            Initialize();
        }

        private void Logs_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
