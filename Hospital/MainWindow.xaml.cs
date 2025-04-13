using Hospital.DatabaseServices;
using Hospital.Exceptions;
using Hospital.Managers;
using Hospital.ViewModels;
using Hospital.Views;
using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Hospital
{
    public sealed partial class MainWindow : Window
    {


        public MainWindow()
        {
            this.InitializeComponent();

        }

        private void OpenApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow newLogInWindow = new LoginWindow();
            newLogInWindow.Activate();
            this.Close();
        }


    }
}
