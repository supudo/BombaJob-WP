﻿using System;
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
using BombaJob.Utilities;

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
            string oid = "";
            if (NavigationContext.QueryString.TryGetValue("oid", out oid))
            {
                int offerID = int.Parse(oid);
                AppSettings.LogThis("Offer id = " + offerID);
            }
            else
                NavigationService.Navigate(new Uri("/Views/Newest.xaml", UriKind.Relative));
        }
    }
}