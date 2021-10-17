namespace Geospatial_based_Recommender_System_via_XAML_UI_and_WPF
{
    /*Patient: Constructor class that stores all information
     * regarding the current user's details */
    public class Patient
    {
        /*default constructor*/
        public Patient()
        {

        }

        public Patient(string firstName, string surName, string address, string age)
        {
            FirstName = firstName;
            SurName = surName;
            Address = address;
            Age = age;
        }

        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Address { get; set; }
        public string Age { get; set; }

    }


}
