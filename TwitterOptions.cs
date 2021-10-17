namespace Geospatial_based_Recommender_System_via_XAML_UI_and_WPF
{
    /* TwitterOptions: Class that holds all relevant information
     * for connecting to twitter via this program
     * through its relevant API */
    public class TwitterOptions
    {
        /* Consumer Key */
        private static string APIKey = "eN6xyy5zTEv9HhWCXuGrVh5iV";
        /* Consumer Secret */
        private static string APISecret = "lD6uxZQnHIKBxFw9CU7dtXsSnQ9uy9i2aKrtxEYkvhWCzd7MG5";
        /* Access Token */
        private static string APIToken = "1398104042430078981-VbUY0K02jIQYKjLPTdkYNUlfV3uOVH";
        /* Access Secret */
        private static string APITokenSecret = "u3SEjmTqj94OcYuhJpdhrW8Yle5lLgVDmfFhh5hTjgkc3";

        /* Default constructor */
        public TwitterOptions()
        {
            ConsumerKey = APIKey;
            ConsumerSecret = APISecret;
            AccessToken = APIToken;
            AccessTokenSecret = APITokenSecret;

        }
        /* initialize keys to relevant class calls */
        public TwitterOptions(string APIKey, string APISecret, string APIToken, string APITokenSecret)
        {
            ConsumerKey = APIKey;
            ConsumerSecret = APISecret;
            AccessToken = APIToken;
            AccessTokenSecret = APITokenSecret;

        }
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }

    }
}
