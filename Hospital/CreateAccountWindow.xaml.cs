using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Exceptions;
using Hospital.ViewModels;
using Microsoft.Data.SqlClient;

namespace Hospital
{
    public sealed partial class CreateAccountWindow : Window
    {

        private readonly AuthViewModel _viewModel;
        public CreateAccountWindow(AuthViewModel viewModel)
        {
            this.InitializeComponent();
            _viewModel = viewModel;
        }

        private async void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameField.Text;
            string password = PasswordField.Password;
            string mail = EmailTextBox.Text;
            string name = NameTextBox.Text;

            if (BirthDateCalendarPicker.Date.HasValue)
            {
                DateOnly birthDate = DateOnly.FromDateTime(BirthDateCalendarPicker.Date.Value.DateTime);
                BirthDateCalendarPicker.Date = new DateTimeOffset(birthDate.ToDateTime(TimeOnly.MinValue));

                string cnp = CNPTextBox.Text;

                try
                {
                    await _viewModel.CreateAccount(username, password, mail, name, birthDate, cnp);
                    LogoutWindow log = new LogoutWindow(_viewModel);
                    log.Activate();
                    this.Close();
                }
                catch (AuthenticationException err)
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

                catch (SqlException)
                {
                    var validationDialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = $"Database Error",
                        CloseButtonText = "OK"
                    };

                    validationDialog.XamlRoot = this.Content.XamlRoot;
                    await validationDialog.ShowAsync();
                }
            }
            else
            {
                var validationDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Birth date is required.",
                    CloseButtonText = "OK"
                };

                validationDialog.XamlRoot = this.Content.XamlRoot;
                await validationDialog.ShowAsync();
            }
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Activate();
            this.Close();
        }
    }
}
