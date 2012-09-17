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
using Microsoft.Phone.Controls;
using System.Threading;
using System.ComponentModel;
using BombaJob.Workers;

namespace BombaJob
{
    public partial class MainPage : PhoneApplicationPage
    {
        private Popup popup;
        private BackgroundWorker backroungWorker;
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
            //ShowSplashScreen();
            this.splashScreen.startSync();
        }

        void splashScreen_SplashComplete(object sender, BJEventArgs e)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                this.popup.IsOpen = false;
            });
        }

        void splashScreen_SplashError(object sender, BJEventArgs e)
        {
            if (e.IsError)
                MessageBox.Show(e.ErrorMessage);
        }

        private void ShowSplashScreen()
        {
            backroungWorker = new BackgroundWorker();
            backroungWorker.DoWork += new DoWorkEventHandler(backroungWorker_DoWork);
            backroungWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backroungWorker_RunWorkerCompleted);
            backroungWorker.RunWorkerAsync();
        }

        void backroungWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                this.popup.IsOpen = false;
            });
        }

        void backroungWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(7000);
        }
    }
}