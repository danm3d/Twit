using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Windows.Web.Http;
using LinqToTwitter;
using System.Threading.Tasks;
using System.Linq;

namespace Twit
{
    class Twitter
    {
        private SingleUserAuthorizer userAuth { get; set; }
        public Twitter()
        {
            userAuth = new SingleUserAuthorizer
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = Settings.ConsumerKey,
                    ConsumerSecret = Settings.ConsumerSecret,
                    AccessToken = Settings.AccessToken,
                    AccessTokenSecret = Settings.AccessTokenSecret
                }
            };
        }

        public async void TweetMessage(string text)
        {
            var twitterCtx = new TwitterContext(userAuth);
            await twitterCtx.TweetAsync(text);
        }

        public async Task<List<Status>> ShowTweets()
        {
            var twitterCtx = new TwitterContext(userAuth);
            var tweets =
                            await
                            (from tweet in twitterCtx.Status
                             where tweet.Type == StatusType.User
                             select tweet)
                            .ToListAsync();
            return tweets;
        }
    }
}
