using System.Windows;

namespace Geospatial_based_Recommender_System_via_XAML_UI_and_WPF
{
    /*UpdatePatientProfile class: A class that enables the user to add, or update their current profile into the system
     * for viewing */
    public partial class UpdatePatientProfile : Window
    {
        public static Patient user = new Patient();

        public UpdatePatientProfile()
        {
            InitializeComponent();
        }

        private void UpdateUserProfile_Click(object sender, RoutedEventArgs e)
        {
            UpdateProfile();
            Close();
        }

        /*UpdateProfile(): obtain all input within designated textboxes
         * pass to patient class for user profile storage */
        public void UpdateProfile()
        {
            user.FirstName = firstNameTextBox.Text;
            user.SurName = surnameTextBox.Text;
            user.Address = AddressTextBox.Text;
            user.Age = ageTextBox.Text;
        }
    }
}
