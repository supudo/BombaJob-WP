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
            fl.Add(AppResources.offer_iam_human);
            fl.Add(AppResources.offer_iam_company);
            this.ddHuman.ItemsSource = fl;

            fl = new List<string>();
            fl.Add(AppResources.freelance_all);
            fl.Add(AppResources.freelance_only);
            fl.Add(AppResources.freelance_no);
            this.ddFreelance.ItemsSource = fl;

            this.ddCategory.ItemsSource = App.DbViewModel.GetAllCategories();
            this.ddCategory.DisplayMemberPath = "Title";

            this.lblHuman.Text = AppResources.offer_iam_title;
            this.lblCategory.Text = AppResources.offer_category_title;
            this.lblFreelance.Text = AppResources.offer_FreelanceYn;
            this.lblTitle.Text = AppResources.offer_Human_Title;
            this.lblEmail.Text = AppResources.offer_Human_Email;
            this.lblNegativ.Text = AppResources.offer_Human_Negativ;
            this.lblPositiv.Text = AppResources.offer_Human_Positiv;
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