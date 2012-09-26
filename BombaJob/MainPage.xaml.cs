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
using System.Threading;
using System.ComponentModel;
using BombaJob.Utilities;

namespace BombaJob
{
    public partial class MainPage : BombaJobBasePage
    {
        public MainPage()
        {
            InitializeComponent();
            base.SyncComplete += new BombaJobBasePage.EventHandler(MainPage_SyncComplete);
            base.DoSync();
        }

        void MainPage_SyncComplete(object sender, BombaJobEventArgs e)
        {
            base.BuildApplicationBar();
            NavigationService.Navigate(new Uri("/Views/Newest.xaml", UriKind.Relative));
        }
    }
}