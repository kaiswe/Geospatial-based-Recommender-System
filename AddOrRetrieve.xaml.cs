using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using GMap.NET.WindowsPresentation;

namespace Geospatial_based_Recommender_System_via_XAML_UI_and_WPF
{
    /* AddOrRetrieve: Class that handles the window to connect the user with various program options */
    public partial class AddOrRetrieve : Window
    {
        MainWindow mainWindow = new MainWindow();
        AddPerson addPerson = new AddPerson();

        public AddOrRetrieve()
        {
            InitializeComponent();
            
        }

        /* AddCollaborator_Click(): On button press, open window to add a collaborator to the map */
        private void AddCollaborator_Click(object sender, RoutedEventArgs e)
        {
            addPerson.ShowDialog();
            Close();
        }

        /* RetrieveCollaborator_Click(): On button press, open window to retrieve the closest
         * collaborator on the map, relative to inside the GUI polygon */
        private void RetrieveCollaborator_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.GetClosestPerson(sender, e);
            Close();
        }

        /* If this window is explicitly closed
         * remove all poylgons on the map */
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            /* Iterate through radiusEvents dictionary and clear every polygon value */
            foreach(KeyValuePair<int, GMapPolygon> circle in MainWindow.radiusEvents)
            {
                circle.Value.Clear();
            }
        }
    }
}
