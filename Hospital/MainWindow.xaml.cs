// <copyright file="MainWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hospital
{
    using Microsoft.UI.Xaml;

    /// <summary>
    /// Main Window that starts the aplication
    /// It's a Window with a single button in the center with the text "OPEN HOSPITAL APPLICATION"
    /// That when it is clicked it opens the Login Window.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
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
