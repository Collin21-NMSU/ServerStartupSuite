using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Windows;

namespace ServerStartupSuite
{
    public partial class AdvancedPane : Window
    {
        public AdvancedPane()
        {
            InitializeComponent();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PowerShell Scripts (*.ps1)|*.ps1";

            if (openFileDialog.ShowDialog() == true)
            {
                txtSelectedPath.Text = openFileDialog.FileName;
                btnExecute.IsEnabled = true;
                lblStatus.Text = "Ready to deploy script.";
            }
        }
     
     private void RunScript_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy Bypass -File \"{txtSelectedPath.Text}\"",
                    UseShellExecute = true,
                    Verb = "runas"
                };

                Process.Start(psi);
                lblStatus.Text = "Executing script as Administrator";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {

            Admindashboard main = new Admindashboard();
            main.Show();
            this.Close();
        }
    }
}