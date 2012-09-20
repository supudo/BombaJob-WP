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
    public partial class About : BombaJobBasePage
    {
        public About()
        {
            InitializeComponent();
            this.pageTitle.Text = AppResources.menu_About;
            this.webView.Loaded += WebBrowser_OnLoaded;
            this.webView.Navigating += WebView_Navigating;
        }

        private void WebBrowser_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.webView.NavigateToString(App.DbViewModel.GetTextContent(35));
        }

        void WebView_Navigating(object sender, NavigatingEventArgs e)
        {
            MessageBox.Show(AppResources.linkClicked);
        }
    }
}