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

namespace ServerStartupSuite
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// Provides a secure entry point for the Server Startup Suite.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Validates user credentials and navigates to the Admin Dashboard
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string correctUser = "Admin";
            string correctPass = "Password";
        //Verifies if input matches required credentials.
        if (txtUsername.Text == correctUser && txtPassword.Password == correctPass)
            {
               //displays main dashboard and closes login pane.
                Admindashboard dashboard = new Admindashboard();
                dashboard.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                dashboard.Show();
                this.Close();
            }
            else
            {
                lblStatus.Text = "Invalid Unsername or Password.";
                txtPassword.Clear();
            }
        }
            
    }
}
