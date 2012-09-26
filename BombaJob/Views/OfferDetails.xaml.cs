using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using BombaJob.Database.Models;
using BombaJob.Utilities;
using BombaJob.Utilities.Extensions;

namespace BombaJob.Utilities.Views
{
    public partial class OfferDetails : BombaJobBasePage
    {
        JobOffer currentOffer;

        public OfferDetails()
        {
            InitializeComponent();
            this.pageTitle.Text = AppResources.appName;
        }

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

        private void back_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void shareEmail_Click(object sender, EventArgs e)
        {
            this.emailPopup();
        }

        private void shareFacebook_Click(object sender, EventArgs e)
        {
            this.shareFacebook();
        }

        private void shareTwitter_Click(object sender, EventArgs e)
        {
            this.shareTwitter();
        }

        #region Email
        private void emailPopup()
        {
        }

        private void sendEmail()
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
            emailComposeTask.To = "";
            emailComposeTask.Show();
        }
        #endregion

        #region Facebook
        private void shareFacebook()
        {
        }
        #endregion

        #region Twitter
        private void shareTwitter()
        {
        }
        #endregion
    }
}