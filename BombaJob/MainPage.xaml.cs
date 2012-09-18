using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        private Popup popup;
        private SplashScreen splashScreen;

        public MainPage()
        {
            InitializeComponent();
            ShowPopup();
        }

        private void ShowPopup()
        {
            if (this.splashScreen == null)
                this.splashScreen = new SplashScreen();
            this.splashScreen.SplashError += new SplashScreen.EventHandler(splashScreen_SplashError);
            this.splashScreen.SplashComplete += new SplashScreen.EventHandler(splashScreen_SplashComplete);

            this.popup = new Popup();
            this.popup.Child = this.splashScreen;
            this.popup.IsOpen = true;
            this.splashScreen.startSync();
        }

        void splashScreen_SplashComplete(object sender, BombaJobEventArgs e)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                this.popup.IsOpen = false;
            });
            RenderNewest();
        }

        void splashScreen_SplashError(object sender, BombaJobEventArgs e)
        {
            if (e.IsError)
                MessageBox.Show(e.ErrorMessage);
        }

        void RenderNewest()
        {
            base.BuildApplicationBar();
        }
    }
}