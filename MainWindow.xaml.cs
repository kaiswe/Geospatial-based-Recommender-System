using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System.Windows.Shapes;


namespace Geospatial_based_Recommender_System_via_XAML_UI_and_WPF
{
    /* MainWindow class that is the core of the application
     * Handles all of the gmaps, collaborators, and GUI interaction */
    public partial class MainWindow : Window
    {
        /* Key value for the peopleMarkers Dictionary */
        private static int personCount = 0;
        /* Key valye for radiusEvents Dictionary */
        private static int polyCount = 0;
        private static Point[] polyPoints;
        public static SortedDictionary<int, GMapMarker> peopleMarkers = new SortedDictionary<int, GMapMarker>();
        public static SortedDictionary<int, GMapPolygon> radiusEvents = new SortedDictionary<int, GMapPolygon>();
        DisplayCollabInfo displayCollabInfo = new DisplayCollabInfo();





        public MainWindow()
        {
            InitializeComponent();
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            /*OpenStreetMap provider*/
            gmaps.MapProvider = OpenStreetMapProvider.Instance;
            /*Set Perth, Western Australia as the map load position*/
            gmaps.Position = new PointLatLng(-31.9523, 115.8613);
            gmaps.MinZoom = 2;
            gmaps.MaxZoom = 17;
            /*Zoom initialized on startup*/
            gmaps.Zoom = 11;
            gmaps.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            gmaps.CanDragMap = true;
            gmaps.DragButton = MouseButton.Left;
            gmaps.ShowCenter = false;

        }

        /*Gmap_GetPoint(): Access Lat and Lng coordinates of current mouse position
         *store in new PointLatLng*/
        private void Gmap_GetPoint(object sender, MouseButtonEventArgs e)
        {
            AddPerson addPerson = new AddPerson();
            Point clickPoint = e.GetPosition(this);
            /* convert the cursor click on the maps coordinates to a PointLatLng coordinate
             * relative to the maps positon and the cursor click */
            PointLatLng coords = gmaps.FromLocalToLatLng((int)clickPoint.X, (int)clickPoint.Y);
            /* pass coordinates to CreatePoly() for polygon creation */
            CreatePolygon(coords);
            /*Pass coordinates to new AddPerson Window to display the new events
             * relative coordinates*/
            addPerson.CollabLatLng(coords);

        }

        /* OnMouseMove(): Capture the mouse's current position
         * relative to the gmap in Lat and Lng
         * store dynamically updating coordinates in textboxes; visible on the GUI*/
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            /* mouse current position over the window*/
            Point p = e.GetPosition(this);
            double x = p.X;
            double y = p.Y;
            /* convert mouse current position over the window
             * and relative to the map
             * into a dynamically updating pair of coordinates on the GUI */
            textBoxLat.Text = gmaps.FromLocalToLatLng((int)x, (int)y).Lat.ToString();
            textBoxLng.Text = gmaps.FromLocalToLatLng((int)x, (int)y).Lng.ToString();
        }

        /* AddOrRetrieve(): Instantiate new window
         * User can decide to add a new collaborator to the map or
         * retrieve the closest person visible on the map; relative to this map click*/
        public void AddOrRetrieve(object sender, RoutedEventArgs e)
        {
            AddOrRetrieve addOrRetrieve = new AddOrRetrieve();
            
            /* ShowDialog() over Show() in regards to displaying new window
             * ShowDialog() makes the child window active and focused, which will remain as so until it is closed; 
             * reduces unnessecary duplicate windows and overhead*/
            addOrRetrieve.ShowDialog();


        }

        /* PlaceCollabMarkers(): Add user inputted collaborator onto the map and saved
         * within the system */
        public void PlaceCollabMarkers(object sender, RoutedEventArgs e)
        {
            AddPerson addPerson = new AddPerson();
            SortedDictionary<int, Collaborator> allPeople;
            /*Get the dictionary containing stored collaborators*/
            allPeople = addPerson.GetCollabDictionary();
            foreach (var person in allPeople)
            {
                /* if the counting variable personCount
                 * equals the current dictionary's value key*/
                if (personCount == person.Key)
                {
                    /*Create new marker based off this person value*/
                    GMapMarker people = new GMapMarker(new PointLatLng(Convert.ToDouble(person.Value.LocationLat), Convert.ToDouble(person.Value.LocationLng)));
                    /*Set marker shape to be the uploaded image of the collaborator*/
                    people.Shape = new Image
                    {

                        Width = 100,
                        Height = 100,
                        Source = new BitmapImage(person.Value.PersonPhoto.UriSource)
                    };
                    /* set the marker tag to the personCount variable
                     * which is also the peopleMarkers dictionary key
                     * this enables marker connection on user interation within DisplayCollaboratorInfo.xaml.cs */
                    people.Tag = personCount.ToString();
                    /*update the personCount variable on each loop iteration*/
                    personCount++;
                    /*add new marker to gmap Markers items*/
                    gmaps.Markers.Add(people);
                    /*add current personCount(key) and this people(value) marker to new SortedDictionary*/
                    peopleMarkers.Add(personCount, people);
                }

            }

        }

        /*Gmap_OnMarkerClick(): access collaborator information stored within this marker*/
        private void Gmap_OnMarkerClick(object sender, MouseButtonEventArgs e)
        {
            DisplayCollabInfo displayCollabInfo = new DisplayCollabInfo();

            foreach (KeyValuePair<int, GMapMarker> marker in peopleMarkers)
            {
                /*If the left mouse button is pressed and is placed over the marker*/
                if (e.LeftButton == MouseButtonState.Pressed && marker.Value.Shape.IsMouseOver)
                {
                    /*display this collaborators info*/
                    displayCollabInfo.CollabInfo(marker.Value);
                }
            }
        }

        /* CreatePolygon(): Create GUI polygon around mouse click coordinates for
         * visual representation of accessing the nearest collaborator */
        private void CreatePolygon(PointLatLng p)
        {
            /* The size of the polygon circle */
            double radius = 0.02;

            /* The points passed to this method, from Gmap_GetPoint(),
             * which holds the coordinates of where the cursor was clicked */
            PointLatLng clickRadius = new PointLatLng(p.Lat, p.Lng);

            /* An int representation of the amount of polygon sides
             * where two lines within the polygon meet */
            int segments = 1000;

            /* New list to hold all the new points that make up the polygon */
            List<PointLatLng> polygons = new List<PointLatLng>();

            /* Global variable for IsPointInPolygon()
             * which determines if a point is inside the circle polygon
             * based off these stored points */
            polyPoints = new Point[segments];

            /* Formula for calculating the area of a segment
             * In this case, the area of the circle radius */
            double seg = Math.PI * 2 / segments;

            /* Iterate over every segment point and store it */
            for (int i = 0; i < segments; i++)
            {
                /* Theta represents an angle */
                double theta = seg * i;
                /* A circle has two radiants
                 * where a and b represents x and y
                 * this formula calculates the point coordinates of the circle
                 * a = center(X) + Math.Cos(angle) * radius
                 * b = center(Y) + Math.Sin(angle) * radius */
                double a = clickRadius.Lat + Math.Cos(theta) * radius;
                double b = clickRadius.Lng + Math.Sin(theta) * radius;

                /* Add the a and b coordinates into a PointLatLat variable */
                PointLatLng gpoi = new PointLatLng(a, b);
                /* Store in Point array */
                polyPoints[i] = new Point(a, b);
                /* Store in polygons list */
                polygons.Add(gpoi);
            }
            /* Instantiate the polygon */
            GMapPolygon circle = new GMapPolygon(polygons);
            /* Call RegenerateShape on circle, otherwise it will be null */
            gmaps.RegenerateShape(circle);
            /* Instantiate styling options for the polygon */
            (circle.Shape as Path).Stroke = Brushes.Black;
            (circle.Shape as Path).StrokeThickness = 1;
            /* Setting effect to null for performance gain */
            (circle.Shape as Path).Effect = null;
            /* Add the polygon to the gmap markers */
            gmaps.Markers.Add(circle);
            /* Increment radiusevent key */
            polyCount++;
            /* Add key and value to radiusEvents */
            radiusEvents.Add(polyCount, circle);
        }

        /* IsPointInPolygon(): determines if a point in within a polygon
         * polygon points are identified by the passed parameters which store
         * the user's click on the map as Point p 
         * Point array stores values from surrounding segment points from CreatePolygon() */
        public bool IsPointInPolygon(Point p, Point[] polygon)
        {
            /*  Original code reference: 
             *  Copyright © 1994-2006, W Randolph Franklin (WRF)
             *  https://wrf.ecse.rpi.edu/Research/Short_Notes/pnpoly.html 
             *  edited method for c# use reference:
             *  https://stackoverflow.com/a/16391873, M Katz
             *  Refactored to represent original Wm. Randolph Franklin code
             *  but in c# */

            /* originally initializes bool inside variable as false */
            bool inside = false;
            /* Point[] polygon represents the number of vertices, stored as points, are within the polygon */
            /* loop executes for each i from 0 to the length of polygon points - 1
             * loop is done once an edge of every vertice is compared */
            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            {
                /* Comparing the stored X and Y coordinates of the polygon's verticies, represented as polygon[i] and polygon[j]
                 * against the stored X and Y coordinates of the cursor click Point P (P.X and P.Y) */
                if ((polygon[i].Y > p.Y) != (polygon[j].Y > p.Y) && p.X < (polygon[j].X - polygon[i].X) * (p.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X)
                {
                    inside = !inside;
                }
            }
            /*Returns bool to determine if a point falls inside the polygon */
            return inside;
        }

        /* GetClosestPerson(): Based off the IsPointInPolygon() Method
         * Determine if collaborator marker is inside polygon
         * Open closest marker for user view */
        public void GetClosestPerson(object sender, RoutedEventArgs e)
        {
            /* Iterate through markers dictionary to access the stored marker coordinates */
            foreach (KeyValuePair<int, GMapMarker> marker in peopleMarkers)
            {
                /* Iterate through polygon dictionary to access all stored polygons */
                foreach (KeyValuePair<int, GMapPolygon> poly in radiusEvents)
                {
                    /* Create a new point based off the PointLatLng value of each marker position */
                    Point p = new Point(marker.Value.Position.Lat, marker.Value.Position.Lng);

                    /* Determine if that point is inside the stored set of segment points of the polygon */
                    bool isInside = IsPointInPolygon(p, polyPoints);

                    if (isInside)
                    {
                        /* If the point is inside, display that markers collaborator information */
                        displayCollabInfo.CollabInfo(marker.Value);
                        /* clear polygon off the map */
                        poly.Value.Clear();
                    }
                    /* if false still clear polygon off the map */
                    poly.Value.Clear();
                }
            }
        }

        /* ShutdownApp(): Explicit shutdown method so program does not hang after mainwindow close */
        private void ShutdownApp(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
        /* OpenTwitterClient_Click(): Open twitter client and access through its relevant API on this button click */
        private void OpenTwitterClient_Click(object sender, RoutedEventArgs e)
        {
            TwittrClient twitterClient = new TwittrClient();
            twitterClient.ShowDialog();
        }

        private void OpenProfile_Click(object sender, RoutedEventArgs e)
        {
            PatientProfile profile = new PatientProfile();
            profile.Show();
        }

        private void UpdateProfile_Click(object sender, RoutedEventArgs e)
        {
            UpdatePatientProfile updateProfile = new UpdatePatientProfile();
            updateProfile.Show();
        }
    }
}
