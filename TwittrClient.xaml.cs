using System;
using System.Windows;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using System.Diagnostics;
namespace Geospatial_based_Recommender_System_via_XAML_UI_and_WPF
{
    public partial class TwittrClient : Window
    {
        /* TwittrClient: Class that handles the twitter API,
         * and creates a connection between their client and
         * this project */

        private static TwitterClient userClient;
        private static IAuthenticatedUser user;
        TwitterOptions tweetOptions = new TwitterOptions();

        public TwittrClient()
        {
            InitializeComponent();
        }


        /* TwittrClientInstance_Loaded(): Method that uses the Consumer key, secret, Acess token
         * and secret to access the twitter account of the person using this program.
         * This method implements PIN authorization for the user to identify themselves */
        public async void TwittrClientInstance_Loaded(object sender, RoutedEventArgs e)
        {
            /* Enable Console window during this method for user PIN authentication */
            /* Create a client application for user */
            TwitterClient appClient = new TwitterClient(tweetOptions.ConsumerKey, tweetOptions.ConsumerSecret);

            /* Begin the authentication process */
            IAuthenticationRequest authenticationRequest = await appClient.Auth.RequestAuthenticationUrlAsync();

            /* Go to the authorization URL so that twitter can authenthicate the user, and provide
             * said user with a PIN code */
            Process.Start(new ProcessStartInfo(authenticationRequest.AuthorizationURL)
            {
                UseShellExecute = true
            });

            /* Ask the user to enter the PIN code they recieved into the code application */
            /* Access AuthDialog: class to prompt user for code input*/
            AuthDialog authenticateUser = new AuthDialog("Please enter your twitter authentication the code", "");
            authenticateUser.ShowDialog();
            /* This code authenticates user */
            string pinCode = authenticateUser.pinCode.Text;

            /* Try block to determine if the user pressed an acceptable input for entering the PIN code */
            try
            {
                ITwitterCredentials userCredentials = await appClient.Auth.RequestCredentialsFromVerifierCodeAsync(pinCode, authenticationRequest);
                /* Saving the user credentials in global variables for use on other twitter API methods */
                userClient = new TwitterClient(userCredentials);
                user = await userClient.Users.GetAuthenticatedUserAsync();
                MessageBox.Show("Congratulations you have authenticated the user: " + user, "Authentication Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            /* Catch a null exception if user input is empty */
            catch (ArgumentNullException)
            {
                MessageBox.Show("Input cannot be empty, please try again", "User input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        /* CreateTweet(): User can write tweet in the program and it publishes to twitter */
        private async void CreateTweet(object sender, RoutedEventArgs e)
        {
            /* Twitter API waits for user to write text in tweet.Textbox, then publishes to twitter */
            ITweet Tweet = await userClient.Tweets.PublishTweetAsync(new PublishTweetParameters(tweet.Text));

            /* Displays message on screen showing what tweet was published, and if it was successful */
            MessageBox.Show("Tweet: " + Tweet + " " + "Successfully published to Twitter! Under the username: " + user, "Tweet Successful!", MessageBoxButton.OK, MessageBoxImage.Information);
           /* Add every tweet published through the program to display on the published tweets tab
            * published tweet tab is a listbox which holds every tweet */
            AddTweets.Items.Add(Tweet);
            tweet.Text = null; 
        }

        /* myTweets_Click(): Grabs all the tweets the user of the program has published */
        private async void myTweets_Click(object sender, RoutedEventArgs e)
        {
            /* Clears listbox on button click */
            grabTweetsBox.Items.Clear();
            /* Gets the tweets from the specific user */
            ITweet[] userTimelineTweets = await userClient.Timelines.GetUserTimelineAsync(user);
            /* Iterate through the ITweet array, printing every tweet value */
            for (int i = 0; i < userTimelineTweets.Length; i++)
            {
                grabTweetsBox.Items.Add(userTimelineTweets[i]);
            }
        }

        /* homeTweets_Click(): Grabs all the tweets the user has on their twitter home page */
        private async void homeTweets_Click(object sender, RoutedEventArgs e)
        {
            /* Clears listbox on button click */
            grabTweetsBox.Items.Clear();
            /* Gets the tweets on the users home page */
            ITweet[] homeTimelineTweets = await userClient.Timelines.GetHomeTimelineAsync();
            /* Iterate through the ITweet array, printing every tweet value */
            for (int i = 0; i < homeTimelineTweets.Length; i++)
            {
                grabTweetsBox.Items.Add(homeTimelineTweets[i]);
            }

        }

        /* mentionsTweets_Click(): Grabs all the tweets where the user is mentioned */ 
        private async void mentionsTweets_Click(object sender, RoutedEventArgs e)
        {
            /* Clears listbox on button click */
            grabTweetsBox.Items.Clear();
            ITweet[] mentionsTimeline = await userClient.Timelines.GetMentionsTimelineAsync();
            /* Iterate through the ITweet array, printing every tweet value */
            for (int i = 0; i < mentionsTimeline.Length; i++)
            {
                grabTweetsBox.Items.Add(mentionsTimeline[i]);
            }

        }

        /* grabRetweets_Click(): Grabs all the tweets that the user has retweeted */
        private async void grabRetweets_Click(object sender, RoutedEventArgs e)
        {
            /* Clears listbox on button click */
            grabTweetsBox.Items.Clear();
            ITweet[] retweetsOfMeTimeline = await userClient.Timelines.GetRetweetsOfMeTimelineAsync();
            /* Iterate through the ITweet array, printing every tweet value */
            for (int i = 0; i < retweetsOfMeTimeline.Length; i++)
            {
                grabTweetsBox.Items.Add(retweetsOfMeTimeline[i]);
            }
        }
    }
}
