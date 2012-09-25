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
using System.Windows.Media.Imaging;
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
            List<String> fl = new List<string>();
            fl.Add(AppResources.freelance_all);
            fl.Add(AppResources.freelance_only);
            fl.Add(AppResources.freelance_no);
            this.ddFreelance.ItemsSource = fl;

            var brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri(@"../Images/btnboom.png", UriKind.Relative));
            this.btnBoom.Background = brush;
        }

        private void post_Click(object sender, EventArgs e)
        {
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Newest.xaml", UriKind.Relative));
        }
    }
}