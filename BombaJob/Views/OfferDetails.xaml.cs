using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;
using BombaJob.Database.Models;
using BombaJob.Sync;
using BombaJob.Utilities;
using BombaJob.Utilities.Controls;
using BombaJob.Utilities.Extensions;

namespace BombaJob.Utilities.Views
{
    public partial class OfferDetails : BombaJobBasePage
    {
        private JobOffer currentOffer;
        private Synchronization syncManager;

        public OfferDetails()
        {
            InitializeComponent();
            this.pageTitle.Text = AppResources.appName;
            this.Loaded += new RoutedEventHandler(OfferDetails_Loaded);
        }

        void OfferDetails_Loaded(object sender, RoutedEventArgs e)
        {
            this.BuildApplicationBar();
        }

        #region Application bar
        private void BuildApplicationBar()
        {
            this.ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("images/menu/tb-back.png", UriKind.Relative));
            appBarButton.Text = "back";
            appBarButton.Click += new System.EventHandler(back_Click);
            this.ApplicationBar.Buttons.Add(appBarButton);

            appBarButton = new ApplicationBarIconButton(new Uri("images/menu/tb-share-email.png", UriKind.Relative));
            appBarButton.Text = "email";
            appBarButton.Click += new System.EventHandler(shareEmail_Click);
            this.ApplicationBar.Buttons.Add(appBarButton);

            appBarButton = new ApplicationBarIconButton(new Uri("images/menu/tb-share-facebook.png", UriKind.Relative));
            appBarButton.Text = "facebook";
            appBarButton.Click += new System.EventHandler(shareFacebook_Click);
            this.ApplicationBar.Buttons.Add(appBarButton);

            appBarButton = new ApplicationBarIconButton(new Uri("images/menu/tb-share-twitter.png", UriKind.Relative));
            appBarButton.Text = "twitter";
            appBarButton.Click += new System.EventHandler(shareTwitter_Click);
            this.ApplicationBar.Buttons.Add(appBarButton);

            ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.offer_sendMessage);
            appBarMenuItem.Click += new System.EventHandler(sendMessage_Click);
            this.ApplicationBar.MenuItems.Add(appBarMenuItem);
        }

        private void back_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void shareEmail_Click(object sender, EventArgs e)
        {
            this.shareEmail();
        }

        private void shareFacebook_Click(object sender, EventArgs e)
        {
            this.shareFacebook();
        }

        private void shareTwitter_Click(object sender, EventArgs e)
        {
            this.shareTwitter();
        }

        private void sendMessage_Click(object sender, EventArgs e)
        {
            this.sendPM();
        }
        #endregion

        #region Screen
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string oid = "", h = "";
            if (NavigationContext.QueryString.TryGetValue("oid", out oid) && NavigationContext.QueryString.TryGetValue("h", out h))
            {
                int offerID = int.Parse(oid);
                bool humanYn = bool.Parse(h);
                AppSettings.LogThis("Offer id = " + offerID);
                this.currentOffer = App.DbViewModel.GetOffer(offerID);
                this.pageTitle.Text = this.currentOffer.Title;

                this.txtCategory.Text = this.currentOffer.CategoryTitle;
                this.txtDate.Text = AppSettings.DoLongDate(this.currentOffer.PublishDate);

                this.txtFreelanceLabel.Text = AppResources.offer_FreelanceYn;
                this.txtFreelance.Text = ((this.currentOffer.FreelanceYn) ? AppResources.yes : AppResources.no);

                this.txtNegativLabel.Text = ((humanYn) ? AppResources.offer_Human_Negativ : AppResources.offer_Company_Negativ);
                RichTextBoxExtensions.SetLinkedText(this.rtbNegativ, AppSettings.Hyperlinkify(this.currentOffer.Negativism));
                this.txtPositivLabel.Text = ((humanYn) ? AppResources.offer_Human_Positiv : AppResources.offer_Company_Positiv);
                RichTextBoxExtensions.SetLinkedText(this.rtbPositiv, AppSettings.Hyperlinkify(this.currentOffer.Positivism));
            }
            else
                NavigationService.Navigate(new Uri("/Views/Newest.xaml", UriKind.Relative));
        }
        #endregion

        #region Message
        private void sendPM()
        {
            NavigationService.Navigate(new Uri("/Views/SendPM.xaml?oid=" + this.currentOffer.OfferId, UriKind.Relative));
        }
        #endregion

        #region Email
        private void shareEmail()
        {
            this.showEmailPopup();
        }

        private void sendEmailViaWS(string toEmail)
        {
            if (this.syncManager == null)
                this.syncManager = new Synchronization();
            this.syncManager.SyncError += new Synchronization.EventHandler(syncManager_SyncError);
            this.syncManager.SyncComplete += new Synchronization.EventHandler(syncManager_SyncComplete);

            Dictionary<string, string> postArray = new Dictionary<string, string>();
            postArray.Add("oid", "" + this.currentOffer.OfferId);
            postArray.Add("toemail", toEmail);
            postArray.Add("fromemail", "");
            this.syncManager.DoSendEmail(postArray);
        }

        void syncManager_SyncComplete(object sender, BombaJobEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (!e.IsError)
                    MessageBox.Show(AppResources.offer_ThankYou);
            });
        }

        void syncManager_SyncError(object sender, BombaJobEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (e.IsError)
                    MessageBox.Show(e.ErrorMessage);
            });
        }

        private void showEmailPopup()
        {
            Popup popup = new Popup();
            popup.Height = 300;
            popup.Width = 400;
            popup.VerticalOffset = 100;
            InputPopup control = new InputPopup();
            popup.Child = control;
            popup.IsOpen = true;

            control.btnOK.Click += (s, args) =>
            {
                popup.IsOpen = false;
                string toEmail = control.txtValue.Text;

                if (!toEmail.Equals("") && AppSettings.ValidateEmail(toEmail))
                {
                    if (AppSettings.ConfInAppEmail)
                        this.sendEmail(toEmail);
                    else
                        this.sendEmailViaWS(toEmail);
                }
            };

            control.btnCancel.Click += (s, args) =>
            {
                popup.IsOpen = false;
            };
        }

        private void sendEmail(string toEmail)
        {
            string emailBody = "";
            emailBody += this.currentOffer.CategoryTitle + "<br/><br/>";
            emailBody += "<b>" + this.currentOffer.Title + "</b><br/><br/>";
            emailBody += "<i>" + AppSettings.DoLongDate(this.currentOffer.PublishDate) + "</i><br/><br/>";
            if (this.currentOffer.FreelanceYn)
                emailBody += AppResources.freelance_only + "<br/><br/>";
            else
                emailBody += AppResources.freelance_no + "<br/><br/>";
            if (this.currentOffer.HumanYn)
            {
                emailBody += "<b>" + AppResources.offer_Human_Positiv + "</b> " + this.currentOffer.Positivism + "<br/><br/>";
                emailBody += "<b>" + AppResources.offer_Human_Negativ + "</b> " + this.currentOffer.Negativism + "<br/><br/>";
            }
            else
            {
                emailBody += "<b>" + AppResources.offer_Company_Positiv + "</b> " + this.currentOffer.Positivism + "<br/><br/>";
                emailBody += "<b>" + AppResources.offer_Company_Negativ + "</b> " + this.currentOffer.Negativism + "<br/><br/>";
            }
            emailBody += "<br /><br /> Sent from BombaJob ...";

            EmailComposeTask emailComposeTask = new EmailComposeTask();
            emailComposeTask.Subject = AppResources.share_Email_Subject + " #" + this.currentOffer.OfferId;
            emailComposeTask.Body = emailBody;
            emailComposeTask.To = toEmail;
            emailComposeTask.Show();
        }
        #endregion

        #region Facebook
        private void shareFacebook()
        {
            /*
            string msg = "";
            msg += this.currentOffer.Positivism;

            ShareLinkTask shareLinkTask = new ShareLinkTask();
            shareLinkTask.LinkUri = new Uri("http://www.bombajob.bg/", UriKind.Absolute);
            shareLinkTask.Message = this.currentOffer.Title;
            shareLinkTask.Message = msg;
            shareLinkTask.Show();
             */
            NavigationService.Navigate(new Uri("/Views/ShareFacebook.xaml?oid=" + this.currentOffer.OfferId, UriKind.Relative));
        }
        #endregion

        #region Twitter
        private void shareTwitter()
        {
            /*
            string msg = "BombaJob.bg - ";
            msg += this.currentOffer.Title + ": http://bombajob.bg/offer/" + this.currentOffer.OfferId;
            msg += " #bombajobbg";

            ShareLinkTask shareLinkTask = new ShareLinkTask();
            shareLinkTask.LinkUri = new Uri("http://www.bombajob.bg/", UriKind.Absolute);
            shareLinkTask.Message = this.currentOffer.Title;
            shareLinkTask.Message = msg;
            shareLinkTask.Show();
             */
            NavigationService.Navigate(new Uri("/Views/ShareTwitter.xaml?oid=" + this.currentOffer.OfferId, UriKind.Relative));
        }
        #endregion
    }
}