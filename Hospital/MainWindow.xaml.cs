
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Hospital.DatabaseServices;
using Hospital.Models;
using Hospital.Views;
using Hospital.DatabaseServices;
using Hospital.Managers;
using Hospital.ViewModels;
using System.Threading.Tasks;
using Hospital.Exceptions;
using Hospital.Views;
using System;
using Microsoft.Data.SqlClient;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Hospital
{
    public sealed partial class MainWindow : Window
    {

        private readonly AuthViewModel _viewModel;

        public MainWindow()
        {
            this.InitializeComponent();

            LogInDatabaseService logInService = new LogInDatabaseService();
            AuthManagerModel managerModel = new AuthManagerModel(logInService);
            _viewModel = new AuthViewModel(managerModel);
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameField.Text;
            string password = PasswordField.Password;

            try
            {
                await _viewModel.Login(username, password);
                LogoutWindow log = new LogoutWindow(_viewModel);
                log.Activate();
                // Show Logger window after successful login just for the presentation (uncomment when needed)
                // LoggerView logger = new LoggerView();
                // logger.Activate();
                this.Close();
            }
            catch (AuthenticationException ex)
            {
                var validationDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = $"{ex.Message}",
                    CloseButtonText = "OK"
                };

                validationDialog.XamlRoot = this.Content.XamlRoot;
                await validationDialog.ShowAsync();
            }
            catch (SqlException err)
            {
                var validationDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = $"{err.Message}",
                    CloseButtonText = "OK"
                };

                validationDialog.XamlRoot = this.Content.XamlRoot;
                await validationDialog.ShowAsync();
            }
        }

        private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            CreateAccountWindow createAccWindow = new CreateAccountWindow(_viewModel);
            createAccWindow.Activate();
            this.Close();
        }
    }
}
