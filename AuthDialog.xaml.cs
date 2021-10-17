using System;
using System.Windows;

namespace Geospatial_based_Recommender_System_via_XAML_UI_and_WPF
{
    /* AuthDialog: class that prompts the user for twitter authentication input */
    public partial class AuthDialog : Window
    {
        /* AuthDialog(): Prompt user with input dialog */
        public AuthDialog(string question, string code)
        {
            InitializeComponent();
            pinCode.Text = code;
        }

        private void authenticateDialog_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            pinCode.SelectAll();
            pinCode.Focus();
        }

    }
}
