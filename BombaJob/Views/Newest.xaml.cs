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

namespace BombaJob.Utilities.Views
{
    public partial class Newest : BombaJobBasePage
    {
        public Newest()
        {
            InitializeComponent();
            this.pageTitle.Text = AppResources.menu_Newest;
            this.Loaded += new RoutedEventHandler(Newest_Loaded);
        }

        void Newest_Loaded(object sender, RoutedEventArgs e)
        {
            this.offersList.ItemsSource = App.DbViewModel.GetNewestOffers();
        }
    }
}