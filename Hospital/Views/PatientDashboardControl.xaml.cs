using Hospital.Managers;
using Hospital.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;


namespace Hospital.Views
{
    public sealed partial class PatientDashboardControl : UserControl
    {
        private PatientViewModel _viewModel;

        // Constructor
        public PatientDashboardControl()
        {
            this.InitializeComponent();
        }
        public PatientDashboardControl(PatientManagerModel patientManagerModel, int userId)
        {
            this.InitializeComponent();
            _viewModel = new PatientViewModel(patientManagerModel, userId);
            this.DataContext = _viewModel; // Bind the ViewModel to the UserControl
        }

        // Update Button Click Handler
        private async void OnUpdateButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            // Example for updating name. You can repeat this for each field.
            bool isNameUpdated = await _viewModel.UpdateName(_viewModel.Name);
            if (isNameUpdated)
            {
                // Optionally show a message or do something on success
            }
            else
            {
                // Optionally show an error message if the update fails
            }

            // Similarly, you can update the other fields by calling respective Update methods.
        }
    }
}
