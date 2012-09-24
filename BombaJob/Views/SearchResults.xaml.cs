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

namespace BombaJob.Views
{
    public partial class SearchResults : BombaJobBasePage
    {
        public SearchResults()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SearchResults_Loaded);
        }

        void SearchResults_Loaded(object sender, RoutedEventArgs e)
        {
            string keyword = "", freelanceYn = "";
            if (NavigationContext.QueryString.TryGetValue("k", out keyword) && NavigationContext.QueryString.TryGetValue("f", out freelanceYn))
                this.offersList.ItemsSource = App.DbViewModel.SearchOffers(keyword, bool.Parse(freelanceYn));
            else
                NavigationService.Navigate(new Uri("/Views/Search.xaml", UriKind.Relative));
        }

        private void Offers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.offersList.SelectedIndex == -1)
                return;
            int oid = ((BombaJob.Database.Models.JobOffer)e.AddedItems[0]).OfferId;
            bool humanYn = ((BombaJob.Database.Models.JobOffer)e.AddedItems[0]).HumanYn;
            NavigationService.Navigate(new Uri("/Views/OfferDetails.xaml?oid=" + oid + "&h=" + humanYn, UriKind.Relative));
            this.offersList.SelectedIndex = -1;
        }
    }
}