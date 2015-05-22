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
        public SingleUserAuthorizer userAuth { get; }
        public Twitter()
        {
            userAuth = new SingleUserAuthorizer
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = "u7sXwunH9BShuhmvWkemvWm6E",
                    ConsumerSecret = "MS9Nh3sDnjugcBrvId4w2q9ovUX4ZIWKukXkJPjcfihy9SMOZC",
                    AccessToken = "36460991-cFfU83zjD4irSWPaCkziO6JWhOCqYQFwmpKr1taaL",
                    AccessTokenSecret = "Rdwd5hrsDiEMYQachJ8Jc5LBqIxAkcUewTX7uyKaU446h"
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
