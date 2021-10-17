using System.Collections.Generic;
using System.Windows;
using GMap.NET.WindowsPresentation;

namespace Geospatial_based_Recommender_System_via_XAML_UI_and_WPF
{
    /* DisplayCollabInfo: Class for display the collaborators inputted information based on its
     * relative marker interaction */

    public partial class DisplayCollabInfo : Window
    {
        public DisplayCollabInfo()
        {
            InitializeComponent();
        }

        /* CollabInfo(): Method for displaying the current markers stored collaborator information */
        public void CollabInfo(GMapMarker marker)
        {
            /* iterate through the global sorted dictionary allPeople within the class AddPerson.xaml.cs*/
            foreach (KeyValuePair<int, Collaborator> person in AddPerson.allPeople)
            {
                /* if the key of allPeople dictionary matches the predefined marker tag which is also the allPeople Key */
                if (person.Key.ToString() == marker.Tag.ToString())
                {
                    /* display all of that collaborators relative information */
                    firstNameTextBox.Text = person.Value.FirstName;
                    firstNameTextBox.IsReadOnly = true;
                    surnameTextBox.Text = person.Value.SurName;
                    surnameTextBox.IsReadOnly = true;
                    mainRoleComboBox.Text = person.Value.Role;
                    mainRoleComboBox.IsReadOnly = true;
                    supportLevelTextBox.Text = person.Value.SupportLevel;
                    supportLevelTextBox.IsReadOnly = true;
                    otherRoleTextBox.Text = person.Value.OtherRole;
                    otherRoleTextBox.IsReadOnly = true;
                    latTextBox.Text = person.Value.LocationLat;
                    latTextBox.IsReadOnly = true;
                    lngTextBox.Text = person.Value.LocationLng;
                    lngTextBox.IsReadOnly = true;
                    startTimeTextBox.Text = person.Value.TimeStart;
                    startTimeTextBox.IsReadOnly = true;
                    endTimeTextBox.Text = person.Value.TimeEnd;
                    endTimeTextBox.IsReadOnly = true;
                    AM_PM1.Text = person.Value.AMorPM_1;
                    AM_PM1.IsReadOnly = true;
                    AM_PM2.Text = person.Value.AMorPM_2;
                    AM_PM2.IsReadOnly = true;
                    Photo_Box.Source = person.Value.PersonPhoto;
                    Photo_Upload_Button.IsEnabled = false;
                    Add_Collab_Button.IsEnabled = false;
                    Show();
                }
            }
        }
    }
}
