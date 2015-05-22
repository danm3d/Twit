using LinqToTwitter;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Twit
{
    public sealed partial class MainPage : Page
    {
        private Twitter t;
        private TwitterContext ctx;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            t = new Twitter();
            ctx = new TwitterContext(t.userAuth);
            //StreamStatus(ctx);
        }

        private void btnTweet_Click(object sender, RoutedEventArgs e)
        {
            t.TweetMessage(txtTweetMessage.Text);
            txtTweetMessage.Text = "";
            RefreshTweets();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RefreshTweets();
        }

        private async void RefreshTweets()
        {
            listTweets.Children.Clear();
            var tweets = await t.ShowTweets();
            foreach (var tweet in tweets)
            {
                listTweets.Children.Add(TweetTextBlock(tweet.Text));
            }
        }

        private void AddTweet(StreamContent strm)
        {
            listTweets.Children.Add(TweetTextBlock(strm.Content));
        }

        private TextBlock TweetTextBlock(string s)
        {
            return new TextBlock()
            {
                Text = s,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 16,
                Padding = new Thickness(8, 16, 16, 4)
            };
        }

        public async void StreamStatus(TwitterContext twitterCtx)
        {
            int count = 0;
            await (from strm in twitterCtx.Streaming
                   where strm.Type == StreamingType.User
                   select strm).StartAsync(async strm =>
                {
                    Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    AddTweet(strm);
                });
                    if (count++ >= 5)
                        strm.CloseStream();
                });
        }
    }
}