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
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using BombaJob.Utilities;
using Facebook;

namespace BombaJob.Views
{
    public partial class ShareFacebook : BombaJobBasePage
    {
        private readonly FacebookClient _fb = new FacebookClient();
        private const string AppId = "162884250446512";
        private const string ExtendedPermissions = "publish_stream";
        private string _accessToken;
        private string _userId;
        private string _lastMessageId;

        public ShareFacebook()
        {
            InitializeComponent();
        }

        private void wbFacebook_Loaded(object sender, RoutedEventArgs e)
        {
            var loginUrl = GetFacebookLoginUrl(AppId, ExtendedPermissions);
            wbFacebook.Navigate(loginUrl);
        }

        private Uri GetFacebookLoginUrl(string appId, string extendedPermissions)
        {
            var parameters = new Dictionary<string, object>();
            parameters["client_id"] = appId;
            parameters["redirect_uri"] = "https://www.facebook.com/connect/login_success.html";
            parameters["response_type"] = "token";
            parameters["display"] = "touch";

            if (!string.IsNullOrEmpty(extendedPermissions))
                parameters["scope"] = extendedPermissions;

            return _fb.GetLoginUrl(parameters);
        }

        private void wbFacebook_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            FacebookOAuthResult oauthResult;
            if (!_fb.TryParseOAuthCallbackUrl(e.Uri, out oauthResult))
                return;

            if (oauthResult.IsSuccess)
            {
                var accessToken = oauthResult.AccessToken;
                LoginSucceded(accessToken);
            }
            else
                MessageBox.Show(oauthResult.ErrorDescription);
        }

        private void LoginSucceded(string accessToken)
        {
            var fb = new FacebookClient(accessToken);

            fb.GetCompleted += (o, e) =>
            {
                if (e.Error != null)
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show(e.Error.Message));
                    return;
                }

                var result = (IDictionary<string, object>)e.GetResultData();
                var id = (string)result["id"];
                var url = string.Format("/Pages/FacebookInfoPage.xaml?access_token={0}&id={1}", accessToken, id);
                //Dispatcher.BeginInvoke(() => NavigationService.Navigate(new Uri(url, UriKind.Relative)));
                _accessToken = accessToken;
                _userId = id;
                this.postMessage();
            };

            fb.GetAsync("me?fields=id");
        }

        private void postMessage()
        {
            string msg = "";

            if (string.IsNullOrEmpty(msg))
            {
                MessageBox.Show("Enter message.");
                return;
            }

            var fb = new FacebookClient(_accessToken);

            fb.PostCompleted += (o, args) =>
            {
                if (args.Error != null)
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show(args.Error.Message));
                    return;
                }

                var result = (IDictionary<string, object>)args.GetResultData();
                _lastMessageId = (string)result["id"];

                Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("Message Posted successfully");
                });
            };

            var parameters = new Dictionary<string, object>();
            parameters["message"] = msg;

            fb.PostAsync("me/feed", parameters);

            NavigationService.GoBack();
        }
    }
}