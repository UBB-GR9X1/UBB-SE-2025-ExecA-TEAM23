// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="YourCompanyName">
//   Copyright (c) YourCompanyName. All rights reserved. Licensed under the MIT License.
// </copyright>
// <summary>
//   Provides application-specific behavior to supplement the default Application class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Hospital
{
    using System;
    using System.Linq;
    using Microsoft.UI.Xaml;

    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private Window? window;

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// This is the first line of authored code executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            if (this.IsRunningAsUnitTest())
            {
                // Skip initialization when running as a test
                return;
            }

            this.InitializeComponent();

            // Prevent unhandled exceptions from breaking into the debugger
            this.UnhandledException += (sender, e) =>
            {
                e.Handled = true;
            };
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            this.window = new MainWindow();
            this.window.Activate();
        }

        private bool IsRunningAsUnitTest()
        {
            // Check for test assemblies more thoroughly
            return AppDomain.CurrentDomain.GetAssemblies()
                .Any(a => a.FullName?.Contains("Test") == true ||
                          a.FullName?.Contains("MSTest") == true ||
                          a.FullName?.Contains("xUnit") == true ||
                          a.FullName?.Contains("NUnit") == true);
        }
    }
}
