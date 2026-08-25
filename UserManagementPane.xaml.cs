using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.DirectoryServices.ActiveDirectory;


namespace ServerStartupSuite
{
    public partial class UserManagementPane : UserControl
    {
        public UserManagementPane()
        {
            InitializeComponent();
            GetCurrentDomain();
        }

        private void GetCurrentDomain()
        {
            try
            {
                //finding domain name 
                Domain domain = Domain.GetComputerDomain();
                //assignment of domain name to UI element
                TxtEnvStatus.Text = domain.Name;
            }
            catch
            {
                TxtEnvStatus.Text = "Not connected to a domain.";
            }
        }

        //making the password box invisible if 
        private void ComboMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //crash prevention
            if (PasswordPanel == null) return;

            //0 is user creation 1 is group creation
            if (ComboMode.SelectedIndex == 1)
            {
                PasswordPanel.Visibility = Visibility.Collapsed;
              
            }
            else
            {
                PasswordPanel.Visibility = Visibility.Visible;
            }

        }
        private void RunPowershellScript(string name, string pass, bool isGroup)
        {
            try
            {
                string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserMgmt.ps1");
                if (!File.Exists(scriptPath))
                {
                    MessageBox.Show($"Script not found within: {scriptPath}", "File Error");
                    return;
                }
                string psBool = isGroup ? "1" : "0";
                string arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"& '{scriptPath}' -TargetName '{name}' -Password '{pass}' -IsGroup {psBool}\"";

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = arguments,
                    UseShellExecute = true,
                    Verb = "runas" //admin for AD perms
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("execution error: " + ex.Message);
            }
        }
        private void BtnExecute_Click(object sender, RoutedEventArgs e)
        {
            string targetName = TxtTargetName.Text.Trim();
            string password = TxtPassword.Password;
            bool isGroup = (ComboMode.SelectedIndex == 1);

            //Validation if is null or whitespace before powershell delivery
            if (string.IsNullOrWhiteSpace(targetName))
            {
                MessageBox.Show("Please Enter a name", "Input required");
                return;
            }

            if (!isGroup && string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Password is required for users.", "Input required");
                return;
            }
            //Running script with gathered data from user
            RunPowershellScript(targetName, password, isGroup);

            //prompt user after successful run
            string actionType = isGroup ? "Group" : "User";
            MessageBox.Show($"{actionType} creation request for '{targetName}' was handled successfully.",
                            "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            //resetting data to empty for new entry
            TxtTargetName.Text = string.Empty;
            TxtPassword.Clear();
            ComboMode.SelectedIndex = 0;

            
        }
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Admindashboard dashboard = new Admindashboard();
            dashboard.Show();
            Window.GetWindow(this).Close();
        }
    }
}