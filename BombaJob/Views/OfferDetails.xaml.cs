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
using BombaJob.Utilities;
using BombaJob.Utilities.Extensions;

namespace BombaJob.Utilities.Views
{
    public partial class OfferDetails : BombaJobBasePage
    {
        public OfferDetails()
        {
            InitializeComponent();
            this.pageTitle.Text = AppResources.appName;
            this.Loaded += new RoutedEventHandler(OfferDetails_Loaded);
        }

        void OfferDetails_Loaded(object sender, RoutedEventArgs e)
        {
            base.BuildApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string oid = "", h = "";
            if (NavigationContext.QueryString.TryGetValue("oid", out oid) && NavigationContext.QueryString.TryGetValue("h", out h))
            {
                int offerID = int.Parse(oid);
                bool humanYn = bool.Parse(h);
                AppSettings.LogThis("Offer id = " + offerID);
                BombaJob.Database.Models.JobOffer jo = App.DbViewModel.GetOffer(offerID);
                this.pageTitle.Text = jo.Title;

                this.txtCategory.Text = jo.CategoryTitle;
                this.txtDate.Text = AppSettings.DoLongDate(jo.PublishDate);

                this.txtFreelanceLabel.Text = AppResources.offer_FreelanceYn;
                this.txtFreelance.Text = ((jo.FreelanceYn) ? AppResources.yes : AppResources.no);

                this.txtNegativLabel.Text = ((humanYn) ? AppResources.offer_Human_Negativ : AppResources.offer_Company_Negativ);
                this.txtNegativ.Text = jo.Negativism;
                this.txtPositivLabel.Text = ((humanYn) ? AppResources.offer_Human_Positiv : AppResources.offer_Company_Positiv);
                //this.txtPositiv.Text = jo.Positivism;
                //RichTextBoxExtensions.SetLinkedText2(this.rtbPositiv, jo.Positivism);
                this.rtbPositiv.Blocks.Add(ParseContent(jo.Positivism));
            }
            else
                NavigationService.Navigate(new Uri("/Views/Newest.xaml", UriKind.Relative));
        }

        private Block ParseContent(string htmlText)
        {
            var paragraph = new Paragraph();
            var lastIndex = 0;

            foreach (Match m in Regex.Matches(htmlText, @"(http(s)?://)?([\w-]+\.)+[\w-]+(/\S\w[\w- ;,./?%&=]\S*)?"))
            {
                if (m.Index > 0)
                {
                    if (m.Index <= lastIndex)
                        paragraph.Inlines.Add(htmlText.Substring(lastIndex, m.Index));
                }

                var hyperlink = new Hyperlink()
                {
                    NavigateUri = new System.Uri(m.Value, System.UriKind.RelativeOrAbsolute),
                    TargetName = "_blank",
                    Foreground = this.rtbPositiv.Foreground
                };

                hyperlink.Inlines.Add(m.Value);

                paragraph.Inlines.Add(hyperlink);

                lastIndex = m.Index + m.Length;
            }

            if (lastIndex < htmlText.Length)
                paragraph.Inlines.Add(htmlText.Substring(lastIndex));

            return paragraph;
        }
    }
}