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
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using BombaJob.Utilities;

namespace BombaJob.Utilities.Views
{
    public partial class Jobs : BombaJobBasePage
    {
        public Jobs()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Jobs_Loaded);
        }

        void Jobs_Loaded(object sender, RoutedEventArgs e)
        {
            base.BuildApplicationBar();
            this.ApplicationBar.Mode = Microsoft.Phone.Shell.ApplicationBarMode.Minimized;
            this.jobsPanorama.Title = AppResources.menu_Jobs;
            this.panCategories.Header = AppResources.menu_Categories;
            this.panOffers.Header = AppResources.menu_Offers;
            this.categoriesList.ItemsSource = App.DbViewModel.GetCategories(false);
            this.offersList.ItemsSource = App.DbViewModel.GetOffers(false);
        }

        private void Categories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.categoriesList.SelectedIndex == -1)
                return;
            //((BombaJob.Database.Tables.Categories)e.AddedItems[0]).CategoryId
            this.categoriesList.SelectedIndex = -1;
        }

        private void Offers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.offersList.SelectedIndex == -1)
                return;
            NavigationService.Navigate(new Uri("/Views/OfferDetails.xaml?oid=" + ((BombaJob.Database.Tables.JobOffers)e.AddedItems[0]).OfferId, UriKind.Relative));
            this.offersList.SelectedIndex = -1;
        }
    }
}