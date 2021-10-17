using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using GMap.NET;
using System.Xml.Linq;


namespace Geospatial_based_Recommender_System_via_XAML_UI_and_WPF
{
    /*Class for adding individuals into the system */
    public partial class AddPerson : Window
    {
        public static PointLatLng personLatLng;
        public static SortedDictionary<int, Collaborator> allPeople = new SortedDictionary<int, Collaborator>();
        private XDocument xmldoc = XDocument.Load(@"AllCollaborators-info.xml");
        private static int personCount = 0;
        Collaborator person = new Collaborator();


        public AddPerson()
        {
            InitializeComponent();

        }

        private void AddPerson_Load(object sender, RoutedEventArgs e)
        {
            /*Initializing all default values for collaborator combobox selections*/
            AMPM_1.Items.Add("AM");
            AMPM_1.Items.Add("PM");
            AMPM_2.Items.Add("AM");
            AMPM_2.Items.Add("PM");
            mainRoleComboBox.Items.Add("Relative");
            mainRoleComboBox.Items.Add("Friend");
            mainRoleComboBox.Items.Add("Health-Care Professional");
            mainRoleComboBox.Items.Add("Other");

            supportLevelComboBox.Items.Add("24/7 Availability");
            supportLevelComboBox.Items.Add("Within specificed time range");

            /*Lat/Lng of user click on the map for collaborator location*/
            latTextBox.Text = personLatLng.Lat.ToString();
            lngTextBox.Text = personLatLng.Lng.ToString();
        }

        /*CollabLatLng(): Accessing map OnClick event to gain coordinates*/
        public void CollabLatLng(PointLatLng point)
        {
            personLatLng = point;
        }


        private void Add_Collab_Button_Click(object sender, RoutedEventArgs e)
        {
            /*If user is trying to add their details into the system
             * yet have no selected a photo for themselves */
            AddCollaboratorToDB();
            if (person.PersonPhoto != null)
            {
                Close();

            }
            else
            {
                /* Display error message until photo is selected or uploaded */
                MessageBox.Show("Please upload a photo for your collaborator profile");
            }
        }

        /* AddCollaboratorToDB(): Accessing and storing all user entered information
         * into the Collaborator constructor
         * for person details storage within the system */
        private void AddCollaboratorToDB()
        {
            person.FirstName = firstNameTextBox.Text;
            person.SurName = surnameTextBox.Text;
            person.Role = mainRoleComboBox.Text;
            person.OtherRole = otherRoleTextBox.Text;
            person.SupportLevel = supportLevelComboBox.Text;
            person.LocationLat = latTextBox.Text;
            person.LocationLng = lngTextBox.Text;
            person.TimeStart = startTimeTextBox.Text;
            person.AMorPM_1 = AMPM_1.Text;
            person.TimeEnd = endTimeTextBox.Text;
            person.AMorPM_2 = AMPM_2.Text;
            /*Add all collaborator data into dictionary*/
            allPeople.Add(personCount, person);
            personCount++;

            /*Adding all user info into a XML document for storage */
            AddPersonToXML(person.FirstName, person.SurName, person.Role, person.OtherRole, person.SupportLevel, person.LocationLat, person.LocationLng, person.TimeStart , person.AMorPM_1, person.TimeEnd, person.AMorPM_2, person.PersonPhoto);
        }

        private void Photo_Upload_Button_Click(object sender, RoutedEventArgs e)
        {
            /*Access file location of all relative photos to their events*/
            string filePath = Environment.CurrentDirectory;
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                InitialDirectory = filePath + @"\Collaborator_Photos"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string file = openFileDialog.FileName;
                try
                {
                    Uri uri = new Uri(file);
                    BitmapImage bitmap = new BitmapImage(uri);
                    Photo_Box.Source = bitmap;
                    person.PersonPhoto = bitmap;

                    Show();
                }
                catch (ArgumentException)
                {
                    MessageBox.Show("Error, check if path file is available", "File Path Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }

        }

        public SortedDictionary<int, Collaborator> GetCollabDictionary()
        {
            return allPeople;
        }
        
        public void AddPersonToXML(string firstName, string surName, string role, string otherRole, string support, string lat, string lng, string start, string ampm1, string end, string ampm2, BitmapImage photo)
        {
            /*Access the XML file for collaborator storage, structure the inputted information in the XML*/
            XDocument xmldoc = XDocument.Load(@"AllCollaborators-info.xml");
            xmldoc.Root.Add(new XElement("Person",
                        new XElement("Name", firstName, surName),
                                new XElement("Role", role),
                                new XElement("OtherRole", otherRole),
                                new XElement("location",
                                    new XElement("lat", lat),
                                    new XElement("long", lng)),
                                new XElement("SupportLevel", support),
                                new XElement("StartTime", start, ampm1),
                                new XElement("EndTime", end, ampm2),
                                new XElement("PesonPhotoFile", photo)));
            /*Save the new input into the same XML*/
            xmldoc.Save(@"AllCollaborators-info.xml");
        }

        private void MainRoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*If user has selected the "other" option for their
             * selected role*/
            if (e.AddedItems.Contains("Other"))
            {
                /*enable the new textbox for additional user input
                 * for better role specification*/
                Collab_Other_Role.Visibility = Visibility.Visible;
                otherRoleTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                /*disable if "other" role deselected*/
                Collab_Other_Role.Visibility = Visibility.Hidden;
                otherRoleTextBox.Visibility = Visibility.Hidden;
            }

        }

        private void SupportLevelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /* By selecting 24/7 support level in supportLevel comboBox*/
            if (e.AddedItems.Contains("24/7 Availability"))
            {
                /* Auto-fill start and end time boxes with previously selected level of support
                 * only if 24/7 availability*
                 * creates less user input/results confusion*/
                startTimeTextBox.Text = "24/7 Availability";
                endTimeTextBox.Text = "24/7 Availability";
                /* set time textBoxes to ReadOnly(true) while 24/7 option selected
                 * removes confusing user input*/
                startTimeTextBox.IsReadOnly = true;
                endTimeTextBox.IsReadOnly = true;
                AMPM_1.Visibility = Visibility.Hidden;
                AMPM_2.Visibility = Visibility.Hidden;
            }
            else
            {
                /* Otherwise set Textboxes/Comboboxes back to normal*/
                startTimeTextBox.Text = null;
                endTimeTextBox.Text = null;
                startTimeTextBox.IsReadOnly = false;
                endTimeTextBox.IsReadOnly = false;
                AMPM_1.Visibility = Visibility.Visible;
                AMPM_2.Visibility = Visibility.Visible;
            }
        }
    }
}