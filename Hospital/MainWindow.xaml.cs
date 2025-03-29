using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Hospital.DatabaseServices;
using Hospital.Managers;
using Hospital.ViewModels;
using System.Threading.Tasks;
using Hospital.Exceptions;
using Hospital.Views;

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
                bool successful = await _viewModel.Login(username, password);
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

            // implement what happens after successful Login
        }
    }
}
