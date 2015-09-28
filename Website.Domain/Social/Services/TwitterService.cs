using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToTwitter;
using Newtonsoft.Json;
using Umbraco.Web.Models;
using Website.Domain.Shared.Services;
using Website.Domain.Social.Models;

namespace Website.Domain.Social.Services
{
    public class TwitterService : WebsiteService
    {
        private readonly FileStatusService _twitterStatus;
        private readonly EncryptionService _crypto;
        private readonly SingleUserAuthorizer _auth;

        public TwitterService()
        {
            _twitterStatus = new FileStatusService("tweets.log");
            _crypto = new EncryptionService();

            _auth = new SingleUserAuthorizer
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = App.Settings.TwitterAppId,
                    ConsumerSecret = App.Settings.TwitterSecret,
                    AccessToken = App.Settings.TwitterAccessToken,
                    AccessTokenSecret = App.Settings.TwitterAccessTokenSecret
                }
            };
        }

        public void UpdateTweets(int tweetCount)
        {
            var handle = App.Settings.TwitterHandle;

            if (!string.IsNullOrWhiteSpace(handle))
            {
                using (var context = new TwitterContext(_auth))
                {
                    var status = (from tweet in context.Status
                        where
                            tweet.Type == StatusType.User && tweet.ScreenName == handle && tweet.Count == tweetCount &&
                            tweet.IncludeRetweets == true && tweet.ExcludeReplies == true
                        select new StatusModel()
                        {
                            Id = tweet.StatusID,
                            Text = tweet.Text,
                            Handle = tweet.ScreenName,
                            CreatedDate = tweet.CreatedAt,
                            Name = tweet.User.Name
                        })
                        .ToList();

                    if (status.Count > 0)
                    {
                        var newTweets = JsonConvert.SerializeObject(status);

                        var newHash = _crypto.GetHashString(newTweets);
                        var hash = _crypto.GetHashString(App.Settings.LatestTweets);

                        if (!newHash.Equals(hash))
                        {
                            App.Settings.LatestTweets = newTweets;
                            App.Services.Content.Save(App.Settings);
                        }
                    }
                }
            }
        }
    }
}
