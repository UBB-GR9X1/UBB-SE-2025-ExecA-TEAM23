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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Hospital.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DoctorDashboardWindow : Window
    {
        public DoctorDashboardWindow(DoctorViewModel doctorViewModel)
        {
            this.InitializeComponent();

            // Create the DoctorDashboardControl and set its DataContext
            DoctorDashboardControl doctorDashboardControl = new DoctorDashboardControl(doctorViewModel);

            // Add it to the grid
            DoctorDashboard.Content = doctorDashboardControl; // DoctorDashboard is the x:Name of your Grid

        }
    }
}
