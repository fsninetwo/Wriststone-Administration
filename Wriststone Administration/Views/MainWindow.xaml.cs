using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wriststone_Administration.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SignUpView_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (SignUpView.IsEnabled == false)
            {
                LoginView.IsEnabled = true;
                LoginView.Visibility = Visibility.Visible;
            }
        }

        private void LoginView_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (LoginView.IsEnabled == false)
            {
                if (LoginView.Parameter.Equals("SignUp"))
                {
                    SignUpView.Visibility = Visibility.Visible;
                    SignUpView.IsEnabled = true;
                }
                else if (LoginView.Parameter.Equals("Recover"))
                {
                    RecoveryView.Visibility = Visibility.Visible;
                    RecoveryView.IsEnabled = true;
                }
                else if (LoginView.Parameter.Equals("MainWindow"))
                {
                    WindowView.Visibility = Visibility.Visible;
                    WindowView.IsEnabled = true;
                }
            }
        }

        private void RecoveryView_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (RecoveryView.IsEnabled == false)
            {
                LoginView.IsEnabled = true;
                LoginView.Visibility = Visibility.Visible;
            }
        }

        private void WindowView_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (WindowView.IsEnabled == false)
            {
                if (WindowView.Parameter.Equals("Logout"))
                {
                    LoginView.Visibility = Visibility.Visible;
                    LoginView.IsEnabled = true;
                }
                else if (WindowView.Parameter.Equals("Exit"))
                {
                    Close();
                }
            }
        }
    }
}
