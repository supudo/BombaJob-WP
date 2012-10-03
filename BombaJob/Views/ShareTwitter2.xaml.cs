using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using BombaJob.Database.Models;
using BombaJob.Utilities;
using BombaJob.Utilities.Twitter;
using Hammock;
using Hammock.Authentication.OAuth;
using Hammock.Web;

namespace BombaJob.Views
{
    public partial class ShareTwitter2 : BombaJobBasePage
    {
        string postMessage = "";
        private string _oAuthTokenSecret;
        private string _oAuthToken;

        public ShareTwitter2()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string oid = "";
            if (NavigationContext.QueryString.TryGetValue("oid", out oid))
            {
                JobOffer jo = App.DbViewModel.GetOffer(int.Parse(oid));

                this.postMessage = "BombaJob.bg - ";
                this.postMessage += jo.Title + ": http://bombajob.bg/offer/" + jo.OfferId;
                this.postMessage += " #bombajobbg";
            }
        }

        private void wbTwitter_Loaded(object sender, RoutedEventArgs e)
        {
            GetTwitterToken();
        }

        private void wbTwitter_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
        }

        private void wbTwitter_Navigating(object sender, NavigatingEventArgs e)
        {
            if (e.Uri.AbsoluteUri.CompareTo(AppSettings.TwitterAuthorizeUri) == 0)
            {
            }

            if (!e.Uri.AbsoluteUri.Contains(AppSettings.TwitterCallbackUri))
                return;

            e.Cancel = true;

            var arguments = e.Uri.AbsoluteUri.Split('?');
            if (arguments.Length < 1)
                return;

            GetAccessToken(arguments[1]);
        }

        private static string GetQueryParameter(string input, string parameterName)
        {
            foreach (string item in input.Split('&'))
            {
                var parts = item.Split('=');
                if (parts[0] == parameterName)
                    return parts[1];
            }
            return String.Empty;
        }

        private void GetTwitterToken()
        {
            var credentials = new OAuthCredentials
            {
                Type = OAuthType.RequestToken,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                ConsumerKey = AppSettings.TwitterConsumerKey,
                ConsumerSecret = AppSettings.TwitterConsumerKeySecret,
                Version = AppSettings.TwitterOAuthVersion,
                CallbackUrl = AppSettings.TwitterCallbackUri
            };

            var client = new RestClient
            {
                Authority = AppSettings.TwitterOAuth,
                Credentials = credentials,
                HasElevatedPermissions = true
            };

            var request = new RestRequest
            {
                Path = "/request_token"
            };
            client.BeginRequest(request, new RestCallback(TwitterRequestTokenCompleted));
        }

        private void TwitterRequestTokenCompleted(RestRequest request, RestResponse response, object userstate)
        {
            _oAuthToken = GetQueryParameter(response.Content, "oauth_token");
            _oAuthTokenSecret = GetQueryParameter(response.Content, "oauth_token_secret");
            var authorizeUrl = AppSettings.TwitterAuthorizeUri + "?oauth_token=" + _oAuthToken;

            if (String.IsNullOrEmpty(_oAuthToken) || String.IsNullOrEmpty(_oAuthTokenSecret))
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("error calling twitter"));
                return;
            }

            Dispatcher.BeginInvoke(() => this.wbTwitter.Navigate(new Uri(authorizeUrl)));
        }

        private void GetAccessToken(string uri)
        {
            var requestToken = GetQueryParameter(uri, "oauth_token");
            if (requestToken != _oAuthToken)
                MessageBox.Show("Twitter auth tokens don't match");

            var requestVerifier = GetQueryParameter(uri, "oauth_verifier");

            var credentials = new OAuthCredentials
            {
                Type = OAuthType.AccessToken,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                ConsumerKey = AppSettings.TwitterConsumerKey,
                ConsumerSecret = AppSettings.TwitterConsumerKeySecret,
                Token = _oAuthToken,
                TokenSecret = _oAuthTokenSecret,
                Verifier = requestVerifier
            };

            var client = new RestClient
            {
                Authority = AppSettings.TwitterOAuth,
                Credentials = credentials,
                HasElevatedPermissions = true
            };

            var request = new RestRequest
            {
                Path = "/access_token"
            };

            client.BeginRequest(request, new RestCallback(RequestAccessTokenCompleted));
        }

        private void RequestAccessTokenCompleted(RestRequest request, RestResponse response, object userstate)
        {
            var twitteruser = new TwitterAccess
            {
                AccessToken = GetQueryParameter(response.Content, "oauth_token"),
                AccessTokenSecret = GetQueryParameter(response.Content, "oauth_token_secret"),
                UserId = GetQueryParameter(response.Content, "user_id"),
                ScreenName = GetQueryParameter(response.Content, "screen_name")
            };

            if (String.IsNullOrEmpty(twitteruser.AccessToken) || String.IsNullOrEmpty(twitteruser.AccessTokenSecret))
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show(response.Content));
                return;
            }

            //Helper.SaveSetting(Constants.TwitterAccess, twitteruser);

            Dispatcher.BeginInvoke(() =>
            {
                if (NavigationService.CanGoBack)
                    MessageBox.Show("1");
                else
                    MessageBox.Show("2");
            });
        }
    }

    public class TwitterAccess
    {
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
        public string UserId { get; set; }
        public string ScreenName { get; set; }
    }
}