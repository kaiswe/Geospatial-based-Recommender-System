using System.Windows.Media.Imaging;

namespace Geospatial_based_Recommender_System_via_XAML_UI_and_WPF
{
    /* Constructor class for accessing and storing people's data into 
     * and from the system */
    public class Collaborator
    {
        /* Default constructor */
        public Collaborator()
        {
            FirstName = "N/A";
            SurName = "N/A";
            Role = "N/A";
            OtherRole = "N/A";
            SupportLevel = "N/A";
            LocationLat = "N/A";
            LocationLng = "N/A";
            TimeStart = "N/A";
            TimeEnd = "N/A";
            AMorPM_1 = "N/A";
            AMorPM_2 = "N/A";
            PersonPhoto = null;
        }

        public Collaborator(string firstName, string surName, string role, string otherRole, string supportLevel, string lat, string lng, string startTime, string endTime, string dayNight1, string dayNight2, BitmapImage personPhoto)
        {
            FirstName = firstName;
            SurName = surName;
            Role = role;
            OtherRole = otherRole;
            SupportLevel = supportLevel;
            LocationLat = lat;
            LocationLng = lng;
            TimeStart = startTime;
            TimeEnd = endTime;
            AMorPM_1 = dayNight1;
            AMorPM_2 = dayNight2;
            PersonPhoto = personPhoto;

        }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Role { get; set; }
        public string OtherRole { get; set; }
        public string SupportLevel { get; set; }
        public string LocationLat { get; set; }
        public string LocationLng { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public string AMorPM_1 { get; set; }
        public string AMorPM_2 { get; set; }
        public BitmapImage PersonPhoto { get; set; }
    }
}

