using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using Microsoft.Phone.Controls;
using BombaJob.Database.Models;
using BombaJob.Utilities;
using BombaJob.Utilities.Twitter;
using Hammock;
using Hammock.Authentication.OAuth;
using Hammock.Web;

namespace BombaJob.Views
{
    public partial class ShareTwitter : BombaJobBasePage
    {
        string postMessage = "";

        string OAuthTokenKey = string.Empty;
        string tokenSecret = string.Empty;

        string accessToken = string.Empty;
        string accessTokenSecret = string.Empty;

        string userID = string.Empty;
        string userScreenName = string.Empty;

        public ShareTwitter()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string oid = "", h = "";
            if (NavigationContext.QueryString.TryGetValue("oid", out oid) && NavigationContext.QueryString.TryGetValue("h", out h))
            {
                int offerID = int.Parse(oid);
                bool humanYn = bool.Parse(h);
                AppSettings.LogThis("Offer id = " + offerID);
                JobOffer jo = App.DbViewModel.GetOffer(offerID);

                this.postMessage = "BombaJob.bg - ";
                this.postMessage += jo.Title + ": http://bombajob.bg/offer/" + jo.OfferId;
                this.postMessage += " #bombajobbg";
            }
        }

        private void wbTwitter_Loaded(object sender, RoutedEventArgs e)
        {
            accessToken = TwitterHelper.GetKeyValue<string>("AccessToken");
            accessTokenSecret = TwitterHelper.GetKeyValue<string>("AccessTokenSecret");
            userScreenName = TwitterHelper.GetKeyValue<string>("ScreenName");

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(accessTokenSecret))
            {
                var requestTokenQuery = TwitterOAuthHelper.GetRequestTokenQuery();
                requestTokenQuery.RequestAsync(AppSettings.TwitterRequestTokenUri, null);
                requestTokenQuery.QueryResponse += new EventHandler<WebQueryResponseEventArgs>(requestTokenQuery_QueryResponse);
            }
            else
            {
                Dispatcher.BeginInvoke(() =>
                {
                    var tweetButton = (Microsoft.Phone.Shell.ApplicationBarIconButton)this.ApplicationBar.Buttons[3];
                    tweetButton.IsEnabled = true;
                });
            }
        }

        private void wbTwitter_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            this.wbTwitter.Visibility = Visibility.Visible;
            this.wbTwitter.Navigated -= wbTwitter_Navigated;
        }

        void requestTokenQuery_QueryResponse(object sender, WebQueryResponseEventArgs e)
        {
            try
            {
                var parameters = TwitterHelper.GetQueryParameters((new StreamReader(e.Response)).ReadToEnd());
                OAuthTokenKey = parameters["oauth_token"];
                tokenSecret = parameters["oauth_token_secret"];
                var authorizeUrl = AppSettings.TwitterAuthorizeUri + "?oauth_token=" + OAuthTokenKey;

                Dispatcher.BeginInvoke(() =>
                {
                    this.wbTwitter.Navigate(new Uri(authorizeUrl));
                });
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show(ex.Message);
                });
            }
        }

        private void objAuthorizeBrowserControl_Navigating(object sender, NavigatingEventArgs e)
        {
            if (e.Uri.ToString().StartsWith(AppSettings.TwitterCallbackUri))
            {
                var AuthorizeResult = TwitterHelper.GetQueryParameters(e.Uri.ToString());
                var VerifyPin = AuthorizeResult["oauth_verifier"];
                this.wbTwitter.Visibility = Visibility.Collapsed;

                var AccessTokenQuery = TwitterOAuthHelper.GetAccessTokenQuery(OAuthTokenKey, tokenSecret, VerifyPin);

                AccessTokenQuery.QueryResponse += new EventHandler<WebQueryResponseEventArgs>(AccessTokenQuery_QueryResponse);
                AccessTokenQuery.RequestAsync(AppSettings.TwitterAccessTokenUri, null);
            }
        }

        void AccessTokenQuery_QueryResponse(object sender, WebQueryResponseEventArgs e)
        {
            try
            {
                var parameters = TwitterHelper.GetQueryParameters((new StreamReader(e.Response)).ReadToEnd());
                accessToken = parameters["oauth_token"];
                accessTokenSecret = parameters["oauth_token_secret"];
                userID = parameters["user_id"];
                userScreenName = parameters["screen_name"];

                TwitterHelper.SetKeyValue<string>("AccessToken", accessToken);
                TwitterHelper.SetKeyValue<string>("AccessTokenSecret", accessTokenSecret);
                TwitterHelper.SetKeyValue<string>("ScreenName", userScreenName);

                Dispatcher.BeginInvoke(() =>
                {
                    var SignInMenuItem = (Microsoft.Phone.Shell.ApplicationBarMenuItem)this.ApplicationBar.MenuItems[0];
                    SignInMenuItem.IsEnabled = false;

                    var SignOutMenuItem = (Microsoft.Phone.Shell.ApplicationBarMenuItem)this.ApplicationBar.MenuItems[1];
                    SignOutMenuItem.IsEnabled = true;

                    var tweetButton = (Microsoft.Phone.Shell.ApplicationBarIconButton)this.ApplicationBar.Buttons[0];
                    tweetButton.IsEnabled = true;
                });
                this.postMessageToTwitter();
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show(ex.Message);
                });
            }
        }

        private void postMessageToTwitter()
        {
            var credentials = new OAuthCredentials
            {
                Type = OAuthType.ProtectedResource,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                ConsumerKey = AppSettings.TwitterConsumerKey,
                ConsumerSecret = AppSettings.TwitterConsumerKeySecret,
                Token = this.accessToken,
                TokenSecret = this.accessTokenSecret,
                Version = "1.0"
            };

            var restClient = new RestClient
            {
                Authority = AppSettings.TwitterStatusUpdateUrl,
                HasElevatedPermissions = true,
                Credentials = credentials,
                Method = WebMethod.Post
            };

            restClient.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            var restRequest = new RestRequest
            {
                Path = "1/statuses/update.xml?status=" + this.postMessage
            };

            var ByteData = Encoding.UTF8.GetBytes(this.postMessage);
            restRequest.AddPostContent(ByteData);
            restClient.BeginRequest(restRequest, new RestCallback(postFinished));
        }

        private void postFinished(RestRequest request, Hammock.RestResponse response, object obj)
        {
            NavigationService.GoBack();
        }
    }
}