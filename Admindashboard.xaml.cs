using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Runtime.Hosting;

namespace ServerStartupSuite
{
    /// <summary>
    /// Interaction logic for Admindashboard.xaml
    /// Provides interface to trigger automation scripts
    /// </summary>
    public partial class Admindashboard : Window
    {
        public Admindashboard()
        {
            InitializeComponent();
        }

       /// <summary>
       /// Handles the user Management button click to trigger account scripts.
       /// </summary>
        private void BtnUser_Click(object sender, RoutedEventArgs e)
        {
            Window userWindow = new Window
            {
                Title = "User Management",
                Content = new UserManagementPane(),
                Width = 600,
                Height = 750,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,

            };

            userWindow.Show();
            this.Close();
        }
        
        /// <summary>
        /// Handles the Workstation button click to trigger workstation configurations.
        /// </summary>
 
        private void BtnSetup_Click(object sender, RoutedEventArgs e)
        {
            RunPowerShellScript("WorkstationSetup.ps1");
        }

        /// <summary>
        /// Handles the NetBaseline button and runs baseline network diagnostics
        /// </summary>
        private void BtnNet_Click(object sender, RoutedEventArgs e)
        {
            RunPowerShellScript("NetBaseline.ps1");
        }
        /// <summary>
        /// Handles the Advanced Button (execution of local .psi scripts.
        /// </summary>
        private void BtnAdvanced_Click(object sender, RoutedEventArgs e)
        {
            AdvancedPane advancedWindow = new AdvancedPane();
            advancedWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            advancedWindow.Show();
            this.Close();
        }

        private void RunPowerShellScript(string scriptName)
        {
            try
            {
                //Mapping the full path to the script based on install location.
                string scriptPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, scriptName);

                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();

                psi.FileName = "powershell.exe";
                // -no:Profile: Fasterloading, -ExecutionPolicy Bypass: Ensures script execution regardless of security.
                psi.Arguments = $"-NoProfile -ExecutionPolicy Bypass -File \"{scriptPath}\"";
                psi.UseShellExecute = true;
                psi.Verb = "runas"; //for administrator running
                System.Diagnostics.Process.Start(psi);

                //notification of successful run
                System.Windows.MessageBox.Show($"{scriptName} Started successfully", "Changes made", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            // Catches Poteion IO or permission error and display to user. 
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("error" + ex.Message);
            }
        }
    }
}
