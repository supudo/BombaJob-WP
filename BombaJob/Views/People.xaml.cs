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
    public partial class People : BombaJobBasePage
    {
        public People()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(People_Loaded);
        }

        void People_Loaded(object sender, RoutedEventArgs e)
        {
            base.BuildApplicationBar();
            this.ApplicationBar.Mode = Microsoft.Phone.Shell.ApplicationBarMode.Minimized;
        }
    }
}