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
    public partial class Post : BombaJobBasePage
    {
        public Post()
        {
            InitializeComponent();
            this.pageTitle.Text = AppResources.menu_Post;
            this.Loaded += new RoutedEventHandler(Post_Loaded);
        }

        void Post_Loaded(object sender, RoutedEventArgs e)
        {
            base.BuildApplicationBar();
        }
    }
}