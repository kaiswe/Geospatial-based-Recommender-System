using System.Windows;

namespace Geospatial_based_Recommender_System_via_XAML_UI_and_WPF
{
    /*PatientProfile class: A class designated to displaying the current 
     * user's stored profile (that they created) */
    public partial class PatientProfile : Window
    {
        public PatientProfile()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UserProfile();
        }

        /*UserProfile(): Access stored user data from Patient class
         * display within respective textboxes */
        private void UserProfile()
        {
            firstNameTextBox.Text = UpdatePatientProfile.user.FirstName;
            firstNameTextBox.IsReadOnly = true;
            surnameTextBox.Text = UpdatePatientProfile.user.SurName;
            surnameTextBox.IsReadOnly = true;
            AddressTextBox.Text = UpdatePatientProfile.user.Address;
            AddressTextBox.IsReadOnly = true;
            ageTextBox.Text = UpdatePatientProfile.user.Age;
            ageTextBox.IsReadOnly = true;

        }
    }

}
