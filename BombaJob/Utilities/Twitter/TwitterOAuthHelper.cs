using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Hammock.Authentication.OAuth;
using Hammock.Web;

namespace BombaJob.Utilities.Twitter
{
    public class TwitterOAuthHelper
    {
        internal static OAuthWebQuery GetRequestTokenQuery()
        {
            var oauth = new OAuthWorkflow
            {
                ConsumerKey = AppSettings.TwitterConsumerKey,
                ConsumerSecret = AppSettings.TwitterConsumerKeySecret,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                RequestTokenUrl = AppSettings.TwitterRequestTokenUri,
                Version = AppSettings.TwitterOAuthVersion,
                CallbackUrl = AppSettings.TwitterCallbackUri
            };

            var info = oauth.BuildRequestTokenInfo(WebMethod.Get);
            var objOAuthWebQuery = new OAuthWebQuery(info, AppSettings.InDebug);
            objOAuthWebQuery.HasElevatedPermissions = true;
            objOAuthWebQuery.SilverlightUserAgentHeader = "Hammock";
            objOAuthWebQuery.Method = WebMethod.Get;
            //objOAuthWebQuery.SilverlightMethodHeader = "GET";
            return objOAuthWebQuery;
        }

        internal static OAuthWebQuery GetAccessTokenQuery(string requestToken, string RequestTokenSecret, string oAuthVerificationPin)
        {
            var oauth = new OAuthWorkflow
            {
                AccessTokenUrl = AppSettings.TwitterAccessTokenUri,
                ConsumerKey = AppSettings.TwitterConsumerKey,
                ConsumerSecret = AppSettings.TwitterConsumerKeySecret,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                Token = requestToken,
                Verifier = oAuthVerificationPin,
                Version = AppSettings.TwitterOAuthVersion
            };

            var info = oauth.BuildAccessTokenInfo(WebMethod.Post);
            var objOAuthWebQuery = new OAuthWebQuery(info, AppSettings.InDebug);
            objOAuthWebQuery.HasElevatedPermissions = true;
            objOAuthWebQuery.SilverlightUserAgentHeader = "Hammock";
            objOAuthWebQuery.Method = WebMethod.Get;
            //objOAuthWebQuery.SilverlightMethodHeader = "GET";
            return objOAuthWebQuery;
        }
    }
}
