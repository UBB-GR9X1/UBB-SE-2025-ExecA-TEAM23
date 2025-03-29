using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using Hospital.ViewModels;
using Hospital.Managers;
using Hospital.Models;
using Hospital.DatabaseServices;
using System.Threading;
using System.Threading.Tasks;

namespace Hospital.Views
{
    public sealed partial class SearchDoctorsView : UserControl
    {
        public SearchDoctorsViewModel ViewModel { get; private set; }

        // For debouncing search input
        private CancellationTokenSource _debounceTokenSource;
        private readonly int _debounceDelay = 300; // milliseconds

        public SearchDoctorsView()
        {
            this.InitializeComponent();

            // This would normally be injected through dependency injection
            var searchManager = new SearchDoctorsManagerModel(new DoctorsDatabaseService());
            ViewModel = new SearchDoctorsViewModel(searchManager, string.Empty);

            this.DataContext = ViewModel;

            // Load initial empty search results
            _ = ViewModel.LoadDoctors();
        }

        private async void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Cancel any previous search operation
            _debounceTokenSource?.Cancel();
            _debounceTokenSource = new CancellationTokenSource();
            var token = _debounceTokenSource.Token;

            try
            {
                // Wait before executing the search to avoid too many searches while typing
                await Task.Delay(_debounceDelay, token);

                // If the token was canceled during the delay, this won't execute
                if (!token.IsCancellationRequested)
                {
                    ViewModel.DepartmentPartialName = SearchTextBox.Text;
                    await ViewModel.LoadDoctors();
                }
            }
            catch (TaskCanceledException)
            {
                // This is expected when debouncing
            }
        }
    }
}
