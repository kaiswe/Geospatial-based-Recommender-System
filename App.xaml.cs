using System.Windows;
using System.Windows.Threading;

namespace Geospatial_based_Recommender_System_via_XAML_UI_and_WPF
{
    /* App: Singleton class, since it defines
     * global resorces which may be used and acccessed from anywhere in
     * this application */
    public partial class App : Application
    {
        public App()
        {
            /*enforcing program properly shutsdown when an explicit shutdown reference is called
             * Reduces the issue of the program hanging around after trying to exit the program
             * by closing the main window*/
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }

        /* global exception handler method
         * Anytime an exception is encountered within the program
         * display this message, plus the exception
         * reduces overhead of having to implement try/catch blocks all over the code (Unless specifically needed) */
        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            /* display the exception message */
            MessageBox.Show("An error occured within the program: " + e.Exception.Message, "System Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            e.Handled = true;
        }
    }

}
